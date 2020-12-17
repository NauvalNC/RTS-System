/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSSystem;
using UnityEngine.UI;

/// <summary>
/// Worker agent, sample of RTSAgent implementation
/// </summary>
public class Worker : RTSAgent
{
    public float woods = 0;
    public float power = 0;
    public Building task;

    /// <summary>
    /// Item detection radius
    /// </summary>
    public float detectRadius = 8f;
    public LayerMask mask;

    Animator anim;
    GameObject target;
    GameObject blackSmith, forest;

    public bool isArrived = false;

    public GameObject HUD;
    public Text statusText;

    public override void CallInAwake()
    {
        base.CallInAwake();
        anim = GetComponent<Animator>();
    }

    public override void CallInUpdate()
    {
        base.CallInUpdate();
        
        if (task == null || task.IsCompleted()) 
        {
            StopAllCoroutines();
            GetAgent.isStopped = true;
            isArrived = false;
            target = null;
            task = null;
        } else 
        {
            GetAgent.isStopped = isArrived;
        }

        DisplayUI();

        CallGOB();
        DetectTarget();
    }

    void DisplayUI() 
    {
        HUD.transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
        statusText.text = "Woods: " + woods.ToString() + "\nPower: " + power.ToString();
    }

    public override void CallInPathFinding()
    {
        base.CallInPathFinding();

        if (isArrived) 
        {
            anim.SetFloat("move", 0);
        }
        else 
        {
            anim.SetBool("attack", false);
            anim.SetFloat("move", GetAgent.velocity.magnitude);
        }
    }

    /// <summary>
    /// Set building task to build by agent
    /// </summary>
    /// <param name="task">Building to build</param>
    public void SetBuildingTask(Building task) 
    {
        this.task = task;
        blackSmith = TaskManager.Instance.GetBlackSmith();
        forest = TaskManager.Instance.GetForest();
    }

    /// <summary>
    /// Goal Orientation Behaviour for agent
    /// </summary>
    void CallGOB() 
    {
        if (task == null || isArrived || task.IsCompleted()) return;

        // Build
        if (woods > 0) 
        {
            target = task.gameObject;
        } 
        // Chop Woods
        else if (power > 0) 
        {
            target = forest;
        } 
        // Go to black smith
        else if (power <= 0) 
        {
            target = blackSmith;
        }

        SetDestination(target.transform.position);
    }

    /// <summary>
    /// Detect interactable object by agent
    /// </summary>
    void DetectTarget() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, mask);

        for (int i = 0; i < colliders.Length; i++) 
        {
            if (colliders[i].gameObject == target) 
            {
                OnArrive();
            }
        }
    }

    /// <summary>
    /// On agent arrive at some target
    /// </summary>
    void OnArrive() 
    {
        if (isArrived) return;

        isArrived = true;
        SetDestination(transform.position);
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    
        // If target is the building task
        if (target == task.gameObject) 
        {
            StartCoroutine(IBuild());
        } else if (target == blackSmith) 
        {
            StartCoroutine(IForgeTool());
        } else if (target == forest) 
        {
            StartCoroutine(IChopTree());
        }
    }

    /// <summary>
    /// On sub-task of main task GOB (Goal Oriented Behaviour)
    /// </summary>
    void OnSubTask()
    {
        isArrived = false;
    }

    IEnumerator IForgeTool() 
    {
        yield return new WaitForSeconds(1f);
        power += 20;
        OnSubTask();
    }

    IEnumerator IChopTree() 
    {
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(3f);
        power -= 10;
        woods += 10;
        OnSubTask();
    }

    IEnumerator IBuild() 
    {
        anim.SetBool("attack", true);
        while (woods > 0 && !task.IsCompleted()) 
        {
            yield return new WaitForSeconds(0.25f);
            woods -= 2;
            task.MakeProgress(2f);
        }
        OnSubTask();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
