using Microsoft.AspNetCore.Mvc;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System;

namespace Seventh.DGuard.Controllers
{
    [ApiController]
    public class VideoController : BaseController<IVideoBO, Video, VideoDTO_In, VideoDTO_Out, VideoFilterDTO>
    {
        public VideoController(IVideoBO business) : base(business) { }

        /// <summary>
        /// Recuperar dados cadastrais de um vídeo
        /// </summary>
        /// <param name="serverId">Id do servidor</param>
        /// <param name="videoId">Id do vídeo</param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverId:guid}/videos/{videoId:guid}")]
        public virtual IActionResult Get(Guid serverId, Guid videoId) => CreateResponse(_business.Get(videoId));

        /// <summary>
        /// Listar todos os vídeos de um servidor
        /// </summary>
        /// <param name="serverId">Id do servidor</param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverId:guid}/videos")]
        public virtual IActionResult FindByServer(Guid serverId) => CreateResponse(_business.FindByServer(serverId));

        /// <summary>
        /// Adicionar um novo vídeo à um servidor
        /// </summary>
        /// <param name="serverId">Id do servidor</param>
        /// <param name="video">Video que será adicionado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("servers/{serverId:guid}/videos")]
        public virtual IActionResult Add(Guid serverId, VideoDTO_In video) => CreateResponse(_business.Add(serverId, video));

        /// <summary>
        /// Download do conteúdo binário de um vídeo
        /// </summary>
        /// <param name="serverId">Id do servidor</param>
        /// <param name="videoId">Id do Video que será baixado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("servers/{serverId:guid}/videos/{videoId:guid}/binary")]
        public virtual IActionResult Download(Guid serverId, Guid videoId)
        {
            var file = _business.Download(videoId);
            return File(file.FileBytes, "application/octet-stream", file.FileName);
        }

        /// <summary>
        /// Remover um vídeo existente
        /// </summary>
        /// <param name="serverId">Id do Server que terá o vídeo deletado</param>
        /// <param name="videoId">Id do Video que será deletado</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("servers/{serverId:guid}/videos/{videoId:guid}")]
        public virtual IActionResult Delete(Guid serverId, Guid videoId) => CreateResponse(_business.Delete(videoId));
    }
}