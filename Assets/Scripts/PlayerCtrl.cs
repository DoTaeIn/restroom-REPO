using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RoomEvent : UnityEvent<Room>
{
    
}

[System.Serializable]
public class ItemEvent : UnityEvent<Item>
{
    
}

public class PlayerCtrl : MonoBehaviour
{
    //TODO: LivingRoom Confiner2D
    [Header("Player Settings")] 
    private float _xInput;
    private float _yInput;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float slowSpeed = 2f;
    bool isHoldingFurniture = false;
    [SerializeField] bool isNearFurniture = false;
    Furniture furniture;
    public float stamina = 100f;
    public bool caught = false;
    public Room currentRoom;
    public EnemyCtrl Enemy;
    private UIManager uimanager;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    private Item temp;
    [SerializeField] private Transform ItemHoldPoint;
    private InventoryManager inventory;
    public Dictionary<UnityEngine.Object, float> damageSources = new Dictionary<UnityEngine.Object, float>();
    
    GameObject _holdingItem;
    GameObject _holdingFurniture;
    [SerializeField] Transform holdingPoint;
    
    float holdTime = 0f;
    [SerializeField] float requiredHoldDuration = 3.0f;
    [SerializeField] float throwForce = 100f;

    public RoomEvent onMoveToOtherRoom = new RoomEvent();

    
    public ItemEvent onGotItem = new ItemEvent();
    public ItemEvent onUseItem = new ItemEvent();

    private void Awake()
    {
        this.transform.position = new Vector3(50, -10, 0);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        uimanager = FindFirstObjectByType<UIManager>();
        inventory = FindFirstObjectByType<InventoryManager>();
    }


