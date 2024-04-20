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
        try
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                return BadRequest(errors);
            }
            else
            {
                var result = await _identityService.SignUpAsync(model);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }        
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInModel model)
    {
        try
        {
            var result = await _identityService.SignInAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Sai tên đăng nhập hoặc mật khẩu!");
            }
            return Ok(result);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
