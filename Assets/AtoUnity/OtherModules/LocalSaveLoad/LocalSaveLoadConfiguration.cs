using AtoGame.Base;
using AtoGame.Base.UnityInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.LocalSaveLoad
{
    [CreateAssetMenu(fileName = "LocalSaveLoadConfiguration", menuName = "Data/OtherModules/LocalSaveLoad/LocalSaveLoadConfiguration")]
    public class LocalSaveLoadConfiguration : ScriptableObject
    {
        [SerializeField] private PipelineLocalStepConfig[] localStepConfigs;

        public int SaveVersion()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return int.Parse(RuntimePlayerSettings.iOSBuildVersion);
            }
            if (Application.platform == RuntimePlatform.Android)
            {
                return RuntimePlayerSettings.AndroidBundleVersionCode;
            }
#if UNITY_ANDROID
            return RuntimePlayerSettings.AndroidBundleVersionCode;
#elif UNITY_IOS
            return RuntimePlayerSettings.iOSBuildVersion;
#endif
            return 0;
        }

        private void OnValidate()
        {
            Array.Sort(localStepConfigs, new Comparison<PipelineLocalStepConfig>((i1, i2) => i1.Version.CompareTo(i2.Version)));
        }

        public List<PipelineLocalStepConfig> GetNextLocalSaveSteps(int preVersion)
        {
            List<PipelineLocalStepConfig> nextSaveSteps = new List<PipelineLocalStepConfig>();
            for (int i = 0; i < localStepConfigs.Length; ++i)
            {
                if (localStepConfigs[i].Version >= preVersion)
                {
                    nextSaveSteps.Add(localStepConfigs[i]);
                }
            }
            return nextSaveSteps;
        }

    }
}
