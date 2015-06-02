using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Utilities.Log;

namespace Utilities.Dm
{
    public static class DmExtensions
    {
        /// <summary>
        /// 将以，号分隔的字符串转换成int列表
        /// </summary>
        /// <param name="hwnds"></param>
        /// <returns></returns>
        public static List<int> GetHwnds(this DmPlugin dm,string hwnds)
        {
            List<int> hList = new List<int>();
            if (string.IsNullOrEmpty(hwnds))
                return hList;
            string[] temp = hwnds.Split(',');
            foreach (string s in temp)
            {
                hList.Add(Convert.ToInt32(s));
            }
            return hList;
        }

        /// <summary>
        /// 获取以逗号分隔的字符串句柄的第一个句柄
        /// </summary>
        /// <param name="hwnds"></param>
        /// <returns>-1为空</returns>
        public static int GetHwnd(this DmPlugin dm,string hwnds)
        {

            if (string.IsNullOrEmpty(hwnds))
                return -1;
            string[] temp = hwnds.Split(',');
            return Convert.ToInt32(temp[0]);
        }

       

        /// <summary>
        /// 找图，返回图片坐标 比如"0|100|200"
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="pic_name"></param>
        /// <param name="Second">如果true,则1秒扫5次</param>
        /// <returns>返回空表示没找到图片</returns>
        public static string FindPicE(this DmPlugin dm, int x1, int y1, int x2, int y2, string pic_name, bool Second = true)
        {
            string picList = "";
            int index = 0;
            if (Second == true)
            {
                while (Second == true)
                {
                    picList = dm.FindPicE(x1, y1, x2, y2, pic_name, "000000", 0.9, 0);
                    if (picList != "-1|-1|-1")
                    {
                        return picList;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(200);
                        index = index + 1;
                        if (index == 5)
                        {
                            return "";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return "";
            }
            else
            {
                picList = dm.FindPicE(x1, y1, x2, y2, pic_name, "000000", 0.9, 0);
                if (picList != "-1|-1|-1")
                {
                    return picList;
                }
                else
                {
                    return picList;
                }
            }
        }

        /// <summary>
        /// 找图单击至找不到
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="pic_name"></param>
        /// <param name="ran1">随机数X=随机数(0,最大坐标-找到的X坐标)</param>
        /// <param name="ran2">随机数Y=随机数(0,最大坐标-找到的Y坐标)</param>
        /// <returns>返回1成功,0失败</returns>
        public static bool FindPicE_LeftClick_Clear(this DmPlugin dm, int x1, int y1, int x2, int y2, string pic_name, int ran1, int ran2, double sim = 0.9)
        {
            int PicX = 0;//图片左上角X
            int PicY = 0;//图片左上角Y
            string picList = "";
            picList = dm.FindPicE(x1, y1, x2, y2, pic_name, "000000", sim, 0);
            if (picList != "-1|-1|-1")
            {
                while (true)
                {
                    picList = dm.FindPicE(x1, y1, x2, y2, pic_name, "000000", sim, 0);
                    if (picList != "-1|-1|-1")
                    {
                        Logger.Info("找到图片" + pic_name);
                        if (picList.Equals("")) throw new Exception("请先进行注册!");
                        string[] picList_Split = picList.Split('|');
                        PicX = int.Parse(picList_Split[1]);
                        PicY = int.Parse(picList_Split[2]);
                        dm.MoveTo(PicX + ran1, PicY + ran2);
                        dm.LeftClick();
                        dm.MoveTo(1, 1);
                    }
                    else
                    {
                        return true; 
                    }
                    System.Threading.Thread.Sleep(dm.RanNumber(500, 600));
                }
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// 找图并单击，失败则延时1秒重试，直到超过最大次数,勿在循环中用
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="pic_name"></param>
        /// <param name="maxCount"></param>
        /// <param name="ran1"></param>
        /// <param name="ran2"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static bool FindPicE_LeftClick_Ex(this DmPlugin dm, int x1, int y1, int x2, int y2, string pic_name, int maxCount = 10, int ran1 = 0, int ran2 = 0, bool Second = true)
        {
            int count = 0;
            bool ret;
            while (count < maxCount)
            {
                ret = dm.FindPicE_LeftClick(x1, y1, x2, y2, pic_name, ran1, ran2, Second);
                if (ret == true)
                {
                    return true;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }

            }
            return false;

        }

        /// <summary>
        /// 返回识别到的字符串,1秒扫5次
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color_format"></param>
        /// <param name="sim"></param>
        /// <param name="Second">如果true,则1秒扫5次</param>
        /// <returns>如果为空说明没找到</returns>
        public static string OCR(this DmPlugin dm, int x1, int y1, int x2, int y2, string color_format, double sim, bool Second = true)
        {
            string s = "";
            int index = 0;
            if (Second)
            {
                while (Second == true)
                {
                    s = dm.Ocr(x1, y1, x2, y2, color_format, sim);
                    if (s.Length > 0)
                    {
                        return s;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(200);
                        index = index + 1;
                        if (index == 5)
                        {
                            return "";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return "";
            }
            else
            {
                s = dm.Ocr(x1, y1, x2, y2, color_format, sim);
                if (s.Length > 0)
                {
                    return s;
                }
                else
                {
                    return "";
                }
            }
        }

      
        /// <summary>
        /// 找图并找到的坐标并左键单击
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="pic_name"></param>
        /// <param name="ran1">随机数X=随机数(0,最大坐标-找到的X坐标)</param>
        /// <param name="ran2">随机数Y=随机数(0,最大坐标-找到的Y坐标)</param>
        /// <param name="Second">如果true,则1秒扫5次</param>
        /// <returns>返回1成功,0失败</returns>
        public static bool FindPicE_LeftClick(this DmPlugin dm, int x1, int y1, int x2, int y2, string pic_name, int ran1 = 0, int ran2 = 0, bool Second = true)
        {
            string picList = "";
            int index = 0;
            int PicX = 0;//图片左上角X
            int PicY = 0;//图片左上角Y
            if (Second == true)
            {
                while (Second == true)
                {
                    picList = dm.FindPicE(x1, y1, x2, y2, pic_name, "000000", 0.9, 0);
                    if (picList != "-1|-1|-1")
                    {
                        string[] picList_Split = picList.Split('|');
                        PicX = int.Parse(picList_Split[1]);
                        PicY = int.Parse(picList_Split[2]);
                        dm.MoveTo(PicX + ran1, PicY + ran2);
                        System.Threading.Thread.Sleep(dm.RanNumber(50, 100));
                        dm.LeftClick();
                        return true;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(200);
                        index = index + 1;
                        if (index == 5)
                        {
                            Logger.Info("未找到图片:"+pic_name);
                            return false;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                Logger.Info("未找到图片:" + pic_name);
                return false;
            }
            else
            {
                picList = dm.FindPicE(x1, y1, x2, y2, pic_name, "000000", 0.9, 0);
                if (picList != "-1|-1|-1")
                {
                    string[] picList_Split = picList.Split('|');
                    PicX = int.Parse(picList_Split[1]);
                    PicY = int.Parse(picList_Split[2]);
                    dm.MoveTo(PicX + ran1, PicY + ran2);
                    System.Threading.Thread.Sleep(dm.RanNumber(50, 100));
                    dm.LeftClick();
                    return true;
                }
                else
                {
                    Logger.Info("未找到图片:" + pic_name);
                    return false;
                }
            }
        }

        /// <summary>
        /// 找字并移动左键单击
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="str">要找的字</param>
        /// <param name="color_format">字的颜色</param>
        /// <param name="ran1">随机数X=随机数(0,最大坐标-找到的X坐标)</param>
        /// <param name="ran2">随机数Y=随机数(0,最大坐标-找到的Y坐标)</param>
        /// <param name="sim"></param>
        /// <returns>返回1成功,0失败</returns>
        public static bool FindStrE_LeftClick(this DmPlugin dm, int x1, int y1, int x2, int y2, string str, string color_format, int ran1 = 0, int ran2 = 0, double sim = 1.0)
        {
            string s = "";
            string picX, picY;
            s = dm.FindStrE(x1, y1, x2, y2, str, color_format, sim);
            if (s != "-1|-1|-1")
            {
                string[] picList_Split = s.Split('|');
                picX = picList_Split[1];
                picY = picList_Split[2];
                int x, y;
                x = int.Parse(picX);
                y = int.Parse(picY);
                dm.MoveTo(x + ran1, y + ran2);
                System.Threading.Thread.Sleep(dm.RanNumber(50, 100));
                dm.LeftClick();
                dm.MoveTo(0,0);
                return true;
            }
            else
            {
                Logger.Info("未找到字:"+str);
                return false;
            }
        }
        /// <summary>
        /// 找到某字符串后返回相同行ocr的字符串
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="ocrColor"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public static string FindStr_Ocr(this DmPlugin dm,int x1,int y1,int x2,int y2,string str,string color,int w,int h,string ocrColor,double sim=1.0)
        {
            int intX,intY;
            int ret=dm.FindStr(x1,y1,x2,y2,str,color,sim,out intX,out intY);
            if (intX >= 0 && intY >= 0)
            {
                return dm.OCR(intX, intY-5, intX + w, intY + h, ocrColor, 0.9);
            }
            else
            {
                Logger.Debug("未找到字:" + str);
                return "";
            }
        }
        /// <summary>
        /// 使用系统字库宋体9，查找字符串
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="intX"></param>
        /// <param name="intY"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public static bool FindStrWithFont_St9(this DmPlugin dm, int x1, int y1, int x2, int y2, string str, string color, out int intX, out int intY, double sim = 1.0)
        {
            int ret=dm.FindStrWithFont(x1,y1,x2,y2,str,color,sim,"宋体",9,0,out intX,out intY);
            if (ret == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 使用系统字库宋体9下划线，查找字符串
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="intX"></param>
        /// <param name="intY"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public static bool FindStrWithFont_St9_(this DmPlugin dm, int x1, int y1, int x2, int y2, string str, string color, out int intX, out int intY, double sim = 1.0)
        {
            int ret=dm.FindStrWithFont(x1, y1, x2, y2, str, color, sim, "宋体", 9, 4, out intX, out intY);
            if (ret == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 使用系统字库宋体9下划线，查找字符串，返回是否存在
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public static bool FindStrWithFont_St9_(this DmPlugin dm, int x1, int y1, int x2, int y2, string str, string color,double sim = 1.0)
        {
            int intX, intY;
            dm.FindStrWithFont(x1, y1, x2, y2, str, color, sim, "宋体", 9, 4, out intX, out intY);
            if(intX>=0&&intY>=0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 使用系统字库宋体9下划线，查找字符串，并点击
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public static bool FindStrWithFont_LeftClick_St9_(this DmPlugin dm, int x1, int y1, int x2, int y2, string str, string color,int w,int h,double sim = 1.0)
        {
            int intX, intY;
            dm.FindStrWithFont(x1, y1, x2, y2, str, color, sim, "宋体", 9, 4, out intX, out intY);
            if (intX >= 0 && intY >= 0)
            {
                dm.MoveTo(intX+w,intY+h);
                dm.LeftClick();
                return true;
            }
            return false;
        }

        public static bool FindStr(this DmPlugin dm, int x1, int y1, int x2, int y2, string str, string color,double sim = 1.0)
        {
            int intX, intY;
            dm.FindStr(x1,y1,x2,y2,str,color,sim,out intX,out intY);
            if (intX >= 0 && intY >= 0)
                return true;
            else
                return false;
        }
      
        /// <summary>
        /// 找字单击至找不到
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="pic_name"></param>
        /// <param name="ran1">随机数X=随机数(0,最大坐标-找到的X坐标)</param>
        /// <param name="ran2">随机数Y=随机数(0,最大坐标-找到的Y坐标)</param>
        /// <returns>返回1成功,0失败</returns>
        public static bool FindStrE_LeftClick_Clear(this DmPlugin dm, int x1, int y1, int x2, int y2, string str,string color,int ran1, int ran2,double sim=1.0 )
        {
            int strX = 0;
            int strY = 0;
            string strList = "";
            strList = dm.FindStrE(x1, y1, x2, y2, str, color, sim);
            if (strList != "-1|-1|-1")
            {
                while (true)
                {
                    strList = dm.FindStrE(x1, y1, x2, y2, str, color, sim);
                    if (strList != "-1|-1|-1")
                    {
                        Logger.Info("找到字" + str);
                        if (strList.Equals("")) throw new Exception("请先进行注册!");
                        string[] picList_Split = strList.Split('|');
                        strX = int.Parse(picList_Split[1]);
                        strY = int.Parse(picList_Split[2]);
                        dm.MoveTo(strX + ran1, strY + ran2);
                        dm.LeftClick();
                        dm.MoveTo(1, 1);
                    }
                    else
                    {
                        return true;
                    }
                    System.Threading.Thread.Sleep(dm.RanNumber(500, 600));
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 利用 GetScreenData 截图，并体现在 pictureBox 上
        /// </summary>
        /// <param name="dm">大漠对象</param>
        /// <param name="x1">截图左上角X</param>
        /// <param name="y1">截图左上角Y</param>
        /// <param name="x2">截图右下角X</param>
        /// <param name="y2">截图右下角Y</param>
        /// <returns></returns>
        public static Bitmap GetBitMap(this DmPlugin dm, int x1, int y1, int x2, int y2)
        {
            int bmpdata = dm.GetScreenData(x1, y1, x2, y2);
            Logger.Info(bmpdata);
            int width = x2 - x1;
            int size = (x2 - x1) * (y2 - y1);
            Bitmap bmp = new Bitmap(x2 - x1, y2 - y1);
            Color c;
            for (int i = 0; i < size; i++)
            {
                int bb = System.Runtime.InteropServices.Marshal.ReadByte((IntPtr)(bmpdata + i * 4));
                int gg = System.Runtime.InteropServices.Marshal.ReadByte((IntPtr)(bmpdata + i * 4 + 1));
                int rr = System.Runtime.InteropServices.Marshal.ReadByte((IntPtr)(bmpdata + i * 4 + 2));
                c = Color.FromArgb(rr, gg, bb);
                //备用方案 上述方案生成不成功选用下面的
                //int cc = System.Runtime.InteropServices.Marshal.ReadInt32((IntPtr)(bmpdata + i * 4));
                //c = Color.FromArgb(cc);
                bmp.SetPixel(i % width, i / width, c);
            }

           // System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)bmpdata);
            return bmp;
        }

      
       
        /// <summary>
        /// 二值化Bitmap
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="filePath"></param>
        /// <param name="newFilePath"></param>
        public static void Binarization(this DmPlugin dm,string filePath, string newFilePath)
        {
            Bitmap btmp = new Bitmap(filePath);

            BitmapData btmpd = btmp.LockBits(new Rectangle(0, 0, btmp.Width, btmp.Height), ImageLockMode.ReadWrite, btmp.PixelFormat);

            //32位深度为4,24位为3
            int PixelSize = 3;
            unsafe
            {
                for (int i = 0; i < btmp.Height; i++)
                {
                    for (int j = 0; j < btmp.Width; j++)
                    {
                        // ded8c7
                        byte* row = (byte*)btmpd.Scan0 + PixelSize * j + (i * btmpd.Stride);

                        if (row[0] + row[1] + row[2] > 382)
                        {
                           // if(row[0]!=(int)0xde&&row[1]!=(int)0xd8&&row[2]!=(int)0xc7)
                            row[0] = row[1] = row[2] = 255;
                            continue;
                        }
                        row[0] = row[1] = row[2] = 0;
                    }
                }
            }
            btmp.Save(newFilePath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        /// <param name="word"></param>
        /// <param name="mis"></param>
        /// <returns>0为改变 1改变 -1识别为空</returns>
        public static int InfoIsChanged(this DmPlugin dm,int x1,int y1,int x2,int y2,string color,string word ,int mis)
        {
            string dm_ret = dm.FetchWord(x1,y1,x2,y2,color,word);
            dm.Delay(mis);
            string dm_ret2 = dm.FetchWord(x1,y1,x2,y2,color,word);
            if (dm_ret.Equals("") || dm_ret2.Equals(""))
                return -1;
            if (!dm_ret.Equals(dm_ret2))
                return 1;
            return 0;
        }
       

       
        /// <summary>
        /// 防封，移动鼠标单击
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool PMoveToClick(this DmPlugin dm, int x, int y)
        {
            int flag = dm.MoveTo( x, y);
             dm.Delay(dm.RanNumber(50,200));

            int a=dm.LeftDown();
            dm.Delay(dm.RanNumber(50,200));
            int sjx = dm.RanNumber(1,5);
            int sjy = dm.RanNumber(1,5);
            int b=dm.MoveTo(x+sjx,y+sjy);
            int c=dm.LeftUp();
            return a==1 && b==1 && c==1&&flag==1 ? true : false;
        }
    }
}
