using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2Lua
{
    class Program
    {
        private static string excelPath = "";
        private static string luaPath = "";
        private const string iniConfPath = "./Conf/Excel2LuaConf.ini";

        static void Main(string[] args)
        {
            if (ParseIniConfig())
            {
                FileExporter.ExportAllLuaFiles(excelPath, luaPath);
            }
            //Console.WriteLine("按任意键继续...");
            //Console.ReadKey();
        }

        /// <summary>
        /// 解析ini配置文件
        /// </summary>
        private static bool ParseIniConfig()
        {
            string section = "Excel2Lua配置";
            string excelkey = "EXCEL_PATH";
            string luakey = "LUA_PATH";
            string fullPath = Path.GetFullPath(iniConfPath);
            if (!File.Exists(iniConfPath))
            {
                Console.WriteLine("获取配置文件失败，请检查路径: {0} ", fullPath);
                return false;
            }
            Console.WriteLine("获取 fullPath 路径: {0} {1}", fullPath, CheckStrExist(fullPath));
            excelPath = IniConfHelper.ReadIniConf(section, excelkey, "", fullPath);
            luaPath = IniConfHelper.ReadIniConf(section, luakey, "", fullPath);
            Console.WriteLine("获取 excelPath 路径: {0} {1}", excelPath, CheckStrExist(excelPath));
            Console.WriteLine("获取 luaPath 路径: {0} {1}", luaPath, CheckStrExist(luaPath));
            return true;
        }

        /// <summary>
        /// 检测是否合法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string CheckStrExist(string str)
        {
            return string.IsNullOrEmpty(str) ? "失败" : "成功";
        }
    }
}
