﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Common.VNPAY_CS_ASPX;
using Application.Users.Commands;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Common;
using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.QuestionLevels.Queries;
using Domain.Entities.UserEntities;
using Application.Topics.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Parents;
using Application.ApplicationUsers.Queries;
using Application.ApplicationUsers;
using Application.TeachingSlots.Queries;
using Application.Chapters.Commands;
using Application.ApplicationUsers.Commands;
using Org.BouncyCastle.Asn1.X9;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.UserRoute)]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;


    public UserController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<BaseResponse<GetApplicationUserResponseModel>> UserInfoById(ISender sender, [FromRoute] string id)
    {
        var query = new GetApplicationUserQuery
        {
            UserId = id,
        };
        return await sender.Send(query);
    }
    [HttpGet("calender/{id}")]
    public async Task<IActionResult> GetCalender(ISender sender, [FromRoute] string id)
    {
        var result = await sender.Send(new GetPaginatedListTeachingSlotByUserIdQuery() with
        {
            Id = id
        });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
    [HttpPost]
    //[Route(RouteNameValues.Register, Name = RouteNameValues.Register)]
    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> RegisterAccount(ISender sender, [FromBody] RegisterUserCommand command)
    {
        return await sender.Send(command);
    }
    [HttpPut]
    //[Route(RouteNameValues.Register, Name = RouteNameValues.Register)]
    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> UpdateAccount(ISender sender, [FromBody] UpdateUserCommand command)
    {

        return await sender.Send(command);
    }
    [HttpGet("info")]
    public async Task<BaseResponse<GetUserInfoResponseModel>> UserInfo(ISender sender)
    {
        // get claim from user object
        var claimIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new GetUserInfoQuery
        {
            UserId = userId,
        };
        return await sender.Send(query);
    }

    public class OrderInfo
    {
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string OrderDesc { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }


    }
    [HttpPost("payment")]
    public IActionResult Payment()
    {
        
        //Get Config Info
        string vnp_Returnurl = _configuration.GetValue<string>("VnPay:vnp_Returnurl");
        string vnp_Url = _configuration.GetValue<string>("VnPay:vnp_Url");
        string vnp_TmnCode = _configuration.GetValue<string>("VnPay:vnp_TmnCode");
        string vnp_HashSecret = _configuration.GetValue<string>("VnPay:vnp_HashSecret");

        //Get payment input
        OrderInfo order = new OrderInfo();
        order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
        order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
        order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
        order.CreatedDate = DateTime.Now;
        //Save order to db

        //Build URL for VNPAY
        VnPayLibrary vnpay = new VnPayLibrary();

        vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
        vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");

        /*        if (bankcode_Vnpayqr.Checked == true)
                {
                    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
                }
                else if (bankcode_Vnbank.Checked == true)
                {
                    vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                }
                else if (bankcode_Intcard.Checked == true)
                {
                    vnpay.AddRequestData("vnp_BankCode", "INTCARD");
                }*/
        var ipAddress = (Request.Headers["HTTP_X_FORWARDED_FOR"].IsNullOrEmpty()) 
            ? Request.Headers["HTTP_X_FORWARDED_FOR"] 
            :  Request.Headers["REMOTE_ADDR"];

        vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_IpAddr", ipAddress);
        vnpay.AddRequestData("vnp_Locale", "vn");
        /*        if (locale_Vn.Checked == true)
                {
                    vnpay.AddRequestData("vnp_Locale", "vn");
                }
                else if (locale_En.Checked == true)
                {
                    vnpay.AddRequestData("vnp_Locale", "en");
                }*/
        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
        vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

        vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

        //Add Params of 2.1.0 Version
        //Billing

        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

        return Redirect(paymentUrl);
    }

}
