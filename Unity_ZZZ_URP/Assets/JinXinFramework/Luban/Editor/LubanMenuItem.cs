using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    [MenuItem("JinXinFramework/Luban/执行LubanBat")]
    public static void ExecuteBat()
    {
        string batPath = Path.Combine(Application.dataPath, "../generate_table.bat");

        if (!File.Exists(batPath))
        {
            Debug.LogWarning($"不存在批处理文件: {batPath}");
            return;
        }

        using (Process process = new Process())
        {
            process.StartInfo.FileName = batPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                Debug.Log($"批处理输出: {output}");
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.Log($"批处理错误: {error}");
            }

            Debug.Log("批处理文件执行完毕");
            AssetDatabase.Refresh();

            Debug.Log("Json文件生成至 JinXinFramework/Resources/Json");
            Debug.Log("Table代码生成至 JinXinFramework/Script/Table");
        }
    }
}
