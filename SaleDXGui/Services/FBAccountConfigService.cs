﻿using MongoDBDao;
using SaleDXGui.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleDXGui.Services
{
    public class FBAccountConfigService : AbstractMongoDBDao<FBAccountConfig, string>
    {
        public FBAccountConfigService()
        {
            InitClient(new MongoDBDao.Model.ConnectionConfig()
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SaleDXMongo"].ConnectionString,
                DataBaseName = "SaleDX",
                CollectionName = "FBAccountConfig"
            });
        }
    }
}