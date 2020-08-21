using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GroundData", menuName = "Data/Ground Data")]
    public class GroundData : ScriptableObject
    {
        [SerializeField] private Sprite groundSprite = null;
        [SerializeField] private List<Sprite> weedSprites = new List<Sprite>();
        [SerializeField] private float weedGrowthRate = 0.05f;
        [SerializeField] private float soilDryingRate = 0.05f;

        public Sprite GroundSprite => groundSprite;
        public List<Sprite> WeedSprites => weedSprites;
        public float WeedGrowthRate => weedGrowthRate;
        public float SoilDryingRate => soilDryingRate;
    }
}