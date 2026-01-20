using cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public PlayerState currentState;
    Dictionary<int, PlayerState> stateData = new Dictionary<int, PlayerState>();

    private void Awake()
    {
        ToNext(1001);
    }
    private void Update()
    {
        
    }

    private void ToNext(int next)
    {
        
    }
}

public class PlayerState
{
    public int id;
    public PlayerStateData excel_config;
}


public enum StateEventType
{
    begin,      //开始进入
    update,     //每帧更新
    end,        //状态退出
    onAnmEnd,   //当动作结束的时候
}