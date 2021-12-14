using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Seventh.DGuard.Core;
using Seventh.DGuard.Database;
using Seventh.DGuard.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Seventh.DGuard.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
            where TEntity : BaseEntity
    {
        protected readonly SeventhDGuardContext _context;
        protected readonly ILogger _logger;

        public BaseRepository(SeventhDGuardContext context, ILogger<TEntity> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Inserir

        public virtual TEntity Add(TEntity t, bool save = true)
        {
            CreateId(t);

            var data = _context.Set<TEntity>().Add(t);

            if (save)
                Save();

            return data.Entity;
        }

        public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> tList, bool save = true)
        {
            foreach (var item in tList)
                CreateId(item);

            _context.Set<TEntity>().AddRange(tList);

            if (save)
                Save();

            return tList;
        }

        protected void CreateId(TEntity t)
        {
            if (t.Id == Guid.Empty)
                t.Id = Guid.NewGuid();
        }

        protected void CreateIdSubItems<TSubEntity>(IEnumerable<TSubEntity> items) where TSubEntity : BaseEntity
        {
            foreach (var item in items)
                if (item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();
        }

        #endregion Add

        #region Atualizar

        public virtual TEntity Update(TEntity t, bool save = true)
        {
            var result = _context.Set<TEntity>().Update(t);

            if (save)
                Save();

            return result.Entity;
        }

        #endregion

        #region Deletar

        public virtual void Delete(Guid id, bool save = true)
        {
            var entity = Get(id);
            Delete(entity, save);
        }

        public virtual void Delete(TEntity t, bool save = true)
        {
            _context.Entry(t).State = EntityState.Detached;
            _context.Set<TEntity>().Remove(t);

            if (save)
                Save();
        }

        protected void Deletar<TSubEntity>(IEnumerable<TSubEntity> items) where TSubEntity : BaseEntity
        {
            foreach (var item in items)
            {
                _context.Entry(item).State = EntityState.Detached;
                _context.Set<TSubEntity>().Remove(item);
            }
        }

        #endregion Delete

        #region Salvar

        public virtual int Save()
        {
            var result = _context.SaveChanges();
            if (result == 0)
                _logger.LogInformation("No updates have been written to the database.");
            return result;
        }

        #endregion

        #region Obter        

        public virtual TEntity Get(Guid id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            Detach(entity);
            return entity;
        }

        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> match)
        {
            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(match);
        }

        public virtual IEnumerable<TEntity> Find(int page = 1, int qty = int.MaxValue)
        {
            var pageIndex = page - 1;
            if (pageIndex > 0)
                return _context.Set<TEntity>().AsNoTracking().Skip(pageIndex * qty).Take(qty).ToList();
            else
                return _context.Set<TEntity>().AsNoTracking().Take(qty).ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> match, bool asc = true, string coluna = null, int page = 1, int qty = int.MaxValue)
        {
            var pageIndex = page - 1;
            var expression = _context.Set<TEntity>().AsNoTracking();

            if (match != null)
                expression = expression.Where(match);

            if (!string.IsNullOrEmpty(coluna))
                if (asc)
                    expression = expression.OrderByProperty(coluna);
                else
                    expression = expression.OrderByPropertyDescending(coluna);

            if (pageIndex > 0)
                expression = expression.Skip(pageIndex * qty).Take(qty);
            else
                expression = expression.Take(qty);

            return expression.ToList();
        }

        public virtual long Count()
        {
            return _context.Set<TEntity>().LongCount();
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
                return _context.Set<TEntity>().LongCount(predicate);
            return _context.Set<TEntity>().LongCount();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Any(predicate);
        }

        //protected IQueryable<TFilter> FilterApply<TFilter>(OrdenacaoDTO filtro, IQueryable<TFilter> expression)
        //{
        //    var pageIndex = filtro.Pagina - 1;

        //    if (filtro.Ordenacao != null)
        //    {
        //        if (filtro.Ordenacao.Asc)
        //            expression = expression.OrderByProperty(filtro.Ordenacao.Coluna);
        //        else
        //            expression = expression.OrderByPropertyDescending(filtro.Ordenacao.Coluna);
        //    }

        //    if (filtro.NumeroItens == 0)
        //        filtro.NumeroItens = int.MaxValue;

        //    if (pageIndex > 0)
        //        expression = expression.Skip(pageIndex * filtro.NumeroItens).Take(filtro.NumeroItens);
        //    else
        //        expression = expression.Take(filtro.NumeroItens);
        //    return expression;
        //}

        #endregion Get

        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>().AsNoTracking().AsQueryable();
        }

        public void Detach(TEntity entity)
        {
            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
