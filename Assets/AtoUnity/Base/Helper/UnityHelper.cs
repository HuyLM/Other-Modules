﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AtoGame.Base.Helper
{
    public static class UnityHelper
    {

        public static Vector2 Down = new Vector2(0.03f, -0.97f);
        public static readonly string strNull = "null";
        static DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public delegate void Task();

        public static SpriteRenderer ChangeAlpha(this SpriteRenderer g, float newAlpha)
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
        public static T ChangeAlpha<T>(this T g, float newAlpha)
         where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }

        public static ParticleSystem ChangeColorParticle(this ParticleSystem g, Color color)
        {
            ParticleSystem.MainModule main = g.main;
            ParticleSystem.MinMaxGradient mmColor = main.startColor;
            Color c = mmColor.color;
            float alpha = c.a;
            c = color;
            c.a = alpha;
            mmColor.color = c;
            main.startColor = mmColor;
            //g.main = main;
            return g;
        }

        public static T ChangeColor<T>(this T g, Color color)
             where T : Graphic
        {
            g.color = color;
            return g;
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : System.ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static void HideTrail(this TrailRenderer trail)
        {
            HideTrail hider = trail.GetComponent<HideTrail>();
            if (hider == null)
            {
                hider = trail.gameObject.AddComponent<HideTrail>();
            }
            hider.Hide();
        }

        public static void ShowTrail(this TrailRenderer trail)
        {
            HideTrail hider = trail.GetComponent<HideTrail>();
            if (hider == null)
            {
                hider = trail.gameObject.AddComponent<HideTrail>();
            }
            hider.Show();
        }
        #region Invoke
        public static void InvokeExtension(this MonoBehaviour mono, Task task, float delay)
        {
            mono.Invoke(task.Method.Name, delay);
        }

        public static void InvokeRepeatExtension(this MonoBehaviour mono, Task task, float delay, float repeatRate)
        {
            mono.InvokeRepeating(task.Method.Name, delay, repeatRate);
        }

        public static void CancelInvokeExtension(this MonoBehaviour mono, Task task)
        {
            mono.CancelInvoke(task.Method.Name);
        }

        #endregion
        public static void DelayWait(this MonoBehaviour mono, float delay, Action onComplete)
        {
            mono.StartCoroutine(IDelayWait(delay, onComplete));
        }

        private static IEnumerator IDelayWait(float delay, Action onComplete)
        {
            yield return Yielder.Wait(delay);
            onComplete?.Invoke();
        }

        public static void DelayFrame(this MonoBehaviour mono, int numberFrame, Action onComplete)
        {
            mono.StartCoroutine(IDelayFrame(numberFrame, onComplete));
        }

        private static IEnumerator IDelayFrame(int numberFrame, Action onComplete)
        {
            for (int i = 0; i < numberFrame; ++i)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }

        public static void Scale(this Transform transfrom, float scale)
        {
            transfrom.localScale = Vector3.one * scale;
        }

        public static void RotateLocalEuler(this Transform transform, float angle)
        {
            transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        public static void CopyRectTransform(RectTransform from, RectTransform to)
        {
            to.anchorMin = from.anchorMin;
            to.anchorMax = from.anchorMax;
            to.anchoredPosition = from.anchoredPosition;
            to.sizeDelta = from.sizeDelta;
        }

        public static DateTime ConvertToDateTime(long time)
        {
            DateTime dateTime = dtDateTime.AddSeconds(time);
            return dateTime;
        }

        public static long ConvertToTotalSeconds(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - dtDateTime;
            return (long)timeSpan.TotalSeconds;
        }
    }
}
