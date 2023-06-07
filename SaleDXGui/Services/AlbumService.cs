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
    public class AlbumService : AbstractMongoDBDao<Album, string>
    {
        public AlbumService()
        {
            InitClient(new MongoDBDao.Model.ConnectionConfig()
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SaleDXMongo"].ConnectionString,
                DataBaseName = "SaleDX",
                CollectionName = "Album"
            });
        }

        public Album GetByCode(string orgCode, string code)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("OrgCode", orgCode) & Builders<BsonDocument>.Filter.Eq("Code", code);
            return GetAll(filter, null)?.FirstOrDefault();
        }
    }
}