using ComputerShopApi.Models;
using ComputerShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ComputerShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public ComputerShopDbContext _context = new();

        [HttpPost("AddComputer")]
        public object AddNewComputer([FromBody] AddNewComputerDto addNewComputerDto)
        {
            var Computer = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addNewComputerDto.Brand,
                Type = addNewComputerDto.Type,
                Display = addNewComputerDto.Display,
                Memory = addNewComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsId = addNewComputerDto.OsId,
            };


            if (Computer != null)
            {
                _context.Computers.Add(Computer);
                _context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres felvétel", result = Computer });
            }

            return StatusCode(400, new { message = "Sikertelen felvétel", result = Computer });
        }
        [HttpGet("allComputer")]
        public object GetAllComputer()
        {
            var computers = _context.Computers.ToList();
            Exception e = new();

            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            else if (computers.Count < 1)
            {
                return StatusCode(200, new { message = "Üres tábla", result = computers });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a lekérdezésben", result = e.Message });
            }


        }
        [HttpPut("updateComputer")]
        public object UpdateComputer([FromQuery] Guid id, [FromBody] UpdateComputerDto updateComputerDto)
        {
            var computer = _context.Computers.FirstOrDefault(os => os.Id == id);
            Exception e = new();


            if (computer != null)
            {
                computer.Brand = updateComputerDto.Brand;
                computer.Type = updateComputerDto.Type;
                computer.Display = updateComputerDto.Display;
                computer.Memory = updateComputerDto.Memory;
                computer.OsId = updateComputerDto.OsId;

                _context.Computers.Update(computer);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = computer });
            }
            else if (computer == null)
            {
                return StatusCode(404, new { message = "Nincs ilyen Id-vel osystem", result = computer });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a frissítésben", result = e.Message });
            }
        }

        [HttpDelete("deleteComputer")]
        public object DeleteComputer([FromQuery] Guid id)
        {
            var computer = _context.Computers.SingleOrDefault(os => os.Id == id);
            Exception e = new();

            if (computer != null)
            {
                _context.Computers.Remove(computer);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés", result = computer });
            }
            else if (computer == null)
            {
                return StatusCode(404, new { message = "Nincs ilyen Id-vel osystem", result = computer });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a frissítésben", result = e.Message });
            }
        }
        [HttpGet("IdGetComputer")]
        public object IdGetComputer([FromQuery] Guid id)
        {
            var computer= _context.Computers.FirstOrDefault(o => o.Id == id);
            Exception e = new();
            if (computer != null)
            {
                return StatusCode(200, new { message = "Sikeres Lekérdezés", result = computer });
            }
            return StatusCode(200, new { message = "Sikerestelen Lekérdezés", result = computer });
        }
        [HttpGet("allComputerWithOsystem")]
        public object AllComputerWithOsystem()
        {
            var computers = _context.Computers.Join(_context.Osystems,c=>c.OsId,o=>o.Id,(c,o)=>new {Márka=c.Brand,Típus=c.Type,OsnÉV=o.Name,OsVerzió=o.Version});
            return StatusCode(200, new { message = "Sikeres Lekérdezés", result = computers });
        }
    }

}
