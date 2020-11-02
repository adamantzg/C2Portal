using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace Portal.Model
{
	public class UnitOfWork : IDisposable, IUnitOfWork
	{
		private DbContext context = null;

		private static Model CreateModel()
		{
			var model = new Model();
			model.Configuration.ProxyCreationEnabled = false;
			model.Configuration.LazyLoadingEnabled = false;
			model.Database.CommandTimeout = 300;
			return model;
		}

		[ExcludeFromCodeCoverage]
		public void Dispose()
		{
			((IDisposable)context).Dispose();
		}

		public UnitOfWork()
		{
			context = CreateModel();
		}

		public UnitOfWork(DbContext context)
		{
			this.context = context;
			context.Configuration.ProxyCreationEnabled = false;
			context.Configuration.LazyLoadingEnabled = false;
			context.Database.CommandTimeout = 300;
		}

		public void Save()
		{
			context.SaveChanges();
		}

		private GenericRepository<Product> productRepository;
		private GenericRepository<Customer> customerRepository;
		private UserRepository userRepository;
		private RoleRepository roleRepository;
        private GenericRepository<Holiday> holidayRepository;
		private GenericRepository<Permission> permissionRepository;

		public IGenericRepository<Product> ProductRepository { get {
				if (productRepository == null)
					productRepository = new GenericRepository<Product>(context);
				return productRepository;
			}
		}
		public IGenericRepository<Customer> CustomerRepository { get {
				if (customerRepository == null)
					customerRepository = new GenericRepository<Customer>(context);
				return customerRepository;
			}
		}

		public IUserRepository UserRepository
		{
			get
			{
				if (userRepository == null)
					userRepository = new UserRepository(context);
				return userRepository;
			}
		}

		public IGenericRepository<Role> RoleRepository
		{
			get
			{
				if (roleRepository == null)
					roleRepository = new RoleRepository(context);
				return roleRepository;
			}
		}

        public IGenericRepository<Holiday> HolidayRepository
        {
            get
            {
                if (holidayRepository == null)
                    holidayRepository = new GenericRepository<Holiday>(context);
                return holidayRepository;
            }
        }

		public IGenericRepository<Permission> PermissionsRepository {
			get
			{
				if (permissionRepository == null)
					permissionRepository = new GenericRepository<Permission>(context);
				return permissionRepository;
			}
		}
	}
}