using cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FSM : MonoBehaviour
{
    public PlayerState currentState;
    Dictionary<int, PlayerState> stateData = new Dictionary<int, PlayerState>();
    public Dictionary<int, Dictionary<StateEventType, List<Action>>> actions = new Dictionary<int, Dictionary<StateEventType, List<Action>>>();
    [HideInInspector] public Animator animator;

    public List<FSMServiceBase> fSMServices = new List<FSMServiceBase>();
    AnimationService animationService;
    int service_count;
    float _targetRotation;
    [HideInInspector]
    public Transform _transform;
    float _rotationVelocity;
    float RotationSmoothTime = 0.05f;
   

    // 新增：攻击输入缓存变量
    private bool hasPendingAtkInput = false; // 是否有待处理的攻击输入
    private int pendingAtkStateId = -1;      // 待切换的攻击状态ID
    private void Awake()
    {
        _transform = this.transform;
        animator = GetComponent<Animator>();
        ServiceInit();
        StateInit();
        ToNext(1001);
    }

    private void StateInit()
    {
        var playerStateData = DataMgr.Instance.playerStateData.DataList;
        if (playerStateData.Count > 0)
        {
            foreach (var item in playerStateData)
            {
                PlayerState p = new PlayerState();
                p.excel_config = item;
                p.id = item.Id;
                stateData[item.Id] = p;

            }
        }

        foreach (var item in stateData)
        {
            if (item.Value.excel_config.OnMove.Count > 0)
            {
                AddListener(item.Key, StateEventType.update, OnMove);
            }
            if (item.Value.excel_config.DoMove == 1)
            {
                AddListener(item.Key, StateEventType.update, PlayerMove);
            }
            if (item.Value.excel_config.OnStop != 0)
            {
                AddListener(item.Key, StateEventType.update, OnStop);
            }
            
            if(item.Value.excel_config.OnEvade.Count > 0)
            {
                AddListener(item.Key, StateEventType.update, OnEvade);
            }

            if(item.Value.excel_config.OnAtk.Count> 0)
            {
                AddListener(item.Key, StateEventType.update, OnAtk);
                AddListener(item.Key, StateEventType.update, CheckPendingAtkInput);
            }

        }
    }

    private void OnAtk()
    {
        if(UInput.GetMouseButtonUp_0())
        {
            // 避免重复缓存：如果已有待处理输入，直接覆盖（或忽略）
            if (!hasPendingAtkInput)
            {
                // 先记录目标状态ID（提前取出，避免后续状态切换后取值错误）
                pendingAtkStateId = (int)currentState.excel_config.OnAtk[2];
                hasPendingAtkInput = true;
            }
        }
    }

    public bool CheckConfig(List<float> config)
    {
        if (config.Count == 0)
        {
            return false;
        }
        else
        {
            if((animationService.normalizedTime>=0 && animationService.normalizedTime <= config[0])
                || animationService.normalizedTime >= config[1])
            {
                return true;
            }
            return false;
        }
    }

    private void OnEvade()
    {
        if(UInput.GetMouseButtonDown_1())
        {
            var x = UInput.GetAxis_Horizontal();
            var z = UInput.GetAxis_Vertical();
            if(x!=0||z!=0)
            {
                ToNext(currentState.excel_config.OnEvade[0]);
            }
            else
            {
                ToNext(currentState.excel_config.OnEvade[1]);
            }
            
        }
    }

    public void AnimationOnPlayEnd()
    {
        var _id = currentState.id;
        DOStateEvent(currentState.id, StateEventType.onAnmEnd);
       
        if (currentState.id != _id)
        {
            return;
        }

        switch (currentState.excel_config.OnAnmEnd)
        {
            case -1:
                break;
            case 0:
                return;
            default:
                ToNext(currentState.excel_config.OnAnmEnd);
                break;
        }


    }
    private void OnStop()
    {
        if (UInput.GetAxis_Horizontal() == 0 && UInput.GetAxis_Vertical() == 0)
        {
            ToNext(currentState.excel_config.OnStop);
        }
    }

    private void PlayerMove()
    {
        var x = UInput.GetAxis_Horizontal();
        var z = UInput.GetAxis_Vertical();
        if (x != 0 || z != 0)
        {
            Vector3 inputDirection = new Vector3(x, 0f, z).normalized;

            //Mathf.Atan2 正切函数 求弧度 * Mathf.Rad2Deg(弧度转度数) >> 度数
            //第一:先求出输入的角度
            //第二:加上当前相机Y轴旋转的量
            //第三:得到目标朝向的角度
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + GameDefine._Camera.eulerAngles.y;

            //做一个插值运动
            float rotation = Mathf.SmoothDampAngle(_transform.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            //角色先旋转到目标角度去
            _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            //计算目标方向 通过这个角度
            

        }
    }

    private void OnMove()
    {
        //键盘输入 wasd
        if (UInput.GetAxis_Horizontal() != 0 || UInput.GetAxis_Vertical() != 0)
        {
            if(CheckConfig(currentState.excel_config.OnMove))
            {
                ToNext((int)currentState.excel_config.OnMove[2]);
            }
        }
    }

    /// <summary>
    /// 添加事件的接口
    /// </summary>
    /// <param name="id">状态ID</param>
    /// <param name="t">事件类型</param>
    /// <param name="action">事件</param>
    public void AddListener(int id, StateEventType t, Action action)
    {
        if (!actions.ContainsKey(id))
        {
            actions[id] = new Dictionary<StateEventType, List<Action>>();
        }

        //如果不包含对应的事件类型
        if (actions[id].ContainsKey(t) == false)
        {
            //actions[id] = new Dictionary<StateEventType, List<Action>>();
            List<Action> list = new List<Action>();
            list.Add(action);
            actions[id][t] = list;
        }
        else
        {
            actions[id][t].Add(action);
        }
    }


    private void Update()
    {
        if (currentState != null)
        {
            if (ServicesOnUpdate() == true)
            {
                
                DOStateEvent(currentState.id, StateEventType.update); //状态每帧执行的事件

                
            }
        }
    }

    /// <summary>
    /// 每帧检测：如果有待处理攻击输入，且动画时间满足条件，执行状态切换
    /// </summary>
    private void CheckPendingAtkInput()
    {
        // 无待处理输入 → 直接返回
        if (!hasPendingAtkInput || pendingAtkStateId == -1) return;

        // 检测动画时间是否满足条件（复用你的CheckConfig逻辑）
        if (CheckConfig(currentState.excel_config.OnAtk))
        {
            // 满足条件 → 执行状态切换
            ToNext(pendingAtkStateId);
            // 清空缓存
            hasPendingAtkInput = false;
            pendingAtkStateId = -1;
            Debug.Log("执行缓存的攻击输入，切换到状态：" + pendingAtkStateId);
        }
    }

    private bool ServicesOnUpdate()
    {
        int crn_state_id = currentState.id;
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].OnUpdate(animationService.normalizedTime, currentState);
            if (currentState.id != crn_state_id)
            {
                return false;
            }

        }
        return true;
    }

    private bool ToNext(int next)
    {
        if(stateData.ContainsKey(next))
        {

            // 切换状态时清空攻击输入缓存，避免跨状态执行
            hasPendingAtkInput = false;
            pendingAtkStateId = -1;

            if (currentState != null)
            {
                Debug.Log($"{this.gameObject.name}:切换状态:{stateData[next].id} 当前是:{currentState.id}");
            }
            else
            {
                Debug.Log($"{this.gameObject.name}:切换状态{stateData[next].id}");
            }
            if (currentState != null)
            {
                //状态绑定的退出事件
                DOStateEvent(currentState.id, StateEventType.end);
                ServicesOnEnd();
            }

            currentState = stateData[next];
            ServicesOnBegin();
            DOStateEvent(currentState.id, StateEventType.begin);
            return true;
        }
        return false;
    }
    public T AddService<T>() where T : FSMServiceBase, new()
    {
        T com = new T();
        fSMServices.Add(com);
        com.Init(this);
        return com;
    }

    private void ServiceInit()
    {
        animationService = AddService<AnimationService>();
        service_count = fSMServices.Count;
    }

    private void ServicesOnBegin()
    {
        for (int i = 0; i < fSMServices.Count; i++)
        {
            fSMServices[i].OnBegin(currentState);
        }
    }

    private void ServicesOnEnd()
    {
        for (int i = 0; i < fSMServices.Count; i++)
        {
            fSMServices[i].OnEnd(currentState);
        }
    }

    private void DOStateEvent(int id, StateEventType t)
    {
        if (actions.TryGetValue(id, out var v))
        {
            if (v.TryGetValue(t, out var lst))
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].Invoke();
                }
            }
        }
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