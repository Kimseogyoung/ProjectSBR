using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class StageClearInfo
{
    public int StarCnt = 1;
    public StageProto ClearedStagePrt { get; private set; }
    public List<int> StageRewardList { get; private set; }

    public StageClearInfo(StageProto clearedStagePrt,int starCnt,  List<int> stageRewardList)
    {
        ClearedStagePrt = clearedStagePrt;
        StarCnt = starCnt;
        StageRewardList = stageRewardList;
    }
}

