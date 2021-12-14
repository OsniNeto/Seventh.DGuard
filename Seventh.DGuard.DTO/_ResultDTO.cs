using System;
using System.Collections.Generic;
using System.Linq;

namespace EducSy.DataTransferObject
{
    public class ResultDTO
    {
        public ResultDTO(bool success, string message, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Errors = errors;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ResultDTO<T> : ResultDTO
    {
        public ResultDTO(bool success, string message, List<string> errors = null) : base(success, message, errors) { }
        public ResultDTO(bool success, string message, T retorno, List<string> errors = null) : base(success, message, errors)
        {
            Return = retorno;
        }

        public T Return { get; set; }
    }

    public class ResultListDTO<T> : ResultDTO
    {
        public ResultListDTO(bool success, string message, List<string> errors = null) : base(success, message, errors) { }
        public ResultListDTO(bool success, string message, IEnumerable<T> list, ReportResultListDTO report, List<string> errors = null) : base(success, message, errors)
        {
            Return = list ?? Enumerable.Empty<T>();
            Report = report;
        }

        public IEnumerable<T> Return { get; set; }

        public ReportResultListDTO Report { get; set; }
    }

    public class ReportResultListDTO
    {
        public long TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public long TotalPages
        {
            get
            {
                if (ItemsPerPage > 0)
                    return (TotalItems + ItemsPerPage - 1) / ItemsPerPage;
                return TotalItems;
            }
        }
    }

    public static class ResultFactory
    {
        public static ResultDTO<T> GenerateResponse<T>(bool success, string message, T retorno) => new ResultDTO<T>(success, message, retorno);
        public static ResultDTO<T> GenerateResponse<T>(bool success, string message, T retorno, List<string> errors) => new ResultDTO<T>(success, message, retorno, errors);
        public static ResultDTO<T> GenerateResponse<T>(T retorno) => new ResultDTO<T>(true, "Success", retorno, null);
        public static ResultDTO<T> GenerateResponse<T>(T retorno, string erro) => new ResultDTO<T>(false, "Error", retorno, new List<string> { erro });
        public static ResultDTO<T> GenerateResponse<T>(bool success, string message) => new ResultDTO<T>(success, message);
        public static ResultDTO<T> GenerateResponse<T>(bool success) => new ResultDTO<T>(success, success ? "Success" : "Error");
        public static ResultDTO<T> GenerateResponse<T>(Exception exception) => new ResultDTO<T>(false, exception.Message);
        public static ResultDTO<T> GenerateResponse<T>(string erro) => new ResultDTO<T>(false, "Error", new List<string> { erro });
        public static ResultDTO<T> GenerateResponse<T>(List<string> errors) => new ResultDTO<T>(false, "Error", errors);
        public static ResultDTO<T> GenerateResponse<T>(string erro, List<string> errors) => new ResultDTO<T>(false, erro, errors);

        public static ResultListDTO<T> GenerateResponseList<T>(bool success, string message) => new ResultListDTO<T>(success, message);
        public static ResultListDTO<T> GenerateResponseList<T>(IEnumerable<T> items, ReportResultListDTO relatorio) => new ResultListDTO<T>(true, "Success", items, relatorio);
        public static ResultListDTO<T> GenerateResponseList<T>(IEnumerable<T> items) => new ResultListDTO<T>(true, "Success", items, new ReportResultListDTO { ItemsPerPage = items.Count(), CurrentPage = 1, TotalItems = items.Count() });
        public static ResultListDTO<T> GenerateResponseList<T>(Exception exception) => new ResultListDTO<T>(false, exception.Message);

        public static void AppendError(this ResultDTO resultado, string erro)
        {
            if (string.IsNullOrEmpty(resultado.Message) || resultado.Message == "Success" || resultado.Success)
                resultado.Message = "Error validating fields.";

            resultado.Success = false;

            if (resultado.Errors == null)
                resultado.Errors = new List<string> { erro };
            else
                resultado.Errors.Add(erro);
        }
        public static void AppendError(this ResultDTO resultado, string message, string erro)
        {
            resultado.Success = false;

            if (!string.IsNullOrEmpty(message))
                resultado.Message = message;

            if (resultado.Errors == null)
                resultado.Errors = new List<string> { erro };
            else
                resultado.Errors.Add(erro);
        }
    }
}
