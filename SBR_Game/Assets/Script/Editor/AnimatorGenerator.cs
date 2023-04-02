using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace SBRAnimator{ 
    public class AnimatorGenerator : EditorWindow
    {

        [SerializeField] private static AnimControllerData _data;

        public bool _isMoving = false;
        private string _isMovingKey = "IsMoving";
        private string _atkSpdKey = "AtkSpd";

        private AnimatorController _controller;


        private static int _characterIdx = 0;
        private static int _copyCharacterIdx = 0;
        private static bool _isCopy;
        private static List<AnimData> _animations = new List<AnimData>();
        private static Dictionary<ECharacterActionType, AnimationClip> _animDict = new Dictionary<ECharacterActionType, AnimationClip>();
        private static UnityEditorInternal.ReorderableList _reorderableList;

        private static CharacterProto[] _characterProtos ;
        private static string[] _options;

        public void OnEnable()
        {
            _animations = new List<AnimData>();
            _reorderableList = new UnityEditorInternal.ReorderableList(_animations, typeof(AnimData), true, true, true, true);
            _reorderableList.drawElementCallback = DrawElementCallback;
            _reorderableList.drawHeaderCallback = DrawHeaderCallback;
        }

        [MenuItem("SBR Animator/Show Generator")]
        public static void ShowWindow()
        {
            if (_data == null)
            {
                Debug.Log("Scriptable Gen"); 
                _data = AssetDatabase.LoadAssetAtPath<AnimControllerData>("Assets/Resources/Character/AnimatorContollerInfo.asset");
            }

            ProtoHelper.Start();
            ProtoHelper.Bind(comparer: Comparer<CharacterProto>.Create((p1, p2) => p1.Id.CompareTo(p2.Id)));
            List<CharacterProto> characterProtos = ProtoHelper.GetAll<CharacterProto>();
            _options = characterProtos.Select(c => c.Name).ToArray();
            _characterProtos = characterProtos.ToArray();

            _animations = new List<AnimData>();
            _reorderableList = new UnityEditorInternal.ReorderableList(_animations, typeof(AnimData), true, true, true, true);
            _reorderableList.drawElementCallback = DrawElementCallback;
            _reorderableList.drawHeaderCallback = DrawHeaderCallback;
            LoadList(1);

            EditorWindow.GetWindow<AnimatorGenerator>().Show();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Character Animator Contoller Generator");

            int prevCharacterIdx = _characterIdx;
            _characterIdx = EditorGUILayout.Popup("Character Name", _characterIdx, _options);

            if (prevCharacterIdx != _characterIdx)
            {
                LoadList(_characterProtos[_characterIdx].Id);
            }

            _isCopy = EditorGUILayout.Toggle("Is Copy Other Character", _isCopy);
            _copyCharacterIdx = EditorGUILayout.Popup("Copy Character Name", _copyCharacterIdx, _options);

            _reorderableList.DoLayoutList(); 
            if (GUILayout.Button("Animator Contoller Generator"))
            {
                Debug.Log($"Character: {_characterProtos[_characterIdx].Name} CopyCharacter {(_isCopy? _characterProtos[_copyCharacterIdx].Name :"")}");
                string controllerPath = $"Assets/Resources/Character/Controller/{_characterProtos[_characterIdx].TeamType}_{_characterProtos[_characterIdx].Id}.controller";

                if (File.Exists(controllerPath))
                {
                    File.Delete(controllerPath);
                }

                _controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

                if (_isCopy)
                {

                    if (!_data.TryGetCharacterAnims(_characterProtos[_copyCharacterIdx].Id, out var anims))
                        return;
                    _data.SetCharacterAnims(_characterProtos[_characterIdx].Id, anims);
                    _animations = anims;
                    _reorderableList.list = _animations;
                }
                else
                {
                    _data.SetCharacterAnims(_characterProtos[_characterIdx].Id, _animations);
                }

                RegisterAnimClip(_animations);
            }

            if (GUILayout.Button("Clear All Anim Data"))
            {
                _data.ClearList();
            }
        }

        private static void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _animations[index];

            rect.y += 2;
            var height = EditorGUIUtility.singleLineHeight;
            var typeRect = new Rect(rect.x, rect.y, rect.width / 2, height);
            var clipRect = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, height);

            element.Type = (ECharacterActionType)EditorGUI.EnumPopup(typeRect, element.Type);
            element.AnimationClip = (AnimationClip)EditorGUI.ObjectField(clipRect, element.AnimationClip, typeof(AnimationClip), true);
        }

        private static void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Clip List");
        }

        private static void LoadList(int id)
        {
            if (!_data.TryGetCharacterAnims(id, out var animDatas))
            {
                animDatas = null;
            }
            _animations = animDatas;
            _reorderableList.list = _animations;
        }

        private void RegisterAnimClip(List<AnimData> anims)
        {
            _animDict.Clear();
            for (int i = 0; i < anims.Count; i++)
            {
                _animDict.Add(anims[i].Type, anims[i].AnimationClip);
            }
            
            // 파라미터 추가
            AnimatorControllerParameter movingParam = new AnimatorControllerParameter();
            movingParam.type = AnimatorControllerParameterType.Bool;
            movingParam.name = _isMovingKey;

            AnimatorControllerParameter atkSpdParam = new AnimatorControllerParameter();
            atkSpdParam.type = AnimatorControllerParameterType.Float;
            atkSpdParam.name = _atkSpdKey;

            _controller.AddParameter(movingParam);
            _controller.AddParameter(atkSpdParam);

            // 상태 머신에서 기본 Idle 상태 추가
            AnimatorState idleState = _controller.layers[0].stateMachine.AddState(ECharacterActionType.IDLE.ToString());
            idleState.motion = _animDict[ECharacterActionType.IDLE];
            _controller.layers[0].stateMachine.defaultState = idleState;


            // 상태 머신에서 Run 상태 추가
            AnimatorState runState = _controller.layers[0].stateMachine.AddState(ECharacterActionType.RUN.ToString());
            runState.motion = _animDict[ECharacterActionType.RUN];

            // Idle -> Run 전이 추가
            AnimatorStateTransition idleToRun = idleState.AddTransition(runState);
            idleToRun.AddCondition(AnimatorConditionMode.If, 0, _isMovingKey);
            idleToRun.duration = 0.1f;

            // Run -> Idle 전이 추가
            AnimatorStateTransition runToIdle = runState.AddTransition(idleState);
            runToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, _isMovingKey);
            runToIdle.duration = 0.1f;


            foreach (var clip in _animDict)
            {

                switch (clip.Key)
                {
                    case ECharacterActionType.NONE:
                    case ECharacterActionType.IDLE:
                    case ECharacterActionType.RUN:
                        continue;
                    case ECharacterActionType.START:
                    case ECharacterActionType.DIE:
                        AnimatorState skillState = _controller.layers[0].stateMachine.AddState(clip.Key.ToString());
                        skillState.motion = clip.Value;
                        break;
                    case ECharacterActionType.ATTACK:
                    case ECharacterActionType.SKILL1:
                    case ECharacterActionType.SKILL2:
                    case ECharacterActionType.SKILL3:
                    case ECharacterActionType.SKILL4:
                    case ECharacterActionType.ULT_SKILL:
                        AnimatorState skillState2 = _controller.layers[0].stateMachine.AddState(clip.Key.ToString());
                        skillState2.motion = clip.Value;
                        AnimatorStateTransition skillToIdle = skillState2.AddTransition(idleState);
                        skillToIdle.hasExitTime = true;
                        break;
                }

            }
        }

    }

}
