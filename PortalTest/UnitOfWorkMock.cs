using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Model;

namespace PortalTest
{
	public class UnitOfWorkMock : IUnitOfWork
	{
		public bool Saved { get; set; }
		private MockData data;
		public MockData  Data 
		{
			get => data;
			set
			{
				data = value;
				productRepository?.SetData(value.Products);
				customerRepository?.SetData(value.Customers);
				userRepository?.SetData(value.Users);
				roleRepository?.SetData(value.Roles);
				holidayRepository?.SetData(value.Holidays);
				data.MockDataChanged += args =>
				{
					switch (args.Kind)
					{
						case MockDataKind.Customers:
							customerRepository?.SetData(args.Data as List<Customer>);
							break;
						case MockDataKind.Products:
							productRepository?.SetData(args.Data as List<Product>);
							break;
						case MockDataKind.Users:
							userRepository?.SetData(args.Data as List<User>);
							break;
						case MockDataKind.Roles:
							roleRepository?.SetData(args.Data as List<Role>);
							break;
						case MockDataKind.Holidays:
							holidayRepository?.SetData(args.Data as List<Holiday>);
							break;
						case MockDataKind.Permissions:
							permissionRepository?.SetData(args.Data as List<Permission>);
							break;
						default:
							break;
					}
				};
			}
		}

		public void Dispose()
		{
			
		}

		public UnitOfWorkMock()
		{

		}

		public UnitOfWorkMock(MockData data)
		{
			this.Data = data;
		}

		
		public void Save()
		{
			Saved = true;
		}

		private GenericRepositoryMock<Product> productRepository;
		private GenericRepositoryMock<Customer> customerRepository;
		private UserRepositoryMock userRepository;
		private GenericRepositoryMock<Role> roleRepository;
		private GenericRepositoryMock<Holiday> holidayRepository;
		private GenericRepositoryMock<Permission> permissionRepository;

		public IGenericRepository<Product> ProductRepository
		{
			get
			{
				if(productRepository == null)
					productRepository = new GenericRepositoryMock<Product>(Data.Products);
				return productRepository;
			}
		}

		public IGenericRepository<Customer> CustomerRepository
		{
			get
			{
				if(customerRepository == null)
					customerRepository = new GenericRepositoryMock<Customer>(Data.Customers);
				return customerRepository;
			}
		}

		public IUserRepository UserRepository
		{
			get
			{
				if(userRepository == null)
					userRepository = new UserRepositoryMock(Data.Users);
				return userRepository;
			}
		}

		public IGenericRepository<Role> RoleRepository
		{
			get
			{
				if(roleRepository == null)
					roleRepository = new GenericRepositoryMock<Role>(Data.Roles);
				return roleRepository;
			}
		}

		public IGenericRepository<Holiday> HolidayRepository
		{
			get
			{
				if(holidayRepository == null)
					holidayRepository = new GenericRepositoryMock<Holiday>(Data.Holidays);
				return holidayRepository;
			}
		}

		public IGenericRepository<Permission> PermissionsRepository {
			get
			{
				if(permissionRepository == null)
					permissionRepository = new GenericRepositoryMock<Permission>(Data.Permissions);
				return permissionRepository;
			}
		}
	}

	public class MockData
	{
		private List<Customer> customers;
		private List<User> users;
		private List<Role> roles;
		private List<Holiday> holidays;
		private List<Product> products;
		private List<Permission> permissions;

		public delegate void MockDataChangedEventHandler(MockDataChangedEventArgs args);

		public MockDataChangedEventHandler MockDataChanged;

		public List<Customer> Customers
		{
			get { return customers; }
			set
			{
				customers = value;
				RaiseDataChanged(MockDataKind.Customers, value);
			}
		}

		public List<User> Users
		{
			get { return users; }
			set
			{
				users = value;
				RaiseDataChanged(MockDataKind.Users, value);
			}
		}

		public List<Role> Roles
		{
			get { return roles; }
			set
			{
				roles = value;
				RaiseDataChanged(MockDataKind.Roles, value);
			}
		}

		public List<Holiday> Holidays
		{
			get { return holidays; }
			set
			{
				holidays = value;
				RaiseDataChanged(MockDataKind.Holidays, value);
			}
		}

		public List<Product> Products
		{
			get { return products; }
			set
			{
				products = value;
				RaiseDataChanged(MockDataKind.Products, value);
			}
		}

		public List<Permission> Permissions
		{
			get { return permissions; }
			set
			{
				permissions = value;
				RaiseDataChanged(MockDataKind.Permissions, value);
			}
		}

		private void RaiseDataChanged(MockDataKind kind, object data)
		{
			MockDataChanged?.Invoke(new MockDataChangedEventArgs { Data = data, Kind = kind });

		}
	}

	public enum MockDataKind
	{
		Customers,
		Users,
		Roles,
		Holidays,
		Products,
		Permissions
	}

	public class MockDataChangedEventArgs
	{
		public MockDataKind Kind { get; set; }
		public object Data { get; set; }
	}
	
}
