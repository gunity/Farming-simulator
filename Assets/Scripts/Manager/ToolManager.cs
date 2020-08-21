using Data;
using Extension;

namespace Manager
{
    public class ToolManager : Singleton<ToolManager>
    {
        public EToolType ToolType { get; private set; }
        public PlantData PlantData { get; private set; }

        public void Awake()
        {
            ToolType = EToolType.None;
        }

        public void SetToolType(EToolType toolType)
        {
            ToolType = toolType;
        }

        public void SetPlantType(PlantData plantDataData)
        {
            PlantData = plantDataData;
        }
    }
}