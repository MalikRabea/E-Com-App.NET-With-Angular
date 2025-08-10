using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Com.Core.DTO;
using E_Com.Core.Entites;
using E_Com.Core.interfaces;
using E_Com.Core.Services;
using E_Com.Core.Sharing;
using Microsoft.AspNetCore.Identity;

namespace E_Com.infrastructure.Repositries
{
    public class AuthRepositry : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;

        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            { 
                return "Username already exists";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "Email already exists";
            }
            var user = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
            };
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded is not true)

            {
                return result.Errors.ToList()[0].Description;   
            }
            //Send Active Email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "please active your email ,click on button to active");
            return "done";
        }
        public async Task SendEmail(string email , string code , string component , string subject ,string message)
        {
            var result = new EmailDTO(email,
                "02205a3a53fcdf",
                subject
                , EmailStringBody.send(email, code, component, message));
            await emailService.SendEmail(result);
            
        }
        public async Task<string> LoginEmail(LoginDTO login)
        {
            if (login == null) 
            {
                return null;
            }
            var finduser = await userManager.FindByEmailAsync(login.Email);
            if (!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
                await SendEmail(finduser.Email, token, "active", "ActiveEmail", "please active your email ,click on button to active");
                return "Email not confirmed, please check your email for confirmation link.";
            }
            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.Password, true);
            if (result.Succeeded) {

                return generateToken.GetAndCreateToken(finduser);
            }
            return "Invalid email or password.";
        }
        public async Task<bool> SendEmailForForgetPassword(string email)
        { 
            var finduser = await userManager.FindByEmailAsync(email);
            if (finduser is null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(finduser);
            await SendEmail(finduser.Email, token, "Reset-Password", "Reset Password", " click on button to Reset your password ");
            return true;



        }
        public async Task<string> Resetpassword(RestPassowrdDTO restPassowrd)
        {
            var findUser = await userManager.FindByEmailAsync(restPassowrd.Email);
            if (findUser is null)
            {
                return null;
            }

            var result = await userManager.ResetPasswordAsync(findUser, restPassowrd.Token, restPassowrd.Password);

            if (result.Succeeded) {

                return "Password change sucess";
            }

            return result.Errors.ToList()[0].Description;
        
        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);
            if (findUser is null)
            {
                return false;
            }

            var reslt = await userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (reslt.Succeeded)
            
                return true;
            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);

            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "please active your email ,click on button to active");
            return false;

        }
    }
}
