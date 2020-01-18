using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JuntoSeguros.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }        
        public string Cidade { get; set; }
    }
}
