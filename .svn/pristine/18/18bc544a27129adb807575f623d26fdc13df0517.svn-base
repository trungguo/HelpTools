using MongoDBDao.Model;
using System.Collections.Generic;

namespace SaleDXGui.Models
{
    public class SearchModel
    {
        public string OrgCode { get; set; }
        public string CurrentUserId { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<SortModel> SortModels { get; set; }
    }
    public class SearchModel<T> : SearchModel
    {
        public T Model { get; set; }
    }

    public class SearchResultModel<T>
    {
        public SearchModel SearchModel { get; set; }
        public T Data { get; set; }
        public long Total { get; set; }
    }

    public class SearchResultModel<T1, T2>
    {
        public SearchModel<T1> SearchModel { get; set; }
        public T2 Data { get; set; }
        public long Total { get; set; }

    }
}