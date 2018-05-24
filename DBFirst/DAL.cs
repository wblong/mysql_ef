using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using testMySQLEF;

namespace DBFirst
{
    public class DAL<T> : IDAL<T> where T : class, new()
    {
         

      int IDAL<T>.Add(T model)
        {
            using (UserEntities db=new UserEntities())
            {
                db.Set<T>().Add(model);
                return db.SaveChanges();
            }
        }

        int IDAL<T>.Delete(Expression<Func<T, bool>> whereLambda)
        {
            using (UserEntities db = new UserEntities())
            {
                var dbQuery = db.Set<T>();
                var list = dbQuery.Where(whereLambda).ToList();
                foreach(var item in list)
                {
                    dbQuery.Remove(item);
                }
                return db.SaveChanges();
            }
        }

        List<T> IDAL<T>.GetModelList(Expression<Func<T, bool>> whereLambda)
        {
           using (UserEntities db=new UserEntities())
            {
                var dbQuery = db.Set<T>();
                if (whereLambda == null)
                    return db.Set<T>().ToList();
                return dbQuery.Where(whereLambda).ToList();
            }
        }

        int IDAL<T>.Update(Expression<Func<T, bool>> whereLambda, string[] propertyNames, object[] propertyValues)
        {
            using (UserEntities db = new UserEntities())
            {
                var list = db.Set<T>().Where<T>(whereLambda).ToList();
                Type t = typeof(T);
                foreach(var item in list)
                {
                    for (int index = 0; index < propertyNames.Length; index++)
                    {
                        string name = propertyNames[index];
                        PropertyInfo pi = t.GetProperty(name);
                        pi.SetValue(item, propertyValues[index], null);
                    }
                        

                }
                return db.SaveChanges();
            }
        }
    }
}
