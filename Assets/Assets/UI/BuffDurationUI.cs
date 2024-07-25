using UnityEngine;
using UnityEngine.UI;

public class BuffDurationUI : MonoBehaviour
{
    [SerializeField] private Image speedBoostBar;
    [SerializeField] private Image shootingBuffBar;

    private float speedBoostDuration;
    private float shootingBuffDuration;

    public void SetSpeedBoostDuration(float duration)
    {
        speedBoostDuration = duration;
        speedBoostBar.fillAmount = 0f; 
    }

    public void SetShootingBuffDuration(float duration)
    {
        shootingBuffDuration = duration;
        shootingBuffBar.fillAmount = 0f;
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
}
