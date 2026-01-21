using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UInput 
{
    public static float GetAxis_Horizontal()
    {
        return Input.GetAxis("Horizontal");
        
    }
    public static float GetAxis_Vertical()
    {
        return Input.GetAxis("Vertical");
        
    }

    public static float GetAxis_Mouse_X()
    {
        return  Input.GetAxis("Mouse X");
        
    }

    public static float GetAxis_Mouse_Y()
    {
        return Input.GetAxis("Mouse Y");
       
    }

    public static float GetAxis_Mouse_ScrollWheel()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }
    
    public static bool GetKeyDown_Space()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public static bool GetMouseButtonUp_0()
    {
        return Input.GetMouseButtonUp(0);
    }

    public static bool GetKeyDown_Q()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public static bool GetKeyDown_E()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public static bool GetKeyDown_R()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    public static bool GetKeyDown_T()
    {
        return Input.GetKeyDown(KeyCode.T);
    }

    public static bool GetMouseButtonDown_1()
    {
        return Input.GetMouseButtonDown(1);
    }

    public static bool GetMouseButtonUP_1()
    {
        return Input.GetMouseButtonUp(1);
    }

    public static bool GetKeyDown_LeftShift()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    internal static bool GetMouseButton_0()
    {
        return Input.GetMouseButton(0);
    }
}
