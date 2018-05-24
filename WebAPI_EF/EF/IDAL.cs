using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_EF.EF
{
    public interface IDAL<T> where T:class,new()
    {
        int Add(T model);
        int Delete(Expression<Func<T, bool>> whereLambda);

        int Update(Expression<Func<T, bool>> whereLambda ,string[] propertyNames,object[]propertyValues);
        List<T> GetModelList(Expression<Func<T, bool>> whereLambda);
    }
}
