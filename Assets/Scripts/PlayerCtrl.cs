using System;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Player Settings")] 
    private float _xInput;
    private float _yInput;
    [SerializeField] float moveSpeed = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Move();
    }

    void Move()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");
        
        Vector3 moveDirection = new Vector3(_xInput, _yInput, 0).normalized;
        
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
