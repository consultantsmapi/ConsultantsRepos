using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Consultants.Models
{
    public class EditBox
    {
        [Key]
        public ObjectId _id { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public String UserName { get; set; }

        public String Text { get; set; }


        public double X { get; set; }

        public double Y { get; set; }


    }
}