using Application.Common;
using Application.Courses;
using Application.Teachables;
using Application.Users.Commands;
using Domain.Entities.UserEntities;
using Infrastructure.Common.VNPAY_CS_ASPX;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.OrderRoute)]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;

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
        public sealed record PaymentCommand 
        {
            public string BankCode { get; set; }
            public int Money { get; set; }
            public string IpAddress { get; set; }
        }
        public sealed record PaymentCommandResponseModel
        {
            public string Url { get; set; }
        }
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentCommand paymentCommand)
        {
            //Get Config Info
            string vnp_Returnurl = _configuration.GetValue<string>("VnPay:vnp_Returnurl");
            string vnp_Url = _configuration.GetValue<string>("VnPay:vnp_Url");
            string vnp_TmnCode = _configuration.GetValue<string>("VnPay:vnp_TmnCode");
            string vnp_HashSecret = _configuration.GetValue<string>("VnPay:vnp_HashSecret");

            //Get payment input
            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = paymentCommand.Money; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", order.Amount.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", paymentCommand.BankCode);

            /*                    if (bankcode_Vnpayqr.Checked == true)
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
            /* var ipAddress = (Request.Headers["HTTP_X_FORWARDED_FOR"].IsNullOrEmpty()) 
                 ? Request.Headers["REMOTE_ADDR"]
                 : Request.Headers["HTTP_X_FORWARDED_FOR"];*/

            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);
            //vnpay.AddRequestData("vnp_IpAddr", "0.0.0.1");


            //var ipAddress1 = HttpContext.Request.Headers["x-forwarded-for"];
            //vnpay.AddRequestData("vnp_IpAddr", ipAddress1);
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            //vnpay.AddRequestData("vnp_IpAddr", paymentCommand.IpAddress);

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
            var response = new PaymentCommandResponseModel() { Url = paymentUrl };
            var response2 = new BaseResponse<PaymentCommandResponseModel>
            {
                Success = true,
                Message = "Create course successful",
                Data = response
            };
            return new ObjectResult(response2)
            {
                StatusCode = response2.Code
            };
        }
        private string GetIpAddress(HttpContext context)
        {
            var xForwardedForHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedForHeader))
            {
                return xForwardedForHeader.Split(',').First().Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}
