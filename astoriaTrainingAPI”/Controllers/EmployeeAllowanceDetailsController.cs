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
    public class EmployeeAllowanceDetailsController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;
    

        public EmployeeAllowanceDetailsController(astoriaTraining80Context context)
        {
            _context = context;
        }

        // GET: api/EmployeeAllowanceDetails
       

        [HttpGet("AllowanceNameLists")]
        public async Task<ActionResult<IEnumerable<AllowanceMaster>>> AllowanceMaster()
        {
            try
            {
                return await _context.AllowanceMaster.ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }


        }


        //[HttpGet("UserInformation")]
        // public async Task<ActionResult<bool>> GetUserinfo(string UserName, string Password)
        //{
        //    var User = await _context.UserInfo.AnyAsync(u => u.UserName == UserName && u.Password == Password);
        //    if (User != null)
        //    {
        //        return User;
        //    }
        //    else
        //    {
        //        return NoContent();
        //    }

        //}


     



        [HttpGet("PresentEmployees")]
        public async Task<ActionResult<IEnumerable<EmpAttendance>>>PresentEmployee()
        {

            var empAtt = (from emp in _context.EmployeeMaster
                         join att in _context.EmployeeAttendance.Where(x => x.ClockDate.Date ==( DateTime.Today).Date)
                         on emp.EmployeeKey equals att.EmployeeKey
                          select new EmpAttendance
                         {
                             EmployeeKey = emp.EmployeeKey,
                             EmployeeName = emp.EmpFirstName + " " + emp.EmpLastName,
                            

                         });

            return await empAtt.ToListAsync();
        }

        [HttpGet("EmployeesAddAllowance")]
        public async Task<ActionResult<IEnumerable<AllowanceMaster>>> EmployeeAllowance()
        {

            var empAtt = (from emp in _context.EmployeeAllowanceDetail.Where(x => x.ClockDate.Date != (DateTime.Today).Date)
                          join em in _context.EmployeeMaster
             on emp.EmployeeKey equals em.EmployeeKey
             select new AllowanceMaster
             {
                 EmployeeKey = emp.EmployeeKey,
                 EmployeeName = em.EmpFirstName + " " + em.EmpLastName,
                 Allowance = emp.AllowanceId,
                 ClockDate = emp.ClockDate,
                 amount = emp.AllowanceAmount

             });
            return await empAtt.ToListAsync();
          
        }


        [HttpPost("postEmployeeAllowance")]
        public async Task<ActionResult<bool>>PostEmployeeAllowances(List<EmployeeAllowanceDetail> AllowanceDetailLists)
        {
            try
            {

                foreach (EmployeeAllowanceDetail Allowance in AllowanceDetailLists)

                {

                    if (Allowance.AllowanceAmount > 0)
                    {
                        EmployeeAllowanceDetail empAllowance = _context.EmployeeAllowanceDetail.Where(e => e.EmployeeKey == Allowance.EmployeeKey
                        && e.ClockDate == Allowance.ClockDate && e.AllowanceId == Allowance.AllowanceId).FirstOrDefault();

                        if (empAllowance != null)
                        {
                            empAllowance.AllowanceAmount = Allowance.AllowanceAmount;
                            empAllowance.ModifiedDate = DateTime.Now;
                            _context.Entry(empAllowance).State = EntityState.Modified;

                        }
                        else
                        {

                            Allowance.CreationDate = Allowance.ModifiedDate = DateTime.Now;
                            _context.EmployeeAllowanceDetail.Add(Allowance);

                        }
                        await _context.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }
        // GET: api/EmployeeAllowanceDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeAllowanceDetail>> GetEmployeeAllowanceDetail(long id)
        {
            var employeeAllowanceDetail = await _context.EmployeeAllowanceDetail.FindAsync(id);

            if (employeeAllowanceDetail == null)
            {
                return NotFound();
            }

            return employeeAllowanceDetail;
        }

        // PUT: api/EmployeeAllowanceDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAllowanceDetail(long id, EmployeeAllowanceDetail employeeAllowanceDetail)
        {
            if (id != employeeAllowanceDetail.EmployeeKey)
            {
                return BadRequest();
            }

            _context.Entry(employeeAllowanceDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAllowanceDetailExists(id))
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

        // POST: api/EmployeeAllowanceDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmployeeAllowanceDetail>> PostEmployeeAllowanceDetail(EmployeeAllowanceDetail employeeAllowanceDetail)
        {
            _context.EmployeeAllowanceDetail.Add(employeeAllowanceDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeAllowanceDetailExists(employeeAllowanceDetail.EmployeeKey))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeAllowanceDetail", new { id = employeeAllowanceDetail.EmployeeKey }, employeeAllowanceDetail);
        }

        // DELETE: api/EmployeeAllowanceDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeAllowanceDetail>> DeleteEmployeeAllowanceDetail(long id)
        {
            var employeeAllowanceDetail = await _context.EmployeeAllowanceDetail.FindAsync(id);
            if (employeeAllowanceDetail == null)
            {
                return NotFound();
            }

            _context.EmployeeAllowanceDetail.Remove(employeeAllowanceDetail);
            await _context.SaveChangesAsync();

            return employeeAllowanceDetail;
        }

        private bool EmployeeAllowanceDetailExists(long id)
        {
            return _context.EmployeeAllowanceDetail.Any(e => e.EmployeeKey == id);
        }
    }

   
  
}
