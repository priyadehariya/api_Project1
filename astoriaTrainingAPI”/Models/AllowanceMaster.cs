using System;
using System.Collections.Generic;

namespace astoriaTrainingAPI_.Models
{
    public partial class AllowanceMaster
    {
        internal decimal amount;

        public AllowanceMaster()
        {
            EmployeeAllowanceDetail = new HashSet<EmployeeAllowanceDetail>();
        }

        public int AllowanceId { get; set; }
        public string AllowanceName { get; set; }
        public string AllowanceDescription { get; set; }
        public string EmployeeName { get; set; }
        public long EmployeeKey { get; set; }

        public DateTime ClockDate { get; set; }

        public virtual ICollection<EmployeeAllowanceDetail> EmployeeAllowanceDetail { get; set; }
       
        public int Allowance { get; internal set; }
    }
}
