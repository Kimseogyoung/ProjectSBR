using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SBRAnimator
{
    // 스크립터블 오브젝트 클래스
    [System.Serializable]
    [CreateAssetMenu(fileName = "AnimatorContollerInfo", menuName = "SBR Animator/AnimControllerData")]
    public class AnimControllerData : ScriptableObject
    {
        public string TestString = "";

        [SerializeField] private List<CharIdAnimData> _animDataList = new List<CharIdAnimData>();
        [SerializeField] private List<int> _idList = new List<int>();

        public void SetCharacterAnims(int id, List<AnimData> list)
        {
            if (!_idList.Contains(id))
            {
                _idList.Add(id);
                _animDataList.Add(new CharIdAnimData { Id = id, AnimList = list });
                return;
            }

            int idx = _idList.IndexOf(id);
            _animDataList[idx] = new CharIdAnimData { Id = id, AnimList = list };
        }

        public bool TryGetCharacterAnims(int id, out List<AnimData> animDatas)
        {
            int idx = _idList.IndexOf(id);
            if (idx == -1)
            {
                animDatas = null;
                return false;
            }

            animDatas = _animDataList[idx].AnimList;
            return true;
        }

        public void ClearList()
        {
            _idList.Clear();
            _animDataList.Clear();
        }

        [Serializable]
        public class CharIdAnimData
        {
            public int Id;
            public List<AnimData> AnimList = new List<AnimData>();
        }
    }

    [Serializable]
    public class AnimData
    {
        public ECharacterActionType Type;
        public AnimationClip AnimationClip;
    }
}
