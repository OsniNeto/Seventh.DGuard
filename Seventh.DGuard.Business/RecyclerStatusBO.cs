using AutoMapper;
using EducSy.DataTransferObject;
using Microsoft.Extensions.Configuration;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using Seventh.DGuard.Repository.Interface;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Seventh.DGuard.Business
{
    public class RecyclerStatusBO : BaseBO<RecyclerStatus, RecyclerStatusDTO_In, RecyclerStatusDTO_Out, RecyclerStatusFilterDTO, IRecyclerStatusRepository>, IRecyclerStatusBO
    {
        protected readonly IVideoBO _videoBO;
        protected readonly IConfiguration _configuration;

        public RecyclerStatusBO(IRecyclerStatusRepository repository, IMapper mapper, IVideoBO videoBO, IConfiguration configuration) : base(repository, mapper)
        {
            _videoBO = videoBO;
            _configuration = configuration;
        }

        public override ResultDTO<RecyclerStatusDTO_Out> Validate_Add(RecyclerStatusDTO_In model)
        {
            var baseResponse = base.Validate_Add(model);
            if (!baseResponse.Success)
                return baseResponse;

            return ResultFactory.GenerateResponse<RecyclerStatusDTO_Out>(true);
        }

        public void Process(int days)
        {
            Task.Run(() => DoRecycle(days));
        }

        private void DoRecycle(int days)
        {
            var connectionString = _configuration.GetConnectionString("cs_seventh_dguard");

            try
            {
                using var context = new SeventhDGuardContext(connectionString);

                context.RecyclerStatus.Add(new RecyclerStatus { StartDate = DateTime.Now, Days = days });

                var minDate = DateTime.Now.AddDays(-days);
                var videosToRemove = context.Video.Where(x => x.AddDate < minDate).ToList();

                foreach (var video in videosToRemove)
                {
                    var resultRemove = context.Video.Remove(video);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", video.Filename);
                    File.Delete(filePath);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: salvar um log de erro.
            }
            finally
            {
                using var context = new SeventhDGuardContext(connectionString);
                var recyclerRunning = context.RecyclerStatus.Where(x => !x.EndDate.HasValue).ToList();
                foreach (var recycler in recyclerRunning)
                    recycler.EndDate = DateTime.Now;
                context.SaveChanges();
            }
        }

        public ResultDTO<RecyclerReportDTO> GetStatus()
        {
            var lastStatus = _repository.Exists(x => !x.EndDate.HasValue);
            return ResultFactory.GenerateResponse(new RecyclerReportDTO { Status = lastStatus ? "running" : "not running" });
        }
    }
}
