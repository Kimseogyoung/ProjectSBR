using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public interface IManager
{
    public void Init();
    public void StartManager();
    public void FinishManager();
}
public interface IManagerUpdatable
{
    public void UpdateManager();
}


