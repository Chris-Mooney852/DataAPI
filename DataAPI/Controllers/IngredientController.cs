using System;
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
        public IEnumerable<Ingredient> Get()
        {
            using (rpgfitnessDBContext dBContext = new rpgfitnessDBContext())
            {
                return dBContext.Ingredients.ToList();
            }
        }

        public Ingredient Get(int id)
        {
            using (rpgfitnessDBContext dbContext = new rpgfitnessDBContext())
            {
                return dbContext.Ingredients.FirstOrDefault(e => e.ID == id);
            }
        }
    }
}
