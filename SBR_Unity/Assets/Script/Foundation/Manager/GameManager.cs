using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    private DataManager _dataManager;
    private SceneManager _sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        _dataManager = new DataManager();
        _sceneManager = new SceneManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
