using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LubanMenuItem 
{
    [MenuItem("JinXinFramework/Luban/打开配置表存储路径")]
    public static void OpenTablePath()
    {
        string tablePath = Path.Combine(Application.dataPath, "../DataTables/Datas/");
        if (Directory.Exists(tablePath))
        {
            EditorUtility.RevealInFinder(tablePath);
        }
        else
        {
            Debug.LogWarning("打开路径失败");
        }
    }
}
