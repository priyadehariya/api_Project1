using System;
using System.Collections.Generic;

namespace astoriaTrainingAPI_.Models
{
    public partial class CompanyMaster
    {
        public CompanyMaster()
        {
            EmployeeMaster = new HashSet<EmployeeMaster>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<EmployeeMaster> EmployeeMaster { get; set; }
    }
}
