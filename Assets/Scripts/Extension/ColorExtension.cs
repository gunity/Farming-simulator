using UnityEngine;
using UnityEngine.UI;

namespace Extension
{
    public static class ColorExtension
    {
        public static void SetAlpha(this Image image, float alpha)
        {
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, alpha);
        }

        public static void AddAlpha(this Image image, float alpha)
        {
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, color.a + alpha);
        }

        public static void SubAlpha(this Image image, float alpha)
        {
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, color.a - alpha);
        }
    }
}