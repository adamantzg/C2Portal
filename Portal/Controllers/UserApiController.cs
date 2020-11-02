using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using KellermanSoftware.CompareNetObjects;
using Portal.JWT;
using Portal.Model;

namespace Portal.Controllers
{
    public class UserApiController : BaseApiController
    {
        PasswordRule rule = new PasswordRule(8, 1);
	    private IEncryption encrypter;

	    public UserApiController(IUnitOfWork unitOfWork,IMailHelper mailHelper, IApiClient apiClient, 
		    IMyJwtDecoder jwtDecoder, IEncryption encrypter, ICache cache) : base(unitOfWork, mailHelper, apiClient, jwtDecoder, cache)
	    {
		    this.encrypter = encrypter;
	    }

	    [Route("api/user")]
        [HttpGet]
        public object GetUser(int id)
        {
            return GetUIObject(uow.UserRepository.Get(u => u.id == id, includeProperties: "Customer, Roles").FirstOrDefault());
        }

        [Route("api/usersModel")]
        [HttpGet]
        public object GetUsersModel()
        {
            var user = GetCurrentUser();
            return new
            {
                users = user.isAdmin ? uow.UserRepository.Get(includeProperties: "Customer, Roles").Select(GetUIObject) :
                    user.isBranchAdmin ? uow.UserRepository.Get(u => (u.Customer.invoice_customer == user.customer_code 
                          || u.customer_code == user.customer_code) && u.isInternal != true, includeProperties: "Customer, Roles").Select(GetUIObject) : null,
                roles = user.isAdmin ? uow.RoleRepository.Get() : uow.RoleRepository.Get(r=> r.id != Role.Admin && r.id != Role.BranchAdmin)
            };
        }

        [Route("api/user/customerSearch")]
        [HttpGet]
        public object CustomerSearch(string code, int? role_id)
        {
            var user = GetCurrentUser();
            if ((role_id != Role.BranchAdmin && role_id != Role.Admin) && user.isBranchAdmin)
            {
				return uow.CustomerRepository.Get(c => (c.code.Contains(code) || c.name.Contains(code) || c.address6.Contains(code)) && c.invoice_customer == user.customer_code 
				          && c.analysis_codes_1 != "CLOSED").Select(GetCustomerUIObject);
            }
			return uow.CustomerRepository.Get(c => c.code.Contains(code) || c.name.Contains(code) || c.address6.Contains(code)).Select(GetCustomerUIObject);
        }

	    [Route("api/user/getCustomer")]
	    [HttpGet]
	    public object GetCustomer(string code)
	    {
		    var user = GetCurrentUser();
		    var customer_code = code.Trim();
		    var userCustomerCode = user.customer_code;
			if(user.isAdmin || user.isInternal == true)
				return uow.CustomerRepository.Get(c => c.code == customer_code).Select(GetCustomerUIObject).FirstOrDefault();
		    if (user.isBranchAdmin)
			    return uow.CustomerRepository.Get(c =>
				    c.code == customer_code && (c.code == userCustomerCode || c.invoice_customer == userCustomerCode)).FirstOrDefault();
		    return code == userCustomerCode ? uow.CustomerRepository.Get(c => c.code == userCustomerCode).FirstOrDefault() : null;
	    }

        [Route("api/user/create")]
        [HttpPost]
        public object Create(User u)
        {
            var currUser = GetCurrentUser();
            if (!(currUser.isAdmin || currUser.isBranchAdmin))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
	        
			if(!ValidateUser(u, out string validationMessage))
				return new HttpResponseMessage {StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(validationMessage)};

			/*if (currUser.isBranchAdmin)
				u.customer_code = currUser.customer_code;*/
            uow.UserRepository.Insert(u);
            uow.Save();
            return GetUIObject(u);
        }

		private bool ValidateUser(User u, out string validationMessage)
		{
			validationMessage = String.Empty;
			if (string.IsNullOrEmpty(u.name) || string.IsNullOrEmpty(u.lastname) || string.IsNullOrEmpty(u.email) ||
			    string.IsNullOrEmpty(u.customer_code))
			{
				validationMessage = "Name, lastname, email and customer code are mandatory.";
				return false;
			}

			var duplicate = uow.UserRepository.Get(x => (x.email == u.email || x.username == u.username) && x.id != u.id)
				.FirstOrDefault();
			if (duplicate != null)
			{
				validationMessage = "E-mail or username already exists";
				return false;
			}

			var customer = uow.CustomerRepository.Get(c => c.code == u.customer_code.Trim()).FirstOrDefault();
			if (customer == null)
			{
				validationMessage = "No customer with that code exists";
				return false;
			}

			var currUser = GetCurrentUser();
			if (currUser.isBranchAdmin)
			{
				var allowedCustomers = GetAllowedCustomersForBranchAdmin(uow, currUser.customer_code).Select(c=>c.code).ToList();
				if (!allowedCustomers.Contains(u.customer_code))
				{
					validationMessage = "You are not allowed to use this customer code";
					return false;
				}
			}

			return true;
		}

