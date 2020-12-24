using System;
using System.Linq;

namespace VotingSystem.Models
{
    public class AccountVM
    {
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NatinalID { get; set; }
        public string Password { get; set; }
        public bool IsVoted { get; set; }
        public bool IsAdmin { get; set; }

        public int AddUser(AccountVM obj)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    if (db.Users.Any(c => c.NatinalID == obj.NatinalID))
                    {
                        //Natinal Exist
                        return 2;
                    }
                    if (db.Users.Any(c => c.Email == obj.Email))
                    {
                        //Email Exist
                        return 3;
                    }
                    if (db.Users.Any(c => c.Phone == obj.Phone))
                    {
                        //Phone Exist
                        return 4;
                    }
                    User dalObj = new User();
                    dalObj.ID = obj.ID;
                    dalObj.Name = obj.Name;
                    dalObj.Email = obj.Email;
                    dalObj.Phone = obj.Phone;
                    dalObj.NatinalID = obj.NatinalID;
                    dalObj.Password = obj.Password;
                    db.Users.Add(dalObj);
                    bool res = db.SaveChanges() > 0;
                    return res ? 1 : 0;
                }
                catch
                {
                    //Error
                    return 0;
                }
            }
        }

        public bool DelUser(Guid ID)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                var user = (from c in db.Users where c.ID == ID select c).FirstOrDefault();
                db.Users.Remove(user);
                bool result = db.SaveChanges() > 0;
                return result;
            }
        }

        public RetLogin LoginUser(string Email, string Password)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                RetLogin obj = new RetLogin();
                try
                {

                    User dalObj = (from c in db.Users where c.Email.ToLower() == Email.ToLower() select c).FirstOrDefault();
                    if (dalObj != null)
                    {
                        if (dalObj.Password.ToLower() == Password.ToLower())
                        {
                            obj.Code = 1;
                            obj.Data = new AccountVM();
                            obj.Data.ID = dalObj.ID;
                            obj.Data.Name = dalObj.Name;
                            obj.Data.Email = dalObj.Email;
                            obj.Data.NatinalID = dalObj.NatinalID;
                            obj.Data.IsVoted = dalObj.IsVoted;
                            return obj;
                        }
                        else
                        {
                            obj.Code = 3;
                            //Incorrect Pass
                            return obj;
                        }
                    }
                    else
                    {
                        obj.Code = 2;
                        //Register First
                        return obj;
                    }
                }
                catch
                {
                    //Error
                    return obj;
                }
            }
        }

        public int ClearAllData()
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM ElectionDate");
                db.Database.ExecuteSqlCommand("DELETE FROM Election");
                db.Database.ExecuteSqlCommand("DELETE FROM Candidate");
                db.Database.ExecuteSqlCommand("DELETE FROM [User]");
                db.SaveChanges();
                return 1;
            }

        }
    }
    public class RetLogin
    {
        public int Code { get; set; }
        public AccountVM Data { get; set; }
    }
}