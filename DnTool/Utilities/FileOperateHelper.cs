
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace DnTool.Utilities
{
    public class FileOperateHelper
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>  
        ///     递归取得文件夹下文件  
        /// </summary>  
        /// <param name="dir"></param>  
        /// <param name="list"></param>  
        public static void GetFiles(string dir, List<string> list)
        {
            GetFiles(dir, list, new List<string>());
        }
        /// <summary>
        /// 返回当前目录符合搜索条件的所有文件完整路径
        /// </summary>
        /// <param name="dir">文件夹的路径</param>
        /// <param name="searchPattern">如 *.txt</param>
        public static List<string> GetFiles(string dir,string searchPattern)
        {
            List<string> list = new List<string>();
            try
            {
                if (!Directory.Exists(dir))
                    return list ;
                DirectoryInfo folder = new DirectoryInfo(dir);
                foreach (FileInfo file in folder.GetFiles(searchPattern))
                {
                    list.Add(file.FullName);
                } 
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return list;
        }
        /// <summary>
        ///文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>  
        ///     递归取得文件夹下文件  
        /// </summary>  
        /// <param name="dir"></param>  
        /// <param name="list"></param>  
        /// <param name="fileExtsions"></param>  
        public static void GetFiles(string dir, List<string> list, List<string> fileExtsions)
        {
            //添加文件   
            string[] files = Directory.GetFiles(dir);
            if (fileExtsions.Count > 0)
            {
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (extension != null && fileExtsions.Contains(extension))
                    {
                        list.Add(file);
                    }
                }
            }
            else
            {
                list.AddRange(files);
            }
            //如果是目录，则递归  
            DirectoryInfo[] directories = new DirectoryInfo(dir).GetDirectories();
            foreach (DirectoryInfo item in directories)
            {
                GetFiles(item.FullName, list, fileExtsions);
            }
        }



        /// <summary>
        /// 写入文件(一次性写入)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="isCreate">是否覆盖同名文件</param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, string content,bool isCreate)
        {
            try
            {
                if (File.Exists(filePath)&&!isCreate)
                    throw new Exception("写入失败：存在相同的文件名" );
                string dirPath=Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath))
                 {
                     //目标目录不存在则创建
                    try
                     {
                        Directory.CreateDirectory(dirPath);
                     }
                     catch (Exception ex)
                    {
                         throw new Exception("创建目标目录失败：" + ex.Message);
                     }
                }
                
               
                var fs = new FileStream(filePath, FileMode.Create);
                //获得字节数组  
                byte[] data = Encoding.Default.GetBytes(content);
                //开始写入  
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流  
                fs.Flush();
                fs.Close();
                return true;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>  
        /// 写入文件(一次性写入)
        /// </summary>  
        /// <param name="filePath">文件名</param>  
        /// <param name="content">文件内容</param>  
        public static void WriteFile(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                //获得字节数组  
                 byte[] data = Encoding.Default.GetBytes(content);
                //开始写入  
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流  
                fs.Flush();
                fs.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>  
        ///     读取文件  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <returns></returns>  
        public static string ReadFile(string filePath)
        {

            return ReadFile(filePath, Encoding.Default);
        }

        /// <summary>  
        ///     读取文件  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <param name="encoding"></param>  
        /// <returns></returns>  
        public static string ReadFile(string filePath, Encoding encoding)
        {
            using (var sr = new StreamReader(filePath, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>  
        ///     读取每一行的数据
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns>每一行的数据</returns>  
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    str.Add(input);
                }
            }
            return str;
        }

        /// <summary>  
        ///     复制文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="sourcePath">待复制的文件夹路径</param>  
        /// <param name="destinationPath">目标路径</param>  
        public static void CopyDirectory(String sourcePath, String destinationPath)
        {
            var info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo) //如果是文件，复制文件  
                    File.Copy(fsi.FullName, destName);
                else //如果是文件夹，新建文件夹，递归  
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        /// <summary>  
        ///     删除文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="directoryPath"></param>  
        public static void DeleteFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
            Directory.Delete(directoryPath); //删除空文件夹  
        }

        /// <summary>  
        ///     清空文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="directoryPath"></param>  
        public static void ClearFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
        }

        /// <summary>  
        ///     取得文件大小，按适当单位转换  
        /// </summary>  
        /// <param name="filepath"></param>  
        /// <returns></returns>  
        public static string GetFileSize(string filepath)
        {
            string result = "0KB";
            if (File.Exists(filepath))
            {
                var size = new FileInfo(filepath).Length;
                int filelength = size.ToString().Length;
                if (filelength < 4)
                    result = size + "byte";
                else if (filelength < 7)
                    result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
                else if (filelength < 10)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
                else if (filelength < 13)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
                else
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
                return result;
            }
            return result;
        }
    }
}  
