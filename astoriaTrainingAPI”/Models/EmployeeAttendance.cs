using System;
using System.Collections.Generic;

namespace astoriaTrainingAPI_.Models
{
    public partial class EmployeeAttendance
    {
        public long EmployeeKey { get; set; }

        public DateTime ClockDate { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public string Remarks { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual EmployeeMaster EmployeeKeyNavigation { get; set; }
    }
}
