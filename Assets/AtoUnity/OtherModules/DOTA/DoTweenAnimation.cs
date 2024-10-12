using DG.Tweening;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Security.Cryptography;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using AtoGame.Base.UI;

namespace AtoGame.OtherModules.DOTA
{
    public class DoTweenAnimation : BaseDoTweenAnimation {
        [SerializeField, LabelText("Select Animation"), TabGroup("Tab1", "Animation Setting"), OnValueChanged(nameof(OnAnimationNameValueChanged))]
        private EName _animationName;
        [SerializeField, LabelText("Play Type"), TabGroup("Tab1", "Animation Setting", order: 1)]
        private EPlayType _playType = EPlayType.ManualPlay;

        #region Target
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowTransformTarget)), OnValueChanged(nameof(OnTransformTargetValueChanged))]
        public Transform _transformTarget;
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowGraphicTarget)), OnValueChanged(nameof(OnGraphicTargetValueChanged))]
        public Graphic _graphicTarget;
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowRectTransformTarget)), OnValueChanged(nameof(OnRectTransformTargetValueChanged))]
        public RectTransform _rectTransformTarget;
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowCanvasGroupTarget)), OnValueChanged(nameof(OnCanvasGroupTargetValueChanged))]
        public CanvasGroup _canvasGroupTarget;
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowImageTarget)), OnValueChanged(nameof(OnImageTargetValueChanged))]
        public Image _imageTarget;
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowSpriteRendererTarget)), OnValueChanged(nameof(OnSpriteRendererTargetValueChanged))]
        public SpriteRenderer _spriteRendererTarget;
        [SerializeField, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowHorizontalOrVerticalLayoutGroupTarget)), OnValueChanged(nameof(OnHorizontalOrVerticalLayoutGroupTargetValueChanged))]
        public HorizontalOrVerticalLayoutGroup _horizontalOrVerticalLayoutGroupTarget;

        #endregion Target

        [SerializeField, HideLabel, TabGroup("Tab1", "Animation Setting"), ShowIf(nameof(CheckShowProperties))]
        public BaseOptions _baseOptions = BaseOptions.Default;

        #region Values
        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), HideLabel, SerializeField, ShowIf(nameof(CheckShowFloatValues))]
        public FloatValues _floatValues; // Alpha, CanvasGroup, FillAmount, SpriteAlpha, GroupLayoutSpacing

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), HideLabel, SerializeField, ShowIf(nameof(CheckShowVector2Values))]
        public Vector2Values _vector2Values; // PunchAnchorPosition

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), HideLabel, SerializeField, ShowIf(nameof(CheckShowRelativeVector2Values))]
        public RelativeVector2Values _relavtiveVector2Values; // AnchorPosition, SizeDelta

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), HideLabel, SerializeField, ShowIf(nameof(CheckShowColorValues))]
        public ColorValues _colorValues = ColorValues.Default; // Color

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), HideLabel, SerializeField, ShowIf(nameof(CheckShowVector3Values))]
        public Vector3Values _vector3Values; // LocalRotation, Rotate, PunchPosition, PunchRotation, PunchScale, 
                                             // ShakePosition, ShakeRotation, ShakeScale

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), HideLabel, SerializeField, ShowIf(nameof(CheckShowRelativeVector3Values))]
        public RelativeVector3Values _relavtiveVector3Values; // LocalPosition, Position, LocalScale, Scale
        #endregion Values

        #region Other Values
        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), SerializeField, ShowIf(nameof(CheckShowRotateMode))]
        public RotateMode _rotateMode;// LocalRotation , Rotate

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), SerializeField, ShowIf(nameof(CheckShowFloat1)), LabelText("$" + nameof(GetFloat1Lable))]
        public float _floatValue_1; // PunchAnchorPosition, PunchPosition, PunchRotation, PunchScale, ShakePosition, ShakeRotation, ShakeScale

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), SerializeField, ShowIf(nameof(CheckShowInt1)), LabelText("$" + nameof(GetInt1Lable))]
        public int _intValue_1; // PunchAnchorPosition, PunchPosition, PunchRotation, PunchScale, ShakePosition, ShakeRotation, ShakeScale

        [FoldoutGroup("Tab1/Animation Setting/Custom Options"), SerializeField, ShowIf(nameof(CheckShowBool1)), LabelText("$" + nameof(GetBool1Lable))]
        public bool _boolValue_1; // ShakePosition, ShakeRotation, ShakeScale  
        #endregion Other Values

        [SerializeField, PropertyOrder(1), LabelText("On Tween Start"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnTweenStart), Space]
        protected BaseDoTweenAnimation tweenStartDota;
        [SerializeField, PropertyOrder(1), LabelText("On Tween Complete"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnTweenComplete), Space]
        protected BaseDoTweenAnimation tweenCompleteDota;
        [SerializeField, PropertyOrder(1), LabelText("On Tween Start"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnTweenStart), Space]
        private UnityEvent tweenStartCallback;
        [SerializeField, PropertyOrder(1), LabelText("On Tween Complete"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnTweenComplete), Space]
        private UnityEvent tweenCompleteCallback;
        

        private BaseDoTween _baseDoTween;
        private Component _target;

        public Tween Tween
        {
            get => _baseDoTween == null ? null : _baseDoTween.Tween;
        }

        private void OnValidate()
        {
            CreateDoTween();
            if(_target != null)
            {
                _transformTarget = _target.transform;
                _graphicTarget = _target.GetComponent<Graphic>();
                _rectTransformTarget = _target.GetComponent<RectTransform>();
                _canvasGroupTarget = _target.GetComponent<CanvasGroup>();
                _imageTarget = _target.GetComponent<Image>();
                _spriteRendererTarget = _target.GetComponent<SpriteRenderer>();
                _horizontalOrVerticalLayoutGroupTarget = _target.GetComponent<HorizontalOrVerticalLayoutGroup>();
            }
        }

        #region Check Show Properties
        private bool CheckShowProperties()
        {
            return _baseDoTween != null;
        }

        private bool CheckShowFloatValues()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowFloatValues();
        }

        private bool CheckShowVector2Values()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowVector2Values();
        }

        private bool CheckShowRelativeVector2Values()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowRelativeVector2Values();
        }

        private bool CheckShowColorValues()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowColorValues();
        }

        private bool CheckShowVector3Values()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowVector3Values();
        }

        private bool CheckShowRelativeVector3Values()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowRelativeVector3Values();
        }

        private bool CheckShowRotateMode()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowRotateMode();
        }

        private bool CheckShowFloat1()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowFloat1();
        }

        private bool CheckShowInt1()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowInt1();
        }

        private bool CheckShowBool1()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowBool1();
        }

        private bool CheckShowTransformTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowTransformTarget();
        }

        private bool CheckShowGraphicTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowGraphicTarget();
        }

        private bool CheckShowRectTransformTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowRectTransformTarget();
        }

        private bool CheckShowCanvasGroupTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowCanvasGroupTarget();
        }

        private bool CheckShowImageTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowImageTarget();
        }

        private bool CheckShowSpriteRendererTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowSpriteRendererTarget();
        }

        private bool CheckShowHorizontalOrVerticalLayoutGroupTarget()
        {
            return CheckShowProperties() && _baseDoTween.CheckShowHorizontalOrVerticalLayoutGroupTarget();
        }

        #endregion Check Show Properties

        #region Lable Text
        private string GetFloat1Lable()
        {
            if(CheckShowFloat1())
            {
                return _baseDoTween.GetFloat1Lable();
            }
            return string.Empty;
        }

        private string GetInt1Lable()
        {
            if (CheckShowInt1())
            {
                return _baseDoTween.GetInt1Lable();
            }
            return string.Empty;
        }

        private string GetBool1Lable()
        {
            if (CheckShowBool1())
            {
                return _baseDoTween.GetBool1Lable();
            }
            return string.Empty;
        }
        #endregion Lable Text

        #region On Value Changed 
        private void OnAnimationNameValueChanged()
        {
            CreateDoTween();
        }

        private void OnTransformTargetValueChanged()
        {
            _target = _transformTarget;
        }
        private void OnGraphicTargetValueChanged()
        {
            _target = _graphicTarget;
        }
        private void OnRectTransformTargetValueChanged()
        {
            _target = _rectTransformTarget;
        }
        private void OnCanvasGroupTargetValueChanged()
        {
            _target = _canvasGroupTarget;
        }
        private void OnImageTargetValueChanged()
        {
            _target = _imageTarget;
        }
        private void OnSpriteRendererTargetValueChanged()
        {
            _target = _spriteRendererTarget;
        }
        private void OnHorizontalOrVerticalLayoutGroupTargetValueChanged()
        {
            _target = _horizontalOrVerticalLayoutGroupTarget;
        }
        #endregion On Value Changed 

        #region Play Editor

