using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace GGL
{
    public class GizmosGLMenu
    {
        [MenuItem("Assets/Create/GizmosGL")]
        private static void AddGizmosGL()
        {
            GameObject clone = new GameObject("GizmosGL");
            clone.AddComponent<GizmosGL>();
        }
    }
}
