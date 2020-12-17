/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage building task to build by agents
/// </summary>
public class TaskManager : MonoBehaviour
{
    /// <summary>
    /// Singletone instance
    /// </summary>
    static TaskManager instance;
    public static TaskManager Instance 
    {
        get 
        {
            if (instance == null) instance = FindObjectOfType<TaskManager>();
            return instance;
        }
    }

    public List<Building> tasks;
    public GameObject[] blackSmith;
    public GameObject[] forest;

    List<Worker> workers = new List<Worker>();

    private void Awake()
    {
        Worker[] temp = FindObjectsOfType<Worker>();
        workers.AddRange(temp);
    }

    private void Update()
    {
        DoTasks();
    }

    /// <summary>
    /// Set agents to do buling tasks
    /// </summary>
    void DoTasks() 
    {
        if (tasks.Count <= 0) return;

        for (int i = 0; i < workers.Count; i++)
        {
            if (workers[i].isArrived == false && workers[i].task == null) workers[i].SetBuildingTask(tasks[Random.Range(0, tasks.Count)]);
        }
    }

    public GameObject GetBlackSmith() 
    {
        return blackSmith[Random.Range(0, blackSmith.Length)];
    }

    public GameObject GetForest()
    {
        return forest[Random.Range(0, forest.Length)];
    }
}
