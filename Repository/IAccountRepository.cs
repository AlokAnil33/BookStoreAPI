using BookStore.API.DataModels;
using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignupModel signupModel);
        Task<string> LoginAsync(SigninModel signinModel);
    }
}
