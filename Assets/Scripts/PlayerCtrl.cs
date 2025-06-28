using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RoomEvent : UnityEvent<Room>
{
    
}

public class ItemEvent : UnityEvent<Item>
{
    
}

public class PlayerCtrl : MonoBehaviour
{
    [Header("Player Settings")] 
    private float _xInput;
    private float _yInput;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float slowSpeed = 2f;
    bool isHoldingFurniture = false;
    bool isNearFurniture = false;
    Furniture furniture;
    public float stamina = 100f;
    public bool caught = false;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    
    GameObject _holdingItem;
    GameObject _holdingFurniture;
    [SerializeField] Transform holdingPoint;
    
    float holdTime = 0f;
    [SerializeField] float requiredHoldDuration = 3.0f;
    [SerializeField] float throwForce = 100f;

    public RoomEvent onMoveToOtherRoom;
    public ItemEvent onGotItem;

    private void Awake()
    {
        this.transform.position = new Vector3(50, -10, 0);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (!caught)
        {
            Move();
            PickupFurniture();
            InvestigateFurniture();
        }
        

        if (_holdingFurniture != null && isHoldingFurniture)
            _holdingFurniture.transform.localPosition = new Vector3(0, 0, 1);
        
        Mathf.Clamp(holdTime, 0f, requiredHoldDuration);
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
            if(Input.GetKeyDown(KeyCode.E))
            {
                holdTime += Time.deltaTime;
            }

            if (holdTime >= 2f)
            {
                if (furniture != null)
                {
                    Item item = furniture.GetItem();
                    onGotItem.Invoke(item);
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
                    ThrowFurniture(holdTime);
                }
                // Drop if released before throw duration
                //DropFurniture();
            }

            if (Input.GetKey(KeyCode.Q))
            {
                _holdingFurniture.transform.SetParent(null);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grabable"))
        {
            if(collision.GetComponent<OutlineCtrl>() != null)
            {
                _holdingFurniture = collision.gameObject;
                _holdingFurniture.GetComponent<OutlineCtrl>().UpdateOutline(true);
            }
            else if (collision.GetComponent<Furniture>() != null)
            {
                isNearFurniture = true;
                furniture = collision.GetComponent<Furniture>();
            }
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
    }
    
    public void TakeDamage(float damage)
    {
        stamina -= damage;
        if (stamina <= 0)
        {
            Debug.Log("Player is dead!");
            // Handle player death (e.g., reload scene, show game over screen, etc.)
        }
    }
    
    
}
