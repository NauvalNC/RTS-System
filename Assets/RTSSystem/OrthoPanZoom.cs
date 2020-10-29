/*
 * Author: Nauval Muhammad Firdaus
 * NIM: 2301906331
 * Kelas: LB04 (Kelas Kecil) / MA04 (Kelas Besar)
 * Matkul: Game Programming (GAME6069)
 */
using UnityEngine;
using System.Collections;

namespace NauvalCodes
{
    /// <summary>
    /// Orthographic Camera Panning dan Zooming
    /// </summary>
    public class OrthoPanZoom : MonoBehaviour
    {
        /// <summary>
        /// Posisi awal dari touch
        /// </summary>
        Vector3 startTouch;

        /// <summary>
        /// Bata maximal zoom in
        /// </summary>
        public float zoomMin = 1;

        /// <summary>
        /// Batas maximal zoom out
        /// </summary>
        public float zoomMax = 0;

        /// <summary>
        /// Kecepatan zoom
        /// </summary>
        public float zoomSpeed = 1.2f;

        /// <summary>
        /// Batas kecepatan pinch saat zoom
        /// </summary>
        public float pinchThreshold = 0.01f;

        private void Update()
        {
            ListenInput();
        }

        /// <summary>
        /// Dengarkan input dari player/user
        /// </summary>
        void ListenInput() 
        {
            // Jika player men-touch layar
            if (Input.GetMouseButtonDown(0)) 
            {
                // Set posisi awal touch pada posisi jari
                startTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            // Jika jumlah touch pada layar adalah dua (dua jari)
            if (Input.touchCount == 2) 
            {
                // Lakukan listen ke pinch untuk zooming
                PinchToZoom();
            } 
            
            // Lakukan panning berdasarkan posisi terkini dari touch
            else if (Input.GetMouseButton(0))
            {
                // Lakukan panning
                Panning();
            }
        }

        /// <summary>
        /// Listen input pinch untuk melakukan zoom
        /// </summary>
        void PinchToZoom() 
        {
            // Dapatkan posisi kedua touch tersebut
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            // Dapatkan posisi awal dari kedua touch
            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            // Kalkulasi jarak antara posisi awal dari touch pertama dan kedua
            float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;

            // Kalkulasi jarak antara posisi terkini dari touch pertama dan kedua
            float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            // Ambil selisih dari jarak posisi awal dengan posisi terkini
            // Jika positif maka zoom in, jika negatif maka zoom out
            float diff = currentMagnitude - prevMagnitude;

            // Masukkan sebagai parameter seberapa banyak scale zoom berdasarkan pinch input
            Zoom(diff * pinchThreshold);
        }

        /// <summary>
        /// Fungsi untuk zoom kamera
        /// </summary>
        /// <param name="scale">Berapa besar zoom yang harus dilakukan</param>
        void Zoom(float scale)
        {
            // Zoom kamera berdasarkan scale dengan batas zoom min dan zoom max
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (scale * zoomSpeed), zoomMin, zoomMax);
        }

        /// <summary>
        /// Fungsi untuk panning camera
        /// </summary>
        void Panning()
        {
            // Dapatkan arah dan jarak yang diperlukan untuk mengatur posisi camera dengan menggunakan selisih dari poisis touch pertama dan touch terkini
            Vector3 dir = startTouch - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Gerakkan kamera berdasarkan nilai panning
            Camera.main.transform.position += dir;
        }
    }
}