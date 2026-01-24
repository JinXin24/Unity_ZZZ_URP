using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameDefine.Init();
       
    }

    // Update is called once per frame
    void Update()
    {
        GameTime.Update();
    }
}
