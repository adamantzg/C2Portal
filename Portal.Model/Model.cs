namespace Portal.Model
{
	using System;
	using System.Data.Entity;
	using System.Linq;

	
	public class Model : DbContext
	{
		// Your context has been configured to use a 'Model' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'Portal.Models.Model' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'Model' 
		// connection string in the application configuration file.
		public Model()
			: base("name=Model")
		{
		}

		public Model(string connString) : base(connString)
		{

		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		// public virtual DbSet<MyEntity> MyEntities { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().ToTable("Product");
			modelBuilder.Entity<Customer>().ToTable("Customer");
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Holiday>().ToTable("Holiday");
			modelBuilder.Entity<UserSession>().ToTable("user_session");
			modelBuilder.Entity<UserPermission>().ToTable("user_permission").HasKey(up=> new {up.user_id, up.permission_id});
			modelBuilder.Entity<CustomerPermission>().ToTable("customer_permission").HasKey(cp=> new {cp.customer_code, cp.permission_id});

			modelBuilder.Entity<User>().HasOptional(u => u.Customer).WithMany().HasForeignKey(u => u.customer_code);
			modelBuilder.Entity<User>().HasMany(u => u.Roles).WithMany().Map(m => m.ToTable("user_role").MapLeftKey("user_id").MapRightKey("role_id"));
			modelBuilder.Entity<User>().HasMany(u => u.Sessions).WithOptional().HasForeignKey(s => s.user_id);

			modelBuilder.Entity<Role>().HasMany(r => r.Permissions).WithMany().Map(m =>
				m.ToTable("role_permission").MapLeftKey("role_id").MapRightKey("permission_id"));

			//modelBuilder.Entity<User>().Property(u => u.customer_code).HasColumnName("customer");
			base.OnModelCreating(modelBuilder);
		}
	}

	//public class MyEntity
	//{
	//    public int Id { get; set; }
	//    public string Name { get; set; }
	//}
}