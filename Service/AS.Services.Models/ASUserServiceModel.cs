using AS.Data.Models;
using AutoMapperConfiguration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Services.Models
{
    public class ASUserServiceModel : IdentityUser, IMapFrom<ASUser>
    {
    }
}
