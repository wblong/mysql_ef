using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI_EF;
using WebAPI_EF.EF;
namespace WebAPI_EF.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        
        // GET: api/Users
        public IEnumerable<users> Get()
        {
            return BLL<users>.GetModelList(null);
        }

        // GET: api/Users/5
        public users Get(int id)
        {
            var user = BLL<users>.GetModelList(e => e.id == id).FirstOrDefault();
            return user;
        }

        // POST: api/Users
        public void Post([FromBody]users user)
        {
            BLL<users>.Add(user);
             
        }

        // PUT: api/Users/5
        public void Put(int id, [FromBody]users user)
        {
            var temp = BLL<users>.GetModelList(e => e.id == id).FirstOrDefault();
            if (temp != null)
            {
                temp.id = user.id;
                temp.firstname = user.firstname;
                temp.lastname = user.lastname;
                temp.phone = user.phone;
                temp.email = user.email;
            }
            else
            {
                BLL<users>.Add(user);
            }
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
            BLL<users>.Delete(e => e.id == id);
        }
    }
}
