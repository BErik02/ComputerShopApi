using ComputerShopApi.Models;
using ComputerShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopApi.Controllers
{
    [Route("osystems")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        public ComputerShopDbContext _context = new();

        [HttpPost("addOsystem")]
        public object AddNewOsystem([FromBody] AddNewOsystemDto addNewOsystemDto)
        {
            var osystem = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDto.Name,
                Version = addNewOsystemDto.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };


            if (osystem != null)
            {
                _context.Osystems.Add(osystem);
                _context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres felvétel", result = osystem });
            }

            return StatusCode(400, new { message = "Sikertelen felvétel", result = osystem });
        }

        [HttpGet("allOsystem")]
        public object GetAllOsysem()
        {
            var osystems = _context.Osystems.ToList();
            Exception e = new();

            if (osystems.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystems });
            }
            else if (osystems.Count < 1)
            {
                return StatusCode(200, new { message = "Üres tábla", result = osystems });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a lekérdezésben", result = e.Message });
            }


        }

        [HttpPut("updateOsystem")]
        public object UpdateOsystem([FromQuery] Guid id, [FromBody] UpdateOsystemDto updateOsystemDto)
        {
            var osystem = _context.Osystems.FirstOrDefault(os => os.Id == id);
            Exception e = new();


            if (osystem != null)
            {
                osystem.Name = updateOsystemDto.Name;
                osystem.Version = updateOsystemDto.Version;
                osystem.UpdateTime = DateTime.Now;

                _context.Osystems.Update(osystem);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = osystem });
            }
            else if (osystem == null)
            {
                return StatusCode(404, new { message = "Nincs ilyen Id-vel osystem", result = osystem });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a frissítésben", result = e.Message });
            }
        }

        [HttpDelete("deleteOsystem")]
        public object DeleteOsystem([FromQuery] Guid id)
        {
            var osystem = _context.Osystems.SingleOrDefault(os => os.Id == id);
            Exception e = new();

            if (osystem != null)
            {
                _context.Osystems.Remove(osystem);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés", result = osystem });
            }
            else if (osystem == null)
            {
                return StatusCode(404, new { message = "Nincs ilyen Id-vel osystem", result = osystem });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a frissítésben", result = e.Message });
            }
        }
        [HttpGet("allOsystemQithComputers")]
        public object AllOsystemsWithComputers()
        {
            var osystems = _context.Osystems.Include(o=>o.Computers);

            return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystems }); 
        }
        [HttpGet("oneOsystemWithComputers")]
        public object OneOsystemWitheComputers([FromQuery]Guid id)
        {
            var osystems = _context.Osystems.SingleOrDefault(o=>o.Id==id);

            var computers = _context.Computers.Where(c=>c.OsId==osystems.Id);
            return StatusCode(200, new { message = "Sikeres lekérdezés", result =new { osystems.Name,computers} });
        }
    }
}
