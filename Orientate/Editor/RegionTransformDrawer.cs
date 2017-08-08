using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
[CustomEditor(typeof(RegionTransformRecord))]
public class RegionTransformDrawer : Editor
{
    RegionTransformRecord _instence;
    string[] activeFloor
    {
        get
        {
            return _instence.regionObj.floorDataObjs.ConvertAll<string>(x => x.name).ToArray();
        }
    }
    private void OnEnable()
    {
        _instence = target as RegionTransformRecord;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        using (var hor = new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("加载信息"))
            {
                LoadData();
            }
        }
        if (_instence.regionObj != null)
        {
            using (var grid = new EditorGUILayout.VerticalScope())
            {
                for (int i = 0; i < activeFloor.Length; i++)
                {
                    var state = _instence.activeFloors.Contains(i);
                    EditorGUI.BeginChangeCheck();
                    var select = EditorGUILayout.Toggle(activeFloor[i], state);
                    var change = EditorGUI.EndChangeCheck();
                    if (change)
                    {
                        if (select)
                        {
                            _instence.activeFloors.Add(i);
                        }
                        else
                        {
                            _instence.activeFloors.Remove(i);
                        }
                    }
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

    public void LoadData()
    {
        if (_instence.regionObj == null)
        {
            return;
        }
        GameObject root = new GameObject(_instence.regionObj.name);
        foreach (var floor in _instence.regionObj.floorDataObjs)
        {
            GameObject floorItem = new GameObject(floor.name);
            IconManager.SetIcon(floorItem, IconManager.Icon.DiamondYellow);
            floorItem.transform.SetParent(root.transform);
            floorItem.transform.position = new Vector3(0, floor.flourHeight, 0);

            foreach (var region in floor.regionList)
            {
                GameObject regionRoot = new GameObject(region.name);
                IconManager.SetIcon(regionRoot, IconManager.LabelIcon.Orange);
                regionRoot.transform.SetParent(floorItem.transform);
                regionRoot.transform.localPosition = new Vector3(region.rect.x, 0, region.rect.z);

                GameObject regionTarget = new GameObject("[" +region.name+"]");
                IconManager.SetIcon(regionTarget, IconManager.LabelIcon.Red);
                regionTarget.transform.SetParent(regionRoot.transform);
                regionTarget.transform.localPosition = new Vector3(region.rect.length, 1, region.rect.weight);
            }
        }
    }
}
