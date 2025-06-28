using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform[] slots; // 슬롯 5개
    [SerializeField] private GameObject itemUIPrefab; // 프리팹

    private int currentIndex = 0;

    public void AddItem(Itemicon item)
    {
        if (currentIndex >= slots.Length)
        {
            Debug.Log("인벤토리 가득 참");
            return;
        }

        // 프리팹 인스턴스 생성 → 슬롯에 붙임
        GameObject newItemUI = Instantiate(itemUIPrefab, slots[currentIndex]);
        newItemUI.GetComponent<ItemUI>().Init(item);

        currentIndex++;
    }
}