using Ftech.Lib.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ftech.Lib.Common
{
    public partial class EventKey
    {
        public struct OnItemInventoryChanged : IEventParams
        {
        }

        public struct OnCupChanged : IEventParams
        {
            public long PreCup;
            public long CurCup;
            public long TopCup;
        }

        public struct OnProfileUserChanged : IEventParams
        {

        }

        public struct OnAvatarChanged : IEventParams
        {

        }
    }
}