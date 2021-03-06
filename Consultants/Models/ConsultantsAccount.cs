﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections;

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

        [Required(ErrorMessage = "נדרש מספר פלאפון")]
        [DataType(DataType.PhoneNumber)]
        public String Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public String HomePhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public String Fax { get; set; }

        [Required(ErrorMessage = "נדרש עיר")]
        [DataType(DataType.Text)]
        public String City { get; set; }

        [Required(ErrorMessage = "נדרש כתובת")]
        [DataType(DataType.Text)]
        public String Street { get; set; }

        [Required(ErrorMessage = "נדרש מספר בית")]
        [DataType(DataType.Text)]
        public String HouseNumber { get; set; }

        [Required(ErrorMessage = "נדרש מספר דירה")]
        [DataType(DataType.Text)]
        public String ApartmentNumber { get; set; }

        [Required(ErrorMessage = "נדרש מיקוד")]
        [DataType(DataType.Text)]
        public String PostalCode { get; set; }

        [Required(ErrorMessage = "נדרש  תחום יעוץ")]
        [DataType(DataType.Text)]
        public String CounsilSubject1 { get; set; }

        [Required(ErrorMessage = "נדרש  תאריך יום הולדת")]
        [DataType(DataType.Text)]
        public String Birthday { get; set; }

        [DataType(DataType.Text)]
        public String CounsilSubject2 { get; set; }

        [Required(ErrorMessage = "נדרש שנות נסיון בתחום 1")]
        public String YearOfExprience1{ get; set; }

        public string Documents1 { get; set; } //לשנות את DATATYPE

        public string Documents2 { get; set; }

        public int CheckBox { get; set; }
    }
}
