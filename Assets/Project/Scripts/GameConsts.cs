using BeauUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public class GameConsts : MonoBehaviour
    {
        public int SkyboxDist = 1000;
    }

    static public class GameEvents
    {
        static public readonly StringHash32 UISwitched = "world:ui-switched";
    }
}
