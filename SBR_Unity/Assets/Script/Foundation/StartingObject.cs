using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingObject : MonoBehaviour
{
    IManager _dataManager = new DataManager();
    // Start is called before the first frame update
    void Start()
    {
        _dataManager.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
