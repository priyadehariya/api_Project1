using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI_.Models
{
    public class Employee
    {
        public long EmployeeKey { get; set; }
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string CompanyName { get; set; }

        public string DesignationName { get; set; }

        public DateTime JoiningDate { get; set; }

        public string Gender { get; set; }

        internal object ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
