using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebSocket.Context;

namespace SampleWebSocket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly RealTimeContext _context;

        public SessionsController(RealTimeContext context)
        {
            _context = context;
        }

       public async Task<List<Session>> GetAsync()
        {
            return  await _context.Sessions.ToListAsync();
        }
    }
}
