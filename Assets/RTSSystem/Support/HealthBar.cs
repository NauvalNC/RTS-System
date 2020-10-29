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
    /// Simple Health UI controller
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        public RectTransform HPBar;
        public RTSAgent agent;
        float initHP;

        void Awake() 
        {
            initHP = agent.agentStats.HP;
        }

        void Update() 
        {
            transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
            HPBar.localScale = new Vector2((agent.agentStats.HP / initHP), HPBar.localScale.y);
        }
    }
}