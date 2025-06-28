using System;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Player Settings")] 
    private float _xInput;
    private float _yInput;

    [SerializeField] float moveSpeed = 5f;

    private Rigidbody2D rb;
    SpriteRenderer sr;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        Move();
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
        
        Vector3 moveDirection = new Vector3(_xInput, _yInput, 0).normalized;
        rb.linearVelocity = moveDirection * moveSpeed;
        //Vector3 targetVelocity = new Vector2(0.8f * 10f * _xInput, 0.8f * 10f * _yInput);
       // rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref m_Velocity, 0.05f);
    }

    void PickupFurniture()
    {
        //if()
    }
}
