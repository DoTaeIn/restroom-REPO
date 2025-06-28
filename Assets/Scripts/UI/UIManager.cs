using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public float maxStamina = 100f;
    public float currentStamina;

    private StaminaBar staminaBar;

    [SerializeField] private GameObject inventoryPanel; // 인벤토리 UI 오브젝트
    private bool isInventoryOpen = false;

    public InventoryManager inventoryManager;

    void Awake()
    {
        currentStamina = maxStamina;
        staminaBar = FindFirstObjectByType<StaminaBar>();

    }

    void Start()
    {
        inventoryPanel.SetActive(isInventoryOpen);


    }

    public void staminaplus(bool isPlus)
    {
        currentStamina += 20f * Time.deltaTime;
    }

    public void staminaminus(bool isMinus)
    {
        currentStamina -= 10f * Time.deltaTime;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();

        }

        staminaminus(true);

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaBar.SetStamina(currentStamina, maxStamina);




    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

}
