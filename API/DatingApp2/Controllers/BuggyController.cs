﻿using DatingApp2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp2.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        //[Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            //return "secret text";
            return Unauthorized(0);
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            //try
            //{
            //    var thing = _context.Users.Find(-1);
            //    var thingToReturn = thing.ToString();

            //    return thingToReturn;
            //}
            //catch(Exception ex)
            //{
            //    return StatusCode(500, "Computer says no!");
            //}

            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();

            return thingToReturn;


        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            //return BadRequest("This was not a good request");
            return BadRequest();

        }
    }
}
