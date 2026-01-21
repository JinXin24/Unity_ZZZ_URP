using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AnimationService : FSMServiceBase
{
    public float normalizedTime; //当前动作播放的进度
    public string now_animClip_name;
    
    public override void Init(FSM fsm)
    {
        base.Init(fsm);
    }

    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
        Play(state);
        
        Debug.Log(state.excel_config.Id);
        
        
       
    }

    private void Play(PlayerState state)
    {
        normalizedTime = 0;
        now_animClip_name = state.excel_config.AnmName;
        Debug.Log(now_animClip_name);
        player.animator.Play(now_animClip_name, 0, 0f);
        player.animator.Update(0);
    }

    public override void OnDisable(PlayerState state)
    {
        base.OnDisable(state);
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
        now_animClip_name = string.Empty;
        normalizedTime = 0;
        
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);
        if (!string.IsNullOrEmpty(now_animClip_name))
        {
            var info = player.animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName(now_animClip_name))
            {
                this.normalizedTime = info.normalizedTime;
                if (this.normalizedTime > 1)
                {
                    // 仅改这里：触发一次结束回调后，不再重复触发
                   
                    player.AnimationOnPlayEnd();
                        
                    // 关键修复：用info.loop（Unity原生属性，不新增变量）区分循环/单次
                    this.normalizedTime = info.loop ? (this.normalizedTime % 1) : 1;
                }
            }
            else
            {
                this.normalizedTime = 0;
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
