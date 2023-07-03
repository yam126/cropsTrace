using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace cropsTrace.Controllers
{
    /// <summary>
    /// 生成二维码的控制器
    /// </summary>
    public class QRCodeController : Controller
    {
        #region Fields

        /// <summary>
        /// 公司logo路径
        /// </summary>
        private readonly string logoPathUrl = @"images\logo.png";

        /// <summary>
        /// 获取网站路径
        /// </summary>
        private readonly IWebHostEnvironment m_webHostEnvironment;
        #endregion

        #region Constructors

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="repository">数据库操作类</param>
        public QRCodeController(IWebHostEnvironment webHostEnvironment)
        {
            m_webHostEnvironment = webHostEnvironment;
        }
        #endregion

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="PumpHouseId">泵房编号</param>
        /// <param name="CropsId">农作物编号</param>
        /// <param name="Year">年份</param>
        /// <returns></returns>

        
        //public IActionResult GetPTQRCode(string url, int pixel = 5)
        //{
        //    url = HttpUtility.UrlDecode(url);
        //    Response.ContentType = "image/jpeg";
        //    var bitmap = QRCodeHelper.GetPTQRCode(url, pixel);
        //    MemoryStream ms = new MemoryStream();
        //    bitmap.Save(ms, ImageFormat.Jpeg);
        //    return File(ms.ToArray(), "image/png");
        //}

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="year">年份</param>
        /// <param name="cropsId">农作物编号</param>
        /// <param name="pumpHouseID">泵房编号</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public IActionResult GetLogoQRCode(
            string companyId,
            string year,
            string cropsId,
            string pumpHouseID,
            int width=300,
            int height=300
            )
        {
            IActionResult result = null;
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();                
                string apiUrlHost = configuration["ApiHost:Url"];
                string logoPath = $"/images/logo.png";
                string url = $"/Mobile/MobilePage?companyId={companyId}&year={year}&cropsId={cropsId}&pumpHouseID={pumpHouseID}";
                var urlHeader = $"{apiUrlHost}";
                if (string.IsNullOrEmpty(url))
                    url = "null";
                url = urlHeader + HttpUtility.UrlDecode(url);
                logoPath = m_webHostEnvironment.WebRootPath + HttpUtility.UrlDecode(logoPath);
                Response.ContentType = "image/jpeg";
                var bitmap = QRCodeHelper.GenerateQRCode(url, logoPath, width,height);
                MemoryStream ms = new MemoryStream();
                IImageEncoder imageEncoder = new JpegEncoder();
                bitmap.Save(ms, imageEncoder);
                result = File(ms.ToArray(), "image/jpeg");
            }
            catch(Exception exp) 
            {
                Response.ContentType = "text/html";
                result = Content($"[\"errorMsg\":\"{exp.Message}\",\"trace\":\"{exp.StackTrace}\"");
            }
            return result;
        }

        public IActionResult MarkQRCode(
            string text,
            int width = 300,
            int height = 300
            ) 
        {
            IActionResult result = null;
            try
            {
                Response.ContentType = "image/jpeg";
                var bitmap = QRCodeHelper.GenerateQRCode(text, string.Empty, width, height);
                MemoryStream ms = new MemoryStream();
                IImageEncoder imageEncoder = new JpegEncoder();
                bitmap.Save(ms, imageEncoder);
                result = File(ms.ToArray(), "image/jpeg");
            }
            catch(Exception exp) 
            {
                Response.ContentType = "text/html";
                result = Content($"[\"errorMsg\":\"{exp.Message}\",\"trace\":\"{exp.StackTrace}\"");
            }
            return result;
        }
    }
}
