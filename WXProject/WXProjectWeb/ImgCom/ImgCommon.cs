using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WXProjectWeb.ImgCom
{
    public class ImgCommon
    {

        /// <summary>
        /// 生成分享图片
        /// </summary>
        /// <param name="backgroundImgStream">背景图stram</param>
        /// <param name="printingImgStream">头像流</param>
        /// <param name="QRcodeImgStream">二维码流</param>
        /// <param name="name">名字</param>
        /// <param name="content">内容</param>
        /// <returns>图像</returns>
        public static byte[] AddWaterPic(Stream backgroundImgStream, Stream printingImgStream, Stream QRcodeImgStream, string name = null, string content = null)
        {
            MemoryStream ms = new MemoryStream();

            using (Image backgroundImg = Image.FromStream(backgroundImgStream))
            {

                //实例化一块画布
                using (Graphics g = Graphics.FromImage(backgroundImg))
                {
                    //将水印文件加载到内存中
                    using (Image printingImg = Image.FromStream(printingImgStream))
                    {
                        var touxiang = printingImg.GetThumbnailImage(85, 85, null, IntPtr.Zero);
                        g.DrawImage(touxiang, new Rectangle(10, 10, touxiang.Width, touxiang.Height));
                    }
                    using (Image QRcodeImg = Image.FromStream(QRcodeImgStream))
                    {
                        var QRcode = QRcodeImg.GetThumbnailImage(85, 85, null, IntPtr.Zero);
                        g.DrawImage(QRcode, new Rectangle(backgroundImg.Width - QRcode.Width - 10, backgroundImg.Height - QRcode.Height - 10, QRcode.Width, QRcode.Height));

                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        g.DrawString(name, new Font(new FontFamily("Microsoft YaHei"), 20), Brushes.Black, 85 + 10 + 10, 15);

                    }
                    if (!string.IsNullOrEmpty(content))
                    {
                        g.DrawString(content, new Font(new FontFamily("Microsoft YaHei"), 20), Brushes.Black, 85 + 10 + 10, 50);

                    }

                    backgroundImg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var imagedata = ms.GetBuffer();
                    return imagedata;
                }

            }
        }
    }
}