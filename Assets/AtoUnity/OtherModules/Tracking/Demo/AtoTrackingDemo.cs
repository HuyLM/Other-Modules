using AtoGame.Tracking.Appsflyer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Tracking
{
    public static class AtoTrackingDemo 
    {
        public static void Preload()
        {
            AtoAppsflyerTracking.Instance.Preload();
        }

        private static void LogEvent(string eventName, ParameterBuilder parameterBuilder)
        {
            LogAppsflyer(eventName, parameterBuilder);
        }

        private static void LogAppsflyer(string eventName, ParameterBuilder parameterBuilder)
        {
            AtoAppsflyerTracking.Instance.LogEvent(eventName, parameterBuilder);
        }

        public static void LogLogin(string loginType)
        {
            ParameterBuilder parameter = ParameterBuilder.Create().Add("method", loginType);
            LogEvent("login", parameter);
        }
    }
}
