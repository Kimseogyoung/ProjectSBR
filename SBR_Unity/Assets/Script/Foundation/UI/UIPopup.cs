using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIBase
{
    public override void Init()
    {
        APP.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()  // �˾��̴ϱ� ���� ĵ����(Scene)�� �ٸ��� �ݴ°� �ʿ�
    {
        APP.UI.ClosePopupUI(this);
    }
}
