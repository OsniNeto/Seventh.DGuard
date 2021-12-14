using Microsoft.AspNetCore.Mvc;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System;

namespace Seventh.DGuard.Controllers
{
    [ApiController]
    public class ServerController : BaseController<IServerBO, Server, ServerDTO_In, ServerDTO_Out, ServerFilterDTO>
    {
        public ServerController(IServerBO business) : base(business) { }

        /// <summary>
        /// Recuperar um servidor existente
        /// </summary>
        /// <param name="serverId">Id do servidor</param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverId:guid}")]
        public virtual IActionResult Get(Guid serverId) => CreateResponse(_business.Get(serverId));

        /// <summary>
        /// Listar todos os servidores
        /// </summary>
        /// <param name="filtro">Filtros do servidor</param>
        /// <returns></returns>
        [HttpPost]
        [Route("servers")]
        public virtual IActionResult Find(ServerFilterDTO filtro) => CreateResponse(_business.Find(filtro));

        /// <summary>
        /// Checar disponibilidade de um servidor
        /// </summary>
        /// <param name="serverId">Id do servidor</param>
        /// <param name="endereco">Dados do IP de conexão</param>
        /// <returns></returns>
        [HttpPost]
        [Route("servers/available/{serverId:guid}")]
        public virtual IActionResult CheckAvailability(Guid serverId, ServerAvailableDTO_In endereco) => CreateResponse(_business.CheckAvailability(serverId, endereco));

        /// <summary>
        /// Criar um novo servidor
        /// </summary>
        /// <param name="servidor">Servidor que será cadastrado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("server")]
        public virtual IActionResult Add(ServerDTO_In servidor) => CreateResponse(_business.Add(servidor));

        /// <summary>
        /// Atualizar um servidor
        /// </summary>
        /// <param name="serverId">Id do Servidor que será atualizado</param>
        /// <param name="servidor">Servidor que será atualizado</param>
        /// <returns></returns>
        [HttpPut]
        [Route("server/{serverId:guid}")]
        public virtual IActionResult Update(Guid serverId, ServerDTO_In servidor) => CreateResponse(_business.Update(serverId, servidor));

        /// <summary>
        /// Remover um servidor existente
        /// </summary>
        /// <param name="serverId">Servidor que será cadastrado</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("servers/{serverId:guid}")]
        public virtual IActionResult Delete(Guid serverId) => CreateResponse(_business.Delete(serverId));

    }
}