using UnityEditor;
using UnityEngine;
namespace RegionSwitch
{
    [CustomEditor(typeof(RegionDataObject))]
    public class RegionDataObjectDrawer : Editor
    {
        RegionDataObject _instence;

        private void OnEnable()
        {
            _instence = target as RegionDataObject;
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

            serializedObject.ApplyModifiedProperties();
        }

        public void LoadData()
        {
            GameObject root = new GameObject(_instence.name);
            var record = root.AddComponent<RegionTransformRecord>();
            foreach (var floor in _instence.floorDataObjs)
            {
                GameObject floorItem = new GameObject(floor.name);
                IconUtility.SetIcon(floorItem, IconUtility.Icon.DiamondYellow);
                floorItem.transform.SetParent(root.transform);
                floorItem.transform.position = new Vector3(0, floor.flourHeight, 0);

                foreach (var region in floor.regionList)
                {
                    GameObject regionRoot = new GameObject(region.name);
                    IconUtility.SetIcon(regionRoot, IconUtility.LabelIcon.Orange);
                    regionRoot.transform.SetParent(floorItem.transform);
                    regionRoot.transform.localPosition = new Vector3(region.rect.x, 0, region.rect.z);

                    GameObject regionTarget = new GameObject("[" + region.name + "]");
                    IconUtility.SetIcon(regionTarget, IconUtility.LabelIcon.Red);
                    regionTarget.transform.SetParent(regionRoot.transform);
                    regionTarget.transform.localPosition = new Vector3(region.rect.length, 1, region.rect.weight);
                }
            }
            record.regionObj = _instence;
        }
    }
}