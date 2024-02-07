using BookStore.API.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration iConfiguaration;

        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration iConfiguaration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.iConfiguaration = iConfiguaration;
        }
        public async Task<IdentityResult> SignUpAsync(SignupModel signupModel)
        {
            var user = new ApplicationUser()
            {
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Email = signupModel.EMail,
                UserName = signupModel.EMail
            };
            return await userManager.CreateAsync(user, signupModel.Password);
        }

        public async Task<string> LoginAsync(SigninModel signinModel)
        {
            var result = await signInManager.PasswordSignInAsync(signinModel.EMail, signinModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signinModel.EMail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(iConfiguaration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: iConfiguaration["JWT:ValidIssuer"],
                audience: iConfiguaration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
