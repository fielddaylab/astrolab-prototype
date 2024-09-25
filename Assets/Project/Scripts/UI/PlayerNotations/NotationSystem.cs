using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AstroLab
{
    /// <summary>
    /// The system that creates and manages user notations / player notes for solving puzzles
    /// </summary>
    public class NotationSystem : MonoBehaviour
    {
        [SerializeField] private GameObject m_NotationContainerPrefab;
        [SerializeField] private GameObject m_NotationPrefab;

        [SerializeField] private GraphicRaycaster m_Raycaster;

        private PointerEventData m_CurrPointerData;

        #region Unity Callbacks

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) {
                // On right click, find the overlapping interactive object

                m_CurrPointerData = new PointerEventData(EventSystem.current);
                m_CurrPointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_CurrPointerData, results);

                if (results.Count > 0) {
                    // find closest obj
                    Debug.Log("[Notation] Hit " + results[0].gameObject.name);
                    Transform currObj = results[0].gameObject.transform;
                    Vector3 clickPos = results[0].worldPosition;

                    // If overlapping an existing note, open it (TODO: this logic is currently buggy)
                    if (currObj.GetComponent<PlayerNotation>())
                    {
                        Debug.Log("[Notation] Opening existing notation " + results[0].gameObject.name);
                        OpenExistingNote();
                    }
                    // else create new note
                    else
                    {
                        CreateNewNote(currObj, clickPos);
                    }
                }
            }
        }

        #endregion // Unity Callbacks

        #region Helpers

        private void OpenExistingNote()
        {

        }

        private void CreateNewNote(Transform currObj, Vector3 clickPos)
        {
            // Climb to root parent
            while (currObj != null)
            {
                if (currObj.GetComponent<NotatableRoot>())
                {
                    Debug.Log("[Notation] NotatableRoot: " + currObj.name);
                    break;
                }
                else
                {
                    currObj = currObj.transform.parent;
                }
            }

            // If not a notatable object, return
            if (currObj == null) { return; }

            NotationContainer container = null;

            // find notation container, or create a new one
            bool foundAnyContainer = false;
            for (int i = 0; i < currObj.childCount; i++)
            {
                var currChild = currObj.GetChild(i);
                if (currChild.GetComponent<NotationContainer>())
                {
                    container = currChild.GetComponent<NotationContainer>();
                    foundAnyContainer = true;
                    break;
                }
            }

            if (!foundAnyContainer)
            {
                // create a new one
                container = Instantiate(m_NotationContainerPrefab, currObj).GetComponent<NotationContainer>();
            }

            // Create a new notation obj where the player clicked
            var newNotation = Instantiate(m_NotationPrefab, clickPos, Quaternion.identity, container.transform);
            newNotation.transform.position = clickPos;

            newNotation.GetComponent<PlayerNotation>().Init();
        }

        #endregion // Helpers
    }
}

