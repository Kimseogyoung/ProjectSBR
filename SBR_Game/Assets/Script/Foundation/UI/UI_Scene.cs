using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
	//씬 UI 의 조상, 팝업처럼 차례대로 뜨는게 아니라 원래 자리잡고 있는 고정적인 UI 캔버스들의 공통적인 부분들.

	public override void Init()
	{
		APP.UI.SetCanvas(gameObject, false);
	}
}
