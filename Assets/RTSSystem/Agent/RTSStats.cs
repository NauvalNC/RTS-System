/*
 * Author: Nauval Muhammad Firdaus
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RTSSystem
{ 
    /// <summary>
    /// RST Agent Stats, default including HP, and DMG Multiplier
    /// </summary>
    [Serializable]
    public class RTSStats
    {
        public float HP = 100;
        public float DMGMultiplier = 1;
    }
}