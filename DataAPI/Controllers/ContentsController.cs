using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controllers
{
    public class ContentsController : ApiController
    {
        /// <summary>
        /// Retrieves list of RecipeContent from the ingredients table in the SQL database
        /// </summary>
        /// <returns>List of ingredients</returns>
        public IEnumerable<RecipeContent> Get()
        {
            using (ContentsDBContext dBContext = new ContentsDBContext())
            {
                return dBContext.RecipeContents.ToList();
            }
        }

        /// <summary>
        /// Gets specific RecipeContent using the provided ID
        /// </summary>
        /// <param name="id">Identification number of the RecipeContent to get</param>
        /// <returns>RecipeContent with the provided ID from the SQL database</returns>
        public RecipeContent Get(int id)
        {
            using (ContentsDBContext dbContext = new ContentsDBContext())
            {
                return dbContext.RecipeContents.FirstOrDefault(e => e.recipeId == id);
            }
        }

        /// <summary>
        /// Creates a new RecipeContent entry in the database
        /// </summary>
        /// <param name="ingredient">RecipeContent to be added to the database</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Post([FromBody] RecipeContent ingredient)
        {
            try
            {
                using (ContentsDBContext dBContext = new ContentsDBContext())
                {
                    dBContext.RecipeContents.Add(ingredient);
                    dBContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, ingredient);
                    message.Headers.Location = new Uri(Request.RequestUri + ingredient.recipeId.ToString());

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
        /// <param name="id">Identification number of the RecipeContent to be updated</param>
        /// <param name="ingredient">New RecipeContent details</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Put(int id, [FromBody]RecipeContent content)
        {
            try
            {
                using (ContentsDBContext dbContext = new ContentsDBContext())
                {
                    var entity = dbContext.RecipeContents.FirstOrDefault(e => e.recipeId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Recipe content with the ID " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.recipeId = content.recipeId;
                        entity.ingredientId = content.ingredientId;

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
        /// Deletes an RecipeContent from the sql database
        /// </summary>
        /// <param name="id">ID of the RecipeContent to be deleted</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ContentsDBContext dbContext = new ContentsDBContext())
                {
                    var entity = dbContext.RecipeContents.FirstOrDefault(e => e.recipeId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Recipe content with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.RecipeContents.Remove(entity);
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
