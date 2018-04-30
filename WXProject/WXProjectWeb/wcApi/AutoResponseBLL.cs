using Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace WXProjectWeb.wcApi
{

    public class AutoResponseBLL
    {
        public static AutoResponse Get(int id)
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.AutoResponse.Where(o => o.ID == id).FirstOrDefault();
            }
        }



        public static AutoResponse GetContentbyQuestion(string question)
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.AutoResponse.Where(o => o.Question.Contains(question)).FirstOrDefault();
            }
        }


        public static List<AutoResponse> GetBaseResponse()
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.AutoResponse.ToList();
            }
        }


        public static AutoResponse Save(AutoResponse response)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var modal = db.AutoResponse.Add(response);
                int row = db.SaveChanges();
                return modal;
            }
        }

        public static int Modify(AutoResponse response)
        {
            using (EFDbContext db = new EFDbContext())
            {
                db.AutoResponse.Attach(response);
                db.Entry(response).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }

        public static int Delete(int id)
        {
            string sql = " DELETE FROM AutoResponse WHERE ID = " + id;
            int rows = DbHelperSQL.ExecuteSql(sql);
            return rows;
        }
    }
}