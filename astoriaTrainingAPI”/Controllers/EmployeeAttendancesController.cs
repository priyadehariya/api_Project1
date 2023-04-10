using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI_.Models;

namespace astoriaTrainingAPI_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeeAttendancesController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        

        public EmployeeAttendancesController(astoriaTraining80Context context)
        {
            _context = context;
        }

        // GET: api/EmployeeAttendances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeAttendance>>> GetEmployeeAttendance()
        {
            return await _context.EmployeeAttendance.ToListAsync();
        }

        [HttpGet("Companydetails")]
        public async Task<ActionResult<IEnumerable<CompanyMaster>>> GetCompanyMaster()
        {
            return await _context.CompanyMaster.ToListAsync();
        }

        /// <summary>
        /// This method returns the all attendaces by passing FilterClockDate and FilterCompanyID
        /// </summary>
        /// <param name="FilterClockDate"></param>
        /// <param name="FilterCompanyID"></param>
        /// <returns>
        /// 
        /// </returns>
        // GET: api/EmployeeAttendances
        [HttpGet("allattendances")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetEmployeeAttendance(string FilterClockDate, int FilterCompanyID = 0)
        {
            if(FilterClockDate == null || FilterCompanyID == null)
            {
                return BadRequest();
            }
            try
            {

                var empAtt = (from emp in _context.EmployeeMaster
                              join att in _context.EmployeeAttendance.Where(x => x.ClockDate.Date == Convert.ToDateTime(FilterClockDate).Date)
                              on emp.EmployeeKey equals att.EmployeeKey
                              into grouping
                              from g in grouping.DefaultIfEmpty()
                              where emp.EmpCompanyId == FilterCompanyID || FilterCompanyID == 0

                              select new Attendance
                              {
                                  EmployeeKey = emp.EmployeeKey,
                                  EmployeeId = emp.EmployeeId,
                                  EmployeeName = (emp.EmpFirstName + emp.EmpLastName),
                                  ClockDate = Convert.ToDateTime(FilterClockDate).Date.ToString("yyyy-MM-dd"),
                                  TimeIn = g.TimeIn == null ? string.Empty : g.TimeIn.ToString("HH:mm"),
                                  TimeOut = g.TimeOut == null ? string.Empty : g.TimeOut.ToString("HH:mm"),
                                  Remarks = g.Remarks == null ? string.Empty : g.Remarks,
                                  CreationDate = g.CreationDate
                              }).ToListAsync();


                var a = await empAtt;
                if (a.Count > 0)
                {
                    return Ok(a);

                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }


       
        // GET: api/EmployeeAttendances/5
        [HttpGet("{id}")]
       
        public async Task<ActionResult<EmployeeAttendance>> GetEmployeeAttendance(long id)
        {
            var employeeAttendance = await _context.EmployeeAttendance.FindAsync(id);

            if (employeeAttendance == null)
            {
                return NotFound();
            }

            return employeeAttendance;
        }


      
        // PUT: api/EmployeeAttendances/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
       
        public async Task<IActionResult> PutEmployeeAttendance(long id, EmployeeAttendance employeeAttendance)
        {
            if (id != employeeAttendance.EmployeeKey)
            {
                return BadRequest();
            }

            _context.Entry(employeeAttendance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAttendanceExists(id))
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
        /// <summary>
        /// This method used to post the Employee Attendace 
        /// </summary>
        /// <param name="employeeAttendanceList"></param>
        /// <returns></returns>
        // POST: api/EmployeeAttendances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("PostAttendance")]
        [ProducesResponseType(typeof(EmployeeAttendance), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
       
        public async Task<ActionResult<EmployeeAttendance>> PostEmployeeAttendance(List<EmployeeAttendance> employeeAttendanceList)
        {
          
            try
            {
                foreach(var employeeAttendance in employeeAttendanceList)

                {
                    if(employeeAttendance.TimeIn> employeeAttendance.TimeOut)
                    {
                        return BadRequest();
                    }

                    bool isEmployeeKeyExists = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == employeeAttendance.EmployeeKey && e.ClockDate==employeeAttendance.ClockDate);
                    if (isEmployeeKeyExists == true)
                    {
                        employeeAttendance.ModifiedDate = DateTime.Now;
                        _context.Entry(employeeAttendance).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        employeeAttendance.ModifiedDate = DateTime.Now;
                        employeeAttendance.CreationDate = DateTime.Now;
                        _context.EmployeeAttendance.Add(employeeAttendance);
                        await _context.SaveChangesAsync();
                    }

                }
                 return Ok();
                

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        // DELETE: api/EmployeeAttendances/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeAttendance>> DeleteEmployeeAttendance(long id)
        {
            var employeeAttendance = await _context.EmployeeAttendance.FindAsync(id);
            if (employeeAttendance == null)
            {
                return NotFound();
            }

            _context.EmployeeAttendance.Remove(employeeAttendance);
            await _context.SaveChangesAsync();

            return employeeAttendance;
        }

        private bool EmployeeAttendanceExists(long id)
        {
            return _context.EmployeeAttendance.Any(e => e.EmployeeKey == id);
        }
    }
}
