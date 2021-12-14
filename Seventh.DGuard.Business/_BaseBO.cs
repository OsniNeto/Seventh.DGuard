using AutoMapper;
using EducSy.DataTransferObject;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Core;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using Seventh.DGuard.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Seventh.DGuard.Business
{
    public class BaseBO<TEntity, TModelIn, TModelOut, TFilter, TRepo> : IBaseBO<TEntity, TModelIn, TModelOut, TFilter>
        where TEntity : BaseEntity
        where TModelIn : BaseDTO
        where TModelOut : BaseDTO
        where TFilter : BaseFilterDTO
        where TRepo : IBaseRepository<TEntity>
    {
        protected readonly TRepo _repository;
        protected readonly IMapper _mapper;

        protected BaseBO(TRepo repository, IMapper mapper)
        {
            _repository = repository; ;
            _mapper = mapper;
        }

        #region Mapper

        internal TModelOut ModelOutFromEntity(TEntity t)
        {
            return t == null ? null : _mapper.Map<TModelOut>(t);
        }

        internal IEnumerable<TModelOut> ModelOutFromEntity(IEnumerable<TEntity> entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TModelOut>>(entities);
        }

        internal TModelIn ModelInFromEntity(TEntity t)
        {
            return t == null ? null : _mapper.Map<TModelIn>(t);
        }

        internal IEnumerable<TModelIn> ModelInFromEntity(IEnumerable<TEntity> entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TModelIn>>(entities);
        }

        internal TModelOut ModelOutFromModelIn(TModelIn t)
        {
            return t == null ? null : _mapper.Map<TModelOut>(t);
        }

        internal IEnumerable<TModelOut> ModelOutFromModelIn(IEnumerable<TModelIn> entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TModelOut>>(entities);
        }

        internal TEntity EntityFromModel(TModelIn t)
        {
            return t == null ? null : _mapper.Map<TEntity>(t);
        }

        internal IEnumerable<TEntity> EntityFromModel(IEnumerable<TModelIn> entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TEntity>>(entities);
        }

        #endregion

        public virtual ResultDTO<TModelOut> Add(TModelIn t)
        {
            try
            {
                Normalize(t);

                var valid = Validate_Add(t);
                if (!valid.Success)
                    return valid;

                var entity = EntityFromModel(t);

                var result = _repository.Add(entity);
                return ResultFactory.GenerateResponse(ModelOutFromEntity(result));
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponse<TModelOut>(ex);
            }
        }

        public virtual ResultDTO<TModelOut> Update(Guid id, TModelIn updated)
        {
            try
            {
                Normalize(updated);

                var valid = Validate_Update(id, updated);
                if (!valid.Success)
                    return valid;

                var result = Get(id);
                if (!result.Success)
                    return result;

                updated.Id = id;

                var data = _repository.Update(EntityFromModel(updated));
                return ResultFactory.GenerateResponse(ModelOutFromEntity(data));
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponse<TModelOut>(ex);
            }
        }

        public virtual ResultDTO<bool> Delete(Guid id)
        {
            try
            {
                var valid = Validate_Delete(id);
                if (!valid.Success)
                    return ResultFactory.GenerateResponse(false, valid.Message);

                var result = Get(id);

                if (!result.Success || result.Return is null)
                    return ResultFactory.GenerateResponse(false, "Entity not found.");

                _repository.Delete(id);
                return ResultFactory.GenerateResponse(true);
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponse<bool>(ex);
            }
        }

        public virtual ResultDTO<TModelOut> Get(Guid id)
        {
            try
            {
                var valid = Validate_Get(id);
                if (!valid.Success)
                    return valid;

                var model = ModelOutFromEntity(_repository.Get(id));

                return ResultFactory.GenerateResponse(model);
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponse<TModelOut>(ex);
            }
        }

        public virtual ResultListDTO<TModelOut> Find(TFilter filtro)
        {
            try
            {
                filtro.Pagination.NormalizeProperties();

                if (!string.IsNullOrEmpty(filtro.Sort.Column) && !filtro.GetType().GetProperties().Any(x => x.Name.ToUpper() == filtro.Sort.Column.ToUpper()))
                    throw new Exception($"The cloumn {filtro.Sort.Column} is not valid.");

                var finalFilterExpression = DbExtensions.CreateFilterExpression<TEntity, TFilter>(filtro);

                var result = _repository.Find(finalFilterExpression, filtro.Sort.Asc, filtro.Sort.Column, filtro.Pagination.Page, filtro.Pagination.ItemsNumber);
                var resultModel = ModelOutFromEntity(result);

                var totalItens = _repository.Count(finalFilterExpression);

                var relatorio = new ReportResultListDTO { ItemsPerPage = filtro.Pagination.ItemsNumber, CurrentPage = filtro.Pagination.Page, TotalItems = totalItens };

                return ResultFactory.GenerateResponseList(resultModel, relatorio);
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponseList<TModelOut>(ex);
            }
        }

        public virtual ResultDTO<long> Count()
        {
            try
            {
                var result = _repository.Count();
                return ResultFactory.GenerateResponse(result);
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponse<long>(ex);
            }
        }

        public virtual ResultDTO<TModelOut> Validate_Get(Guid id)
        {
            if (id == Guid.Empty)
                return ResultFactory.GenerateResponse<TModelOut>("Id is empty.");

            return ResultFactory.GenerateResponse<TModelOut>(true);
        }

        public virtual ResultDTO<TModelOut> Validate_Add(TModelIn model)
        {
            if (model.Id != Guid.Empty)
                return ResultFactory.GenerateResponse<TModelOut>("Error in field validation.", new List<string> { "Id must be empty." });

            return ResultFactory.GenerateResponse<TModelOut>(true);
        }

        public virtual ResultDTO<TModelOut> Validate_Update(Guid id, TModelIn model)
        {
            if (model == null)
                return ResultFactory.GenerateResponse<TModelOut>("Object is empty.");
            if (id == Guid.Empty)
                return ResultFactory.GenerateResponse<TModelOut>("Id is empty.");

            return ResultFactory.GenerateResponse<TModelOut>(true);
        }

        public virtual ResultDTO<TModelOut> Validate_Delete(Guid id)
        {
            if (id == Guid.Empty)
                return ResultFactory.GenerateResponse<TModelOut>("Id is empty.");

            return ResultFactory.GenerateResponse<TModelOut>(true);
        }

        public virtual void Normalize(TModelIn model)
        {
            var stringProperties = model.GetType().GetProperties().Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                var currentValue = (string)stringProperty.GetValue(model, null);
                stringProperty.SetValue(model, currentValue?.Trim(), null);
            }
        }
    }
}
