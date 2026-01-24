using cfg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttEntity
{
    public int id;
    public float hp;
    public float atk;
    public float critical_hit_rate;
    public float critical_hit_multiple;

    public static UnitAttEntity Create(int id)
    {
        if (DataMgr.Instance.unitAttData.DataMap.ContainsKey(id))
        {
            UnitAttData data = DataMgr.Instance.unitAttData.DataMap[id];
            UnitAttEntity entity = new UnitAttEntity();
            entity.id = data.Id;
            entity.hp = data.Hp;
            entity.atk = data.Atk;
            entity.critical_hit_rate = data.CriticalHitRate;
            return entity;
        }
       return null;
        
    }
}
