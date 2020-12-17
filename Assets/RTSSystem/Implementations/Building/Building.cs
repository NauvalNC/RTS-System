/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Define a bulding that can be build using RTSSystem
/// </summary>
public class Building : MonoBehaviour
{
    float progress = 0;
    public float woodNeeded = 100f;

    public GameObject HUD;
    public GameObject progressHUD;
    public GameObject buildBtn;

    public RectTransform progressBar;

    MeshRenderer mRenderer;
    Material nativeMat;
    public Material progressMat;

    bool completed = false;

    private void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();

        if (mRenderer != null) nativeMat = mRenderer.material;
        else
        {
            nativeMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        }
    }

    void Update()
    {
        if (completed) return;

        if (progress >= woodNeeded) 
        {
            Finished();
        } else 
        {
            HUD.transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
            progressBar.localScale = new Vector2((progress / woodNeeded), progressBar.localScale.y);
        }
    }

    /// <summary>
    /// Called when bulding progress is finished
    /// </summary>
    public void Finished() 
    {
        // Set material back to native material
        if (mRenderer != null) mRenderer.material = nativeMat;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().material = nativeMat;
            }
        }

        completed = true;
        HUD.SetActive(false);
        TaskManager.Instance.tasks.Remove(this);
    }

    /// <summary>
    /// Called when bulding is spawned to the game environment
    /// </summary>
    public void Spawn() 
    {
        progress = 0;
        
        // Set material to temporary building material
        if (mRenderer != null) mRenderer.material = progressMat;
        for (int i = 0; i < transform.childCount; i++) 
        {
            if (transform.GetChild(i).GetComponent<MeshRenderer>() != null) 
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().material = progressMat;
            }
        }

        HUD.SetActive(true);
    }

    public void Build() 
    {
        progressHUD.SetActive(true);
        buildBtn.SetActive(false);
        TaskManager.Instance.tasks.Add(this);
    }

    public void MakeProgress(float amount) 
    {
        progress += amount;
        if (progress > woodNeeded) progress = woodNeeded;
    }

    public bool IsCompleted() 
    {
        return completed;
    }
}
