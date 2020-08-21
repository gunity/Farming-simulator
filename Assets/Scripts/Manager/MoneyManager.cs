using System.Collections.Generic;
using Extension;

namespace Manager
{
    public class MoneyManager : Singleton<MoneyManager>
    {
        private int _moneyAmount = 10;
        public List<string> Transactions { get; } = new List<string>();
        public string MoneyAmountAsString => $"{_moneyAmount} C";

        public void AddMoney(int value, string action)
        {
            _moneyAmount += value;
            Transactions.Add($"+{value} C Sell {action}");
            NotificationManager.Instance.SendNotification($"+{value} C");
        }

        public bool SubMoney(int value, string action)
        {
            if (value > _moneyAmount)
            {
                NotificationManager.Instance.SendNotification("not enough money");
                return false;
            }

            _moneyAmount -= value;
            Transactions.Add($"-{value} C Buy {action}");
            NotificationManager.Instance.SendNotification($"-{value} C");
            return true;
        }
    }
}