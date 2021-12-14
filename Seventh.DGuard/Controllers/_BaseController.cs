using EducSy.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System.Collections.Generic;

namespace Seventh.DGuard.Controllers
{
    [ApiController]
    [Route("api")]
    public class BaseController<TBusiness, TEntity, TModelIn, TModelOut, TFilter> : Controller
        where TEntity : BaseEntity
        where TModelIn : BaseDTO
        where TModelOut : BaseDTO
        where TFilter : BaseFilterDTO
        where TBusiness : IBaseBO<TEntity, TModelIn, TModelOut, TFilter> 
    {
        protected readonly TBusiness _business;

        protected BaseController(TBusiness business)
        {
            _business = business;
        }

        protected IActionResult CreateResponse(ResultDTO result = null)
        {
            if (result is null)
                return Ok();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        protected IActionResult CreateAcceptedResponse(ResultDTO result = null)
        {
            if (result is null)
                return Accepted();

            if (!result.Success)
                return BadRequest(result);

            return Accepted(result);
        }

        protected IActionResult CreateResponse<TModel>(ResultDTO<TModel> result = null) where TModel : class
        {
            if (result is null)
                return Ok();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        protected IActionResult CreateResponse<TModel>(ResultListDTO<TModel> result = null)
        {
            if (result is null)
                return Ok();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
