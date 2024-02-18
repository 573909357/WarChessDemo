using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_ControlManager
{
    public Module currentModule;
    
    public void Update()
    {
        currentModule?.Update();
    }

    public abstract class Module
    {
        public abstract void Update();
    }
}

