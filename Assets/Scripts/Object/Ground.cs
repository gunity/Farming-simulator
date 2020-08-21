using System.Collections;
using Data;
using Extension;
using Manager;
using UnityEngine;

namespace Object
{
    public class Ground : MonoBehaviour
    {
        public SpriteRenderer groundSpriteRenderer;
        public SpriteRenderer weedSpriteRenderer;

        private SpriteRenderer _plantSpriteRenderer;
        private bool _readyToCollect;

        public EConditionType ConditionType
        {
            get
            {
                if (SoilMoisture < 30 || WeedQuantity > 80)
                    return EConditionType.Bad;
                if (SoilMoisture > 80 && WeedQuantity < 30)
                    return EConditionType.Excellent;
                return EConditionType.Good;
            }
        }

        public GroundData GroundData { get; set; }
        public float SoilMoisture { get; private set; }
        public float WeedQuantity { get; private set; }
        public PlantData PlantData { get; private set; }

        private void Update()
        {
            DryingSoil();
            WeedAppearance();
        }

        private void OnMouseDown()
        {
            if (WindowManager.Instance.IsShownWindow)
                return;
            switch (ToolManager.Instance.ToolType)
            {
                case EToolType.Select:
                    Select();
                    break;
                case EToolType.Water:
                    Water();
                    break;
                case EToolType.Weed:
                    Weed();
                    break;
                case EToolType.Seed:
                    Seed();
                    break;
                case EToolType.Collect:
                    Collect();
                    break;
                case EToolType.None:
                default:
                    break;
            }
        }

        private void DryingSoil()
        {
            if (!(SoilMoisture > 0)) return;
            SoilMoisture -= GroundData.SoilDryingRate;
            var channel = 1 - 3 * SoilMoisture / 1000;
            groundSpriteRenderer.color = new Color(channel, channel, channel, 1f);
        }

        private void WeedAppearance()
        {
            if (!(WeedQuantity < 100)) return;
            WeedQuantity += GroundData.WeedGrowthRate;
            weedSpriteRenderer.sprite =
                GroundData.WeedSprites[(int) WeedQuantity * (GroundData.WeedSprites.Count - 1) / 100];
        }

        private IEnumerator ToGrow()
        {
            for (var index = 1; index < PlantData.Sprites.Count; index++)
            {
                float time = 0;
                while (time < PlantData.GrowTime * (int) ConditionType *
                    (int) WeatherManager.Instance.CurrentWeather.weatherType)
                {
                    time += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                _plantSpriteRenderer.sprite = PlantData.Sprites[index];
            }

            _readyToCollect = true;
            yield return true;
        }

        private void Water()
        {
            SoilMoisture = 100;
        }

        public void Water(float value)
        {
            SoilMoisture = SoilMoisture < 100 ? SoilMoisture + value : 100;
        }

        private void Select()
        {
            WindowManager.Instance.OpenInformationWindow(this);
        }

        private void Weed()
        {
            WeedQuantity = 0;
        }

        private void Seed()
        {
            if (PlantData != null) return;

            if (!MoneyManager.Instance.SubMoney((PlantData = ToolManager.Instance.PlantData).PurchasePrice,
                PlantData.PlantName))
            {
                PlantData = null;
                return;
            }

            _readyToCollect = false;
            var plant = new GameObject(PlantData.PlantName);
            _plantSpriteRenderer = plant.AddComponent<SpriteRenderer>();
            _plantSpriteRenderer.sprite = PlantData.Sprites[0];
            _plantSpriteRenderer.sortingOrder = 10;
            plant.transform.SetParent(transform);
            plant.transform.localPosition = Vector3.zero;
            StartCoroutine(nameof(ToGrow));
        }

        private void Collect()
        {
            if (!_readyToCollect) return;
            _readyToCollect = false;
            StorageManager.Instance.AddPlant(PlantData);
            PlantData = null;
            Destroy(_plantSpriteRenderer.gameObject);
        }
    }
}