using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
public interface IOrientateCtrl
{
    string GetRegion(Vector3 pos);
    string GetRegion(int stare, Vector2 pos);
    int GetStare(float y);
}
/// <summary>
/// 利用人物当前坐标获取地理信息
/// </summary>
public class OrientateController : IOrientateCtrl
{
    private List<List<FloorData.Region>> regions;
    private FloorData[] positions;
    public OrientateController(RegionDataObject regionObj)
    {
        positions = regionObj.floorDataObjs.ToArray();
        for (int i = 0; i < positions.Length; i++)
        {
            for (int j = 0; j < positions.Length - i - 1; j++)
            {
                if (positions[j].flour > positions[j + 1].flour)
                {
                    FloorData temp = positions[j + 1];
                    positions[j + 1] = positions[j];
                    positions[j] = temp;
                }
            }
        }

        regions = new List<List<global::FloorData.Region>>();
        foreach (FloorData item in positions)
        {
            regions.Add(item.regionList);
        }
    }

    public string GetRegion(Vector3 pos)
    {
        var currStare = GetStare(pos.y);
        return GetRegion(currStare, new Vector2(pos.x, pos.z));
    }
    public string GetRegion(int stare,Vector2 pos)
    {
        if (regions == null || regions.Count <= stare) return null;
        List<FloorData.Region> regionlist = regions[stare];
        FloorData.Region region;
        for (int i = 0; i < regionlist.Count; i++)
        {
            region = regionlist[i];
            if (region.rect.IsPointInSide(pos))
            {
                return region.name;
            }
        }
        return null;

    }
    public int GetStare(float y)
    {
        for (int i = 0; i < positions.Length - 1; i++)
        {
            float nextHight = positions[i + 1].flourHeight;
            if (nextHight > y)
            {
                return i;
            }
        }
        return positions.Length - 1;
    }
}
