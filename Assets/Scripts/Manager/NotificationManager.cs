using Extension;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        public Text notificationText;
        private Animation _animation;

        private void Awake()
        {
            _animation = notificationText.GetComponent<Animation>();
        }

        public void SendNotification(string text)
        {
            _animation.Stop();
            notificationText.text = text.ToUpper();
            _animation.Play();
        }
    }
}