using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDao;
using SaleDXGui.Domains;
using SaleDXGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleDXGui.Services
{
    public class CustomerService : AbstractMongoDBDao<Customer, string>
    {
        public CustomerService()
        {
            InitClient(new MongoDBDao.Model.ConnectionConfig()
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SaleDXMongo"].ConnectionString,
                DataBaseName = "SaleDX",
                CollectionName = "Customer"
            });
        }

        public List<Customer> Search(SearchModel<CustomerSearchModel> searchModel, out long total)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("OrgCode", searchModel.OrgCode);
            if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
            {
                searchModel.Keyword = searchModel.Keyword.Trim();
                var reg = new BsonRegularExpression(searchModel.Keyword, "i");
                filter &= (Builders<BsonDocument>.Filter.Eq("Code", reg) | Builders<BsonDocument>.Filter.Eq("Name", reg)
                    | Builders<BsonDocument>.Filter.Eq("Phone", reg) | Builders<BsonDocument>.Filter.Eq("Email", reg)
                    | Builders<BsonDocument>.Filter.Eq("Note", reg) | Builders<BsonDocument>.Filter.Eq("Sex", reg));
            }
            if (searchModel.Model?.DOBFrom != null)
            {
                var fromMonth = searchModel.Model?.DOBFrom.Value.Month;
                var fromDay = searchModel.Model?.DOBFrom.Value.Day;
                filter &= (Builders<BsonDocument>.Filter.Gt("DOBMonth", fromMonth) |
                    (Builders<BsonDocument>.Filter.Eq("DOBMonth", fromMonth) & Builders<BsonDocument>.Filter.Lte("DOBDay", fromDay)));
            }
            if (searchModel.Model?.DOBTo != null)
            {
                var toMonth = searchModel.Model?.DOBTo.Value.Month;
                var toDay = searchModel.Model?.DOBTo.Value.Day;
                filter &= (Builders<BsonDocument>.Filter.Lt("DOBMonth", toMonth) |
                    (Builders<BsonDocument>.Filter.Eq("DOBMonth", toMonth) & Builders<BsonDocument>.Filter.Lte("DOBDay", toDay)));
            }
            if (searchModel.Model?.Channels != null)
            {
                filter &= Builders<BsonDocument>.Filter.In("ContactPoints.Channel.Code", searchModel.Model.Channels);
            }
            if (searchModel.Model?.Type != null)
            {
                filter &= Builders<BsonDocument>.Filter.Eq("Type", searchModel.Model?.Type);
            }
            if (searchModel.Model?.Labels != null && searchModel.Model?.Labels.Count > 0)
            {
                filter &= Builders<BsonDocument>.Filter.In("Labels", searchModel.Model?.Labels);
            }

            var sort = Builders<BsonDocument>.Sort.Ascending("Code");

            total = 0;
            var data = Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);

            return data;
        }
    }
}