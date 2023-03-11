using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AtoGame.OtherModules.Tutorial.Demo
{
    public class TutorialSave : ITutorialSave
    {
        private List<int> lsTutorialKey;
        private bool isCompleteTutorial;

        public void Init(Action onCompleted)
        {
            onCompleted?.Invoke();
        }

        public void Load(Action<bool> onLoaded)
        {
            isCompleteTutorial = PlayerPrefExtension.GetBool("IsCompleteTutorial", false);
            string keys = PlayerPrefExtension.GetString("TutorialKeys", string.Empty);
            lsTutorialKey = new List<int>();
            if (keys.Length == 0)
            {
            }
            else
            {
                string[] savedKeys = keys.Split(',');
                for(int i = 0; i < savedKeys.Length; ++i)
                {
                    lsTutorialKey.Add(int.Parse(savedKeys[i]));
                }
            }
            onLoaded?.Invoke(true);
        }
        public bool CheckHasKey(int key)
        {
            for(int i = 0; i< lsTutorialKey.Count; ++i)
            {
                if(lsTutorialKey[i] == key)
                {
                    return true;
                }
            }
            return false;
        }

        public void PushSave(Action<bool> onResult)
        {
            Save();
        }

        public bool IsTutorialCompleted()
        {
            return isCompleteTutorial;
        }

        public void Save(int[] keys, Action<bool> onResult)
        {
            lsTutorialKey.AddRange(keys);
            Save();
        }

        public void SetTutorialCompleted()
        {
            isCompleteTutorial = true;
            Save();
        }

        private void Save()
        {
            PlayerPrefExtension.SetBool("IsCompleteTutorial", isCompleteTutorial);
            string keysSave = string.Empty;
            for (int i = 0; i < lsTutorialKey.Count; ++i)
            {
                keysSave += lsTutorialKey[i];
                if (i != lsTutorialKey.Count - 1)
                {
                    keysSave += ",";
                }
            }
            PlayerPrefExtension.SetString("TutorialKeys", keysSave);
        }
    }
}
