using JuntoSeguros.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuntoSeguros.Services
{
    public interface IUsuarioService
    {
        public Task<IdentityResult> CreateAsync(ApplicationUser user, string Password);
        public UserToken BuildToken(UserInfo userInfo);

        public Task<SignInResult> PasswordSignInAsync(UserInfo userInfo);

        public IQueryable<ApplicationUser> GetAllUser();

        public Task<ApplicationUser> FindUserByEmail(string email);

        public Task<ApplicationUser> GetById(string id);

        public Task<IdentityResult> UpdateAsync(ApplicationUser user);

        public Task<IdentityResult> UpdatePasswordUser(string id, string senhaAntiga, string novaSenha);

        public Task<IdentityResult> DeleteUser(string id);
    }
}
