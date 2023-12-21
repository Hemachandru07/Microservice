﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Web.Models;
using MS.Web.Service.IService;
using MS.Web.Utility;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MS.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            ResponseDto? responseDto = await _authService.LoginAsync(requestDto);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto? loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto?.Message;
                return View(requestDto);
            }
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>() 
            {
                new SelectListItem{Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin},
                new SelectListItem{Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer}
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto requestDto)
        {
            ResponseDto? result = await _authService.RegisterAsync(requestDto);
            ResponseDto? assignRole;

            if(result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(requestDto.Role))
                {
                    requestDto.Role = StaticDetails.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(requestDto);
                if(assignRole != null && assignRole.IsSuccess) 
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result?.Message;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin},
                new SelectListItem{Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer}
            };

            ViewBag.RoleList = roleList;
            return View(requestDto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto responseDto)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(responseDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));
            
            identity.AddClaim(new Claim(ClaimTypes.Email,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}