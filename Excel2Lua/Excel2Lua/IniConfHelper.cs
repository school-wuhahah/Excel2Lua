using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Excel2Lua
{
    class IniConfHelper
    {
        private const int INI_BUFFER_SIZE = 1024;

        /// <summary>
        /// ini文件中指定的节点的字符串
        /// </summary>
        /// <param name="lpAppName">节点名称</param>
        /// <param name="lpKeyName">项名</param>
        /// <param name="lpDefault">默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">INI文件完整路径</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符</returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 修改INI文件中内容
        /// </summary>
        /// <param name="lpApplicationName">节点名称</param>
        /// <param name="lpKeyName">项名称</param>
        /// <param name="lpString">写入字符串</param>
        /// <param name="lpFileName">ini文件完整路径</param>
        /// <returns>非零表示成功，零表示失败</returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);


        /// <summary>
        /// 读取ini文件的值
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">项名称</param>
        /// <param name="def">默认名称</param>
        /// <param name="filePath">ini文件路径</param>
        /// <returns>ini文件的内容</returns>
        public static string ReadIniConf(string section, string key, string def, string filePath)
        {
            StringBuilder sb = new StringBuilder(INI_BUFFER_SIZE);
            GetPrivateProfileString(section, key, def, sb, INI_BUFFER_SIZE, filePath);
            return sb.ToString();
        }

        /// <summary>
        /// 写入ini文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">项名称</param>
        /// <param name="data">数据</param>
        /// <param name="filePath">ini文件路径</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteIniConf(string section, string key, string data, string filePath)
        {
            int isSucc = WritePrivateProfileString(section, key, data, filePath);
            return isSucc == 0  ? false : true;
        }

    }
}
