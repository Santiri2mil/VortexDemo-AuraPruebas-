﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITweening : MonoBehaviour
{
    PlayerDetector PD;
    public void Open()
    {
        
        LeanTween.scale(gameObject, new Vector3(1, 1,1), 0.5f);
    }
   
    public void OnClose()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f);//.setOnComplete(DestroyMe);
    }

    
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
