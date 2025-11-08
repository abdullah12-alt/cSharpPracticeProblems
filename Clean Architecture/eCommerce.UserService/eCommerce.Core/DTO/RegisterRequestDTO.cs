using eCommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO
{
 
    public record RegisterRequestDTO(string? Email, string? Password , string? PersonName, GenderOptions Gender);


}
