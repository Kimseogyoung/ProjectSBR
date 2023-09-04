using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerStateMachine : StateMachineBase
{
    private CinemachineVirtualCamera _playerCam;
    void Start()
    {
        GameObject playerCamObj = UTIL.Instantiate("Prefab/PlayerCamera");
        _playerCam = UTIL.GetComponent<CinemachineVirtualCamera>(playerCamObj);
        _playerCam.Follow = gameObject.transform;
    }
    protected override void Init()
    {
        
    }
  
}
