using System.Diagnostics.Metrics;
using BeanMind.Application.Common.Interfaces;
using BeanMind.Application.Common.Models;
using BeanMind.Infrastructure.Identity;
using BeanMind.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class AuthController : ApiControllerBase
{
    private IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpModel model)
    {
        var result = await _identityService.SignUpAsync(model);
        if (result.Succeeded)
        {
            return Ok(result.Succeeded);
        }
        return BadRequest("Đăng ký thất bại!");
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInModel model)
    {
        var result = await _identityService.SignInAsync(model);
        if (string.IsNullOrEmpty(result))
        {
            return BadRequest("Sai tên đăng nhập hoặc mật khẩu!");
        }
        return Ok(result);
    }
}
