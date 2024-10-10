using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    [System.Serializable]
    public struct BoolValues 
    {
        public bool FromCurrent;
        [HideIf("FromCurrent")] public bool From;
        public bool To;
    }

    [System.Serializable]
    public struct FloatValues {
        public bool FromCurrent;
        [HideIf("FromCurrent")] public float From;
        public float To;
    }

    [System.Serializable]
    public struct IntValues {
        public bool FromCurrent;
        [HideIf("FromCurrent")] public int From;
        public int To;
    }

    [System.Serializable]
    public struct Vector2Values {
        public bool FromCurrent;
        [OnValueChanged(nameof(OnUniformValueChanged))] public bool Uniform;
        [HideIf("@FromCurrent || Uniform")] public Vector2 From;
        [HideIf("Uniform")] public Vector2 To;
        [ShowIf("@(Uniform && !FromCurrent)"), LabelText("From")] public float FromUniformValue;
        [ShowIf("Uniform"), LabelText("To")] public float ToUniformValue;

        private void OnUniformValueChanged()
        {
            if (Uniform)
            {
                FromUniformValue = From.x;
                ToUniformValue = To.x;
            }
            else
            {
                From = Vector2.one * FromUniformValue;
                To = Vector2.one * ToUniformValue;
            }
        }
    }

    [System.Serializable]
    public struct Vector3Values {
        public bool FromCurrent;
        [OnValueChanged(nameof(OnUniformValueChanged))] public bool Uniform;
        [HideIf("@FromCurrent || Uniform")] public Vector3 From;
        [HideIf("Uniform")] public Vector3 To;
        [ShowIf("@(Uniform && !FromCurrent)"), LabelText("From")] public float FromUniformValue;
        [ShowIf("Uniform"), LabelText("To")] public float ToUniformValue;
     
        private void OnUniformValueChanged()
        {
            if (Uniform)
            {
                FromUniformValue = From.x;
                ToUniformValue = To.x;
            }
            else
            {
                From = Vector3.one * FromUniformValue;
                To = Vector3.one * ToUniformValue;
            }
        }
    }

    [System.Serializable]
    public struct RelativeVector2Values {
        public bool FromCurrent;
        [OnValueChanged(nameof(OnUniformValueChanged))] public bool Uniform;
        [HideIf("@FromCurrent || Uniform")] public Vector2 From;
        [HideIf("Uniform")] public Vector2 To;
        [ShowIf("@(Uniform && !FromCurrent)"), LabelText("From")] public float FromUniformValue;
        [ShowIf("Uniform"), LabelText("To")] public float ToUniformValue;
        public bool IsRelative;

        private void OnUniformValueChanged()
        {
            if (Uniform)
            {
                FromUniformValue = From.x;
                ToUniformValue = To.x;
            }
            else
            {
                From = Vector2.one * FromUniformValue;
                To = Vector2.one * ToUniformValue;
            }
        }
    }

    [System.Serializable]
    public struct RelativeVector3Values {
        public bool FromCurrent;
        [OnValueChanged(nameof(OnUniformValueChanged))] public bool Uniform;
        [HideIf("@FromCurrent || Uniform")] public Vector3 From;
        [HideIf("Uniform")] public Vector3 To;
        [ShowIf("@(Uniform && !FromCurrent)"), LabelText("From")] public float FromUniformValue;
        [ShowIf("Uniform"), LabelText("To")] public float ToUniformValue;
        public bool IsRelative;
        private void OnUniformValueChanged()
        {
            if (Uniform)
            {
                FromUniformValue = From.x;
                ToUniformValue = To.x;
            }
            else
            {
                From = Vector3.one * FromUniformValue;
                To = Vector3.one * ToUniformValue;
            }
        }
    }

    [System.Serializable]
    public struct ColorValues {
        public bool FromCurrent;
        [HideIf("FromCurrent")] public Color From;
        public Color To;

        public ColorValues(bool fromCurrent, Color from, Color to)
        {
            FromCurrent = fromCurrent;
            From = from;
            To = to;    
        }

        public static ColorValues Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new ColorValues(false, Color.white, Color.black);
            }
        }

    }
}
