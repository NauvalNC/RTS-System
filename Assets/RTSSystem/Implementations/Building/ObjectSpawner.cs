/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawn object based on where mouse input drop
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    /// <summary>
    /// Singletone instance
    /// </summary>
    static ObjectSpawner instance;
    public static ObjectSpawner Instance 
    {
        get 
        {
            if (instance == null) instance = FindObjectOfType<ObjectSpawner>();
            return instance;
        }
    }

    Building obj;
    public LayerMask mask;

    private void Update()
    {
        RaySpawn();
    }

    /// <summary>
    /// Shoot ray to detect which point to spawn the object based on mouse position click.
    /// </summary>
    void RaySpawn() 
    {
        if (obj != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                obj.transform.position = hit.point;
                obj.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    obj.Spawn();
                    obj = null;
                }
            }
            else obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set object to spawn
    /// </summary>
    /// <param name="obj">Building to spawn</param>
    public void SetObject(Building obj) 
    {
        this.obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
    }
}
