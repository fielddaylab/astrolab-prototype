using BeauUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public class GameConsts : MonoBehaviour
    {
        public int SkyboxDist = 1000;

        public int IncorrectIDPenalty = -1;

        public Color CorrectColor = Color.green;
        public Color IncorrectColor = Color.red;
    }

    static public class GameEvents
    {
        static public readonly StringHash32 UISwitched = "world:ui-switched";
        static public readonly StringHash32 FocusableClicked = "world:focusable-clicked"; // UIFocusable
        static public readonly StringHash32 Unfocus = "world:unfocus";

        static public readonly StringHash32 InstrumentUnlocksChanged = "world:instrument-unlocks-changed";
        static public readonly StringHash32 NotebookUnlocksChanged = "world:notebook-unlocks-changed";

        static public readonly StringHash32 NotebookTabClicked = "world:notebook-tab-clicked"; // NotebookFlags
        static public readonly StringHash32 CelestialObjIdentified = "world:celestial-obj-identified";

        static public readonly StringHash32 DraggableGrabbed = "world:draggable-grabbed";
        static public readonly StringHash32 DraggableDropped = "world:draggable-dropped";

        static public readonly StringHash32 DataSlotFilled = "world:data-slot-filled";
        static public readonly StringHash32 DataSlotCleared = "world:data-slot-cleared";

    }

}
