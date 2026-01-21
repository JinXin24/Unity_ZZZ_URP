using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime 
{
    public static float deltaTime;
    public static float time;
    public static int frame_now;
    public static void Update()
    {
        deltaTime = Time.deltaTime;
        time = Time.time;
        frame_now = Time.frameCount; //游戏已经进行了多少帧
    }
}
