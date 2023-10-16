using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Models
{
    public class PasswordHashAndSalt
    {
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
    }
}
