using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Star : UI_Panel
{
    private int _currentLevel;
    private Sprite _emptyStar;
    private Sprite _filledStar;
    private void Awake()
    {
        _currentLevel = 0;

        _emptyStar = SG.UTIL.Load(AppPath.CasualUI, AppPath.EmptyStar);
        _filledStar = SG.UTIL.Load(AppPath.CasualUI,AppPath.FilledStar);

        Bind<Image>(UI.StarImage1.ToString());
        Bind<Image>(UI.StarImage2.ToString());
        Bind<Image>(UI.StarImage3.ToString());

        SetStarLevel(0);
    }

    public void SetStarLevel(int level)
    {
        if(_currentLevel == level) return;
        _currentLevel = level;

        Get<Image>(UI.StarImage1.ToString()).sprite = _currentLevel >= 1 ? _filledStar : _emptyStar;
        Get<Image>(UI.StarImage2.ToString()).sprite = _currentLevel >= 2 ? _filledStar : _emptyStar;
        Get<Image>(UI.StarImage3.ToString()).sprite = _currentLevel >= 3 ? _filledStar : _emptyStar;
    }

    enum UI
    {
        StarImage1, 
        StarImage2, 
        StarImage3
    }
}
