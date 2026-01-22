using cfg;
using SimpleJSON;
using UnityEngine;

public class DataMgr : BaseManager<DataMgr>
{
    // 全局配置表对象，其他脚本直接用这个就行
    public TbPlayerStateData playerStateData;

    private DataMgr()
    {

        // 1. 加载JSON文件 (切记：路径不加.json后缀)
        TextAsset json = Resources.Load<TextAsset>("Json/tbplayerstatedata");
        // 2. 解析JSON + 初始化配置表（核心一行，这就是加载方法）
        playerStateData = new TbPlayerStateData(JSON.Parse(json.text));
    }

    
}