using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LobbyScene : UI_Scene
{

    //�̷��� ���� ����.
    //[SerializeField] private Button pointButton; // "PointButton" UI ������Ʈ ����
    //[SerializeField] private Text pointText; // "PointText" UI ������Ʈ ����
    //[SerializeField] private Text scoreText; // "ScoreText" UI ������Ʈ ����
    //[SerializeField] private GameObject go; // "TestObject" ������Ʈ ����
    //[SerializeField] private Image itemIcon; // "ItemIcon" UI ������Ʈ ����

    //GameObject �̸����� enum���� ����
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