	    public static List<Customer> GetAllowedCustomersForBranchAdmin(IUnitOfWork uow, string customer_code)
	    {
		    return uow.CustomerRepository.Get(c => c.invoice_customer == customer_code || c.code == customer_code)
			    .ToList();
	    }

		[Route("api/user/update")]
        [HttpPost]
        public object Update(User u)
        {
            var currUser = GetCurrentUser();
            if (!(currUser.isAdmin || currUser.isBranchAdmin))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
	        
	        if(!ValidateUser(u, out string validationMessage))
		        return new HttpResponseMessage {StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(validationMessage)};
			var oldUser = uow.UserRepository.GetByID(u.id);
			u.password = oldUser?.password;
            uow.UserRepository.Update(u);
            uow.Save();
            return GetUIObject(u);
        }

        [Route("api/user/delete")]
        [HttpPost]
        public object Delete(int id)
        {
            var currUser = GetCurrentUser();
            if (!(currUser.isAdmin || currUser.isBranchAdmin))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            uow.UserRepository.DeleteByIds(new[] { id });
            return true;
        }

        public static object GetUIObject(User user)
        {
            return new
            {
                user.id,
                user.name,
                user.lastname,
                user.username,
                user.customer_code,
                user.address,
                user.email,
                user.phone,
                user.token,
				user.isInternal,
				user.lastLogin,
                isBranchAdmin = user.Roles != null ? user.Roles.Any(r => r.id == Role.BranchAdmin) : false,
                isTopAdmin = user.Roles != null ? user.Roles.Any(r => r.id == Role.Admin) : false,
                customer = user.Customer != null ? GetCustomerUIObject(user.Customer) : null,
                roles = user.Roles?.Select(r => new { r.id, r.name }),
                activated = !string.IsNullOrEmpty(user.password),
				permissions = user.Permissions?.Select(p=> new {p.id, p.name})
            };
        }

        public static object GetCustomerUIObject(Customer c)
        {
            return new
            {
                c.code,
                c.address1,
                c.address2,
                c.address3,
                c.address4,
                c.address5,
                c.address6,
                c.currency,
                c.analysis_codes_1,
                c.county,
                c.town_city,
                c.name,
				combined_name = $"{c.code} {c.name}{(!string.IsNullOrEmpty(c.address6) ? $" ({c.address6})" : string.Empty)}"
            };
        }


        [Route("api/user/updateData")]
        [HttpPost]
        public object UpdateData(User user)
        {
            var currUser = GetCurrentUser();
            if (!(currUser.isAdmin || currUser.id == user.id))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            var oldUser = uow.UserRepository.Get(u => u.id == user.id, includeProperties: "Customer").FirstOrDefault();
            if (oldUser != null)
            {
				if (!string.IsNullOrEmpty(oldUser.customer_code) && oldUser.Customer == null)
				{
					//Fix for spaces
					var cust_code = oldUser.customer_code.Trim();
					oldUser.Customer = uow.CustomerRepository.Get(c => c.code == cust_code).FirstOrDefault();
				}
					
                var compareLogic = new CompareLogic();
                compareLogic.Config.MaxDifferences = 5;
                var ignored = new[] { ".password", ".token", ".Roles" };
                var result = compareLogic.Compare(user, oldUser);
                if (!result.AreEqual && result.Differences.Any(d => !ignored.Contains(d.PropertyName)))
                {
                    user.password = oldUser.password;   //preserve password
                    uow.UserRepository.Copy(user, oldUser);
					if(oldUser.Customer != null)
						uow.CustomerRepository.Copy(user.Customer, oldUser.Customer);
                    uow.Save();
	                var bodyParts = new List<Tuple<string, string>>()
	                {
		                new Tuple<string, string>(".name", $"name: {user.name}"),
		                new Tuple<string, string>(".lastname",$"lastname: {user.lastname}"),
		                new Tuple<string, string>(".Customer.name",$"business name: {user.Customer?.name}"), 
		                new Tuple<string, string>(".Customer.address1",$"address line 1: {user.Customer?.address1}"),
		                new Tuple<string, string>(".Customer.address2", $"address line 2: {user.Customer?.address2}"),
		                new Tuple<string, string>(".Customer.address3", $"address line 3: {user.Customer?.address3}"),
		                new Tuple<string, string>(".Customer.address4",$"address line 4: {user.Customer?.address4}"),
		                new Tuple<string, string>(".Customer.address6", $"postcode: {user.Customer?.address6}"),
		                new Tuple<string, string>(".Customer.address5",$"phone: {user.Customer?.address5}")
	                };
                    //send email
                    var body = $@"User {user.name} {user.lastname} data changed. Changed properties are shown in bold.<br>
								 Changed by: {currUser.email} <br>
								 <br>
								 {string.Join("<br/>",bodyParts.Select(p=>result.Differences.Any(d=>d.PropertyName == p.Item1) ? $"<strong>{p.Item2}</strong>" : p.Item2))}
								";
                    mailHelper.SendMail(Properties.Settings.Default.MailFrom, Properties.Settings.Default.UserDataChangedMailTo, "Customer portal: user data changed", body);
                    return "Data was updated successfully.";
                }
                return "No changes. Data was not updated.";
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Error. No user found.") };
        }

