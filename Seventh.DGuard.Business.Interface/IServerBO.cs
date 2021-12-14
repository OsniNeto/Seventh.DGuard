using EducSy.DataTransferObject;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System;

namespace Seventh.DGuard.Business.Interface
{
    public interface IServerBO : IBaseBO<Server, ServerDTO_In, ServerDTO_Out, ServerFilterDTO>
    {
        ResultDTO<ServerAvailableDTO_Out> CheckAvailability(Guid id, ServerAvailableDTO_In server);
    }
}