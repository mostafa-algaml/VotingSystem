using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class CandidateVM
    {
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public int ElectoralNumber { get; set; }
        public string Photo { get; set; }
        public string ElectoralSymbol { get; set; }
        public HttpPostedFileBase PhotoFile { get; set; }
        public HttpPostedFileBase ElectoralSymbolFile { get; set; }
        public string CandidatePercentage { get; set; }

        public int AddCandidate(CandidateVM obj)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    if (db.Candidates.Any(c => c.Name.ToLower() == obj.Name.ToLower()))
                    {
                        return 2;
                    }
                    if (db.Candidates.Any(c => c.ElectoralNumber == obj.ElectoralNumber))
                    {
                        return 3;
                    }
                    Candidate dalObj = new Candidate();
                    dalObj.ID = obj.ID;
                    dalObj.Name = obj.Name;
                    dalObj.ElectoralNumber = obj.ElectoralNumber;
                    if (obj.Photo != "")
                    {
                        dalObj.Photo = obj.Photo;
                    }
                    if (obj.ElectoralSymbol != "")
                    {
                        dalObj.ElectoralSymbol = obj.ElectoralSymbol;
                    }
                    db.Candidates.Add(dalObj);
                    bool res = db.SaveChanges() > 0;
                    return res ? 1 : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int EditCandidate(CandidateVM obj)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    if (db.Candidates.Any(c => c.Name.ToLower() == obj.Name.ToLower() && c.ID != obj.ID))
                    {
                        return 2;
                    }
                    if (db.Candidates.Any(c => c.ElectoralNumber == obj.ElectoralNumber && c.ID != obj.ID))
                    {
                        return 3;
                    }
                    Candidate dalObj = db.Candidates.Where(c => c.ID == obj.ID).FirstOrDefault();
                    dalObj.Name = obj.Name;
                    dalObj.ElectoralNumber = obj.ElectoralNumber;
                    if (obj.Photo != "")
                    {
                        dalObj.Photo = obj.Photo;
                    }
                    if (obj.ElectoralSymbol != "")
                    {
                        dalObj.ElectoralSymbol = obj.ElectoralSymbol;
                    }
                    db.Entry(dalObj).State = System.Data.Entity.EntityState.Modified;
                    bool res = db.SaveChanges() > 0;
                    return res ? 1 : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int DeleteCandidate(Guid ID)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    if (db.Elections.Any(c => c.CandidateID == ID))
                    {
                        return 2;
                    }
                    Candidate dalObj = db.Candidates.Where(c => c.ID == ID).FirstOrDefault();
                    db.Candidates.Remove(dalObj);
                    bool res = db.SaveChanges() > 0;
                    return res ? 1 : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public CandidateVM GetObj(Guid ID)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    Candidate dalObj = db.Candidates.Where(c => c.ID == ID).FirstOrDefault();
                    CandidateVM obj = new CandidateVM();
                    obj.ID = dalObj.ID;
                    obj.Name = dalObj.Name;
                    obj.ElectoralNumber = dalObj.ElectoralNumber;
                    obj.ElectoralSymbol = dalObj.ElectoralSymbol;
                    obj.Photo = dalObj.Photo;
                    return obj;
                }
                catch
                {
                    return null;
                }
            }
        }
        public List<CandidateVM> GetAll(bool IsForAdmin)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                var query = db.Candidates.ToList();
                List<CandidateVM> lst = new List<CandidateVM>();
                foreach (var dalObj in query)
                {
                    CandidateVM obj = new CandidateVM();
                    obj.ID = dalObj.ID;
                    obj.Name = dalObj.Name;
                    obj.ElectoralNumber = dalObj.ElectoralNumber;
                    obj.ElectoralSymbol = dalObj.ElectoralSymbol;
                    obj.Photo = dalObj.Photo;
                    if (IsForAdmin == false)
                    {
                        int elcCount = db.Elections.Count();
                        if (elcCount > 0)
                        {
                            int userCount = db.Elections.Where(c => c.CandidateID == obj.ID).Count();
                            decimal per = (userCount / elcCount) * 100;
                            obj.CandidatePercentage = Math.Round(per, 2) + " %";
                        }
                        else
                        {
                            obj.CandidatePercentage = "0 %";
                        }

                    }
                    lst.Add(obj);
                }
                if (!IsForAdmin)
                {
                    lst = lst.OrderByDescending(c => c.CandidatePercentage).ToList();
                }
                return lst;
            }
        }
    }
}