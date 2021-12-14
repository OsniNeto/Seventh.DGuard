using AutoMapper;
using EducSy.DataTransferObject;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using Seventh.DGuard.Repository.Interface;
using System;

namespace Seventh.DGuard.Business
{
    public class ServerBO : BaseBO<Server, ServerDTO_In, ServerDTO_Out, ServerFilterDTO, IServerRepository>, IServerBO
    {
        public ServerBO(IServerRepository repository, IMapper mapper) : base(repository, mapper) { }

        public override ResultDTO<ServerDTO_Out> Validate_Add(ServerDTO_In model)
        {
            var baseResponse = base.Validate_Add(model);
            if (!baseResponse.Success)
                return baseResponse;

            return ResultFactory.GenerateResponse<ServerDTO_Out>(true);
        }

        public virtual ResultDTO<ServerAvailableDTO_Out> CheckAvailability(Guid id, ServerAvailableDTO_In endereco)
        {
            try
            {
                var model = ModelOutFromEntity(_repository.Get(id));

                return ResultFactory.GenerateResponse<ServerAvailableDTO_Out>(
                    new ServerAvailableDTO_Out
                    {
                        Available = endereco.Ip == model.Ip && endereco.Port == endereco.Port
                    });
            }
            catch (Exception ex)
            {
                return ResultFactory.GenerateResponse<ServerAvailableDTO_Out>(ex);
            }
        }

    }
}
