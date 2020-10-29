/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

namespace RTSSystem 
{
    /// <summary>
    /// RTS Agent with Pathfinding AI and predefined useful functions
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class RTSAgent : MonoBehaviour
    {
        [Header("General Settings")]
        public RTSStats agentStats;
        RTSAgent enemy;

        /// <summary>
        /// Is agent forced to reach its destination
        /// </summary>
        bool isForced = false;

        NavMeshAgent agent;

        /// <summary>
        /// Destination position
        /// </summary>
        Vector3 dest;

        /// <summary>
        /// Visual representation when agent is being selected
        /// </summary>
        GameObject selectedUI;

        bool isSelected = false;
        bool isDead = false;

        #region Unity Systems

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            dest = transform.position;

            // Get selected UI
            int count = transform.childCount;
            for (int i = 0; i < count; i++) 
            {
                if (transform.GetChild(i).name == "selected") 
                {
                    selectedUI = transform.GetChild(i).gameObject;
                    break;
                }
            }

            if (selectedUI != null) selectedUI.SetActive(isSelected);

            CallInAwake();
        }

        /// <summary>
        /// Methods that will be called on Unity Awake, use this instead of Unity Awake (must).
        /// </summary>
        public virtual void CallInAwake() 
        {
        
        }

        private void Update()
        {
            if (isDead) return;

            if (agentStats.HP <= 0) Dead();

            Pathfinding();
            if (isForced) return;

            CallInUpdate();
        }

        /// <summary>
        /// Methods that will be called on Unity Update, use this instead of Unity Update (must).
        /// </summary>
        public virtual void CallInUpdate() 
        {
        
        }

        #endregion

        #region Damage

        /// <summary>
        /// Invoke damage to enemy agent
        /// </summary>
        /// <param name="dmg">Damage amount</param>
        public virtual void InvokeDamage(float dmg) 
        {
            if (Enemy != null)
            {
                if (Enemy.agentStats.HP <= 0)
                {
                    enemy = null;
                    return;
                }
                Enemy.agentStats.HP -= dmg * agentStats.DMGMultiplier;
            }
        }

        /// <summary>
        /// Take damage to agent
        /// </summary>
        /// <param name="dmg">Damage amount</param>
        public virtual void TakeDamage(float dmg) 
        {
            agentStats.HP -= dmg;
            if (agentStats.HP <= 0) Dead();
        }

        #endregion

        #region PathFinding

        /// <summary>
        /// Pathfinding function
        /// </summary>
        void Pathfinding()
        {
            agent.SetDestination(dest);
            if (RemainingDistance() == 0) isForced = false;
            CallInPathFinding();
        }

        /// <summary>
        /// Script to call when agent is in pathfinding mode
        /// </summary>
        public virtual void CallInPathFinding()
        {

        }

        /// <summary>
        /// On agent is being forced to reach its destination
        /// </summary>
        public virtual void OnForced()
        {
            enemy = null;
        }

        /// <summary>
        /// Set agent destination
        /// </summary>
        /// <param name="point">Destination position</param>
        /// <param name="force">Is being forced to reach its destination</param>
        public void SetDestination(Vector3 point, bool force = false)
        {
            isForced = force;
            if (isForced) OnForced();

            dest = point;
            Pathfinding();
        }

        /// <summary>
        /// Remaining distance between agent and its destination
        /// </summary>
        /// <returns>Remaining distance</returns>
        public int RemainingDistance()
        {
            return (int)Vector3.Distance(transform.position, dest);
        }

        /// <summary>
        /// Get destination position
        /// </summary>
        /// <returns>Destination position</returns>
        public Vector3 GetDestination() { return dest; }

        #endregion

        #region Agent Management

        /// <summary>
        /// Select agent
        /// </summary>
        public void Select() 
        {
            isSelected = true;
            if (selectedUI != null) selectedUI.SetActive(isSelected);
        }

        /// <summary>
        /// Unselect agent
        /// </summary>
        public void Unselect() 
        {
            isSelected = false;
            if (selectedUI != null) selectedUI.SetActive(isSelected);
        }

        /// <summary>
        /// Get agent (NavMeshAgent Component)
        /// </summary>
        public NavMeshAgent GetAgent 
        {
            get { return agent; }
        }

        public bool IsSelected 
        {
            get { return isSelected; }
        }

        public bool IsDead { get { return isDead; } }

        /// <summary>
        /// Get and set agent enemy
        /// </summary>
        public RTSAgent Enemy { get { return enemy; } set { enemy = value; } }

        /// <summary>
        /// Function to call when agent is dead
        /// </summary>
        void Dead()
        {
            RTSControl.Instance.DeselectAgent(this);

            GetComponent<Collider>().enabled = false;

            agentStats.HP = 0;
            isDead = true;

            CallInDead();
        }

        /// <summary>
        /// Function to call when agent is dead (for inherit class)
        /// </summary>
        public virtual void CallInDead() 
        {
    
        }

        /// <summary>
        /// Destroy agent from world
        /// </summary>
        public void DestroyAgent()
        {
            GameObject.Destroy(gameObject);
        }

        #endregion
    }
}