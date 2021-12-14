using System;

namespace Seventh.DGuard.DTO
{
    public class ServerDTO: BaseDTO
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
    }

    public class ServerDTO_In : ServerDTO
    {
    }

    public class ServerDTO_Out : ServerDTO
    {
    }

    public class ServerAvailableDTO_In
    {
        public string Ip { get; set; }
        public int Port { get; set; }
    }
    public class ServerAvailableDTO_Out
    {
        public bool Available { get; set; }
    }
}
