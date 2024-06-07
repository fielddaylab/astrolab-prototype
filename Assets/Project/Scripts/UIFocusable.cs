using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [RequireComponent(typeof(Renderer))]
    public class UIFocusable : MonoBehaviour
    {
        [SerializeField] private UIFocus m_focusVisual;

        private Renderer m_renderer;

        #region Unity Callbacks

        private void Start()
        {
            m_renderer = GetComponent<Renderer>();

            if (m_renderer)
            {
                m_focusVisual.gameObject.SetActive(m_renderer.isVisible);
            }
            else
            {
                m_focusVisual.gameObject.SetActive(false);
            }
        }

        private void OnBecameVisible()
        {
            m_focusVisual.gameObject.SetActive(true);
        }

        private void OnBecameInvisible()
        {
            m_focusVisual.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            // calculate this object's screen position
            var point = GameMgr.Instance.MainCamera.WorldToScreenPoint(this.transform.position);

            m_focusVisual.Rect.anchoredPosition = point;
        }

        #endregion // Unity Callbacks
    }
}