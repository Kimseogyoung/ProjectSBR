using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public interface IManager
{
    public void Init();
    public void Prepare();
    public void Start();
    public void Finished();
}
/*
abstract public class ManagerBase : IManager
{
    public void Init()
    {
      
    }
    public void Prepare()
    {
       
    }
    public void Start()
    {
       
    }
    public void Finished()
    {

    }


    private void Pause()
    {

    }
    private void Play()
    {

    }
    private void Restart()
    {

    }


}
*/
