using Microsoft.AspNetCore.Mvc;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;

namespace Seventh.DGuard.Controllers
{
    [ApiController]
    public class RecyclerStatusController : BaseController<IRecyclerStatusBO, RecyclerStatus, RecyclerStatusDTO_In, RecyclerStatusDTO_Out, RecyclerStatusFilterDTO>
    {
        public RecyclerStatusController(IRecyclerStatusBO business) : base(business) { }

        /// <summary>
        /// Reciclar vídeos antigos
        /// </summary>
        /// <param name="days">Número de dias para limpeza dos videos</param>
        /// <returns></returns>
        [HttpPost]
        [Route("recycler/process/{days:int}")]
        public virtual IActionResult Process(int days)
        {
            _business.Process(days);
            return CreateAcceptedResponse();
        }

        /// <summary>
        /// Reciclagem (status de processamento)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("recycler/process/status")]
        public virtual IActionResult GetStatus() => CreateResponse(_business.GetStatus());
    }
}