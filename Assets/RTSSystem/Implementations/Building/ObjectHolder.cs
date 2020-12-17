/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object holder to spawn (used on UI button to spawn object)
/// </summary>
public class ObjectHolder : MonoBehaviour
{
    /// <summary>
    /// Object to hold
    /// </summary>
    public Building obj;

    /// <summary>
    /// Call to spawn object via Object Spawner
    /// </summary>
    public void SpawnObject() 
    {
        ObjectSpawner.Instance.SetObject(obj);
    }
}
