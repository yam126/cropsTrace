using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;
using ZXing;
using ZXing.Common;
using ZXing.ImageSharp;
using ZXing.QrCode.Internal;

namespace Common
{
    public class QRCodeHelper
    {


        #region 普通二维码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">存储内容</param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        //public static Bitmap GetPTQRCode(string url, int pixel)
        //{
        //    QRCodeGenerator generator = new QRCodeGenerator();
        //    QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
        //    QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);
        //    Bitmap qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, true);
        //    return qrImage;
        //}
        #endregion

        #region 带logo的二维码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">存储内容</param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        //public static Bitmap GetLogoQRCode(string url, string logoPath, int pixel)
        //{
        //    QRCodeGenerator generator = new QRCodeGenerator();
        //    QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
        //    QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);
        //    Bitmap icon = new Bitmap(logoPath);
        //    Bitmap qrImage = qrcode.GetGraphic(pixel, System.Drawing.Color.Black, System.Drawing.Color.White, icon, 15, 6, true);
        //    #region 参数介绍
        //    //GetGraphic方法参数介绍
        //    //pixelsPerModule //生成二维码图片的像素大小 ，我这里设置的是5
        //    //darkColor       //暗色   一般设置为Color.Black 黑色
        //    //lightColor      //亮色   一般设置为Color.White  白色
        //    //icon             //二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
        //    //iconSizePercent  //水印图标的大小比例 ，可根据自己的喜好设置
        //    //iconBorderWidth  // 水印图标的边框
        //    //drawQuietZones   //静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
        //    #endregion
        //    return qrImage;
        //}

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="logoPath">中间logo的文件路径</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="text">二维码的内容</param>
        public static SixLabors.ImageSharp.Image GenerateQRCode(string text,string logoPath,int width,int height)
        {
            //Logo 图片
            SixLabors.ImageSharp.Image logo = null; 
            
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

            #region 生成二维码
            BitMatrix bm = writer.encode(
                text, 
                BarcodeFormat.QR_CODE, 
                width, 
                height, 
                hint);
            #endregion

            //生成原始二维码图片
            ZXing.ImageSharp.BarcodeWriter<Rgba32> barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>();
            SixLabors.ImageSharp.Image map = barcodeWriter.Write(bm);


            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            if (!string.IsNullOrEmpty(logoPath))
            {
                logo = SixLabors.ImageSharp.Image.Load(logoPath);
                #region 计算插入图片的大小和位置
                int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
                int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);
                int middleL = (map.Width - middleW) / 2;
                int middleT = (map.Height - middleH) / 2;
                //对logo图片调整大小
                logo.Mutate(mk =>
                {
                    mk.BackgroundColor(SixLabors.ImageSharp.Color.White);
                    mk.Resize(middleW, middleH);
                });
                #endregion

                #region 将img转换成bmp格式，否则后面无法创建Graphics对象
                //Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
                //Image<Rgba32> ;
                //using (Graphics g = Graphics.FromImage(bmpimg))
                //{
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //g.DrawImage(map, 0, 0);
                //}
                #endregion

                #region 将logo与二维码图片结合
                map.Mutate(oi =>
                {
                    SixLabors.ImageSharp.Point point = new SixLabors.ImageSharp.Point();
                    point.X = middleL;
                    point.Y = middleT;
                    oi.DrawImage(logo, point, 1);
                });
                #endregion
            }


            //将二维码插入图片
            //Graphics myGraphic = Graphics.FromImage(bmpimg);

            //白底
            //myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
            //myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);


            return map;
        }
        #endregion


    }
}
