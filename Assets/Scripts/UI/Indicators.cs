using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Indicators : MonoBehaviour
    {
        public Image foodBar;
        public Image waterBar;
        public Image healthBar;

        public float healthAmount = 100;
        public float foodAmount = 100;
        public float waterAmount = 100;

        public float secondsToEmptyFood = 60;
        public float secondsToEmptyWater = 30;
        public float secondsToEmptyHealth = 60;

        public bool isInWater = false;

        public void Start()
        {
            healthBar.fillAmount = healthAmount / 100;
            waterBar.fillAmount = waterAmount / 100;
            foodBar.fillAmount = foodAmount / 100;
        }

        public void Update()
        {
            if (isInWater)
            {
                if (Input.GetKeyDown(KeyCode.E))
                    ChangeWaterAmount(50);
            }
            if (foodAmount > 0)
            {
                foodAmount -= 100 / secondsToEmptyFood * Time.deltaTime;
                foodBar.fillAmount = foodAmount / 100;
            }
            else
                foodAmount = 0;

            if (waterAmount > 0)
            {
                waterAmount -= 100 / secondsToEmptyWater * Time.deltaTime;
                waterBar.fillAmount = waterAmount / 100;
            }
            else waterAmount = 0;

            if (foodAmount <= 0)
            {
                healthAmount -= 100 / secondsToEmptyHealth * Time.deltaTime;
            }
            if (waterAmount <= 0)
            {
                healthAmount -= 100 / secondsToEmptyHealth * Time.deltaTime;
            }

            healthBar.fillAmount = healthAmount / 100;
        }

        public void ChangeFoodAmount(float changeValue)
        {
            if (foodAmount + changeValue > 100)
                foodAmount = 100;
            else
                foodAmount += changeValue;
        }
        
        public void ChangeWaterAmount(float changeValue)
        {
            if (waterAmount + changeValue > 100)
                waterAmount = 100;
            else
                waterAmount += changeValue;
        }
        public void ChangeHitPointsAmount(float changeValue)
        {
            if (healthAmount + changeValue > 100)
                healthAmount = 100;
            else
                healthAmount += changeValue;
        }
    }
}