using Newtonsoft.Json;
using System;

namespace Seventh.DGuard.DTO
{
    public class BaseDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
