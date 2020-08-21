using System.Collections.Generic;
using Classes;
using Extension;

namespace Manager
{
    public class UpgradeManager : Singleton<UpgradeManager>
    {
        public List<Upgrade> upgrades = new List<Upgrade>();

        public void UpgradeGround()
        {
            GroundManager.Instance.Expand();
        }
    }
}