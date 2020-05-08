using AspNetCore.HostedServices.Attributes;
using AspNetCore.HostedServices.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AspNetCore.HostedServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {

        public StudentsController(salesdbContext salesdbContext)
        {
            SalesdbContext = salesdbContext;
        }

        public salesdbContext SalesdbContext { get; }

        [Cached(time: 10)]
        public async Task<IActionResult> GetAll() => Ok(await SalesdbContext.Employees.ToListAsync());

    }
}
