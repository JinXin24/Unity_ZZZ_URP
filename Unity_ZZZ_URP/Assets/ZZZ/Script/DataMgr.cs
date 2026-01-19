using cfg;
using SimpleJSON;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    // 全局配置表对象，其他脚本直接用这个就行
    public static TbPlayerStates playerStatesCfg;

    void Start()
    {
        // 1. 加载JSON文件 (切记：路径不加.json后缀)
        TextAsset json = Resources.Load<TextAsset>("Json/TbPlayerStates");
        // 2. 解析JSON + 初始化配置表（核心一行，这就是加载方法）
        playerStatesCfg = new TbPlayerStates(JSON.Parse(json.text));

        foreach(var item in playerStatesCfg.DataList)
        {
            print($"{item.Id}:{item.Info}");
        }    
    }

    
}