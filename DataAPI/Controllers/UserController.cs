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
            using (PlayerDBContext dBContext = new PlayerDBContext())
            {
                return dBContext.Players.ToList();
            }
        }

        public Player Get(string name)
        {
            using (PlayerDBContext dBContext = new PlayerDBContext())
            {
                return dBContext.Players.FirstOrDefault(p => p.UserName == name);
            }
        }

        public HttpResponseMessage Post([FromBody] Player player)
        {
            try
            {
                using (PlayerDBContext dBContext = new PlayerDBContext())
                {
                    dBContext.Players.Add(player);

                    dBContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, player);
                    message.Headers.Location = new Uri(Request.RequestUri + player.UserID.ToString());

                    return message;
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Player player)
        {
            try
            {
                using (PlayerDBContext dbContext = new PlayerDBContext())
                {
                    var entity = dbContext.Players.FirstOrDefault(e => e.UserID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Player with the ID " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.UserID = player.UserID;
                        entity.UserName = player.UserName;
                        entity.UserPassword = player.UserPassword;
                        entity.UserEmail = player.UserEmail;
                        entity.CurrentWeight = player.CurrentWeight;
                        entity.GoalWeight = player.GoalWeight;
                        entity.MaxDailyIntake = player.MaxDailyIntake;
                        entity.ConsumedCalories = player.ConsumedCalories;
                        entity.TargetDailySteps = player.TargetDailySteps;
                        entity.CurrentSteps = player.CurrentSteps;
                        entity.LastLogin = TimeZoneInfo.ConvertTimeToUtc((DateTime)player.LastLogin, TimeZoneInfo.Local);



                        dbContext.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (PlayerDBContext dbContext = new PlayerDBContext())
                {
                    var entity = dbContext.Players.FirstOrDefault(e => e.UserID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Ingredient with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.Players.Remove(entity);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