        [Route("api/user/activate")]
        [HttpPost]
        public object Activate(int id)
        {
            var currUser = GetCurrentUser();
            if (!(currUser.isAdmin || currUser.isBranchAdmin))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            var code = HttpUtility.UrlEncode(encrypter.Protect(id.ToString()));
            var user = uow.UserRepository.GetByID(id);
            if (user != null)
            {
                var body = $@"To activate your account on C2 customer portal, click on the following link: <br>
						 <a href=""{Utilities.GetSiteUrl()}/setpass/{code}"">Click to go to portal and set up your password.</a>";
                mailHelper.SendMail(Properties.Settings.Default.MailFrom, user.email, "C2 customer portal activation", body);
                return true;
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("User not found in database.") };

        }

        [Route("api/user/checkUser")]
        [HttpPost]
        public object CheckUser(ChangePassword data)
        {
            var decoded = encrypter.UnProtect(data.code.ToString());
            int id;
            if (int.TryParse(decoded, out id))
            {
                var user = uow.UserRepository.GetByID(id);
                if (user != null)
                {

                    return new ChangePassword { code = data.code, username = user.username, ruleText = rule.GetRuleText() };
                }

            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Error checking user's code.") };
        }

        [Route("api/user/updatePassword")]
        [HttpPost]
        public object UpdatePassword(ChangePassword data)
        {
            var decoded = encrypter.UnProtect(data.code.ToString());
            int id;
            if (int.TryParse(decoded, out id))
            {
                if (data.password1 != data.password2)
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Passwords don't match.") };
                string validationError;
                if (!rule.ValidatePassword(data.password1, out validationError))
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(validationError) };
                var user = uow.UserRepository.GetByID(id);
                if (user != null)
                {
                    user.password = data.password1;
                    uow.Save();
                    return true;
                }
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Error checking user's code.") };
        }

		
	    [Route("api/user/resetpassword")]
	    [HttpPost]
	    public object ResetPassword(int id)
	    {
		    var currUser = GetCurrentUser();
		    if (!(currUser.isAdmin || currUser.isBranchAdmin))
			    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
		    var code = HttpUtility.UrlEncode(encrypter.Protect(id.ToString()));
		    var user = uow.UserRepository.GetByID(id);
		    if (user != null)
		    {
			    user.password = null;
				uow.Save();
			    var body = $@"To set up your new password on C2 customer portal, click on the following link: <br>
						 <a href=""{Utilities.GetSiteUrl()}/setpass/{code}"">Click to go to portal and set up your password.</a>";
			    mailHelper.SendMail(Properties.Settings.Default.MailFrom, user.email, "C2 customer portal reset password notification", body);
			    return true;
		    }
		    return new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("User not found in database.") };

	    }

