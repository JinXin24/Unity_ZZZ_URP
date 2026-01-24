using cfg;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "配置/创建状态配置")]
public class StateScriptableObject : ScriptableObject, ISerializationCallbackReceiver
{
    //承载所有实时编辑的数据
    [SerializeField]
    public List<StateEntity> states = new List<StateEntity>();

    public void OnAfterDeserialize()
    {

    }
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (states.Count == 0)
        {
            Debug.Log(1);
            var dct = DataMgr.Instance.playerStateData.DataMap;
            foreach (var item in dct)
            {
                var info = item.Value;
                StateEntity entity = new StateEntity();
                entity.id = info.Id;
                entity.info = info.Id + "_" + info.Info;
                states.Add(entity);
            }
        }
        else
        {
            
            var dct = DataMgr.Instance.playerStateData.DataMap;
            if(dct.Count != states.Count)
            {
                
                foreach(var item in dct)
                {
                    var info = item.Value;
                    bool add = true;
                    for (int i = 0; i < states.Count; i++)
                    {
                        if (states[i].id == info.Id)
                        {
                            add = false;
                            continue;
                        }
                    }
                    if (add == true)
                    {
                        StateEntity stateEntity = new StateEntity();
                        stateEntity.id = info.Id;
                        stateEntity.info = info.Id + "_" + info.Info;
                        states.Add(stateEntity);
                    }
                    
                }
                List<StateEntity> remove = new List<StateEntity>();
                foreach (var item in states)
                {
                    if (dct.ContainsKey(item.id) == false)
                    {
                        remove.Add(item);
                        continue;
                    }
                }

                foreach (var item in remove)
                {
                    states.Remove(item);
                }
            }
        }
#endif
    }

    public void OnBeforeSerialize()
    {
        
    }

#if UNITY_EDITOR
    private void Reset()
    {
        OnEnable();// 复用OnEnable的初始化逻辑，不用重复写代码
    }
#endif

}

[System.Serializable]
public class StateEntity
{
    public int id;
    public string info;

    [Header("物体显示/隐藏控制")]
    public List<Obj_State> obj_States;
    [Header("命中检测")]
    public List<HitConfig> hitConfigs;
}

[System.Serializable]
public class Obj_State
{
    [Header("注释说明")]
    public string info;

    [Header("触发点")]
    public float trigger;

    [Header("需要操作的物体对象")]
    public string[] obj_id;

    [Header("打钩激活/反之则隐藏")]
    public bool act;

    [Header("状态提前结束，是否也强制执行该配置")]
    public bool force;

    [Header("循环执行(循环动作)")]
    public bool loop;
}

[System.Serializable]
public class HitConfig
{
    [Header("触发点")]
    public float trigger;
    [Header("结束点")]
    public float end;
    [Header("类型:0射线 1盒子 2球体")]
    public int type;
    [Header("射线:起点  配置子物体的路径")]
    public string begin;
    [Header("射线长度")]
    public float length;
    [Header("命中特效")]
    public string hitObj;

    [Space(20)]
    [Header("盒子中心点")]
    public Vector3 box_center;
    [Header("盒子大小")]
    public Vector3 box_size;

    //每一帧检测 感兴趣的范围 : 距离 xx角度


}
