using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LobbyScene : UI_Scene
{

    //이렇게 안할 것임.
    //[SerializeField] private Button pointButton; // "PointButton" UI 오브젝트 연결
    //[SerializeField] private Text pointText; // "PointText" UI 오브젝트 연결
    //[SerializeField] private Text scoreText; // "ScoreText" UI 오브젝트 연결
    //[SerializeField] private GameObject go; // "TestObject" 오브젝트 연결
    //[SerializeField] private Image itemIcon; // "ItemIcon" UI 오브젝트 연결

    //GameObject 이름들을 enum으로 저장
    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }
}
