﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using prsquest_api_controllers.Models;

namespace prsquest_api_controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly prsquestContext _context;

        public RequestsController(prsquestContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            var requests = _context.Requests.Include(r => r.User);
            return await requests.ToListAsync();
            //return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.Include(r => r.User)
                                                 .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Requests/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Request>> PostRequestCreate(RequestCreateDTO requestDTO)
        {
            Request need = new Request();
            need.RequestNumber = "RyyMMdd####";
            need.Total = 0.0m;
            need.SubmittedDate = DateTime.Now;
            need.Status = "NEW";
            need.UserId = requestDTO.UserId;
            need.DateNeeded = requestDTO.DateNeeded;
            need.DeliveryMode = requestDTO.DeliveryMode;
            need.Justification = requestDTO.Justification;
            need.Description = requestDTO.Description;


            _context.Requests.Add(need);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = need.Id }, need);
        }

        public async string ReqNumGen()
        {
            //"RyyMMdd####"
            string requestNum = "R";
            requestNum += DateTime.Now.ToString("yyMMdd");
            var prefixn = _context.Requests.Where(r => r.RequestNumber.StartsWith(requestNum)).LastOrDefault();
            if (prefixn != null)
            {
                string lastFour = prefixn.RequestNumber.Substring(-1, 4);
            }
            else
            {
                requestNum += "0001";
            }
            // result of prefixn and append an incrimenting number to it
            //string fourNum

            return requestNum;
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
