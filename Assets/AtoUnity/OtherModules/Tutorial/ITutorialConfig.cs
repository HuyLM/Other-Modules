using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Tutorial
{
    public interface ITutorialConfig
    {
        TutorialData[] GetTutorialDatas();
        int[] GetEndTutorialKeys();
        TutorialData[] GetExtraTutorialDatas();
        bool EnableLog();
        bool EnableTutorial();
        bool EnableSkipAll();
    }
}
