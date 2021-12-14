namespace Seventh.DGuard.DTO.Filter
{
    public class ServerFilterDTO : BaseFilterDTO
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }
    }
}
