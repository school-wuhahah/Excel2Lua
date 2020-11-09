using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2Lua
{
    /// <summary>
    /// 表格字段类型枚举
    /// </summary>
    public enum FiledType : byte
    {
        c_unkonw,
        c_number,
        c_string,
        c_table,
    }

    /// <summary>
    /// 表头字段描述
    /// </summary>
    public class ColoumnDes
    {
        public int index = -1;
        public string comment = "";
        public string typeStr = "";
        public string name = "";
        public FiledType type;

        public override String ToString()
        {
            return string.Format("index = {0}, comment = {1}, typeStr = {2}, name = {3}, type = {4}", index, comment, typeStr, name, type);
        }
    }

}
