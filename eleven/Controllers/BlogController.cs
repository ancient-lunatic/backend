using System;
using System.Threading.Tasks;
using eleven.Models;
using Microsoft.AspNetCore.Mvc;

namespace eleven.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        public BlogController(appDb db)
        {
            Db = db;
        }

        // GET api/blog/user
 

        [HttpGet("user")]
        public async Task<IActionResult> GetUserInfo()
        {
            await Db.Connection.OpenAsync();
            var query = new employee(Db);
            var result = await query.getUserInformation();
            return new OkObjectResult(result);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            await Db.Connection.OpenAsync();
            var query = new employee(Db);
            var result = await query.FindOneAsync(id);
           // if (result is null)
             ///   return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPost("user/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]editDetails body)
        {
            await Db.Connection.OpenAsync();
            var query = new employee(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Id = body.Id;
            result.salutation = body.salutation;
            result.firstName = body.firstName;
            result.MiddleName = body.MiddleName;
            result.lastName = body.lastName;
            result.gender = body.gender;
            result.addressLine1 = body.addressLine1;
            result.addressLine2 = body.addressLine2;
            result.Locality = body.Locality;
      
            await query.UpdateAsync(result);
            return new OkObjectResult(result);
        }

        [HttpPost("user/insert")]
        public async Task<IActionResult> insertnewAsync([FromBody]insertData body)
        {
            
            await Db.Connection.OpenAsync();
            var query = new employee(Db);
         
            var result = new insertData();
            
            result.salutation = body.salutation;
            result.firstName = body.firstName;
            result.MiddleName = body.MiddleName;
            result.lastName = body.lastName;
            result.gender = body.gender;
            result.nationality = body.nationality;
            result.date = body.date;
            result.addressLine1 = body.addressLine1;
            result.addressLine2 = body.addressLine2;
            result.Locality = body.Locality;
            result.city = body.city;
            result.contact = body.contact;

            await query.insertAsync(result);
            return new OkObjectResult(result);
        }

        [HttpDelete("userdel/{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new employee(Db);
            var result = await query.delete(id);
            if (result is null)
                return new NotFoundResult();
            return new OkResult();
        }




        /*
                [HttpPut("user/{id}/{firstName}/{addressLine1}")]
                public async Task<IActionResult> PutOne(int id, [FromBody]employees es)
                {
                    await Db.Connection.OpenAsync();
                    var query = new employee(Db);
                    await query.UpdateAsync(id);
                    /*
                     if (result is null)
                         return new NotFoundResult();


                    //return new OkObjectResult(result);
                    return Ok();
                }
                */


        public appDb Db { get; }
    }

}
