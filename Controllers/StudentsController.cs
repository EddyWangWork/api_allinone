using demoAPI.Data.School;
using demoAPI.Model.School;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetStudents")]
        public async Task<IEnumerable<Student>> GetAsync()
        {
            return await _context.Students.ToListAsync();
        }
    }
}
