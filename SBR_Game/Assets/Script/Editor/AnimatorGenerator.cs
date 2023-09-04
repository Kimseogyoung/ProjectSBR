using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

namespace SBRAnimator{

    [CustomEditor(typeof(CharacterEventHandler))]
    public class CharacterEventHandlerInspector : Editor

    {
        private int _copyCharacterIdx = 0;
        private static string[] _options;
        private static CharacterProto[] _characterProtos;

        private static UnityEditorInternal.ReorderableList _reorderableList;
        CharacterEventHandler _handler;
        GameObject _gameObject;

        

        // Editor에서 OnEnable은 실제 에디터에서 오브젝트를 눌렀을때 활성화됨
        private void OnEnable()
        {
            // target은 Editor에 있는 변수로 선택한 오브젝트를 받아옴.
            if (AssetDatabase.Contains(target))
            {
                _handler = null;
                _gameObject = null;
            }
            else
            {
                // target은 Object형이므로 Enemy로 형변환
                _handler = (CharacterEventHandler)target;
                _gameObject = _handler.gameObject;
                _reorderableList = new UnityEditorInternal.ReorderableList(_handler.AnimList, typeof(AnimData), true, true, true, true);
                _reorderableList.drawElementCallback = DrawElementCallback;
                _reorderableList.drawHeaderCallback = DrawHeaderCallback;

                ProtoHelper.Start();
                ProtoHelper.Bind(comparer: Comparer<CharacterProto>.Create((p1, p2) => p1.Id.CompareTo(p2.Id)));
                List<CharacterProto> characterProtos = ProtoHelper.GetAll<CharacterProto>();
                _options = characterProtos.Select(c => c.Name).ToArray();
                _characterProtos = characterProtos.ToArray();
            }
        }

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            GUILayout.Space(30);
            EditorGUILayout.LabelField("Animation Field");
            _handler.protoIndex = EditorGUILayout.Popup("Current Character Name", _handler.protoIndex, _options);
            _reorderableList.DoLayoutList();
            if (GUILayout.Button("Animator Contoller Generate"))
            {
                GenerateAnimData(_gameObject, (List<AnimData>)_reorderableList.list);
            }
            if (GUILayout.Button("Set All ActionType"))
            {
                LoadAllEnum();

            }

            GUILayout.Space(30);
            EditorGUILayout.LabelField("Copy Field");
            _copyCharacterIdx = EditorGUILayout.Popup("Copy Character Name", _copyCharacterIdx, _options);

            if (GUILayout.Button("Copy from Other Character"))
            {
                var copyObj = UTIL.LoadRes<GameObject>(_characterProtos[_copyCharacterIdx].Prefab);
                if (copyObj == null)
                    return;
                var copyHandler = copyObj.GetComponentInChildren<CharacterEventHandler>();
                if (copyHandler == null)
                    return;


                _reorderableList.list = copyHandler.AnimList;
                GenerateAnimData(_handler, copyHandler.AnimList);
                Debug.Log(copyObj.name + " -> " + _gameObject.transform.parent.name +" 카피 성공!");
            }

            if (GUILayout.Button("Copy to Other Character"))
            {
                var copyObj = UTIL.LoadRes<GameObject>(_characterProtos[_copyCharacterIdx].Prefab);
                if (copyObj == null)
                    return;
                
                GenerateAnimData(copyObj.transform.GetChild(0).gameObject, _handler.AnimList);
                Debug.Log(_gameObject.transform.parent.name + " -> " + copyObj.transform.GetChild(0).gameObject.name + " 카피 성공!");
            }

        }

        private void GenerateAnimData(GameObject targetObj, List<AnimData> animDatas)
        {
            var handler =targetObj.GetComponent<CharacterEventHandler>();
            if (handler == null){
                handler = targetObj.AddComponent<CharacterEventHandler>(); 
                EditorUtility.SetDirty(targetObj); }

            handler.AnimList= animDatas;
            RegisterAnimClip(targetObj, _handler.AnimList);
            EditorUtility.SetDirty(handler);
        }
        private void GenerateAnimData(CharacterEventHandler targetObj, List<AnimData> animDatas)
        {
            if (targetObj == null)
                return;

            targetObj.AnimList = animDatas;
            RegisterAnimClip(targetObj.gameObject, targetObj.AnimList);
            EditorUtility.SetDirty(targetObj.gameObject);
        }

