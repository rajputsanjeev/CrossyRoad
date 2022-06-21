using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;

public abstract class Behaviour<T1> : MonoBehaviour where T1 : View
{
    protected UIPanelManager UIPanelManager => UIPanelManager.Instance;
    protected T1 Prefab;

    public static Behaviour<T1> Instance;

    protected virtual void Awake()
    {
        Instance = this;
        Prefab = this.GetComponent<T1>();
        Init();
    }

    protected virtual void Init() {

       // Debug.LogError("Init is getting called");
    }
}

public abstract class Behaviour<T1,T2> : Behaviour<T1> where T1:View
{
    protected T2 Model;

    protected override void Awake()
    {
        Model = this.GetComponent<T2>();
        base.Awake();
    }
}
