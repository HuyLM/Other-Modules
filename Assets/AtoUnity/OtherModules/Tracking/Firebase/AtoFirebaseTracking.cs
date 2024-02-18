using AtoGame.Base;
#if FIREBASE_ENABLE
using Firebase;
using Firebase.Analytics;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtoGame.Tracking.FB
{
    public class AtoFirebaseTracking : Singleton<AtoFirebaseTracking>
    {
        private bool available;
        private event System.Action<bool> callOnAvailable;


        protected override void Initialize()
        {
            base.Initialize();

            if (Initialized)
                return;

            available = false;
#if FIREBASE_ENABLE
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((Task<DependencyStatus> task) =>
            {
                var dependencyStatus = task.Result;

                if (dependencyStatus == DependencyStatus.Available)
                {
                    OnFirebaseAvailable();
                }
                else
                {
                    TrackingLogger.Log($"[FIREBASE] Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
#else
            TrackingLogger.Log("FIREBASE_ENABLE flag has't defined");
#endif
        }

#if FIREBASE_ENABLE
        private void OnFirebaseAvailable()
        {
            if (available) return;
            TrackingLogger.Log("[FIREBASE] OnFirebaseAvailable");
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            TrackingLogger.Log("[FIREBASE] OnFirebaseAvailable - After set analyticscollectionenable true");

            if (callOnAvailable != null)
            {
                TrackingLogger.Log("[FIREBASE] OnFirebaseAvailable - callOnAvailable");
                callOnAvailable.Invoke(false);
                TrackingLogger.Log("[FIREBASE] OnFirebaseAvailable - after callOnAvailable");
                callOnAvailable = null;
            }
            available = true;
        }


        private void LogEvent(string eventName, params Parameter[] para)
        {
            Push((bool isPush) =>
            {
                if (para == null)
                    FirebaseAnalytics.LogEvent(eventName);
                else
                    FirebaseAnalytics.LogEvent(eventName, para);
            });
        }
#endif
        public void LogEvent(string eventName, ParameterBuilder parameterBuilder)
        {
#if FIREBASE_ENABLE
            this.LogEvent(eventName, parameterBuilder != null ? parameterBuilder.BuildFirebase() : null);
            DebugLog(eventName, parameterBuilder);
#else
            TrackingLogger.Log("FIREBASE_ENABLE flag has't defined");
#endif
        }

        private void DebugLog(string eventName, ParameterBuilder parameterBuilder)
        {
            StringBuilder paramLogs = new StringBuilder();
            if (parameterBuilder != null && parameterBuilder.Params.Count > 0)
            {
                paramLogs.Append(" /");
                foreach (KeyValuePair<string, object> entry in parameterBuilder.Params)
                {
                    paramLogs.Append(" " + entry.Key + "=" + entry.Value.ToString());
                }
            }
            TrackingLogger.Log($"[FIREBASE-Analytics:" + " EventName = " + eventName + paramLogs.ToString());
        }

        private void Push(System.Action<bool> action)
        {
            try
            {
                if (available)
                    action.Invoke(true);
                else
                    callOnAvailable += action;
            }
            catch { throw; }
        }
    }
}
