using UnityEngine;
using UnityEngine.UI;


public class TestItemGiver : MonoBehaviour
{
    public Sprite testIcon1;
    public Sprite testIcon2;
    private InventoryManager inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Itemicon item = new Itemicon("포션", testIcon1);
            inventory.AddItem(item);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Itemicon item = new Itemicon("열쇠", testIcon2);
            inventory.AddItem(item);
        }
    }
}