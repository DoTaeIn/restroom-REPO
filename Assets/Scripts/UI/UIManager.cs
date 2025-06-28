using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;

    public float maxStamina = 100f;

    [SerializeField] private Slider staminaBar;

    [SerializeField] private GameObject inventoryPanel; // 인벤토리 UI 오브젝트
    [SerializeField] private GameObject lockPanel; // 인벤토리 UI 오브젝트

    private bool isInventoryOpen = false;
    private bool isLockOpen = false;

    public InventoryManager inventoryManager;
    public LockSystem locksystem;

    void Awake()
    {
        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
        inventoryManager = GetComponent<InventoryManager>();
        locksystem = GetComponent<LockSystem>();
    }

    void Start()
    {
        inventoryPanel.SetActive(isInventoryOpen);
        lockPanel.SetActive(isLockOpen);

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
    public void LockShow()
    {
        lockPanel.SetActive(true);
    }
    public void LockHide()
    {
        lockPanel.SetActive(false);
    }

}
