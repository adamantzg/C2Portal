using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Portal.Model;

namespace PortalTest
{
	public class GenericRepositoryMock<TEntity>: IGenericRepository<TEntity> where TEntity : class
	{
		private IList<TEntity> _data;

		public GenericRepositoryMock(IList<TEntity> data)
		{
			this._data = data;
		}

		public void SetData(List<TEntity> data)
		{
			this._data = data;
		}

		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? take = null, int? skip = null,
			string includeProperties = "")
		{
			return GetQuery(filter, orderBy, take, skip, includeProperties).ToList();
		}

		public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? take = null, int? skip = null,
			string includeProperties = "")
		{
			IQueryable<TEntity> query = _data.AsQueryable();

			if (filter != null) {
				query = query.Where(filter);
			}

			/*foreach (var includeProperty in includeProperties.Split
				(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
				query = query.Include(includeProperty);
			}*/

			if (orderBy != null) {
				if (take == null) {
					if (skip == null)
						return orderBy(query);
					return orderBy(query).Skip(skip.Value);
				}

				if (skip == null)
					return orderBy(query).Take(take.Value);
				return orderBy(query).Skip(skip.Value).Take(take.Value);
			}

			return query;
		}



		public virtual TEntity GetByID(object id)
		{
			return GetByKey(id);
		}

		public void Insert(TEntity entity)
		{
			_data.Add(entity);
		}

		public void BulkInsert(IList<TEntity> records)
		{
			throw new NotImplementedException();
		}

		public void Delete(object id)
		{
			var entity = GetByKey(id);
			if(entity != null)
				Delete(entity);
		}

		public void Delete(TEntity entityToDelete)
		{
			_data.Remove(entityToDelete);
		}

		public void Update(TEntity entityToUpdate)
		{
			var existing = FindEntity(entityToUpdate);
			if (existing != null)
			{
				existing = entityToUpdate;
			}
		}

		public void LoadCollection(TEntity entity, string collName)
		{
			throw new NotImplementedException();
		}

		public void LoadReference(TEntity entity, string refName)
		{
			throw new NotImplementedException();
		}

		public void Detach(TEntity entity)
		{
			throw new NotImplementedException();
		}

		public void Copy(TEntity source, TEntity destination)
		{
			var properties = typeof(TEntity).GetProperties();
			string[] propertyTypes = {"string", "int", "long", "double", "datetime", "decimal"};
			foreach (var prop in properties)
			{
				if(prop.CanWrite && propertyTypes.Contains(prop.PropertyType.Name.ToLower()))
					prop.SetValue(destination, prop.GetValue(source));
			}
		}

		public void DeleteAll()
		{
			throw new NotImplementedException();
		}

		public void DeleteByIds(IEnumerable<int> ids, bool exclusion = false)
		{
			foreach (var id in ids)
			{
				Delete(id);
			}
		}

		private TEntity GetByKey(object id)
		{
			var keyProperty = GetKeyProperty();
			if (keyProperty != null)
			{
				ParameterExpression pe = Expression.Parameter(typeof(TEntity), "e");
				MemberExpression me = Expression.Property(pe, keyProperty);
				ConstantExpression ce = Expression.Constant(id);
				BinaryExpression be = Expression.Equal(me, ce);
				var lambda = Expression.Lambda<Func<TEntity, bool>>(be, new[] {pe});

				return _data.FirstOrDefault(lambda.Compile());

			}

			return null;
		}

		private PropertyInfo GetKeyProperty()
		{
			return typeof(TEntity).GetProperties()
				       .FirstOrDefault(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute))) ??
			       typeof(TEntity).GetProperties().FirstOrDefault(p => p.Name == "id");

		}

		private TEntity FindEntity(TEntity entity)
		{
			var keyProperty = GetKeyProperty();
			if (keyProperty != null)
			{
				var keyValue = keyProperty.GetValue(entity);
				return GetByKey(keyValue);
			}

			return null;
		}
	}
}
