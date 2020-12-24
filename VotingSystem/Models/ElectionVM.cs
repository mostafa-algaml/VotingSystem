using System;
using System.Linq;

namespace VotingSystem.Models
{
    public class ElectionVM
    {
        public System.Guid ID { get; set; }
        public System.Guid UserID { get; set; }
        public System.Guid CandidateID { get; set; }

        public int AddElection(Guid UserID,Guid CandidateID)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    Election dalObj = new Election();
                    dalObj.ID = Guid.NewGuid();
                    dalObj.UserID = UserID;
                    dalObj.CandidateID = CandidateID;
                    db.Elections.Add(dalObj);
                    bool res = db.SaveChanges() > 0;
                    if (res)
                    {
                        var userdate = db.Users.Where(c => c.ID == UserID).FirstOrDefault();
                        userdate.IsVoted = true;
                        db.Entry(dalObj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return res ? 1 : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int AddEditElectionDate(DateTime elcDate)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                try
                {
                    if (db.ElectionDates.Any())
                    {
                        ElectionDate dalObj = db.ElectionDates.FirstOrDefault();
                        dalObj.ElectionData = elcDate;
                        db.Entry(dalObj).State = System.Data.Entity.EntityState.Modified;
                        bool res = db.SaveChanges() > 0;
                        return res ? 1 : 0;
                    }
                    else
                    {
                        ElectionDate dalObj = new ElectionDate();
                        dalObj.ID = Guid.NewGuid();
                        dalObj.ElectionData = elcDate;
                        db.ElectionDates.Add(dalObj);
                        bool res = db.SaveChanges() > 0;
                        return res ? 1 : 0;
                    }

                }
                catch
                {
                    return 0;
                }
            }
        }
        public static DateTime GetCandidateDate()
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                var query = db.ElectionDates.FirstOrDefault();
                if (query != null)
                {
                    return query.ElectionData;
                }
                else
                {
                    return DateTime.Now.Date;
                }
            }
        }
        public static bool HasCandidateDate()
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                var query = db.ElectionDates.FirstOrDefault();
                if (query != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool ElectionFinished()
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                var query = db.ElectionDates.FirstOrDefault();
                if (query != null)
                {
                    if (query.ElectionData.Date <= DateTime.Now.Date)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }
    }
    public class ElectionVoted
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public string ElectionDate { get; set; }
        public bool IsHaveDate { get; set; }

        public ElectionVoted GetCurrent(Guid UserID)
        {
            using (ElectionEntities db = new ElectionEntities())
            {
                var query = db.Elections.Where(c => c.UserID == UserID).FirstOrDefault();
                if (query != null)
                {
                    var obj = new ElectionVoted();
                    obj.Name = query.Candidate.Name;
                    obj.Photo = query.Candidate.Photo;
                    var querydate = db.ElectionDates.FirstOrDefault();
                    if (querydate != null)
                    {
                        obj.IsHaveDate = true;
                        obj.ElectionDate = querydate.ElectionData.ToString("MM/dd/yyyy");
                    }
                    return obj;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}