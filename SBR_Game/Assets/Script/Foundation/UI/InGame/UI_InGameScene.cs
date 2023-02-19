using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UI_InGameScene : UI_Scene
{
    private void Awake()
    {
        Bind<Button>(UI.PauseButton.ToString()).onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_InGamePausePopup>(); });
    }

    private void Update()
    {
        
    }

    enum UI{
        //Top Panel
        PauseButton
    }
}
