using Extension;
using Manager;
using UnityEngine;

namespace UI
{
    public class ToolButton : MonoBehaviour
    {
        public EToolType toolType;

        public void Click()
        {
            ToolManager.Instance.SetToolType(toolType);
        }
    }
}