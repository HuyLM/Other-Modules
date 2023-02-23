using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Tutorial
{
    public class TutorialController : Singleton<TutorialController>
    {
        ITutorialConfig config;
        ITutorialSave saver;
        ITutorialUI ui;

        private bool isInitialized;
        private bool isShowingTutorial;
        private Action onCompleted;
        private TutorialData curTutorialData;

        public ITutorialConfig Config { get => config; }
        public ITutorialSave Saver { get => saver; }
        public ITutorialUI UI { get => ui; }

        public void Init(ITutorialConfig config, ITutorialSave save, ITutorialUI ui)
        {
            this.config = config;
            saver = save;
            this.ui = ui;
            isInitialized = true;
        }


        public void ShowTutorial(int key, Action onCompleted = null, Action onFailed = null)
        {
            if (!isInitialized)
            {
                Log($"Tutorial is not initialize");
                onFailed?.Invoke();
                return;
            }
            if (!Config.EnableTutorial())
            {
                Log($"Tutorial is disabled");
                onFailed?.Invoke();
                return;
            }
            if (isShowingTutorial)
            {
                Log($"Tutorial is showing");
                onFailed?.Invoke();
                return;
            }
         
            TutorialData curTutorialData = FindTutorialData(key);
            if (curTutorialData == null)
            {
                Log($"Not have key: {key}");
                onFailed?.Invoke();
                return;
            }
            if(curTutorialData.CanShowTutorial())
            {
                this.onCompleted = onCompleted;
                ShowingTutorial(curTutorialData);
            }
        }

        public TutorialData FindTutorialData(int key)
        {
            TutorialData[] tutorialDatas = Config.GetTutorialDatas();
            for (int i = 0; i < tutorialDatas.Length; ++i)
            {
                if (tutorialDatas[i].Key == key)
                {
                    return tutorialDatas[i];
                }
            }
            TutorialData[] extraTutorialDatas = Config.GetExtraTutorialDatas();
            for (int i = 0; i < extraTutorialDatas.Length; ++i)
            {
                if (extraTutorialDatas[i].Key == key)
                {
                    return extraTutorialDatas[i];
                }
            }
            return null;
        }

        public bool IsTutorialCompleted()
        {
            bool result = Saver.IsTutorialCompleted() && isShowingTutorial == false;
            return result;
        }

        public bool CheckTutorialCompleted(params int[] keys)
        {
            if (!isInitialized)
            {
                Log($"Tutorial is not initialize");
                return true;
            }
            if (!Config.EnableTutorial())
            {
                Log($"Tutorial is disabled");
                return true;
            }
            if (IsTutorialCompleted())
            {
                return true;
            }
            if (keys == null)
            {
                return false;
            }
            if (keys.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < keys.Length; ++i)
            {
                TutorialData data = FindTutorialData(keys[i]);
                if (isShowingTutorial && data == curTutorialData)
                {
                    return false;
                }

                if (data != null && data.IsActive == false)
                {
                    return true;
                }

                bool hasKey = Saver.CheckHasKey(keys[i]);
                if (hasKey == true)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckExtraTutorialCompleted(params int[] keys)
        {
            if (!isInitialized)
            {
                Log($"Tutorial is not initialize");
                return true;
            }
            if (!Config.EnableTutorial())
            {
                Log($"Tutorial is disabled");
                return true;
            }
            if (keys == null)
            {
                return false;
            }
            if (keys.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < keys.Length; ++i)
            {
                TutorialData data = FindTutorialData(keys[i]);
                if (isShowingTutorial && data == curTutorialData)
                {
                    return false;
                }

                if (data != null && data.IsActive == false)
                {
                    return true;
                }

                bool hasKey = Saver.CheckHasKey(keys[i]);
                if (hasKey == true)
                {
                    return true;
                }
            }
            return false;
        }

        protected void ShowingTutorial(TutorialData tutorialData)
        {
            isShowingTutorial = true;
            curTutorialData = tutorialData;
            tutorialData.Show(OnTutorialCompleted);
            
            if (Config.EnableSkipAll())
            {
                UI.SetShowSkipAllButton(true);
            }
        }

        private void OnTutorialCompleted()
        {
            isShowingTutorial = false;
            if (Config.EnableSkipAll())
            {
                UI.SetShowSkipAllButton(false);
            }
            onCompleted?.Invoke();
        }

        public void ShowCurrentStep()
        {
            if (isInitialized)
            {
                Log($"Tutorial is not initialize");
                return;
            }
            if (!Config.EnableTutorial())
            {
                Log($"Tutorial is disabled");
                return;
            }
            if (!isShowingTutorial)
            {
                Log($"Tutorial is not showing");
                return;
            }
            if (curTutorialData == null)
            {
                Log($"curTutorialData null");
                return;
            }
            curTutorialData.ForceShowCurrentStep();
        }


        public void EndCurrentStep()
        {
            if (isInitialized)
            {
                Log($"Tutorial is not initialize");
                return;
            }
            if (!Config.EnableTutorial())
            {
                Log($"Tutorial is disabled");
                return;
            }
            if (curTutorialData == null)
            {
                Log($"curTutorialData null");
                return;
            }
            curTutorialData.EndCurrentStep();
        }

        public void AssignTarget(int key, int stepIndex, GameObject target)
        {
            if (isInitialized)
            {
                return;
            }
            if (!Config.EnableTutorial())
            {
                return;
            }
            TutorialData data = FindTutorialData(key);
            if (data != null)
            {
                TutorialStep step = data.GetTutorialStep(stepIndex);
                if (step != null)
                {
                    step.AssignTarget(target);
                }
            }
        }

        public void AssignCallBackStep(int key, int stepIndex, Action onStart = null, Action onEnd = null)
        {
            if (isInitialized)
            {
                return;
            }
            if (!Config.EnableTutorial())
            {
                return;
            }
            if (onStart == null && onEnd == null)
            {
                return;
            }
            TutorialData data = FindTutorialData(key);
            if (data != null)
            {
                TutorialStep step = data.GetTutorialStep(stepIndex);
                if (step != null)
                {
                    step.AssignCallback(onStart, onEnd);
                }
            }
        }

        public void AssignSkipCallBack(int key, Action onSkip = null)
        {
            if (isInitialized)
            {
                return;
            }
            if (!Config.EnableTutorial())
            {
                return;
            }
            if (onSkip == null)
            {
                return;
            }
            TutorialData data = FindTutorialData(key);
            if (data != null)
            {
                data.AssignSkipCallBack(onSkip);
            }
        }

        public void Skip()
        {
            if (isShowingTutorial)
            {
                curTutorialData.Skip();
            }
        }

        public void SkipAll()
        {
            Skip();
            Saver.SetTutorialCompleted();
        }

        public void RemoveReference()
        {
            TutorialData[] tutorialDatas = Config.GetTutorialDatas();
            for (int i = 0; i < tutorialDatas.Length; ++i)
            {
                tutorialDatas[i].RemoveReference();
            }
            TutorialData[] extraTutorialDatas = Config.GetExtraTutorialDatas();
            for (int i = 0; i < extraTutorialDatas.Length; ++i)
            {
                extraTutorialDatas[i].RemoveReference();
            }
        }


        public void SaveKeys(int[] keys)
        {
            Saver.Save(keys, (result) => {
                SaveKeys(keys);
            });
        }

        public void ForceSave()
        {
            Saver.ForceSave((result) => {
                ForceSave();
            });
        }

        public void Log(string log)
        {
            if (Config.EnableLog())
            {
                Debug.Log("[Tutorial] " + log);
            }
        }
    }
}
