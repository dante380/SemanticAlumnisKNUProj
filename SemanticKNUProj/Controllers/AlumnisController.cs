using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemanticKNUProj.Data;
using SemanticKNUProj.Model;

namespace SemanticKNUProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnisController : ControllerBase
    {
        private readonly AppDBCont _context;

        public AlumnisController(AppDBCont context)
        {
            _context = context;
        }

        // GET: api/Alumnis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumniModel>>> GetalumniModels()
        {
          if (_context.alumniModels == null)
          {
              return NotFound();
          }
            return await _context.alumniModels.ToListAsync();
        }

        // GET: api/Alumnis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlumniModel>> GetAlumniModel(Guid id)
        {
          if (_context.alumniModels == null)
          {
              return NotFound();
          }
            var alumniModel = await _context.alumniModels.FindAsync(id);

            if (alumniModel == null)
            {
                return NotFound();
            }

            return alumniModel;
        }

        // PUT: api/Alumnis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumniModel(Guid id, AlumniModel alumniModel)
        {
            if (id != alumniModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(alumniModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumniModelExists(id))
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

        // POST: api/Alumnis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AlumniModel>> PostAlumniModel(AlumniModel alumniModel)
        {
          if (_context.alumniModels == null)
          {
              return Problem("Entity set 'AppDBCont.alumniModels'  is null.");
          }
            alumniModel.Id = Guid.NewGuid();
            _context.alumniModels.Add(alumniModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlumniModel", new { id = alumniModel.Id }, alumniModel);
        }

        [HttpPost("sparql")]
        //public async Task<IActionResult> PostRdfAlumni(AlumniModel alumniModel)
        public async Task<ActionResult<AlumniModel>> PostRdfAlumni(AlumniModel alumniModel)
        {
            if (_context.alumniModels == null)
            {
                return Problem("Entity set 'AppDBCont.alumniModels'  is null.");
            }
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));

            //Make a SELECT query against the Endpoint
            SparqlResultSet results = endpoint.QueryWithResultSet(@" 
            SELECT ?alumni
            WHERE {
                ?alumni rdf:type dbo:Person .
                ?alumni dbo:almaMater dbr:Taras_Shevchenko_National_University_of_Kyiv .
            }");
            foreach (SparqlResult result in results)
            {

                string[] prnt = result.ToString().Split('/')
                    .Last().Split('_');
                if (prnt.Length > 2)
                    prnt[1] = prnt[2];
    
                alumniModel.Id = Guid.NewGuid();
                alumniModel.Name = prnt[0];
                alumniModel.Link = prnt[1];
            //_context.alumniModels.Add(alumniModel);
            }
            //await _context.SaveChangesAsync();

            return NoContent();

            //return CreatedAtAction("GetAlumniModel", new { id = alumniModel.Id }, alumniModel);
        }


        // DELETE: api/Alumnis/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAlumniModel(Guid id)
        {
            if (_context.alumniModels == null)
            {
                return NotFound();
            }
            var alumniModel = await _context.alumniModels.FindAsync(id);
            if (alumniModel == null)
            {
                return NotFound();
            }

            _context.alumniModels.Remove(alumniModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumniModelExists(Guid id)
        {
            return (_context.alumniModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }



    }
}
