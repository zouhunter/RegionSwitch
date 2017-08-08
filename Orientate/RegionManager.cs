using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
public class RegionManager : MonoBehaviour
{
    public static RegionManager Instence;
    public RegionDataObject regionObj;
    public string Region { get; private set; }
    public int Flour { get; private set; }
    public OnRegionChange onRegionChanged;
    [Range(0.1f, 5)]
    public float updateTime;
    private IOrientateCtrl _controller;
    private float timer;

    private void Awake(){
        Instence = this;
    }
    void Start()
    {
        _controller = new OrientateController(regionObj);
    }

    public void SetRegion(Vector3 pos)
    {
        Flour = _controller.GetStare(pos.y);
        var region = _controller.GetRegion(Flour, new Vector2(pos.x, pos.z));
        if (region != Region)
        {
            Region = region;
            if (onRegionChanged != null) onRegionChanged(Region);
            Debug.Log("Flour:" + Flour);
            Debug.Log("Region:" + Region);
        }
    }

    private void Update()
    {
        SetRegion(transform.position);
    }
}
