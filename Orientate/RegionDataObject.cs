using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace RegionSwitch
{
    [CreateAssetMenu(fileName = "RegionDataObject.asset", menuName = "生成/坐标组")]
    public class RegionDataObject : ScriptableObject
    {
        public List<FloorData> floorDataObjs = new List<FloorData>();
    }

    [System.Serializable]
    public class FloorData
    {
        [Serializable]
        public class RectRange
        {
            public float x;
            public float z;
            public float length;
            public float weight;
            public bool IsPointInSide(Vector2 pos)
            {
                if (pos.x > x && pos.y > z && pos.x - x < length && pos.y - z < weight)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Serializable]
        public class Region
        {
            public string name;
            public RectRange rect;
            public Region(string name, RectRange rect)
            {
                this.name = name;
                this.rect = rect;
            }
        }
        public string name;
        public int flour;
        public float flourHeight;
        public List<Region> regionList = new List<Region>();
    }
}