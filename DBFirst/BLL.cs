using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBFirst
{
    public static class BLL<T> where T:class,new()
    {
    
        private static IDAL<T> dal = new DAL<T>();
        public static int Add(T model)
        {
            return dal.Add(model);
        }
        public static int Delete(Expression<Func<T, bool>> whereLambda)
        {
            return dal.Delete(whereLambda);
        }
        public static  int Update(Expression<Func<T, bool>> whereLambda,string[] propertyNames,string[] propertyValues)
        {
            return dal.Update(whereLambda, propertyNames, propertyValues);
        }
        public static List<T> GetModelList(Expression<Func<T, bool>> whereLambda)
        {
            return dal.GetModelList(whereLambda);
        }
    }
}
