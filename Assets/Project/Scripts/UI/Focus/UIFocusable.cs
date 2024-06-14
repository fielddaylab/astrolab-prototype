using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [RequireComponent(typeof(Renderer))]
    public class UIFocusable : MonoBehaviour
    {
        public Renderer Renderer;

        public EventHandler BecameVisible;
        public EventHandler BecameInvisible;

        public CelestialObject CelestialObj;

        public string ID;

        #region Unity Callbacks

        public void Init(CelestialObject celObj)
        {
            Renderer = GetComponent<Renderer>();

            CelestialObj = celObj;
            ID = celObj.name;
        }

        private void OnBecameVisible()
        {
            BecameVisible?.Invoke(this, EventArgs.Empty);
        }

        private void OnBecameInvisible()
        {
            BecameInvisible?.Invoke(this, EventArgs.Empty);
        }

        #endregion // Unity Callbacks
    }
}