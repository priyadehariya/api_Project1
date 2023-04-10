using astoriaTrainingAPI_.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly astoriaTraining80Context _context;
        public DashboardController(astoriaTraining80Context context)
        {
            _context = context;
        }


        [HttpGet("employeeCount")]
        public IEnumerable<int> GetEmployeeCount()
        {

            int EmployeeResignedCount = _context.EmployeeMaster.Where(emp => emp.EmpResignationDate < DateTime.Today || emp.EmpResignationDate != null).Count();

            int totalEmployeeCount = _context.EmployeeMaster.Count();
            int activeEmployeeCount = (totalEmployeeCount - EmployeeResignedCount);
            return new int[]
            {
                EmployeeResignedCount,
                activeEmployeeCount

            };

        }

        [HttpGet("employeeWorkingHours")]
        public IEnumerable<Dashboard> GetEmployeeWorkingHours()
        {
            try
            {
                var Emp_workingHour = _context.EmployeeAttendance
                    .GroupBy(C_date => C_date.ClockDate).Select(C_date => new Dashboard()
            {
                    ClockDate = C_date.Key.Date,
                    WorkingHours = C_date.Sum(C_date => C_date.TimeOut.Hour - C_date.TimeIn.Hour)
                     }).OrderByDescending(C_date => C_date.ClockDate).Take(5).ToList();
                return Emp_workingHour;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet("EmployeeSalary")]
        public IEnumerable<Dashboard> GetEmployeeSalary()
        {
            var emp_salary = (from ea in _context.EmployeeAttendance
                              join em in _context.EmployeeMaster
                              on ea.EmployeeKey equals em.EmployeeKey
                              select new { ea, em }).GroupBy(x => x.ea  .ClockDate).Select(x => new Dashboard
                              {
                                  ClockDate = x.Key.Date,
                                  Salary = x.Sum(x =>( x.ea.TimeOut.Hour - x.ea.TimeIn.Hour)* x.em.EmpHourlySalaryRate )
                              }).OrderByDescending(x => x.ClockDate).Take(5);
            var emp_Allowance = (from ea in _context.EmployeeAttendance
                                 join ead in _context.EmployeeAllowanceDetail on new { ea.ClockDate, ea.EmployeeKey } equals
                                 new { ead.ClockDate, ead.EmployeeKey }
                                 into grp
                                 from i in grp.DefaultIfEmpty()
                                 select new { ea, i }).GroupBy(x => x.ea.ClockDate).Select(x => new Dashboard
                                 {
                                     ClockDate = x.Key.Date,
                                     Salary = x.Sum(x => x.i.AllowanceAmount == null ? 0 : x.i.AllowanceAmount)
                                 }).OrderByDescending(x => x.ClockDate).Take(5);
            var CombinedSalary = (from s in emp_salary
                                  from a in emp_Allowance
                                  where s.ClockDate == a.ClockDate
                                  select new Dashboard()
                                  {
                                      ClockDate = s.ClockDate,
                                      Salary = s.Salary + a.Salary
                                  }
                                  ).ToList();

            return CombinedSalary;
                                 

        }

    }
}
