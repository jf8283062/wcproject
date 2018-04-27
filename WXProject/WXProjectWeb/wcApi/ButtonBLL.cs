using Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WXProjectWeb.wcApi
{
    public class ButtonBLL
    {
        public static Button Get(int id)
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.Button.Where(o => o.id == id).FirstOrDefault();
            }
        }

        public static List<Button> GetBaseButton()
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.Button.Where(o => o.baseid ==0).ToList();
            }
        }
        public static List<Button> GetSubButton(int id)
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.Button.Where(o => o.baseid == id).ToList();
            }
        }
        
        public static Button Save(Button button)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var modal = db.Button.Add(button);
                int row = db.SaveChanges();
                return button;
            }
        }

        public static int Modify(Button button)
        {
            using (EFDbContext db = new EFDbContext())
            {
                db.Button.Attach(button);
                db.Entry(button).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }

        public static int Delete(int id)
        {
            string sql = " DELETE FROM Button WHERE id = " + id;
            int rows = DbHelperSQL.ExecuteSql(sql);
            return rows;
        }
    }
}