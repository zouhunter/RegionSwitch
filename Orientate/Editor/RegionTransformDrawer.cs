using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
namespace RegionSwitch
{
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
            base.OnInspectorGUI();
            if (_instence != null)
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
        }
    }
   
}