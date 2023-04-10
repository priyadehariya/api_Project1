using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace astoriaTrainingAPI_.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeeMastersController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeMastersController(astoriaTraining80Context context)
        {
            _context = context;
        }
        /// <summary>
        /// This method returns all Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet("allemployee")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var emp =
                          (from em in _context.EmployeeMaster
                           join cm in _context.CompanyMaster on em.EmpCompanyId equals cm.CompanyId
                           join dm in _context.DesignationMaster on em.EmpDesignationId equals dm.DesignationId
                           select new Employee
                           {
                               EmployeeKey = em.EmployeeKey,
                               EmployeeId = em.EmployeeId,
                               EmployeeName = em.EmpFirstName + " " + em.EmpLastName,
                               CompanyName = cm.CompanyName,
                               DesignationName = dm.DesignationName,
                               JoiningDate = em.EmpJoiningDate,
                               Gender = em.EmpGender
                           }).ToListAsync();

                var Emp = await emp;
                if (Emp.Count > 0)
                {
                    return Ok(Emp);

                }

                return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: api/EmployeeMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployeeMaster()
        {
            var employees = await _context.EmployeeMaster.ToListAsync();
            return employees;
        }

        [HttpGet("EmployeeMasterList")]
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployeeMasterList()
        {
            var employees = await _context.EmployeeMaster.ToListAsync();
            return Ok();
        }

        [HttpGet("employeeCount")]
        public int GetEmployeeMasterCount()
        {
            int empCount = _context.EmployeeMaster.Count();
            return empCount;
        }

        [HttpGet("CheckEmployeeIDExists")]
        public async Task<ActionResult<bool>> GetEmployeeIdExists(string EmployeeID, long employeeKey)
        {
            bool IsEmployeeIDExists = await _context.EmployeeMaster.AnyAsync(e => e.EmployeeKey != employeeKey && e.EmployeeId.ToLower().Trim() == EmployeeID.ToLower().Trim());
            return IsEmployeeIDExists;
        }

        [HttpGet("CheckEmployeeInUse")]
        public async Task<ActionResult<bool>> GetEmployeeInUse(long employeeKey)
        {
            var employeeMaster = await _context.EmployeeMaster.FindAsync(employeeKey);
            if (employeeMaster == null)
            {
                return NotFound();
            }
            bool IsEmployeeInUse = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == employeeKey);
            if (!IsEmployeeInUse)
                IsEmployeeInUse = await _context.EmployeeAllowanceDetail.AnyAsync(e => e.EmployeeKey == employeeKey);
            return IsEmployeeInUse;
        }


        [HttpGet("Companydetails")]
        public async Task<ActionResult<IEnumerable<CompanyMaster>>> GetCompanyMaster()
        {
            if (_context.CompanyMaster.ToListAsync().Result.Count > 0){
                return await _context.CompanyMaster.ToListAsync();

            }
            else
            {
              return  NoContent();
            }

        }


        [HttpGet("designation")]
        public async Task<ActionResult<IEnumerable<DesignationMaster>>> GetDesignationMaster()
        {
            try
            {
                if (_context.DesignationMaster.ToListAsync().Result.Count > 0)
                {
                    return await _context.DesignationMaster.ToListAsync();
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

        /// <summary>
        /// This method return Employee passing EmployeeKey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeMaster), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployeeMaster(long id)
        {
            
            var employeeMaster = await _context.EmployeeMaster.FindAsync(id);
            if (employeeMaster == null)
            {
                return NotFound();
            }
            return Ok(employeeMaster);
        }

        /// <summary>
        /// This method used to Edit by passing EmployeeKey
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeMaster"></param>
        /// <returns></returns>

        // PUT: api/EmployeeMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutEmployeeMaster(long id, EmployeeMaster employeeMaster)
        {
            var IsEmpKeyExsists = _context.EmployeeMaster.Any(e => e.EmployeeKey == id);
            if (!IsEmpKeyExsists)
            {
                return NotFound();
            }

            bool ExistEmployeeID = _context.EmployeeMaster.Any(e => e.EmployeeId == employeeMaster.EmployeeId && e.EmployeeKey != employeeMaster.EmployeeKey);
           
            if (string.IsNullOrEmpty(employeeMaster.EmployeeId) ||
                   string.IsNullOrEmpty(employeeMaster.EmpFirstName) ||
                   string.IsNullOrEmpty(employeeMaster.EmpLastName) ||
                   string.IsNullOrEmpty(employeeMaster.EmpGender) ||
                   employeeMaster.EmployeeId.Length > 20 ||
                   employeeMaster.EmpFirstName.Length > 100 ||
                   employeeMaster.EmpLastName.Length > 100 ||
                   (employeeMaster.EmpJoiningDate>employeeMaster.EmpResignationDate)||
                   (id != employeeMaster.EmployeeKey))
            {
                return BadRequest();
            }
            if (ExistEmployeeID == true)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            employeeMaster.ModifiedDate = DateTime.Now;
            _context.Entry(employeeMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// This method used to post the employees
        /// </summary>
        /// <param name="employeeMaster"></param>
        /// <returns></returns>
        // POST: api/EmployeeMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeMaster), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<EmployeeMaster>> PostEmployeeMaster(EmployeeMaster employeeMaster)
        {
            try
            {
                if (employeeMaster.EmpJoiningDate > employeeMaster.EmpResignationDate)
                {
                    return BadRequest("joiningDate should be small");
                }
                if (_context.EmployeeMaster.Any(e => e.EmployeeId == employeeMaster.EmployeeId))
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                if (string.IsNullOrEmpty(employeeMaster.EmployeeId) ||
                    string.IsNullOrEmpty(employeeMaster.EmpFirstName) ||
                    string.IsNullOrEmpty(employeeMaster.EmpLastName) ||
                    string.IsNullOrEmpty(employeeMaster.EmpGender) ||
                    employeeMaster.EmployeeId.Length > 20 ||
                    employeeMaster.EmpFirstName.Length > 100 ||
                    employeeMaster.EmpLastName.Length > 100
                    
                    )

                {
                    return BadRequest();
                }
                employeeMaster.CreationDate = DateTime.Now;
                employeeMaster.ModifiedDate = DateTime.Now;
                _context.EmployeeMaster.Add(employeeMaster);
                await _context.SaveChangesAsync();
                return Ok(employeeMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete an employee permanantly from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        // DELETE: api/EmployeeMasters/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ActionResult<EmployeeMaster>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeMaster>> DeleteEmployeeMaster(long id)
        {
            var employeeMaster = await _context.EmployeeMaster.FindAsync(id);

            bool IsEmpKeyInUse = await _context.EmployeeAllowanceDetail.AnyAsync(e => e.EmployeeKey == id);
            bool IsEmployeeKeyInUse = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == id);

            if (employeeMaster == null)
            {
                return NotFound();
            }


            if (IsEmpKeyInUse || IsEmployeeKeyInUse)
            {
                return Conflict("Employee Key In Use");
            }

            _context.EmployeeMaster.Remove(employeeMaster);
            await _context.SaveChangesAsync();

            return Ok(employeeMaster);
        }
    
        private bool EmployeeMasterExists(long id)
        {
            return _context.EmployeeMaster.Any(e => e.EmployeeKey == id);
        }
    }
}
