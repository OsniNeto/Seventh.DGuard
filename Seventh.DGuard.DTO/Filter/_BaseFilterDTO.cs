using System;

namespace Seventh.DGuard.DTO.Filter
{
    public class BaseFilterDTO
    {
        public BaseFilterDTO()
        {
            Pagination = new PaginationTO();
            Sort = new SortDTO();
        }

        public PaginationTO Pagination { get; set; }
        public SortDTO Sort { get; set; }
    }

    public class PaginationTO
    {
        public PaginationTO()
        {
            Page = 1;
        }

        public int Page { get; set; }

        public int ItemsNumber { get; set; }

        public void NormalizeProperties()
        {
            if (Page <= 0)
                Page = 1;
            if (ItemsNumber > 1000)
                ItemsNumber = 1000;
            if (ItemsNumber <= 0)
                ItemsNumber = 20;
        }
    }

    public class SortDTO
    {
        public bool Asc { get; set; }

        public string Column { get; set; }
    }
}
