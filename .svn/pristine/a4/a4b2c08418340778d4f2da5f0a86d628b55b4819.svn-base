using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDao;
using SaleDXGui.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleDXGui.Services
{
    public class AutoLoginConfigService : AbstractMongoDBDao<AutoLoginConfig, string>
    {
        public AutoLoginConfigService()
        {
            InitClient(new MongoDBDao.Model.ConnectionConfig()
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SaleDXMongo"].ConnectionString,
                DataBaseName = "SaleDX",
                CollectionName = "AutoLoginConfig"
            });
        }

        internal AutoLoginConfig GetByCode(string code)
        {
            code = code.Trim();
            var filter = Builders<BsonDocument>.Filter.Eq("Code", code);
            return GetAll(filter, null)?.FirstOrDefault();
        }
    }
}