using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Extension
{
    [Serializable]
    public struct TransformationSettingsStruct
    {
        public ETransformationType transformationType;
        public bool use;
        public PlantData plantData;
        public int transformationTime;

        public TransformationSettingsStruct(ETransformationType transformationType)
        {
            this.transformationType = transformationType;
            use = false;
            plantData = null;
            transformationTime = 10;
        }
    }

    [Serializable]
    public struct TransformationStruct
    {
        public ETransformationType transformationType;
        public Image plusImage;
        public Image inputImage;
        public Image outputImage;
        public Text nameText;
        public Animation arrowAnimation;
        [HideInInspector] public PlantData plantData;
    }

    [Serializable]
    public struct WeatherStruct
    {
        [Header("General settings")] public string weatherName;

        public EWeatherType weatherType;

        [Range(0.1f, 1f)] public float synthesisIntensity;

        public ParticleSystem weatherParticle;

        [Header("Solid color settings")] public Image weatherColor;

        [Range(0f, 1f)] public float weatherColorIntensity;
    }
}