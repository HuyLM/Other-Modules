using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialConfig", menuName = "Data/OtherModules/Tutorial/TutorialConfig")]
    public class TutorialConfig : ScriptableObject, ITutorialConfig
    {
        [SerializeField] private TutorialData[] tutorialDatas;
        [SerializeField] private int[] endTutorialKeys;
        [SerializeField] private TutorialData[] extraTutorialDatas;
        [SerializeField] private bool enableLog = true;
        [SerializeField] private bool enableTutorial = true;
        [SerializeField] private bool enableSkipAll;

        public bool EnableLog()
        {
            return enableLog;
        }

        public bool EnableSkipAll()
        {
            return enableSkipAll;
        }

        public bool EnableTutorial()
        {
            return enableTutorial;
        }

        public int[] GetEndTutorialKeys()
        {
            return endTutorialKeys;
        }

        public TutorialData[] GetExtraTutorialDatas()
        {
            return extraTutorialDatas;
        }

        public TutorialData[] GetTutorialDatas()
        {
            return tutorialDatas;
        }

        public TutorialData FindTutorialData(int key)
        {
            foreach (var item in tutorialDatas)
            {
                if(item.Key == key)
                {
                    return item;
                }
            }

            foreach (var item in extraTutorialDatas)
            {
                if (item.Key == key)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
