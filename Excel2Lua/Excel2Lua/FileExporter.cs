using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;

namespace Excel2Lua
{
    public class FileExporter
    {
        /// <summary>
        /// 清空导出的lua
        /// </summary>
        /// <param name="luaPath"></param>
        public static void ClearLuaDir(string luaPath)
        {
            if (!Directory.Exists(luaPath))
            {
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(luaPath);
            FileSystemInfo[] fsinfos = directoryInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)
                {
                    try
                    {
                        Directory.Delete(fsinfo.FullName, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("警告：目录删除失败 " + e.Message);
                    }
                }
                else
                {
                    try
                    {
                        File.Delete(fsinfo.FullName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("警告：文件删除失败 " + e.Message);
                    }
                }
                Console.WriteLine("删除成功 {0}", fsinfo.FullName);
            }
        }

        /// <summary>
        /// 导出所有的lua文件
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="luaPath"></param>
        public static void ExportAllLuaFiles(string excelPath, string luaPath)
        {
            ClearLuaDir(luaPath);
            List<string> allExcelList = Directory.GetFiles(excelPath, "*.*", SearchOption.AllDirectories).ToList();
            Console.WriteLine("开始转表.....");
            foreach (var itemExcel in allExcelList)
            {
                ExportSingleLuaFile(itemExcel, luaPath);
            }
            Console.WriteLine("结束转表.....");
        }


        public static void ExportSingleLuaFile(string ExcelName, string luaDir)
        {
            string extension = Path.GetExtension(ExcelName).ToLower();
            if (".xls" != extension && ".xlsx" != extension)
            {
                Console.WriteLine("{0} 表格式不符合要求", Path.GetFileName(ExcelName));
                return;
            }

            //打开文件流
            FileStream fs = null;
            try
            {
                fs = File.Open(ExcelName, FileMode.Open);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            if (null == fs) return;

            IWorkbook book = null;

            if (".xls" == extension)
            {
                book = new HSSFWorkbook(fs);
            }
            else
            {
                book = new XSSFWorkbook(fs);
            }
            fs.Close();

            if (book != null)
            {
                for (int i = 0; i < book.NumberOfSheets; i++)
                {
                    ISheet sheet = book.GetSheetAt(i);
                    if (!CheckSheetNameLegit(sheet.SheetName))
                    {
                        continue;
                    }
                    Console.WriteLine("转表中 {0} {1}", Path.GetFileName(ExcelName), sheet.SheetName);
                    string fileContent = ExcelTransfer.GenLuaFile(sheet, Path.GetFileName(ExcelName));

                    string outPath = Path.Combine(luaDir, sheet.SheetName + ".lua");
                    if (!string.IsNullOrEmpty(fileContent))
                    {
                        File.WriteAllText(outPath, fileContent);
                    }
                }
            }
        }

        /// <summary>
        /// 检测sheetName是否合法
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static bool CheckSheetNameLegit(string sheetName)
        {
            //如果是 sheet+数字 自动忽略
            if (Regex.IsMatch(sheetName, @"Sheet-?[1-9]\d*"))
            {
                return false;
            }
            return true;
        }

    }
}
