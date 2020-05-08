using System;
using System.Collections.Generic;

namespace AspNetCore.HostedServices.EF
{
    public partial class Employees
    {
        public Employees()
        {
            Sales = new HashSet<Sales>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Sales> Sales { get; set; }
    }
}
