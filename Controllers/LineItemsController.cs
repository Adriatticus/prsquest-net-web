using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prsquest_api_controllers.Models;

namespace prsquest_api_controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineItemsController : ControllerBase
    {
        private readonly prsquestContext _context;

        public LineItemsController(prsquestContext context)
        {
            _context = context;
        }

        // GET: api/LineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItems()
        {
            var lineitems = _context.LineItems.Include(li => li.Product)
                                              .Include(li => li.Request);
            return await lineitems.ToListAsync();
            //return await _context.LineItems.ToListAsync();
        }

        // GET: api/LineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.Include(li => li.Product)
                                                   .Include(li => li.Product)
                                                   .FirstOrDefaultAsync(li => li.Id == id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        // PUT: api/LineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                RecalculateRequestTotal(lineItem.RequestId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
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

        // POST: api/LineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem(LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();

            RecalculateRequestTotal(lineItem.RequestId);

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        // DELETE: api/LineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();
            RecalculateRequestTotal(lineItem.RequestId);

            return NoContent();
        }
        //api/LineItems/lines-for-req/{reqId}
        [HttpGet("lines-for-req/{reqID}")]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItemsForRequest(int reqID)
        {
            var lineItems = _context.LineItems.Include(li => li.Product)
                                              //.Include(li => li.Request)
                                              .Where(li => li.RequestId == reqID);

            return await lineItems.ToListAsync();
            //return await _context.LineItems.ToListAsync();
        }

        private void RecalculateRequestTotal(int requestId)
        {
            // Insert, Update, Del has occurred
            var request = _context.Requests.Find(requestId);
            // get all LI records for this request 
            var lineItems = _context.LineItems.Include(li => li.Product)
                                              .Where(li => li.RequestId == requestId);
            // loop through all LI's and sum all ProductPrice values
            decimal sum = 0;
            foreach (LineItem lineItem in lineItems)
            {
                sum += lineItem.Product.Price*lineItem.Quantity;
            }
            // set the sum in the request.total property
            request.Total = sum;
            // save that shit
            _context.SaveChanges();
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }
    }
}
