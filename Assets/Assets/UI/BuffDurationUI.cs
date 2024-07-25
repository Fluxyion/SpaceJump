using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffDurationUI : MonoBehaviour
{
    [SerializeField] private Image speedBoostBar;
    [SerializeField] private Image shootingBuffBar;

    private float speedBoostDuration;
    private float shootingBuffDuration;
    public float currentSpeedBoostTime;
    public float currentShootingBuffTime;

    private void Start()
    {
        speedBoostBar.fillAmount = 0f;
        shootingBuffBar.fillAmount = 0f;
    }

    public void SpeedBoostCoundown(float duration)
    {
        currentSpeedBoostTime = duration;
    }
    public void ShootingCoundown(float duration)
    {
        currentShootingBuffTime = duration;
    }

    public void SetSpeedBoostDuration(float duration)
    {
        speedBoostDuration = duration;
        
    }

    public void SetShootingBuffDuration(float duration)
    {
        shootingBuffDuration = duration;
    }

    public void UpdateSpeedBoostBar(float timeRemaining)
    {
        if (speedBoostDuration > 0)
        {
            speedBoostBar.fillAmount = timeRemaining / speedBoostDuration;
        }
    }

    public void UpdateShootingBuffBar(float timeRemaining)
    {
        if (shootingBuffDuration > 0)
        {
            shootingBuffBar.fillAmount = timeRemaining / shootingBuffDuration;
        }
    }

    private void Update()
    {
        // Update speed boost bar
        if (currentSpeedBoostTime > 0)
        {
            currentSpeedBoostTime -= Time.deltaTime;
            UpdateSpeedBoostBar(currentSpeedBoostTime);
        }

        // Update shooting buff bar
        if (currentShootingBuffTime > 0)
        {
            currentShootingBuffTime -= Time.deltaTime;
            UpdateShootingBuffBar(currentShootingBuffTime);
        }
    }
}
