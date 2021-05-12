using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects
{
    public class Page<T> where T: class 
    {
        public int PageSize { get; set; }
     
        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        
        public int MaxPage { get; set; }

        
        public IEnumerable<T> PageItems { get; set; }

        public Page(IEnumerable<T> pageItems, int totalItems, int pageSize, int currentPage)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            PageItems = pageItems;
            MaxPage = (int)Math.Ceiling((decimal)totalItems / pageSize);
        }
        
    }
}
