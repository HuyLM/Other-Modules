using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace AtoGame.Base.UI
{
    public class HUDManager : SingletonBind<HUDManager>
    {

        private class HUDComparer : IComparer<HUD>
        {
            public int Compare(HUD x, HUD y)
            {
                return x.Order.CompareTo(y.Order);
            }
        }

        private readonly List<HUD> huds = new List<HUD>();
        private static readonly HUDComparer comparer = new HUDComparer();
        private EventSystem eventSystem;

        private void Start()
        {
            eventSystem = EventSystem.current;
        }

        public static int GetHudCount()
        {
            return Instance.huds.Count;
        }

        public static IEnumerable<HUD> GetHuds()
        {
            return Instance.huds;
        }

        public static void Clear()
        {
            Instance.huds.Clear();
        }

        public static void Add(HUD hud)
        {
            if (Instance.huds.Contains(hud))
            {
                return;
            }

            Instance.huds.Add(hud);
            Instance.huds.Sort(comparer);
        }

        public static bool Remove(HUD hud)
        {
            if (Initialized)
            {
                return Instance.huds.Remove(hud);
            }
            return false;
        }

        public static void IgnoreUserInput(bool ignore)
        {
            if (Instance)
            {
                if (!Instance.eventSystem)
                {
                    Instance.eventSystem = EventSystem.current;
                }
                if (Instance.eventSystem)
                {
                    Instance.eventSystem.enabled = !ignore;
                }
                Instance.enabled = !ignore;
            }
        }

        private void Update()
        {
            return;
            for (int i = huds.Count - 1; i >= 0; i--)
            {
                if (huds[i].OnUpdate())
                {
                    return;
                }
            }
        }
    }
}
