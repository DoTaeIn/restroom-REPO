using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    [SerializeField] private Image fillImage;

    public void SetStamina(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

}
