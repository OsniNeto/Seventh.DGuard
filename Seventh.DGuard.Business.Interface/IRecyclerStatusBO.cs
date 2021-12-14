using EducSy.DataTransferObject;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System;

namespace Seventh.DGuard.Business.Interface
{
    public interface IRecyclerStatusBO : IBaseBO<RecyclerStatus, RecyclerStatusDTO_In, RecyclerStatusDTO_Out, RecyclerStatusFilterDTO>
    {
        void Process(int days);
        ResultDTO<RecyclerReportDTO> GetStatus();
    }
}