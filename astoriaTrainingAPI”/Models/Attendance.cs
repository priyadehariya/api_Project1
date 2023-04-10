using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI_.Models
{
    public class Attendance
    {
        public long EmployeeKey { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ClockDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string Remarks { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
