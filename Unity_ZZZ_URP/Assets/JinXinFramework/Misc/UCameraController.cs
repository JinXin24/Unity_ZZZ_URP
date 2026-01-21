using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCameraController : MonoBehaviour
{
    public Transform target;
    //滑动鼠标 相机旋转
    //鼠标滚轮 相机离角色远近
    // Start is called before the first frame update

    CharacterController controller;
     Vector3 hight_offset;
    void Start()
    {
        if(target != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            controller = target.GetComponent<CharacterController>();
            hight_offset = controller.center * 1.75f;
        }
    }

    float xMouse;
    float yMouse;

    float distanceFromTarget;
    public float mouse_scrollwheel_scale = 10;
    public float speed;
    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            //鼠标滑动 输入的值
            xMouse += UInput.GetAxis_Mouse_X();
            yMouse -= UInput.GetAxis_Mouse_Y();

            yMouse = Mathf.Clamp(yMouse, -30f, 80f);

            //鼠标滚轮的输入 往前滑动正数 往后滑动负数
            distanceFromTarget -= UInput.GetAxis_Mouse_ScrollWheel() * mouse_scrollwheel_scale; //拉进或者拉远 人物镜头

            distanceFromTarget = Mathf.Clamp(distanceFromTarget, 2, 15);

            Quaternion targetRotation = Quaternion.Euler(yMouse, xMouse, 0);
            Vector3 targetPosition =new Vector3( target.position.x,target.position.y+1f,target.position.z) + targetRotation * new Vector3(0, 0, -distanceFromTarget);
            

            speed = controller.velocity.magnitude > 0.1f ? Mathf.Lerp(speed, 7.5f, 5f * GameTime.deltaTime):Mathf.Lerp(speed,25f,5f*GameTime.deltaTime);

            //角色控制器 移动状态的话 让它慢 -- 插值比较快

            transform.position = Vector3.Lerp(transform.position, targetPosition,GameTime.deltaTime *speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GameTime.deltaTime * 25f);

        }
    }
}
