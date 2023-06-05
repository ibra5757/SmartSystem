using FinalYear.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FinalYear.Models
{
    public class Users : IUsers
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();
        private string id;

        public string ID
        {
            get { return id; }
            set {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("User ID is required");
                }
                id = value;
            }

        }
        private string username;

        public string UserName
        {
            get { return username; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Username is required");
                }
                username = value;
            }

        }
        private string contact;

        public string Contact
        {
            get { return contact; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Contact is required");
                }
                // Validate contact
                if (!Regex.IsMatch(value, @"^\d{10}$"))
                {
                    throw new Exception("Invalid contact number");
                }
                contact = value;
            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Password is required");
                }
                password = value; }
        }

        private string role;

        public string Role
        { //Use ChatGpt if needed ok
            get { return role; }
            set { role = value; } // only this class can set the role of user
        }
        // set user roles
        
        public  IUsers Login()
        {

            //Users obj = db.Users.Where(a => a.UserName.Equals(this.UserName) && a.Password.Equals(this.Password) && a.IsActive == true).FirstOrDefault();
            //var unamecheck = db.Users.Where(a => a.Password.Equals(username)).FirstOrDefault();
            //var upasswordcheck = db.Users.Where(a => a.Password.Equals(this.Password)).FirstOrDefault();
            return this ;
                     
          

             
        }
        //what are the other methods to define
        //Edit delete Set Roles GEt User  UserName Alreadt exur 

        Users IUsers.Login()
        {
            throw new NotImplementedException();
        }
    }
}