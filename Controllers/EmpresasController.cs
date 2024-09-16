using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint1_2semestre.Data;
using Sprint1_2semestre.Models;
using Sprint1_2semestre.Services; // Importa o ConfigManager
using System.Linq;
using System.Threading.Tasks;

namespace Sprint1_2semestre.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ConfigManager _configManager;

        // Injeção de dependência do ApplicationDbContext e ConfigManager
        public EmpresasController(ApplicationDbContext context, ConfigManager configManager)
        {
            _context = context;
            _configManager = configManager;
        }

        // GET: api/Empresas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            try
            {
                var empresas = await _context.Empresas.Include(e => e.KPIs).ToListAsync();
                if (empresas == null || empresas.Count == 0)
                {
                    return NotFound(new { message = "Nenhuma empresa encontrada." });
                }

                return Ok(empresas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // GET: api/Empresas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            try
            {
                var empresa = await _context.Empresas
                    .Include(e => e.KPIs)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (empresa == null)
                {
                    return NotFound(new { message = "Empresa não encontrada." });
                }

                return Ok(empresa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // POST: api/Empresas
        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa([FromBody] Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao criar a empresa", details = ex.Message });
            }
        }

        // PUT: api/Empresas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, [FromBody] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return BadRequest(new { message = "ID da empresa não corresponde ao ID fornecido." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(empresa).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
                {
                    return NotFound(new { message = "Empresa não encontrada." });
                }
                else
                {
                    return StatusCode(500, new { message = "Erro de concorrência ao atualizar a empresa" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar a empresa", details = ex.Message });
            }

            return NoContent();
        }

        // DELETE: api/Empresas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            try
            {
                var empresa = await _context.Empresas.FindAsync(id);
                if (empresa == null)
                {
                    return NotFound(new { message = "Empresa não encontrada." });
                }

                _context.Empresas.Remove(empresa);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao excluir a empresa", details = ex.Message });
            }
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    }
}
