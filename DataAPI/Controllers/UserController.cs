using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAPI.Models;

namespace DataAPI.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<Player> Get()
        {
            using (UserDBContext dBContext = new UserDBContext())
            {
                return dBContext.Players.ToList();
            }
        }

        public Player Get(int id)
        {
            using (UserDBContext dBContext = new UserDBContext())
            {
                return dBContext.Players.FirstOrDefault(p => p.UserID == id);
            }
        }
    }
}
