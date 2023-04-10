using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI_.Models
{
    public class EmpAttendance
    {
        public long EmployeeKey { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ClockDate { get; set; }
    }
}
