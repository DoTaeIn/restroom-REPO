using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;

    public float maxStamina = 100f;

    [SerializeField] private Slider staminaBar;

    [SerializeField] private GameObject inventoryPanel; // 인벤토리 UI 오브젝트
    private bool isInventoryOpen = false;

    public InventoryManager inventoryManager;

    void Awake()
    {
        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    void Start()
    {
        inventoryPanel.SetActive(isInventoryOpen);
    }

    void OnEnable()
    {
        playerCtrl.onGotItem.AddListener(UpdateInventoryUI);
    }

    void UpdateInventoryUI(Item item)
    {
        inventoryManager.AddItem(item);
    }
    
    public void staminaplus(bool isPlus)
    {
        playerCtrl.stamina += 20f * Time.deltaTime;
    }

    public void staminaminus(bool isMinus)
    {
        playerCtrl.stamina -= 10f * Time.deltaTime;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();

        }

        staminaminus(true);
        
        staminaBar.value = playerCtrl.stamina / maxStamina;

    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

}
