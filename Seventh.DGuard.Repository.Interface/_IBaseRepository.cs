using Seventh.DGuard.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Seventh.DGuard.Repository.Interface
{
    public interface IBaseRepository<TEntity>
         where TEntity : BaseEntity
    {
        void Detach(TEntity entity);

        #region Add

        /// <summary>
        /// Inserts a single object to the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="t">The object to insert</param>
        /// <returns>The resulting object including its primary key after the insert</returns>
        TEntity Add(TEntity t, bool save = true);

        /// <summary>
        /// Inserts a collection of objects into the database and commits the changes
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="tList">An IEnumerable list of objects to insert</param>
        /// <returns>The IEnumerable resulting list of inserted objects including the primary keys</returns>
        IEnumerable<TEntity> Add(IEnumerable<TEntity> tList, bool save = true);

        #endregion Add

        #region Update

        /// <summary>
        /// Create or update a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="t">Object to create or update</param>
        /// <returns>The resulting updated object</returns>
        TEntity Update(TEntity tsave, bool save = true);

        #endregion Update

        #region Delete

        /// <summary>
        /// Deletes a single object from the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="id">The primary key of the object to remove</param>
        void Delete(Guid id, bool save = true);

        /// <summary>
        /// Deletes a single object from the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="t">The object to delete</param>
        void Delete(TEntity t, bool save = true);

        #endregion

        #region Save

        /// <summary>
        /// Save changes in the database
        /// </summary>
        /// <returns>The number of state entries written to the underlying database. This can include</returns>
        int Save();

        #endregion Save

        #region Get

        TEntity Get(Guid id);

        TEntity GetSingle(Expression<Func<TEntity, bool>> match);

        /// <summary>
        /// Gets a collection of all objects in the database
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>An IEnumerable of every object in the database</returns>
        IEnumerable<TEntity> Find(int page = 1, int qty = int.MaxValue);

        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An IEnumerable of object which match the expression filter</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> match, bool asc = true, string coluna = null, int page = 1, int qty = int.MaxValue);

        /// <summary>
        /// Gets the count of the number of objects in the databse
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The count of the number of objects</returns>
        long Count();

        /// <summary>
        /// Gets the count of the number of objects in the databse
        /// </summary>
        /// <param name="predicate">Lambda expression</param>
        /// <returns>Total of objects</returns>
        long Count(Expression<Func<TEntity, bool>> predicate);

        bool Exists(Expression<Func<TEntity, bool>> predicate);

        #endregion Get

        IQueryable<TEntity> GetQueryable();
    }
}
