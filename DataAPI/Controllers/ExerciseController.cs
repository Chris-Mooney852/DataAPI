using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controllers
{
    public class ExerciseController : ApiController
    {
        /// <summary>
        /// Retrieves list of recipies from the recipies table in the SQL database
        /// </summary>
        /// <returns>List of recipies</returns>
        public IEnumerable<Exercise> Get()
        {
            using (ExerciseDBContext dBContext = new ExerciseDBContext())
            {
                return dBContext.Exercise.ToList();
            }
        }

        /// <summary>
        /// Gets specific recipie using the provided ID
        /// </summary>
        /// <param name="id">Identification number of the recipe to get</param>
        /// <returns>Ingredient with the provided ID from the SQL database</returns>
        public Exercise Get(int id)
        {
            using (ExerciseDBContext dbContext = new ExerciseDBContext())
            {
                return dbContext.Exercise.FirstOrDefault(e => e.ex_ID == id);
            }
        }

        /// <summary>
        /// Creates a new recipie entry in the database
        /// </summary>
        /// <param name="recipe">Recipe to be added to the database</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Post([FromBody] Exercise exercise)
        {
            try
            {
                using (ExerciseDBContext dBContext = new ExerciseDBContext())
                {
                    dBContext.Exercise.Add(exercise);
                    dBContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, exercise);
                    message.Headers.Location = new Uri(Request.RequestUri + exercise.ex_ID.ToString());

                    return message;
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Updates entry in the database
        /// </summary>
        /// <param name="id">Identification number of the recipe to be updated</param>
        /// <param name="recipe">New recipe details</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Put(int id, [FromBody]Exercise exercise)
        {
            try
            {
                using (ExerciseDBContext dbContext = new ExerciseDBContext())
                {
                    var entity = dbContext.Exercise.FirstOrDefault(e => e.ex_ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Exercise with the ID " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.ex_ID = exercise.ex_ID;
                        entity.ex_Name = exercise.ex_Name;

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

        /// <summary>
        /// Deletes a recipe from the sql database
        /// </summary>
        /// <param name="id">ID of the recipe to be deleted</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ExerciseDBContext dbContext = new ExerciseDBContext())
                {
                    var entity = dbContext.Exercise.FirstOrDefault(e => e.ex_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Exercise with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.Exercise.Remove(entity);
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
