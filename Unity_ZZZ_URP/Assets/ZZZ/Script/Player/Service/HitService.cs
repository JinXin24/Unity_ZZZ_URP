using System.Collections;
using System.Collections.Generic;
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
