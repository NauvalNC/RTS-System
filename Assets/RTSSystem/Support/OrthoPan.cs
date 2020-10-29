/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSSystem 
{
    /// <summary>
    /// Orthographic camera panning and zooming
    /// </summary>
    public class OrthoPan : MonoBehaviour
    {
        Vector3 panStart;
        public float zoomMin, zoomMax;
        public float zoomSpeed = 1.5f;

        void Update() 
        {
            Panning();

            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }

        /// <summary>
        /// Camera panning
        /// </summary>
        void Panning() 
        {
            if (Input.GetMouseButtonDown(2))
            {
                panStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(2))
            {
                Vector3 dir = panStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position += dir;
            }
        }
        
        /// <summary>
        /// Camera zooming, based on scale (mouse scroll)
        /// </summary>
        /// <param name="scale">Zoom input scale (mouse scroll)</param>
        void Zoom(float scale) 
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (scale * zoomSpeed), zoomMin, zoomMax);
        }
    }
}