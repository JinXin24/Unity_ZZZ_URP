using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitService : FSMServiceBase
{
    List<int> hit_target = new List<int>();
    Vector3 last_end;
    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
       
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
        ReSetAllExcuted();
        hit_target.Clear();
        last_end = Vector3.zero;
    }

    public override void OnDisable(PlayerState state)
    {
        base.OnDisable(state);
        ReSetAllExcuted();
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);
        var configs = state.stateEntity.hitConfigs;
        if(configs != null && configs.Count >0)
        {
            for(int i=0; i< configs.Count; i++)
            {
                var e = configs[i];
                if(normalizedTime >= e.trigger && normalizedTime <= e.end)
                {
                    DO(e,state);
                    SetExcuted(i);
                }
            }
        } 
    }

    private void DO(HitConfig config, PlayerState state)
    {
        var obj = player.GetHangPoint(config.begin);
        Vector3 begin = obj.transform.position;
        if(config.type ==0)
        {
            Vector3 end = begin + obj.transform.right * config.length;
            Linecast(begin, end, config, state);
        }
    }

    public bool Linecast(Vector3 begin,Vector3 end, HitConfig config, PlayerState state)
    {
        
        var result = Physics.Linecast(begin, end, out var hitInfo, player.GetEnemyLayerMask(), QueryTriggerInteraction.Collide);

        if (result)
        {
            OnHit(begin,config,state,hitInfo);
        }

        return false;
    }

    public void OnHit(Vector3 begin,HitConfig config, PlayerState state,RaycastHit hitInfo)
    {
        var fsm = hitInfo.transform.GetComponent<FSM>();
        if (fsm != null)
        {
            if(hit_target.Contains(fsm.instance_id*100+(int)(config.trigger*100)) == false)
            {
                hit_target.Add(fsm.instance_id * 100 + (int)(config.trigger * 100));
                //1.命中特效
                VFXMgr.Instance.ReuseHitObj(hitInfo.point);

                //2.命中音效
                VFXMgr.Instance.ReuseHitSound(hitInfo.point);

            }
        }
    }

    public override void ReLoop(PlayerState state)
    {
        base.ReLoop(state);
    }

    public override void ReStart(PlayerState state)
    {
        base.ReStart(state);
    }
}
