using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Com.Core.DTO;

namespace E_Com.Core.interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);

        Task<string> LoginEmail(LoginDTO login);

        Task<bool> SendEmailForForgetPassword(string email);

        Task<string> Resetpassword(RestPassowrdDTO restPassowrd);

        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);

    }
}
