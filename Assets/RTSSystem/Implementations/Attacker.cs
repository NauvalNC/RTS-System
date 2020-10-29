/*
 * Author: Nauval Muhammad Firdaus
 */

using RTSSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Implemenation sample class that demonstrate how to use RTS Agent.
/// This class is about RTS Agent that intended to be an attacker.
/// Agent will stat attack if enemy agent is within range.
/// </summary>
[RequireComponent(typeof(Animator))]
public class Attacker : RTSAgent
{
    /// <summary>
    /// Enemy tag to attack
    /// </summary>
    public string enemyTag = "enemy";

    /// <summary>
    /// Enemy detection radius
    /// </summary>
    public float detectRadius = 8f;

    /// <summary>
    /// Start attack enemy when its reach enemy by offset of this variable.
    /// </summary>
    public float startAttackDist = 5f;

    Animator anim;

    public override void CallInAwake()
    {
        base.CallInAwake();
        anim = GetComponent<Animator>();
    }

    public override void CallInPathFinding()
    {
        base.CallInPathFinding();
        anim.SetBool("attack", false);
        anim.SetFloat("move", GetAgent.velocity.magnitude);
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    public override void CallInUpdate()
    {
        base.CallInUpdate();
        DetectEnemy();
        Attack();
    }

    public override void CallInDead() 
    {
        anim.SetBool("attack", false);
        anim.SetBool("dead", true);
    }

    /// <summary>
    /// Invoke attack procedure
    /// </summary>
    void Attack() 
    {
        if (Enemy != null) 
        {
            SetDestination(Enemy.transform.position);
            if (Vector3.Distance(transform.position, GetDestination()) <= startAttackDist) 
            {
                SetDestination(transform.position);
                anim.SetBool("attack", true);
                transform.LookAt(new Vector3(Enemy.transform.position.x, transform.position.y, Enemy.transform.position.z));
            }
        }
    }

    /// <summary>
    /// Detect enemy within detect range
    /// </summary>
    void DetectEnemy() 
    {
        if (Enemy != null) 
        {
            if (Vector3.Distance(transform.position, Enemy.transform.position) > detectRadius) Enemy = null;
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
        List<RTSAgent> enemies = new List<RTSAgent>();
        RTSAgent agent;
        for (int i = 0; i < colliders.Length; i++) 
        {
            agent = colliders[i].GetComponent<RTSAgent>();
            if (agent != null && agent.tag == enemyTag) enemies.Add(agent);
        }

        if (enemies.Count > 0) 
        {
            Enemy = enemies[Random.Range(0, enemies.Count)];
        }
    }
}
