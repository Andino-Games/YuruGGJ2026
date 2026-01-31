using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    

    [Header("Player Configuration")]
    Rigidbody2D rb;
    [SerializeField] private float _speed, _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    private bool isGrounded;

    

    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput.x * _speed, rb.linearVelocity.y);

        if(moveInput.y > 0.5f)
        {
            Jump();
        }

        
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, _jumpForce);
        }

    }
    private void OnDrawGizmos()
    {
        if (_groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
        }
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    
    
}
