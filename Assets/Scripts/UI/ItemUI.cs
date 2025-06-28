using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void Init(Itemicon item)
    {
        iconImage.sprite = item.icon;
    }
}