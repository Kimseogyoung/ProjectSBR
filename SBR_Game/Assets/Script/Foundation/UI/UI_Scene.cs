using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
	//�� UI �� ����, �˾�ó�� ���ʴ�� �ߴ°� �ƴ϶� ���� �ڸ���� �ִ� �������� UI ĵ�������� �������� �κе�.

	public override void Init()
	{
		APP.UI.SetCanvas(gameObject, false);
	}
}
