/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSSystem 
{
    /// <summary>
    /// Playable RTS Agents controller
    /// </summary>
    public class RTSControl : MonoBehaviour
    {
        private static RTSControl instance;
        public static RTSControl Instance 
        {
            get { if (instance == null) instance = FindObjectOfType<RTSControl>(); return instance; }
        }

        [Header("RTS General")]
        public Camera cam;
    
        public RectTransform selectionBox;

        /// <summary>
        /// Playable agents mask layer
        /// </summary>
        public LayerMask mask;

        [Header("RTS Formation Settings")]
        public float formationOffset = 2; //Offset between agent in formation position

        /// <summary>
        /// Last Left Mouse Click position
        /// </summary>
        Vector2 LMCPos;
    
        /// <summary>
        /// Destination position where agent should move to.
        /// </summary>
        Vector3 destPos;

        /// <summary>
        /// Object that hit by ray.
        /// </summary>
        GameObject hitObj;

        public List<RTSAgent> playableUnits = new List<RTSAgent>();
        List<RTSAgent> selectedUnit = new List<RTSAgent>();

        RaycastHit hit;
        Ray ray;

        private void Awake()
        {
            GameObject[] playable = GameObject.FindGameObjectsWithTag("playable");
            for (int i = 0; i < playable.Length; i++) 
            {
                playableUnits.Add(playable[i].GetComponent<RTSAgent>());
            }
        }

        private void Update()
        {
            ListenInputEvent();        
        }

        /// <summary>
        /// Listen to input event by user
        /// </summary>
        void ListenInputEvent() 
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetDestination();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectionBox.sizeDelta != Vector2.zero)
                {
                    ReleaseSelectionBox();
                }
                else
                {
                    RayHitEvent(hitObj);
                }
            }

            if (Input.GetMouseButton(0))
            {
                UpdateSelectionBox(Input.mousePosition);
            }
    
            if (Input.GetMouseButtonDown(1)) 
            {
                UnselectAllUnit();
            }
        }

        /// <summary>
        /// Set destination position for agents by clicking the screen
        /// </summary>
        void SetDestination() 
        {
            LMCPos = Input.mousePosition;

            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, mask))
            {
                destPos = hit.point;
                hitObj = hit.collider.gameObject;
            }
        }

        /// <summary>
        /// On release selection box, select all agents within the selection box
        /// </summary>
        void ReleaseSelectionBox() 
        {
            selectionBox.gameObject.SetActive(false);

            UnselectAllUnit();

            Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
            Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

            Vector3 screenPos;
            foreach (RTSAgent agent in playableUnits) 
            {
                if (agent == null) continue;

                screenPos = cam.WorldToScreenPoint(agent.transform.position);
            
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y) 
                {
                    selectedUnit.Add(agent);
                    agent.Select();
                }
            }

            selectionBox.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Unselect all agents from selected agents.
        /// </summary>
        void UnselectAllUnit() 
        {
            foreach (RTSAgent agent in selectedUnit) agent.Unselect();
            selectedUnit.Clear();
        }

        /// <summary>
        /// Create selection box to select multiple agents
        /// </summary>
        /// <param name="curMousePos">Current mouse position</param>
        void UpdateSelectionBox(Vector2 curMousePos) 
        {
            float width = curMousePos.x - LMCPos.x;
            float height = curMousePos.y - LMCPos.y;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = LMCPos + new Vector2(width / 2, height / 2);

            if (!selectionBox.gameObject.activeInHierarchy) selectionBox.gameObject.SetActive(true);
        }

        /// <summary>
        /// Decide what to do with hit object by raycast
        /// </summary>
        /// <param name="obj">Hit object by raycast</param>
        void RayHitEvent(GameObject obj) 
        {
            // Select one agent
            if (obj.tag == "playable" && selectionBox.sizeDelta == Vector2.zero)
            {
                RTSAgent agent = obj.GetComponent<RTSAgent>();
                UnselectAllUnit();
                selectedUnit.Add(agent);
                agent.Select();
            } else 
            {
                MoveUnit();
            }
        }

        /// <summary>
        /// Move agents to destination
        /// </summary>
        void MoveUnit() 
        {
            Vector3[] formation = GetFormationPos(selectedUnit.Count, formationOffset);

            int count = 0;
            foreach (RTSAgent unit in selectedUnit)
            {
                unit.SetDestination(destPos + formation[count++], true);
            }
        }

        /// <summary>
        /// Deselect one agent from selected agents
        /// </summary>
        /// <param name="agent">Agent ot deselect</param>
        public void DeselectAgent(RTSAgent agent) 
        {
            agent.Unselect();
            selectedUnit.Remove(agent);
        }

        /// <summary>
        /// Generate formation position in grid NxN
        /// </summary>
        /// <param name="numOfUnit">Number of agents to form</param>
        /// <param name="offset">Offset between agents in formation</param>
        /// <returns>Array of formation position in NxN</returns>
        Vector3[] GetFormationPos(int numOfUnit, float offset) 
        {
            int sqrFactor = Mathf.CeilToInt(Mathf.Sqrt(numOfUnit));
        
            Vector3[] formation = new Vector3[sqrFactor * sqrFactor];

            int count = 0;
            for (int i = 0; i < sqrFactor; i++) 
            {
                for (int j = 0; j < sqrFactor; j++) 
                {
                    formation[count] = new Vector3(i, 0, j);
                    formation[count] *= offset;
                    count++;
                }
            }

            return formation;
        }
    }
}