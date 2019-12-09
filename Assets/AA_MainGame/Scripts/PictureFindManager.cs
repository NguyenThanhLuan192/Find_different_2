using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IceFoxStudio
{
    public class PictureFindManager : MonoBehaviour
    {
        public PointDifferent[] points;

        private void Awake()
        {
            if (points.Length == 0)
            {
                points = transform.GetComponentsInChildren<PointDifferent>();
            } 
        }

        public void SetPoint(bool onActive, Action<string,Vector3> cb)
        {
            foreach (var p in points)
            {
                p.ShowItem(onActive);
                p.cb = cb;
            }
        }
        
        public bool hasGetClue
        {
            set { PlayerPrefs.SetInt("GetClue_" + name.Remove(name.IndexOf("(")), value ? 1 : 0); }
        }
    }
}