    private void Update()
    {
        Mathf.Clamp(stamina, 0f, 100f);
        if (!caught)
        {
            Move();
            PickupFurniture();
            InvestigateFurniture();
        }

        if (caught)
        {
            rb.linearVelocity = Vector2.zero;
            Shake();
            
        }
        

        if (_holdingFurniture != null && isHoldingFurniture)
            _holdingFurniture.transform.localPosition = new Vector3(0, 0, 1);
        
        Mathf.Clamp(holdTime, 0f, requiredHoldDuration);
        
        for (int i = 1; i <= 5; i++)
        {
            // 숫자키 상단 알파벳 키 (Alpha1, Alpha2 …) 를 체크
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                Debug.Log(i);
                useItem(i);
            }
            // (옵션) 숫자패드로도 받고 싶으면
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                useItem(i);
            }
        }
    }

    private float movementX;
    private Vector3 _movement;
    private Vector3 movement;
    private Vector3 m_Velocity = Vector3.zero;
    void Move()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        if (_xInput > 0)
            sr.flipX = false;
        else if (_xInput < 0)
            sr.flipX = true;

        if (Mathf.Abs(_xInput) > 0.01f || Mathf.Abs(_yInput) > 0.01f)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);
        
        
        Vector3 moveDirection = new Vector3(_xInput, _yInput, 0).normalized;

        
        rb.linearVelocity = moveDirection * (isHoldingFurniture ? slowSpeed : moveSpeed);

        rb.linearVelocity = moveDirection * moveSpeed;
        //Vector3 targetVelocity = new Vector2(0.8f * 10f * _xInput, 0.8f * 10f * _yInput);
       // rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref m_Velocity, 0.05f);

    }
    
    void InvestigateFurniture()
    {
        if (isNearFurniture)
        {
            if(Input.GetKey(KeyCode.E))
            {
                Debug.Log("Loading Furniture!");
                holdTime += Time.deltaTime;
            }

            if (holdTime >= 2f)
            {
                if (furniture != null && !furniture.isAquired)
                {
                    Debug.Log("Got Furniture!");
                    Item item = furniture.GetItem();
                    onGotItem.Invoke(item);
                    holdTime = 0f;
                }
            }
        }
    }

    void PickupFurniture()
    {
        if (_holdingFurniture != null && !isHoldingFurniture)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("isRasing");
                anim.SetBool("isHolding", true);
                anim.SetBool("isThrowing", true);
                isHoldingFurniture = true;
                _holdingFurniture.transform.SetParent(holdingPoint);
                _holdingFurniture.transform.localPosition = Vector3.zero;
                _holdingFurniture.transform.localRotation = Quaternion.identity;
                _holdingFurniture.GetComponent<BoxCollider2D>().enabled = false;
                
            }
        }
        else if (isHoldingFurniture)
        {
            if (Input.GetKey(KeyCode.F))
            {
                holdTime += Time.deltaTime;
                Debug.Log("Holding Furniture");
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                Debug.Log("Throwing Furniture ; hold time: " + holdTime);
                if (holdTime >= 0.5f)
                {
                    anim.SetBool("isHolding", false);
                    anim.SetBool("isThrowing", true);
                    ThrowFurniture(holdTime);
                    anim.SetBool("isThrowing", false);

                }
                // Drop if released before throw duration
                //DropFurniture();
            }

            if (Input.GetKey(KeyCode.Q))
            {
                _holdingFurniture.transform.SetParent(null);
                anim.SetBool("isHolding", false);
                anim.SetBool("isThrowing", false);
            }
        }
    }
    
    void ThrowFurniture(float force)
    {
        isHoldingFurniture = false;

        _holdingFurniture.transform.SetParent(null);

        var col = _holdingFurniture.GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = true;

        var rb = _holdingFurniture.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            _holdingFurniture.GetComponent<Furniture>().isThrown = true;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mouseWorld - _holdingFurniture.transform.position).normalized;
            Vector2 forceVector = dir * force * holdTime;
            rb.linearVelocity = forceVector;
            //rb.linearVelocity = dir * throwForce * (force / requiredHoldDuration) * 10f;
        }

        _holdingFurniture = null;
    }

    void Shake()
    {
        if (caught)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(Enemy != null)
                    Enemy.DecreaseGrab(10);
                else
                    Debug.Log("Enemy is null, cannot shake off!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grabable"))
        {
            
            if (collision.GetComponent<OutlineCtrl>() != null)
            {
                _holdingFurniture = collision.gameObject;
                _holdingFurniture.GetComponent<OutlineCtrl>().UpdateOutline(true);
            }
        }
        
        if (collision.GetComponent<Furniture>() != null)
        {
            isNearFurniture = true;
            furniture = collision.GetComponent<Furniture>();
        }

        if (collision.CompareTag("Toilet"))
        {
            Debug.Log("touch");
            uimanager.LockShow();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grabable") && collision.gameObject == _holdingFurniture && !isHoldingFurniture)
        {
            _holdingFurniture.GetComponent<OutlineCtrl>().UpdateOutline(false);
            _holdingFurniture = null;
        }

        if (collision.GetComponent<Furniture>() != null)
        {
            isNearFurniture = false;
            furniture = null;
        }
        if (collision.CompareTag("Toilet"))
        {
            Debug.Log("sdasdfw");
            uimanager.LockHide();
        }
    }
    
    public void TakeDamage(float damage, UnityEngine.Object hitter)
    {
        stamina += damage;
        damageSources.Add(hitter, damage);
        if (stamina <= 0)
        {
            Debug.Log("Player is dead!");
            // Handle player death (e.g., reload scene, show game over screen, etc.)
        }
    }


    void useItem(int num)
    {
        Item item = inventory.GetItem(num-1);
        if (item != null)
        {
            if (item.type == ItemType.Weapon)
            {
                GameObject gm = Instantiate(item.gameObject);
                gm.transform.SetParent(holdingPoint);
                
                gm.transform.localPosition = Vector3.zero;
                
                StartCoroutine(Swing(gm.transform, 90f, 0.2f));
            }

            if (item.type == ItemType.Heal)
            {
                Debug.Log("Player is healing");
                onUseItem.Invoke(item);
            }
        }
        
    }
    
    IEnumerator Swing(Transform weapon, float swingAngle = 90f, float duration = 0.1f)
    {
        float half = duration * 0.5f;
        // swing out
        for (float t = 0; t < half; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0, swingAngle, t / half);
            weapon.localRotation = Quaternion.Euler(0,0,a);
            yield return null;
        }
        weapon.localRotation = Quaternion.identity;
        
        yield return new WaitForSecondsRealtime(duration);
        Destroy(weapon.gameObject);
    }
}
