using ComputerShopApi.Models;
using ComputerShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        public ComputerShopDbContext _context=new();

        [HttpPost("addOsystem")]
        public object AddNewOsystem([FromBody] AddNewOsystemDTO addNewOsystemDto)
        {
            var osystem = new Osystem
            {
                Id = Guid.NewGuid(),
                Version = addNewOsystemDto.Version,
                RegisterTime=DateTime.Now,
                UpdateTime=DateTime.Now,
            };
            if (osystem != null)
            {
                _context.Osystems.Add(osystem);
                _context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres felvétel", result = osystem });
            }
            return StatusCode(400, new { message = "Sikertelen felvétel", result = osystem });
        }
    }
}
