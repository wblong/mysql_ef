using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/users
        public IEnumerable<Users> Get()
        {
            List<Users> listUser = new List<Users>();
            MySqlConnection mysql = getMySqlConnection();
            MySqlCommand mysql_command = getSqlCommand("select*from user",mysql);
            mysql.Open();
            MySqlDataReader reader = mysql_command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        Users user = new Users();
                        user.UserID = reader.GetInt32("ID");
                        user.UserName = reader.GetString("UserName");
                        user.UserEmail = reader.GetString("UserEmail");
                        listUser.Add(user);
                    }
                }
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            finally
            {
                mysql.Close();
            }
            return listUser;
        }
        // GET api/Users/5
        public Users GetUserByID(int id)
        {
            Users user = new Users();
            MySqlConnection mysql = getMySqlConnection();
            MySqlCommand mySqlCommand = getSqlCommand("select * from USER where ID=" + id, mysql);
            mysql.Open();
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        user.UserID = reader.GetInt32(0);
                        user.UserName = reader.GetString(1);
                        user.UserEmail = reader.GetString(2);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            finally
            {
                reader.Close();
            }

            return user;
        }
        //GET api/Users/?username=xx
        public IEnumerable<Users> GetUserByName(string userName)
        {
            List<Users> listuser = new List<Users>();
            MySqlConnection msql = getMySqlConnection();
            MySqlCommand mySqlCommand = getSqlCommand("select * from USER where username like'%" + userName + "%'", msql);
            msql.Open();
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        Users user = new Users();
                        user.UserID = reader.GetInt32("ID");
                        user.UserName = reader.GetString("USERNAME");
                        user.UserEmail = reader.GetString("USEREMAIL");
                        listuser.Add(user);
                    }

                }
                reader.Close();
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            finally
            {
                reader.Close();
            }
            return listuser;
        }
        // POST api/users
        public void Post([FromBody]string value)
        {
        }

        // PUT api/users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        public void Delete(int id)
        {
        }
        private static MySqlConnection getMySqlConnection()
        {
            MySqlConnection mysql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString);
            return mysql;
        }
        public static MySqlCommand getSqlCommand(string sql, MySqlConnection mysql)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mysql);
            return mySqlCommand;
        }
    }
    
}
