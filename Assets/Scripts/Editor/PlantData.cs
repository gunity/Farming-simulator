using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Data.PlantData))]
    public class PlantData : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ((Data.PlantData) target).CustomInspector();
        }
    }
}