#if UNITY_EDITOR
        private static Action<DoTweenAnimation, Tween> prepareTweenForPreviewFunc;
        public static void AddPrepareTweenForPreviewFunction(Action<DoTweenAnimation, Tween> previewFunc)
        {
            prepareTweenForPreviewFunc = previewFunc;
        }

#endif
        #endregion

        public void PlayPreview()
        {
            if (_baseDoTween != null)
            {
                _baseDoTween.PlayPreview(this);
            }
        }

        public override void Play(Action onCompleted)
        {
            base.Play(onCompleted);
            if (Application.isPlaying)
            {
                Play(true);
            }
            else
            {

                if (_baseDoTween != null)
                {
                    _baseDoTween.PlayPreview(this);
                    if (prepareTweenForPreviewFunc != null)
                    {
                        prepareTweenForPreviewFunc.Invoke(this, _baseDoTween.Tween);
                    }
                }
            }
        }


        public void Play(bool restart)
        {
            Stop();
            if (restart)
            {
                ResetState();
            }
            if (_baseDoTween != null)
            {
                _baseDoTween.Play(this, () => {
                    CheckOnCompleted();
                });
            }
        }

        public void ResetState()
        {
            if (_baseDoTween != null)
            {
                _baseDoTween.Stop(this);
                _baseDoTween.ResetState(this);
            }
        }

        public override void Stop()
        {
            Stop(false);
        }

        public void Stop(bool complete)
        {
            if (_baseDoTween != null)
            {
                if (Application.isPlaying)
                {
                    _baseDoTween.Stop(this, complete);
                }
                else
                {
                    _baseDoTween.StopPreview(this);
                }
            }
        }

        private void CreateDoTween()
        {
            DoTweenFactory.AnimationName = _animationName;
            _baseDoTween = DoTweenFactory.CreateDoTween();
        }

        public void OnTweenStartEvent()
        {
            tweenStartCallback?.Invoke();
            if (tweenStartDota != null)
            {
                dotaCallingCounter++;
                tweenStartDota.Play(() => {
                    CheckOnCompleted();
                });
            }
        }

        public void OnTweenCompleteEvent()
        {
            tweenCompleteCallback?.Invoke();
            if (tweenCompleteDota != null)
            {
                dotaCallingCounter++;
                tweenCompleteDota.Play(() => {
                    CheckOnCompleted();
                });
            }
        }
    }
}
