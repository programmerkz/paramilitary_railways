using System;
using System.Collections.Generic;
using System.Text;

namespace RWS.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; }
        public int ItemCount { get; set; }


        public PagedResponse(T data, int pageNumber, int pageSize, int itemCount, string message = null)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.ItemCount = itemCount;
            this.PageCount = GetPageCount(pageSize, itemCount);

            this.Data = data;
            this.Message = message;
            this.Succeeded = true;
            this.Errors = null;
        }

        private int GetPageCount(int pageSize, int itemCount)
        {
            if ((pageSize <= 0) || (itemCount <= 0))
                return 0;
            else
            {
                int pageCount = itemCount / pageSize;
                if ((itemCount % pageSize) != 0)
                    pageCount++;

                return pageCount;
            }
        }
    }
}
