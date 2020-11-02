namespace Portal.Model
{
	public interface IUnitOfWork
	{
		void Dispose();
		void Save();
		IGenericRepository<Product> ProductRepository { get; }
		IGenericRepository<Customer> CustomerRepository { get; }
		IUserRepository UserRepository { get; }
		IGenericRepository<Role> RoleRepository { get; }
		IGenericRepository<Holiday> HolidayRepository { get; }
		IGenericRepository<Permission> PermissionsRepository { get; }
	}
}