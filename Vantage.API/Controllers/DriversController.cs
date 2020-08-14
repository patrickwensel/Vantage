using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vantage.Data;
using Vantage.Common.Models;
using Vantage.API.Models;

namespace Vantage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers
                .Include(x => x.Product)
                .Include(x => x.Group)
                .Include(x => x.Attempts).ThenInclude(x => x.Infractions)
                .Include(x => x.Attempts).ThenInclude(x => x.Lesson)
                .AsNoTracking().ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            var driver = await _context.Drivers
                .Include(x => x.Product)
                .Include(x => x.Group)
                .Include(x => x.Attempts).ThenInclude(x => x.Infractions)
                .Include(x => x.Attempts).ThenInclude(x => x.Lesson)
                .AsNoTracking().FirstOrDefaultAsync(x => x.DriverID == id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        // GET: api/Drivers/testuser
        [HttpGet("GetDriverByUsername/{username}")]
        public async Task<ActionResult<Driver>> GetDriverByUsername(string username)
        {
            var driver = await _context.Drivers
                .Include(x => x.Product)
                .Include(x => x.Group)
                .Include(x => x.Attempts).ThenInclude(x => x.Infractions)
                .Include(x => x.Attempts).ThenInclude(x => x.Lesson)
                .AsNoTracking().OrderBy(x => x.DriverID).FirstOrDefaultAsync(x => x.UserName == username);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation("AuthenticateDriver")]
        public async Task<ActionResult> Authenticate([FromBody] Driver loginParam)
        {
            var upperUserName = loginParam.UserName.ToUpper();
            Driver driver = _context.Drivers
                .FirstOrDefault(x => x.UserName.ToUpper().Equals(upperUserName));

            if (driver != null)
            {
                if (driver.Pin == loginParam.Pin)
                {
                    DriverInfo driverInfo = new DriverInfo()
                    {
                        DriverID = driver.DriverID,
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        UserName = driver.UserName,
                        Pin = driver.Pin,
                        IsActive = driver.IsActive,
                        GroupID = driver.GroupID,
                        Group = driver.Group,
                        ProductID = driver.ProductID,
                    };

                    var lessons = _context.Lessons
                        .Include(x => x.Attempts)
                        .AsNoTracking()
                        .Where(x => x.ProductID == driver.ProductID)
                        .OrderBy(x => x.LessonOrder);

                    var lessonInfoList = new List<LessonInfo>();
                    foreach (Lesson lesson in lessons)
                    {
                        LessonInfo lessonInfo = new LessonInfo()
                        {
                            LessonID = lesson.LessonID,
                            PackType = lesson.PackType,
                            PackID = lesson.PackID,
                            IsActive = lesson.IsActive,
                            Name = lesson.Name,
                            LessonOrder = lesson.LessonOrder
                        };

                        var highScoreAttempt = lesson.Attempts.Where(x => x.DriverID == driver.DriverID).OrderByDescending(x => x.Score).FirstOrDefault();
                        if (highScoreAttempt != null)
                        {
                            lessonInfo.Attempt = new HighScoreAttempt()
                            {
                                AttemptID = highScoreAttempt.AttemptID,
                                CumulativeLessonTime = highScoreAttempt.CumulativeLessonTime,
                                DateCompleted = highScoreAttempt.DateCompleted,
                                IsComplete = highScoreAttempt.IsComplete,
                                Score = highScoreAttempt.Score,
                                TimeToComplete = highScoreAttempt.TimeToComplete,
                            };
                        }

                        lessonInfoList.Add(lessonInfo);
                    }

                    driverInfo.Lessons = lessonInfoList;

                    return Ok(driverInfo);
                }

                return Unauthorized();
            }

            return NotFound();
        }

        // PUT: api/Drivers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (id != driver.DriverID)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        // POST: api/Drivers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            var existingDriver = await _context.Drivers.OrderBy(x => x.DriverID).FirstOrDefaultAsync(x => x.UserName == driver.UserName);
            if(existingDriver != null && existingDriver.DriverID != 0)
            {
                return BadRequest("Username already exists. Please try another.");
            }

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.DriverID }, driver);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.DriverID == id);
        }
    }
}
