using System;
using System.Collections.Generic;

namespace astoriaTrainingAPI_.Models
{
    public partial class DesignationMaster
    {
        public DesignationMaster()
        {
            EmployeeMaster = new HashSet<EmployeeMaster>();
        }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<EmployeeMaster> EmployeeMaster { get; set; }
    }
}
