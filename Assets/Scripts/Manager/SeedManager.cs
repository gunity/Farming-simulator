using System.Collections.Generic;
using Data;
using Extension;
using UnityEngine;

namespace Manager
{
    public class SeedManager : Singleton<SeedManager>
    {
        [SerializeField] private List<PlantData> plantsData = new List<PlantData>();

        public List<PlantData> PlantsData => plantsData;
    }
}