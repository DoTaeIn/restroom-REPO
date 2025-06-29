using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;

    public float maxStamina = 100f;

    [SerializeField] private Slider staminaBar;

    [SerializeField] private GameObject inventoryPanel; // 인벤토리 UI 오브젝트
    [SerializeField] private GameObject lockPanel; // 인벤토리 UI 오브젝트
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private GameObject ButtonChooseGroup;
    [SerializeField] private GameObject ButtonCloseGroup;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemNameText;
    [SerializeField] private List<Sprite> endingScreens;
    public Image endingImage;
    GameHandler gameHandler;
    
    private bool isInventoryOpen = false;
    private bool isLockOpen = false;

    public InventoryManager inventoryManager;
    public LockSystem locksystem;
    private Item tmep;
    
    

    void Awake()
    {
        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
        inventoryManager = GetComponent<InventoryManager>();
        locksystem = GetComponent<LockSystem>();
        gameHandler = FindFirstObjectByType<GameHandler>();
    }

    void Start()
    {
        inventoryPanel.SetActive(isInventoryOpen);
        lockPanel.SetActive(isLockOpen);
        gameHandler.endingEvent.AddListener(ShowEnding);
    }

    void OnEnable()
    {
        playerCtrl.onGotItem.AddListener(UpdateInventoryUI);
        playerCtrl.onUseItem.AddListener(UseItemUI);
    }

    void UseItemUI(Item item)
    {
        if (item.type == ItemType.Key)
        {
            tmep = item;
            itemPanel.SetActive(true);
            ButtonChooseGroup.SetActive(false);
            ButtonCloseGroup.SetActive(true);
            itemImage.sprite = tmep.icon;
            itemNameText.text = tmep.keyPos + 1 + "번째 비밀번호는 " + tmep.keyId;
        }
        else
        {
            tmep = item;
            itemPanel.SetActive(true);
            ButtonChooseGroup.SetActive(true);
            ButtonCloseGroup.SetActive(false);
            itemImage.sprite = tmep.icon;
            itemNameText.text = tmep.name + "다! 먹어볼까?";
        }
        
    }

    public void Consume()
    {
        float damage = tmep.Consume();
        playerCtrl.TakeDamage(damage, tmep);
        itemImage.sprite = tmep.icon;
        itemNameText.text = tmep.name + "였다.";
        ButtonChooseGroup.SetActive(false);
        ButtonCloseGroup.SetActive(true);
        tmep = null;
    }

    public void CloseItemPanel()
    {
        itemPanel.SetActive(false);
    }

    public void ThrowAway()
    {
        CloseItemPanel();
        tmep = null;
    }

    void ShowEnding(EndingType endingType)
    {
        endingImage.transform.parent.gameObject.SetActive(true);
        switch (endingType)
        {
            case EndingType.Caught:
                endingImage.sprite = endingScreens[0];
                break;
            case EndingType.Death:
                endingImage.sprite = endingScreens[1];
                break;
            case EndingType.Weapon:
                endingImage.sprite = endingScreens[2];
                break;
            case EndingType.Escape:
                endingImage.sprite = endingScreens[3];
                break;
        }

        StartCoroutine("MoveCoroutine");
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

        //staminaminus(true);

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

    
    private IEnumerator MoveCoroutine()
    {
        Time.timeScale = 0;
        Vector2 endPos = endingImage.GetComponent<RectTransform>().anchoredPosition + Vector2.up * 200f;
        float elapsed = 0f;

        while (elapsed < 10f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 10f);
            endingImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(endingImage.GetComponent<RectTransform>().anchoredPosition, endPos, t);
            yield return null;
        }

        endingImage.GetComponent<RectTransform>().anchoredPosition = endPos;
    }
}
