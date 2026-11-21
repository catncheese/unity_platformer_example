using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public const string GROUND_TAG = "Ground";

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject _coinOrigin;

    private float _linearVelocity;
    private bool _flipRight;
    private bool _onTheGround;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    void Start()
    {
        _flipRight = true;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current.dKey.isPressed
            || Keyboard.current.rightArrowKey.isPressed)
        {
            _linearVelocity = 1;
            _flipRight = true;
        }
        else if(Keyboard.current.aKey.isPressed
            || Keyboard.current.leftArrowKey.isPressed)
        {
            _linearVelocity = -1;
            _flipRight = false;
        }
        else
        {
            _linearVelocity = 0;
        }


        _rigidbody.linearVelocity = new Vector2(_linearVelocity * _speed,
            _rigidbody.linearVelocity.y);

        _spriteRenderer.flipX = !_flipRight;

        _animator.SetFloat("Speed", Mathf.Abs(_linearVelocity));

        if(Keyboard.current.spaceKey.wasPressedThisFrame && _onTheGround)
        {
            _rigidbody.AddForce(_jumpForce * new Vector2(0, 1), ForceMode2D.Impulse);

            // TODO: change to separate thread (next lesson)
            // GameObject newObj = Instantiate(_coinOrigin, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
        }
    }

    public void IncrementScore()
    {
        _score++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == GROUND_TAG)
            _onTheGround = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Do Nothing
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GROUND_TAG)
            _onTheGround = false;
    }
}
