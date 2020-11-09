using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2Lua
{
    /// <summary>
    /// excel表格转换处理核心类
    /// </summary>
    class ExcelTransfer
    {
        /// <summary>
        /// Excel数组开始行数
        /// </summary>
        const int BGN_ROW = 5;

        /// <summary>
        /// 源字符组
        /// </summary>
        private static readonly char[] charSymbol = { '[', ']', '，' };

        /// <summary>
        /// 替换字符组
        /// </summary>
        private static readonly char[] insteadSymbol = { '{', '}', ','};

        /// <summary>
        /// 根据字符串返回对应的字段类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FiledType StringToFieldType(string str)
        {
            str = str.Trim().ToLower();
            switch (str)
            {
                case "number":
                    return FiledType.c_number;
                case "string":
                    return FiledType.c_string;
                case "table":
                    return FiledType.c_table;
                default:
                    break;
            }
            return FiledType.c_unkonw;
        }

        /// <summary>
        /// 根据字段类型，返回对应的字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FiledTypeToString(FiledType type)
        {
            switch (type)
            {
                case FiledType.c_number:
                    return "number";
                case FiledType.c_string:
                    return "string";
                case FiledType.c_table:
                    return "table";
                default:
                    break;
            }
            return "";
        }

        /// <summary>
        /// 获取表格的列数，表头碰到空白列直接中断
        /// </summary>
        /// <returns></returns>
        public static int GetSheetColonums(ISheet sheet)
        {
            int rowNum = sheet.LastRowNum;
            IRow firstRow = sheet.GetRow(0);
            if (firstRow != null)
            {
                return firstRow.LastCellNum;
            }
            return 0;
        }


        /// <summary>
        /// 获取表格行数
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static int GetSheetRows(ISheet sheet)
        {
            return sheet.LastRowNum;
        }

        /// <summary>
        /// 获取当前sheet的表头信息
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static List<ColoumnDes> GetColoumnDesc(ISheet sheet)
        {
            int coloumnCount = GetSheetColonums(sheet);
            List<ColoumnDes> coloumnDesList = new List<ColoumnDes>();
            for (int i = 1; i < coloumnCount; i++)
            {
                ColoumnDes coloumnDes = new ColoumnDes();
                coloumnDes.index = i;
                coloumnDes.comment = sheet.GetRow(1).GetCell(i).ToString().Trim();
                coloumnDes.name = sheet.GetRow(2).GetCell(i).ToString().Trim();
                coloumnDes.typeStr = sheet.GetRow(3).GetCell(i).ToString().Trim();
                coloumnDes.type = StringToFieldType(coloumnDes.typeStr);
                coloumnDesList.Add(coloumnDes);
            }
            return coloumnDesList;
        }

        /// <summary>
        /// 生成lua文件
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static string GenLuaFile(ISheet sheet, string fileName)
        {
            List<ColoumnDes> coloumnDesList = GetColoumnDesc(sheet);
            StringBuilder stringBuilder = new StringBuilder();
            if (null == coloumnDesList || coloumnDesList.Count <= 0)
            {
                return string.Empty;
            }
            stringBuilder.AppendFormat("--[[Notice: This lua config file is auto generate by {0}，don't modify it manually! --]]\n\n", fileName);

            //表头映射表
            Dictionary<int, FiledType> filedMap = new Dictionary<int, FiledType>();
            #region 转化变量名称
            stringBuilder.Append("local indexData = {\n");
            for (int i = 0; i < coloumnDesList.Count; i++)
            {
                ColoumnDes item = coloumnDesList[i];
                if (item != null && !string.IsNullOrEmpty(item.name))
                {
                    filedMap.Add(item.index, item.type);
                    stringBuilder.AppendFormat("\t{0} = {1}, --{2} \n", item.name, i + 1, item.comment);
                }
            }
            stringBuilder.Append("}\n\n");
            #endregion 转化变量名称

            #region 转化内容
            stringBuilder.Append("local data = {\n");
            for (int i = BGN_ROW; i <= GetSheetRows(sheet); i++)
            {
                string data = sheet.GetRow(i).GetCell(1).ToString().Trim();
                if (!string.IsNullOrEmpty(data))
                {
                    stringBuilder.AppendFormat("\t[{0}] = ", data);
                    stringBuilder.Append("{");
                    for (int j = 1; j < GetSheetColonums(sheet); j++)
                    {
                        string cellData = sheet.GetRow(i).GetCell(j).ToString().Trim();
                        if (!string.IsNullOrEmpty(cellData))
                        {
                            cellData = ParseCellDataByType(filedMap[j], cellData);
                            stringBuilder.AppendFormat("[{0}]={1},", j, cellData);
                        }
                    }
                    stringBuilder.Append("},\n");
                }
            }
            stringBuilder.Append("}\n\n");
            #endregion 转化内容

            #region lua matetable
            //设置元表
            string str =
              "local mt = {}\n" +
              "mt.__index = function(t,k)\n" +
              "\tif indexData[k] then\n" +
              "\t\treturn rawget(t,indexData[k]) \n" +
              "\tend\n" +
              "\treturn\n" +
              "end\n" +
              "mt.__newindex = function(t,k,v)\n" +
              "\terror('do not edit config')\n" +
              "end\n" +
              "mt.__metatable = false\n" +
              "for _,v in pairs(data) do\n\t" +
              "setmetatable(v,mt)\n" +
              "end\n\n" +
              "return data";
            stringBuilder.Append(str);
            #endregion
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 解析cell数据
        /// </summary>
        /// <param name="filedType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ParseCellDataByType(FiledType filedType, string data)
        {
            StringBuilder sb = new StringBuilder();
            switch (filedType)
            {
                case FiledType.c_unkonw:
                    return data;
                case FiledType.c_number:
                    return data;
                case FiledType.c_string:
                    sb.Append("\"" + data + "\"");
                    return sb.ToString();
                case FiledType.c_table:
                    return DealCellDataByTable(data);
                default:
                    return data;
            }
        }


        /// <summary>
        /// 处理table类型的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string DealCellDataByTable(string data)
        {
            for (int i = 0; i < charSymbol.Length; i++)
            {
                data = data.Replace(charSymbol[i], insteadSymbol[i]);
            }
            return data;
        }




    }


}
