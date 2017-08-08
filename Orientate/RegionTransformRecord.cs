using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
namespace RegionSwitch
{
    public class RegionTransformRecord : MonoBehaviour
    {
        public RegionDataObject regionObj;
        private Dictionary<FloorData.RectRange, Color> colorDic = new Dictionary<FloorData.RectRange, Color>();
        private Dictionary<FloorData.RectRange, float> height = new Dictionary<FloorData.RectRange, float>();
        [HideInInspector]
        public List<int> activeFloors;
        void OnDrawGizmos()
        {
            if (regionObj != null)
            {
                AutoRecord();
                ReLoadData();
                if (colorDic != null)
                {
                    foreach (var item in colorDic)
                    {
                        Vector3 pos = new Vector3(item.Key.length + 2 * item.Key.x, height[item.Key] * 2, item.Key.weight + 2 * item.Key.z) * 0.5f;
                        Vector3 size = new Vector3(item.Key.length, 1, item.Key.weight);
                        Gizmos.color = item.Value;
                        Gizmos.DrawCube(pos + transform.position, size);
                    }

                }
            }
        }
        private void AutoRecord()
        {
            if (regionObj == null)
            {
                RegionDataObject reobj = ScriptableObject.CreateInstance<RegionDataObject>();
                ProjectWindowUtil.CreateAsset(reobj, "regionObj.asset");
                regionObj = reobj;
            }
            regionObj.floorDataObjs.Clear();
            foreach (Transform regionTransform in transform)
            {
                FloorData flourData = new FloorData();
                flourData.name = regionTransform.name;
                flourData.flour = regionTransform.GetSiblingIndex();
                flourData.flourHeight = regionTransform.localPosition.y;
                flourData.regionList.AddRange(LoadRegion(regionTransform));
                regionObj.floorDataObjs.Add(flourData);
            }
        }
        private List<FloorData.Region> LoadRegion(Transform regionTransform)
        {
            List<FloorData.Region> regionList = new List<FloorData.Region>();
            FloorData.RectRange range;
            FloorData.Region region;
            foreach (Transform item in regionTransform)
            {
                range = new FloorData.RectRange();
                range.x = item.transform.localPosition.x;
                range.z = item.transform.localPosition.z;
                range.length = item.transform.GetChild(0).localPosition.x;
                range.weight = item.transform.GetChild(0).localPosition.z;
                region = new FloorData.Region(item.name, range);
                regionList.Add(region);
            }
            return regionList;
        }
        private void ReLoadData()
        {
            colorDic.Clear();
            height.Clear();
            activeFloors.Sort();
            foreach (var activeFloor in activeFloors)
            {
                if (activeFloor < regionObj.floorDataObjs.Count)
                {
                    for (int j = 0; j < regionObj.floorDataObjs[activeFloor].regionList.Count; j++)
                    {
                        colorDic.Add(regionObj.floorDataObjs[activeFloor].regionList[j].rect, CalcColor(activeFloor, regionObj.floorDataObjs.Count, j, regionObj.floorDataObjs[activeFloor].regionList.Count));
                        height.Add(regionObj.floorDataObjs[activeFloor].regionList[j].rect, regionObj.floorDataObjs[activeFloor].flourHeight);
                    }
                }
            }

        }
        private Color CalcColor(int floor, int totalFloor, int region, int totoalRegion)
        {
            float r = floor / (totalFloor + 0f);
            float g = region / (totoalRegion + 0f);
            float b = 1;
            return new Color(r, g, b, 1f);
        }
    }
#endif
}