using System;
using System.Collections.Generic;

namespace astoriaTrainingAPI_.Models
{
    public partial class EmployeeAllowanceDetail
    {
        public long EmployeeKey { get; set; }
        public int AllowanceId { get; set; }
        public DateTime ClockDate { get; set; }
        public decimal AllowanceAmount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        //public string AllowanceName { get; set; }


        public virtual AllowanceMaster Allowance { get; set; }
        public virtual EmployeeMaster EmployeeKeyNavigation { get; set; }
    }
}
