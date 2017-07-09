using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace Consultants.Models
{
    public class OurDbContext
    {
        public String connectionString = "mongodb://localhost";
        public String DataBaseName = "ConsultantsDB";
        public MongoDatabase Database;
        
       public OurDbContext()
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(DataBaseName);
        }
       
        public MongoCollection<UserAccount> Users
        {
            get
            {
                var users = Database.GetCollection<UserAccount>("Users");
                return users;
            }
        }

        public MongoCollection<TextBox> TextBox
        {
            get
            {
                var TextBox = Database.GetCollection<TextBox>("TextBox");
                return TextBox;
            }
        }

        public MongoCollection<EditBox> EditBox
        {
            get
            {
                var EditBox = Database.GetCollection<EditBox>("EditBox");
                return EditBox;
            }
        }

        public MongoCollection<ConsultantsAccount> Consultants
        {
            get
            {
                var consultants = Database.GetCollection<ConsultantsAccount>("Consultants");
                return consultants;
            }
        }


    }
}