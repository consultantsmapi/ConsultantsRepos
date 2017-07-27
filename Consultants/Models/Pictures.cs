using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;




namespace Consultants.Models
{
    public class Pictures
    {
        [Key]
        public ObjectId _id { get; set; }
        public String userName { get; set; }
        public String type { get; set; }
        public String lastModified { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
     
        public string FileName { get; set; }
        public string PictureDataAsString { get; set; }
    }
}
