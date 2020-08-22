using System.Collections.Generic;
using Data;
using Extension;
using Object;
using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Manager
{
    public class PlantDataEvent : UnityEvent<PlantData>
    {
    }

    public class WindowManager : Singleton<WindowManager>
    {
        #region General functions

        public bool IsShownWindow { get; private set; }
        private readonly PlantDataEvent _plantDataEvent = new PlantDataEvent();
        public GameObject rectButtonPrefab;
        
        private void RefreshContent(Component content, UnityAction<PlantData> unityAction, GameObject button)
        {
            content.transform.DestroyAllChildrenObjects();
            foreach (var plant in StorageManager.Instance.Plants)
            {
                var buttonStructure =
                    Instantiate(button, Vector3.zero, Quaternion.identity, content.transform)
                        .GetComponent<ButtonStructure>();
                buttonStructure.imageComponent.sprite = plant.Key.PlantIcon;
                buttonStructure.textComponent.text = plant.Key.PlantName +
                                                     (plant.Value > 1 ? $" x{plant.Value}" : string.Empty) +
                                                     $"\n{plant.Key.SellingPriceAsString}";
                //buttonStructure.GetComponent<Button>().onClick.AddListener(() => StoragePlantClick(plant.Key));
                buttonStructure.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SoundManager.Instance.PlayClick();
                    _plantDataEvent.RemoveAllListeners();
                    _plantDataEvent.AddListener(unityAction);
                    _plantDataEvent.Invoke(plant.Key);
                });
            }
        }

        #endregion

        #region Seeds

        [Header("Seeds window")] public GameObject seedsPanel;

        public RectTransform seedsContent;
        
        public void SetVisibleSeedsWindow(bool show)
        {
            IsShownWindow = show;
            seedsPanel.SetActive(show);

            if (!show) return;

            seedsContent.DestroyAllChildrenObjects();
            SeedManager.Instance.PlantsData.ForEach(plantData =>
            {
                var button = Instantiate(rectButtonPrefab, Vector3.zero, quaternion.identity, seedsContent)
                    .GetComponent<ButtonStructure>();
                button.textComponent.text = $"{plantData.PlantName}\n" +
                                            $"{plantData.PurchasePrice} C";
                button.imageComponent.sprite = plantData.PlantIcon;

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ToolManager.Instance.SetPlantType(plantData);
                    SetVisibleSeedsWindow(false);
                });
            });
        }

        #endregion

        #region Cash

        [Header("Cash window")] public GameObject cashPanel;

        public Text cashValue;
        public GameObject titlePrefab;
        public RectTransform cashContent;
        
        public void SetVisibleCashWindow(bool show)
        {
            SoundManager.Instance.PlayClick();
            IsShownWindow = show;
            cashPanel.SetActive(show);
            if (!show) return;
            cashValue.text = MoneyManager.Instance.MoneyAmountAsString;
            cashContent.DestroyAllChildrenObjects();
            MoneyManager.Instance.Transactions.ForEach(transaction =>
            {
                Instantiate(titlePrefab, Vector3.zero, Quaternion.identity, cashContent).GetComponent<Text>().text =
                    transaction;
            });
        }

        #endregion

        #region Sale

        [Header("Sale window")] public GameObject salePanel;

        public RectTransform saleContent;
        
        public void SetVisibleSaleWindow(bool show)
        {
            SoundManager.Instance.PlayClick();
            IsShownWindow = show;
            salePanel.SetActive(show);

            if (!show) return;

            RefreshContent(saleContent, SalePlant, rectButtonPrefab);
        }

        private void SalePlant(PlantData plantData)
        {
            if (!StorageManager.Instance.RemovePlant(plantData))
                return;
            MoneyManager.Instance.AddMoney(plantData.SellingPrice, plantData.PlantName);
            RefreshContent(saleContent, SalePlant, rectButtonPrefab);
        }

        #endregion

        #region Weather

        [Header("Weather window")] public GameObject weatherPanel;

        public Text weatherText;
        
        public void SetVisibleWeatherWindow(bool show)
        {
            SoundManager.Instance.PlayClick();
            weatherPanel.SetActive(show);

            if (!show) return;

            weatherText.text = $"{WeatherManager.Instance.CurrentWeather.weatherName}\n" +
                               $"{WeatherManager.Instance.ForecastWeather.weatherName}";
        }

        #endregion

        #region Information

        [Header("Information window")] public GameObject informationPanel;

        public Text informationValues;
        
        public void OpenInformationWindow(Ground ground)
        {
            SoundManager.Instance.PlayClick();
            IsShownWindow = true;
            informationValues.text = $"{ground.ConditionType.ToString()} \n" +
                                     $"{ground.PlantData?.PlantName ?? "none"} \n" +
                                     $"{ground.SoilMoisture:00} pc. \n" +
                                     $"{ground.WeedQuantity:00} pc. \n" +
                                     "time days";
            informationPanel.SetActive(true);
        }

        public void CloseInformationWindow()
        {
            SoundManager.Instance.PlayClick();
            IsShownWindow = false;
            informationPanel.SetActive(false);
        }

        #endregion

        #region Storage

        [Header("Storage window")] public GameObject storagePanel;

        public RectTransform storageContent;
        public GameObject storageButtonPrefab;
        public List<TransformationStruct> transformations;
        
        public void SetVisibleStorageWindow(bool show)
        {
            SoundManager.Instance.PlayClick();
            IsShownWindow = show;
            storagePanel.SetActive(show);

            if (!show) return;

            RefreshContent(storageContent, StoragePlantClick, storageButtonPrefab);
        }

        private void StoragePlantClick(PlantData plantData)
        {
            transformations.ForEach(transformation =>
            {
                if (!plantData.CheckCanBeTransformation(transformation.transformationType)) return;
                var button = transformation.plusImage.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    StoragePlusClick(plantData, transformation);
                    SoundManager.Instance.PlayClick();
                });
                transformation.plusImage.enabled = true;
            });
        }

        private void StoragePlusClick(PlantData plantData, TransformationStruct transformationStruct)
        {
            transformations.ForEach(t => t.plusImage.enabled = false);

            transformationStruct.plantData = plantData;
            transformationStruct.inputImage.sprite = plantData.PlantIcon;
            transformationStruct.inputImage.enabled = true;
            transformationStruct.nameText.enabled = false;
            transformationStruct.arrowAnimation.Play();

            StorageManager.Instance.RemovePlant(plantData);
            RefreshContent(storageContent, StoragePlantClick, storageButtonPrefab);
            TransformationManager.Instance.Transformation(transformationStruct);
        }

        public void TransformationComplete(TransformationStruct transformationStruct)
        {
            var juicer =
                transformationStruct.plantData.GetTransformationPlantData(transformationStruct.transformationType);
            transformationStruct.outputImage.sprite = juicer.PlantIcon;
            transformationStruct.outputImage.enabled = true;
            transformationStruct.outputImage.GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayClick();
                StorageManager.Instance.AddPlant(juicer);
                RefreshContent(storageContent, StoragePlantClick, storageButtonPrefab);
                transformationStruct.outputImage.sprite = null;
                transformationStruct.outputImage.enabled = false;
                transformationStruct.outputImage.GetComponent<Button>().onClick.RemoveAllListeners();
            });

            transformationStruct.plantData = null;
            transformationStruct.inputImage.sprite = null;
            transformationStruct.inputImage.enabled = false;
            transformationStruct.nameText.enabled = true;
            transformationStruct.arrowAnimation.Stop();
        }

        #endregion

        #region Upgrade

        [Header("Upgrade window")] public GameObject upgradePanel;

        public RectTransform upgradeContent;
        
        public void SetVisibleUpgradeWindow(bool show)
        {
            SoundManager.Instance.PlayClick();
            IsShownWindow = show;
            upgradePanel.SetActive(show);

            if (!show) return;

            RefreshUpgradeContent();
        }

        private void RefreshUpgradeContent()
        {
            upgradeContent.DestroyAllChildrenObjects();
            UpgradeManager.Instance.upgrades.ForEach(upgrade =>
            {
                var button = Instantiate(rectButtonPrefab, Vector3.zero, Quaternion.identity, upgradeContent)
                    .GetComponent<ButtonStructure>();
                button.imageComponent.sprite = upgrade.UpgradeIcon;
                button.textComponent.text = $"{upgrade.UpgradeName}\n" +
                                            $"{upgrade.UpgradeCostAsString}";
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SoundManager.Instance.PlayClick();
                    upgrade.UpgradeThis();
                    RefreshUpgradeContent();
                });
            });
        }

        #endregion
    }
}