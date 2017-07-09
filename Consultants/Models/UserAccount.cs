using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Consultants.Models
{
    public class UserAccount
    {
        [Key]
        public ObjectId _id { get; set; }

        [Required(ErrorMessage ="נדרש שם פרטי")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "נדרש שם משפחה")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "נדרש כתובת אימייל")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage ="פורמט אימייל לא תקין")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        
        [Required(ErrorMessage = "נדרש שם משתמש")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "נדרש סיסמא")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Compare("Password", ErrorMessage ="בבקשה אשר את הסיסמא")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }

        public  int CheckBox { get; set;}
    }
}