	    [Route("api/user/sendPasswordRecoveryLink")]
	    [HttpPost]
	    public object SendPasswordRecoveryLink(string email)
	    {
		    var user = uow.UserRepository.Get(u => u.email == email).FirstOrDefault();
			if (user != null)
		    {
			    var code = HttpUtility.UrlEncode(encrypter.Protect(user.id.ToString()));
			    var body = $@"To set up your new password on C2 customer portal, click on the following link: <br>
						 <a href=""{Utilities.GetSiteUrl()}/setpass/{code}"">Click to go to portal and set up your password.</a>";
			    mailHelper.SendMail(Properties.Settings.Default.MailFrom, user.email, "C2 customer portal forgotten password notification", body);
			    return true;
		    }
		    return new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("User with given e-mail not found.") };

	    }

	    [Route("api/user/getRolesModel")]
	    [HttpGet]
	    public object GetRolesModel()
	    {
		    var user = GetCurrentUser();
		    if (!user.isAdmin)
			    return Unauthorized();
		    return new
		    {
			    roles = uow.RoleRepository.Get(includeProperties:"Permissions").Select(GetRoleUIObject),
			    permissions = uow.PermissionsRepository.Get()
		    };
	    }

	    [Route("api/user/createRole")]
	    [HttpPost]
	    public object CreateRole(Role r)
	    {
		    var currUser = GetCurrentUser();
		    if (!currUser.isAdmin)
			    return Unauthorized();
	        
		    if(string.IsNullOrEmpty(r.name))
			    return new HttpResponseMessage {StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role name is mandatory")};
			
		    uow.RoleRepository.Insert(r);
		    uow.Save();
		    return GetRoleUIObject(r);
	    }

	    [Route("api/user/updateRole")]
	    [HttpPost]
	    public object UpdateRole(Role r)
	    {
		    var currUser = GetCurrentUser();
		    if (!currUser.isAdmin)
			    return Unauthorized();
	        
		    if(string.IsNullOrEmpty(r.name))
			    return new HttpResponseMessage {StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role name is mandatory")};

			if(r.id == Role.Admin)
				return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content =  new StringContent("Admin role can't be modified")};
		    uow.RoleRepository.Update(r);
		    uow.Save();
		    return GetRoleUIObject(r);
	    }

	    [Route("api/user/deleteRole")]
	    [HttpPost]
	    public object DeleteRole(int id)
	    {
		    var currUser = GetCurrentUser();
		    if (!currUser.isAdmin)
			    return Unauthorized();
			if(id <= 3)
				return new HttpResponseMessage{StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Cannot delete built-in roles")};
			uow.RoleRepository.DeleteByIds(new[] { id });
		    return true;
	    }

	    public object GetRoleUIObject(Role r)
	    {
		    return new
		    {
			    r.id,
			    r.name,
			    permissions = r.Permissions.Select(p => new {p.id, p.name})
		    };
	    }
    }

    public class ChangePassword
	{
		public string code { get; set; }
		public string username { get; set; }
		public string password1 { get; set; }
		public string password2 { get; set; }
		public string ruleText { get; set; }
	}

	public class PasswordRule
	{
		public int MinLength { get; set; }
		public int NumOfUpperCase { get; set; }
		public bool SpecialChars { get; set; }
		public string LastValidationError { get; set; }

		public PasswordRule(int minLength, int numOfUpperCase = 0, bool specialChars = false)
		{
			MinLength = minLength;
			NumOfUpperCase = numOfUpperCase;
			SpecialChars = specialChars;
			
		}

		public bool ValidatePassword(string password, out string errorMessage)
		{
			errorMessage = string.Empty;
			if (password.Length < MinLength)
			{
				errorMessage = $"Password doesn't meet the requirement: Minimal length: {MinLength}.";
				goto exit;
			}				
			if (NumOfUpperCase > 0 && password.Count(c => c >= 'A' && c <= 'Z') < NumOfUpperCase)
			{
				errorMessage = $"Password doesn't meet the requirement: At least {NumOfUpperCase} upper case letter{(NumOfUpperCase > 1 ? "s" : "")}.";
				goto exit;
			}				
			if (SpecialChars && password.All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
				errorMessage = $"Password doesn't meet the requirement: At least one non-alphanumeric character.";
			exit: 
			return errorMessage.Length == 0;
		}

		public string GetRuleText()
		{
			return $@"Password length: min {MinLength} characters. {(NumOfUpperCase > 0 ? $" At least {NumOfUpperCase} upper case letter{(NumOfUpperCase > 1 ? "s." : ".")}" : "")}
					{(SpecialChars ? $". At least 1 non-alphanumeric character" : "")}";
		}
	}
}