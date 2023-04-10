using System;
using System.Collections.Generic;

namespace astoriaTrainingAPI_.Models
{
    public partial class EmployeeMaster
    {
        public EmployeeMaster()
        {
            EmployeeAllowanceDetail = new HashSet<EmployeeAllowanceDetail>();
            EmployeeAttendance = new HashSet<EmployeeAttendance>();
        }

        public long EmployeeKey { get; set; }
        public string EmployeeId { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public int? EmpCompanyId { get; set; }
        public int? EmpDesignationId { get; set; }
        public string EmpGender { get; set; }
        public DateTime EmpJoiningDate { get; set; }
        public DateTime? EmpResignationDate { get; set; }
        public decimal EmpHourlySalaryRate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual CompanyMaster EmpCompany { get; set; }
        public virtual DesignationMaster EmpDesignation { get; set; }
        public virtual ICollection<EmployeeAllowanceDetail> EmployeeAllowanceDetail { get; set; }
        public virtual ICollection<EmployeeAttendance> EmployeeAttendance { get; set; }
    }
}
