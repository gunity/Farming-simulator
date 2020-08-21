using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace Manager
{
    public class WeatherManager : Singleton<WeatherManager>
    {
        [SerializeField] private int weatherChangeTime = 10;
        [SerializeField] private List<WeatherStruct> weathers = new List<WeatherStruct>();

        public WeatherStruct CurrentWeather { get; private set; }
        public WeatherStruct ForecastWeather { get; private set; }

        private void Start()
        {
            ForecastWeather = GetRandomWeather();
            StartCoroutine(nameof(ChangeWeather));
        }

        private WeatherStruct GetRandomWeather()
        {
            WeatherStruct newWeather;
            do
            {
                newWeather = weathers[Random.Range(0, weathers.Count)];
            } while (newWeather.weatherType == CurrentWeather.weatherType);

            return newWeather;
        }

        private IEnumerator ChangeWeather()
        {
            while (true)
            {
                CurrentWeather = ForecastWeather;
                ForecastWeather = GetRandomWeather();
                StartCoroutine(nameof(SetSolidColorAlpha), CurrentWeather);
                yield return new WaitForSeconds(weatherChangeTime);
            }
        }

        private IEnumerator SetSolidColorAlpha(WeatherStruct weatherStruct)
        {
            weatherStruct.weatherColor.SetAlpha(0);
            weatherStruct.weatherColor.enabled = true;

            ParticleSystem particle;
            if ((particle = weatherStruct.weatherParticle) != null) particle.Play();

            var changeSpeed = weatherChangeTime / 2.2f;
            while (weatherStruct.weatherColor.color.a < weatherStruct.weatherColorIntensity)
            {
                weatherStruct.weatherColor.AddAlpha(weatherStruct.weatherColorIntensity * Time.fixedDeltaTime /
                                                    changeSpeed);
                if (weatherStruct.weatherType == EWeatherType.Rain)
                    GroundManager.Instance.Grounds.ForEach(ground => ground.Water(Time.fixedDeltaTime * 7f));
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            while (weatherStruct.weatherColor.color.a > 0)
            {
                weatherStruct.weatherColor.SubAlpha(weatherStruct.weatherColorIntensity * Time.fixedDeltaTime /
                                                    changeSpeed);
                if (weatherStruct.weatherType == EWeatherType.Rain)
                    GroundManager.Instance.Grounds.ForEach(ground => ground.Water(Time.fixedDeltaTime * 7f));
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            if (particle != null) particle.Stop();

            yield return true;
        }
    }
}