        private void LoadAllEnum()
        {
            List<AnimData> list = new List<AnimData>();
            foreach (ECharacterActionType type in Enum.GetValues(typeof(ECharacterActionType)))
            {
                list.Add(new AnimData { Type = type });
            }
            _reorderableList.list = list;
            EditorUtility.SetDirty(_gameObject);
            GenerateAnimData(_gameObject, list);
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _handler.AnimList[index];

            rect.y += 2;
            var height = EditorGUIUtility.singleLineHeight;
            var typeRect = new Rect(rect.x, rect.y, rect.width / 2, height);
            var clipRect = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, height);

            element.Type = (ECharacterActionType)EditorGUI.EnumPopup(typeRect, element.Type);
            element.AnimationClip = (AnimationClip)EditorGUI.ObjectField(clipRect, element.AnimationClip, typeof(AnimationClip), true);
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Clip List");
        }





        private string _isMovingKey = "IsMoving";
        private string _atkSpdKey = "AtkSpd";

        private void RegisterAnimClip(GameObject obj, List<AnimData> anims)
        {
            string controllerPath = $"Assets/Resources/Character/Controller/{_characterProtos[_handler.protoIndex].TeamType}_{_characterProtos[_handler.protoIndex].Id}.controller";
            if (File.Exists(controllerPath))
            {
                File.Delete(controllerPath);
            }
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

            var animator = obj.GetComponent<Animator>();
            if(animator == null)
                animator = obj.AddComponent<Animator>();

            animator.runtimeAnimatorController= controller;
            EditorUtility.SetDirty(obj);


            Dictionary<ECharacterActionType, AnimationClip> animDict = new Dictionary<ECharacterActionType, AnimationClip>();
            for (int i = 0; i < anims.Count; i++)
            {
                animDict.Add(anims[i].Type, anims[i].AnimationClip);
            }

            // 파라미터 추가
            AnimatorControllerParameter movingParam = new AnimatorControllerParameter();
            movingParam.type = AnimatorControllerParameterType.Bool;
            movingParam.name = _isMovingKey;

            AnimatorControllerParameter atkSpdParam = new AnimatorControllerParameter();
            atkSpdParam.type = AnimatorControllerParameterType.Float;
            atkSpdParam.name = _atkSpdKey;

            controller.AddParameter(movingParam);
            controller.AddParameter(atkSpdParam);

            // 상태 머신에서 기본 Idle 상태 추가
            AnimatorState idleState = controller.layers[0].stateMachine.AddState(ECharacterActionType.IDLE.ToString());
            idleState.motion = animDict[ECharacterActionType.IDLE];
            controller.layers[0].stateMachine.defaultState = idleState;


            // 상태 머신에서 Run 상태 추가
            AnimatorState runState = controller.layers[0].stateMachine.AddState(ECharacterActionType.RUN.ToString());
            runState.motion = animDict[ECharacterActionType.RUN];

            // Idle -> Run 전이 추가
            AnimatorStateTransition idleToRun = idleState.AddTransition(runState);
            idleToRun.AddCondition(AnimatorConditionMode.If, 0, _isMovingKey);
            idleToRun.duration = 0.1f;

            // Run -> Idle 전이 추가
            AnimatorStateTransition runToIdle = runState.AddTransition(idleState);
            runToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, _isMovingKey);
            runToIdle.duration = 0.1f;


            foreach (var clip in animDict)
            {

                switch (clip.Key)
                {
                    case ECharacterActionType.NONE:
                    case ECharacterActionType.IDLE:
                    case ECharacterActionType.RUN:
                        continue;
                    case ECharacterActionType.START:
                    case ECharacterActionType.DIE:
                        AnimatorState skillState = controller.layers[0].stateMachine.AddState(clip.Key.ToString());
                        skillState.motion = clip.Value;
                        break;
                    case ECharacterActionType.ATTACK:
                    case ECharacterActionType.SKILL1:
                    case ECharacterActionType.SKILL2:
                    case ECharacterActionType.SKILL3:
                    case ECharacterActionType.SKILL4:
                    case ECharacterActionType.ULT_SKILL:
                        AnimatorState skillState2 = controller.layers[0].stateMachine.AddState(clip.Key.ToString());
                        skillState2.motion = clip.Value;
                        AnimatorStateTransition skillToIdle = skillState2.AddTransition(idleState);
                        skillToIdle.hasExitTime = true;
                        break;
                }

            }
        }
    }

}
