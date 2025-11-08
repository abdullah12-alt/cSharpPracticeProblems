using eCommerce.Core.DTO;
using eCommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts;

    public  interface IUserService
    {
       Task<AuthenticationResponse> Login(string Email, string Password);
       Task<AuthenticationResponse> Register(RegisterRequestDTO registerRequest);
    }

