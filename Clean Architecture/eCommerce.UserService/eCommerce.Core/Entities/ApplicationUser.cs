using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Gender { get; set; }


    }
}
