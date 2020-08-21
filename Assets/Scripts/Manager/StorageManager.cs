using System.Collections.Generic;
using Data;
using Extension;

namespace Manager
{
    public class StorageManager : Singleton<StorageManager>
    {
        public Dictionary<PlantData, int> Plants { get; } = new Dictionary<PlantData, int>();

        public void AddPlant(PlantData plantData)
        {
            if (Plants.ContainsKey(plantData))
                Plants[plantData]++;
            else
                Plants.Add(plantData, 1);
        }

        private void RemovePlantStack(PlantData plantData)
        {
            Plants.Remove(plantData);
        }

        public bool RemovePlant(PlantData plantData, int amount = 1)
        {
            if (amount > Plants[plantData])
                return false;
            if (amount == Plants[plantData])
            {
                RemovePlantStack(plantData);
                return true;
            }

            Plants[plantData] -= amount;
            return true;
        }
    }
}