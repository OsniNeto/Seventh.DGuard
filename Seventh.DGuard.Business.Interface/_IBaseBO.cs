using EducSy.DataTransferObject;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System;

namespace Seventh.DGuard.Business.Interface
{
    public interface IBaseBO<TEntity, TModelIn, TModelOut, TFilter>
        where TEntity : BaseEntity
        where TModelIn : BaseDTO
        where TModelOut : BaseDTO
        where TFilter : BaseFilterDTO
    {
        ResultDTO<TModelOut> Add(TModelIn t);
        ResultDTO<TModelOut> Update(Guid id, TModelIn updated);

        ResultDTO<bool> Delete(Guid id);

        ResultDTO<long> Count();
        ResultDTO<TModelOut> Get(Guid id);
        ResultListDTO<TModelOut> Find(TFilter param);

        ResultDTO<TModelOut> Validate_Get(Guid id);
        ResultDTO<TModelOut> Validate_Add(TModelIn model);
        ResultDTO<TModelOut> Validate_Update(Guid id, TModelIn model);
        ResultDTO<TModelOut> Validate_Delete(Guid id);
    }
}
