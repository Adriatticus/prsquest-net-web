using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

            // POST: api/Requests/Create
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(RequestCreateDTO requestDTO)
        {
            //string RequestStatus = "";
            Request need = new Request();
            need.UserId = requestDTO.UserId;
            need.RequestNumber = await ReqNumGen();
            need.Description = requestDTO.Description;
            need.Justification = requestDTO.Justification;
            need.DateNeeded = requestDTO.DateNeeded;
            need.DeliveryMode = requestDTO.DeliveryMode;
            need.Status = "NEW";
            //need.Status = RequestStatus.NEW;
            need.Total = 0.0m;
            need.SubmittedDate = DateTime.Now;


            _context.Requests.Add(need);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = need.Id }, need);
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

        //api/Requests/submit-review/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("submit-review/{id}")]
        public async Task<IActionResult> PutRequestForReview(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if (request.Total <= 50)
            {
                request.Status = "APPROVED";
            }
            else
            {
                request.Status = "REVIEW";
            }

            request.SubmittedDate = DateTime.Now;

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

        //api/Requests/list-review/{userId}
        [HttpGet("list-review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsReadyForReview(int userId)
        {

            var requests = _context.Requests.Where(r => r.Status == "REVIEW" && r.UserId != userId);
            return await requests.ToListAsync();
            //return await _context.Requests.ToListAsync();
        }

        //api/Requests/approve/{requestId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("approve/{Id}")]
        public async Task<IActionResult> PutApproved(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "APPROVED";

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

        //api/Requests/reject/{requestId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("reject/{Id}")]
        public async Task<IActionResult> PutDenied(int id, RejectDTO denied)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "REJECTED";
            request.ReasonForRejection = denied.ReasonForRejection;

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

        public async Task<string> ReqNumGen()
        {
            //"RyyMMdd####"
            //requestNum += DateTime.Now.ToString("yyMMdd");
                StringBuilder requestNbr = new StringBuilder("R");
                string dateStr = String.Format("{0:yyMMdd}", DateTime.Now);
                requestNbr.Append(dateStr);
                var prefixn = await _context.Requests
                    .Where(r => r.RequestNumber.StartsWith(requestNbr.ToString()))
                    .OrderBy(r => r.RequestNumber)
                    .LastOrDefaultAsync();
            if (prefixn != null)
            {
                string lastFour = prefixn.RequestNumber.Substring(prefixn.RequestNumber.Length -4);
                // what does lastFour have in it = ex.0001
                int counter = Int32.Parse(lastFour);
                counter++;
                lastFour = counter.ToString().PadLeft(4, '0');
                requestNbr.Append(lastFour);
            }
            else
            {
                requestNbr.Append("0001");
            }
            // result of prefixn and append an incrimenting number to it
            //string fourNum

            return requestNbr.ToString();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
