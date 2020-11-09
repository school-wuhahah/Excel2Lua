
using XLua;
using System.Collections.Generic;
using System;

[LuaCallCSharp]
[ReflectionUse]
public static class UnityObjectExtention
{
    // 说明：lua侧判Object为空全部使用这个函数
    public static bool IsNull(this UnityEngine.Object o)
    {
        return o == null;
    }
}

#if UNITY_EDITOR
public static class UnityObjectExtentionExporter
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(UnityObjectExtention),
    };
}
#endif