using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controllers
{
    public class RecipeController : ApiController
    {
        /// <summary>
        /// Retrieves list of recipies from the recipies table in the SQL database
        /// </summary>
        /// <returns>List of recipies</returns>
        public IEnumerable<Recipe> Get()
        {
            using (recipeDBContext dBContext = new recipeDBContext())
            {
                return dBContext.Recipes.ToList();
            }
        }

        /// <summary>
        /// Gets specific recipie using the provided ID
        /// </summary>
        /// <param name="id">Identification number of the recipe to get</param>
        /// <returns>Ingredient with the provided ID from the SQL database</returns>
        public Recipe Get(int id)
        {
            using (recipeDBContext dbContext = new recipeDBContext())
            {
                return dbContext.Recipes.FirstOrDefault(e => e.recipeId == id);
            }
        }

        /// <summary>
        /// Creates a new recipie entry in the database
        /// </summary>
        /// <param name="recipe">Recipe to be added to the database</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Post([FromBody] Recipe recipe)
        {
            try
            {
                using (recipeDBContext dBContext = new recipeDBContext())
                {
                    dBContext.Recipes.Add(recipe);
                    dBContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, recipe);
                    message.Headers.Location = new Uri(Request.RequestUri + recipe.recipeId.ToString());

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
        public HttpResponseMessage Put(int id, [FromBody]Recipe recipe)
        {
            try
            {
                using (recipeDBContext dbContext = new recipeDBContext())
                {
                    var entity = dbContext.Recipes.FirstOrDefault(e => e.recipeId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Recipe with the ID " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.recipeId = recipe.recipeId;
                        entity.recipeName = recipe.recipeName;

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
                using (recipeDBContext dbContext = new recipeDBContext())
                {
                    var entity = dbContext.Recipes.FirstOrDefault(e => e.recipeId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Recipe with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.Recipes.Remove(entity);
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
