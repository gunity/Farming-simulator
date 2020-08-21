using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Classes
{
    [Serializable]
    public class Upgrade
    {
        [SerializeField] private string upgradeName = string.Empty;
        [SerializeField] private Sprite upgradeIcon = null;
        [SerializeField] private List<int> upgradeCost = new List<int>();
        [SerializeField] private UnityEvent clickEvent = new UnityEvent();

        private int _currentUpgradeLevel;

        public Upgrade()
        {
            _currentUpgradeLevel = 0;
        }

        public string UpgradeName => upgradeName;
        public Sprite UpgradeIcon => upgradeIcon;

        public string UpgradeCostAsString => _currentUpgradeLevel < upgradeCost.Count
            ? $"{upgradeCost[_currentUpgradeLevel]} C"
            : "MAX LEVEL";

        public void UpgradeThis()
        {
            if (_currentUpgradeLevel == upgradeCost.Count ||
                !MoneyManager.Instance.SubMoney(upgradeCost[_currentUpgradeLevel], $"upgrade {upgradeName}"))
                return;

            _currentUpgradeLevel++;
            clickEvent.Invoke();
        }
    }
}