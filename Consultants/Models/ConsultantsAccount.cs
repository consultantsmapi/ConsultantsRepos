using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Consultants.Models
{
    public class ConsultantsAccount
    {
        [Key]
        public ObjectId _id { get; set; }

        [Required(ErrorMessage = "נדרש שם פרטי")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "נדרש שם משפחה")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "נדרש כתובת אימייל")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "נדרש שם משתמש")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "נדרש סיסמא")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Compare("Password", ErrorMessage = "בבקשה אשר את הסיסמא")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "נדרש מס פלאפון")]
        [DataType(DataType.PhoneNumber)]
        public String Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public String HomePhone { get; set; }


        [DataType(DataType.PhoneNumber)]
        public String Fax { get; set; }


        [Required(ErrorMessage = "נדרש כתובת")]
        [DataType(DataType.Text)]
        public String Adress { get; set; }


        [Required(ErrorMessage = "נדרש  תחום יעוץ")]
        [DataType(DataType.Text)]
        public String CounsilSubject1 { get; set; }

        [DataType(DataType.Text)]
        public String CounsilSubject2 { get; set; }

        [Required(ErrorMessage = "נדרש שנות נסיון בתחום 1")]
        public String YearOfExprience1{ get; set; }

        [DataType(DataType.Upload)]
        public String Documents1 { get; set; } //לשנות את DATATYPE

        [DataType(DataType.Upload)]
        public String Documents2 { get; set; }

        public String YearOfExprience2 { get; set; }

        public int CheckBox { get; set; }
    }




}
