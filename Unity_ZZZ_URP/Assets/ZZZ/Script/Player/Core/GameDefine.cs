using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    public static Vector3 _Gravity = new Vector3(0, -9.81f, 0);
    public static Transform _Camera;
    public static Vector3 _Ground_Dst = new Vector3(0, 0.02f, 0);

    public static int Ground_LayerMask;
    public static int Player_LayerMask;
    public static int Enemy_LayerMask;
    public static string WeaponTag = "Weapon";
    public static void Init()
    {
        _Camera = GameObject.Find("Camera").transform;
        Ground_LayerMask = LayerMask.GetMask("Default");
        Player_LayerMask = LayerMask.GetMask("Player");
        Enemy_LayerMask = LayerMask.GetMask("Enemy");
    }
}
