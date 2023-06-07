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

    public class ChannelService : AbstractMongoDBDao<Channel, string>
    {
        public ChannelService()
        {
            InitClient(new MongoDBDao.Model.ConnectionConfig()
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SaleDXMongo"].ConnectionString,
                DataBaseName = "SaleDX",
                CollectionName = "Channel"
            });
        }

        public Channel GetByCode(string orgCode)
        {
            var reg = new BsonRegularExpression(orgCode, "i");
            var filter = Builders<BsonDocument>.Filter.Eq("Code", reg);
            return GetAll(filter, null)?.FirstOrDefault();
        }
    }

}