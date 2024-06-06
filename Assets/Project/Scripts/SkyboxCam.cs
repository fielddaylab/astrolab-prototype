using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {
    [RequireComponent(typeof(Camera))]
    public class SkyboxCam : MonoBehaviour
    {
        private Camera m_cam;

        private void Start()
        {
            m_cam.farClipPlane = GameMgr.Instance.Consts.SkyboxDist;
        }
    }
}