using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Extension;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlantData", menuName = "Data/Plant Data")]
    public class PlantData : ScriptableObject
    {
        [SerializeField] private string plantName;
        [SerializeField] private Sprite plantIcon;
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private int growTime;
        [SerializeField] private int purchasePrice;
        [SerializeField] private int sellingPrice;
        [SerializeField] private List<TransformationSettingsStruct> settings = new List<TransformationSettingsStruct>();
        [SerializeField] private bool canBePlanted;

        public string PlantName => plantName;
        public Sprite PlantIcon => plantIcon;
        public List<Sprite> Sprites => sprites;
        public int GrowTime => growTime;
        public int PurchasePrice => purchasePrice;
        public int SellingPrice => sellingPrice;
        public string SellingPriceAsString => $"{sellingPrice} C";
        public List<TransformationSettingsStruct> Settings => settings;

        public PlantData GetTransformationPlantData(ETransformationType transformationType)
        {
            return settings.Where(setting => setting.transformationType == transformationType)
                .Select(setting => setting.plantData).FirstOrDefault();
        }

        public bool CheckCanBeTransformation(ETransformationType transformationType)
        {
            return settings.Any(setting => setting.transformationType == transformationType && setting.use);
        }

        public int GetTransformationTime(ETransformationType transformationType)
        {
            return settings.FirstOrDefault(setting => setting.transformationType == transformationType)
                .transformationTime;
        }

#if UNITY_EDITOR
        private bool _showSprites;

        [SuppressMessage("ReSharper", "AssignmentInConditionalExpression")]
        public void CustomInspector()
        {
            plantName = EditorGUILayout.TextField("Plant name", plantName);
            sellingPrice = EditorGUILayout.IntField("Selling price", sellingPrice);
            plantIcon = (Sprite) EditorGUILayout.ObjectField("Plant icon", plantIcon, typeof(Sprite), false);
            EditorGUILayout.BeginVertical("Box");
            if (canBePlanted = EditorGUILayout.Toggle("Can be planted", canBePlanted))
            {
                if (_showSprites = EditorGUILayout.Foldout(_showSprites, "Sprites"))
                {
                    for (var index = 0; index < sprites.Count; index++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        sprites[index] = (Sprite) EditorGUILayout.ObjectField(sprites[index], typeof(Sprite), false);
                        if (GUILayout.Button("Delete"))
                        {
                            sprites.Remove(sprites[index]);
                            return;
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Add new sprite")) sprites.Add(null);
                }

                growTime = EditorGUILayout.IntField("Grow time", growTime);
                purchasePrice = EditorGUILayout.IntField("Purchase price", purchasePrice);
            }

            EditorGUILayout.EndVertical();
            for (var index = 0; index < settings.Count; index++)
            {
                EditorGUILayout.BeginVertical("Box");
                var setting = settings[index];
                if (setting.use = EditorGUILayout.Toggle($"Use in a {setting.transformationType}", setting.use))
                {
                    setting.plantData =
                        (PlantData) EditorGUILayout.ObjectField($"{setting.transformationType} plant data",
                            setting.plantData,
                            typeof(PlantData),
                            false);
                    setting.transformationTime =
                        EditorGUILayout.IntField("Transformation time", setting.transformationTime);
                }

                settings[index] = setting;
                EditorGUILayout.EndVertical();
            }

            EditorUtility.SetDirty(this);
        }

        private void Awake()
        {
            var transformationTypes = Enum.GetValues(typeof(ETransformationType));
            if (settings.Count == transformationTypes.Length) return;

            settings.Clear();
            foreach (var transformationType in transformationTypes)
                settings.Add(new TransformationSettingsStruct((ETransformationType) transformationType));
        }
#endif
    }
}