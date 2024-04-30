using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct MoveSettings
{
    public InputAction move;
    public float movementSpeed;
}

[Serializable]
public struct JumpSettings
{
    public InputAction jump;
    public float jumpForce;
    public uint jumpCount;
    public LayerMask layer;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MoveSettings _moveSettings = new() { movementSpeed = 10 };
    [SerializeField] private JumpSettings _jumpSettings = new() { jumpForce = 13, jumpCount = 2 };

    private SpriteRenderer _sprtRndr;
    private Rigidbody2D _rb2D;
    private Animator _anim;

    private uint _currentJumpCount;

    private bool IsGrounded() => Physics2D.BoxCast(transform.position - Vector3.up, new Vector2(0.9f, 0.1f), 0, Vector2.zero, 100, _jumpSettings.layer);

    private void OnEnable()
    {
        _moveSettings.move.Enable();
        _jumpSettings.jump.Enable();
    }
    private void OnDisable()
    {
        _moveSettings.move.Disable();
        _jumpSettings.jump.Disable();
    }

    private void Awake()
    {
        _sprtRndr = GetComponent<SpriteRenderer>();
        _rb2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _jumpSettings.jump.performed += Jump;
    }

    private void Update()
    {
        _anim.SetBool("IsGrounded", IsGrounded());
        _anim.SetFloat("YVelocity", _rb2D.velocity.y);

        if (IsGrounded())
        {
            _currentJumpCount = _jumpSettings.jumpCount - 1;
        }

        Move(_moveSettings.move.ReadValue<Vector2>());
    }

    public void Move(Vector2 movement)
    {
        Vector2 velocity = _rb2D.velocity;
        velocity.x = movement.x * _moveSettings.movementSpeed;
        _rb2D.velocity = velocity;

        _anim.SetFloat("X", movement.x);

        if (IsGrounded())
        {
            if (movement.x > 0)
                _sprtRndr.flipX = false;
            else if (movement.x < 0)
                _sprtRndr.flipX = true;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (_currentJumpCount == 0)
            return;

        _rb2D.velocity = new(_rb2D.velocity.x, _jumpSettings.jumpForce);
        _anim.SetTrigger("Jump");

        --_currentJumpCount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position - Vector3.up, new Vector2(0.9f, 0.1f));
    }
}
