﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAPI.Models;

namespace DataAPI.Controllers
{
    public class IngredientController : ApiController
    {

        /// <summary>
        /// Retrieves list of ingredients from the ingredients table in the SQL database
        /// </summary>
        /// <returns>List of ingredients</returns>
        public IEnumerable<Ingredient> Get()
        {
            using (IngredientDBContext dBContext = new IngredientDBContext())
            {
                return dBContext.Ingredients.ToList();
            }
        }

        /// <summary>
        /// Gets specific ingredient using the provided ID
        /// </summary>
        /// <param name="id">Identification number of the ingredient to get</param>
        /// <returns>Ingredient with the provided ID from the SQL database</returns>
        public Ingredient Get(int id)
        {
            using (IngredientDBContext dbContext = new IngredientDBContext())
            {
                return dbContext.Ingredients.FirstOrDefault(e => e.ID == id);
            }
        }

        /// <summary>
        /// Creates a new ingredient entry in the database
        /// </summary>
        /// <param name="ingredient">Ingredient to be added to the database</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Post([FromBody] Ingredient ingredient)
        {
            try
            {
                using (IngredientDBContext dBContext = new IngredientDBContext())
                {
                    dBContext.Ingredients.Add(ingredient);
                    dBContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, ingredient);
                    message.Headers.Location = new Uri(Request.RequestUri + ingredient.ID.ToString());

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
        /// <param name="id">Identification number of the ingredient to be updated</param>
        /// <param name="ingredient">New ingredient details</param>
        /// <returns>Confirmation message</returns>
        public HttpResponseMessage Put(int id, [FromBody]Ingredient ingredient)
        {
            try
            {
                using (IngredientDBContext dbContext = new IngredientDBContext())
                {
                    var entity = dbContext.Ingredients.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ingredient with the ID " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.ID = ingredient.ID;
                        entity.Name = ingredient.Name;
                        entity.Calories = ingredient.Calories;

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
        /// Deletes an ingredient from the sql database
        /// </summary>
        /// <param name="id">ID of the ingredient to be deleted</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (IngredientDBContext dbContext = new IngredientDBContext())
                {
                    var entity = dbContext.Ingredients.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Ingredient with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.Ingredients.Remove(entity);
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
