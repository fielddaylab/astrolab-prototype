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
        static public readonly StringHash32 FocusableClicked = "world:focusable-clicked"; // UIFocusable

        static public readonly StringHash32 InstrumentUnlocksChanged = "world:instrument-unlocks-changed";
        static public readonly StringHash32 NotebookUnlocksChanged = "world:notebook-unlocks-changed";

        static public readonly StringHash32 NotebookTabClicked = "world:notebook-tab-clicked"; // NotebookFlags
    }
}
