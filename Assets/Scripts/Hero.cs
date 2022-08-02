using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement))]
public class Hero : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private SwipeControl _swipeControl;
    [SerializeField] private int _maxCristalCount;
    [SerializeField] private Bite _biteTemplate;
    [SerializeField] private float _startAnimationTime;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private SpriteRenderer _shieldRenderer;
    [SerializeField] private float _speedRotate;
    [SerializeField] private LayerMask _obstacleLayer;

    private readonly Dictionary<Vector2, Quaternion> _directions = new Dictionary<Vector2, Quaternion>()
    {
        { Vector2.up, Quaternion.Euler(0f, 0f, 270f) },
        { Vector2.down, Quaternion.Euler(0f, 0f, 90f) },
        { Vector2.left, Quaternion.Euler(0f, 0f, 0f) },
        { Vector2.right, Quaternion.Euler(0f, 0f, 180f) }
    };
    private GameManager _manager;
    private int _currentCristalCount;
    private Vector2 _direction;
    private float _expiredTime;
    private float _duration = 4;
    private bool _isPlayAnimation = false;
    private bool _isStartPlayAnimation = true;

    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public EnemyType EnemyType => _enemyType;
    private bool _isUseShield = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
        _manager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        _nextPosition = transform.position;
    }

    private void OnEnable()
    {
        _swipeControl.Swiped += Swipe;
    }

    private void OnDisable()
    {
        _swipeControl.Swiped -= Swipe;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.Move(movement.direction, movement.speed);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.StopMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Fire fire))
            FindObjectOfType<GameManager>().HeroCaught();

        if (collision.TryGetComponent(out Cristal cristal))
        {
            cristal.HideWall();

            _currentCristalCount++;
            Destroy(cristal.gameObject);

            if (_currentCristalCount == _maxCristalCount)
            {
                FindObjectOfType<PortalControl>().Show();
            }
        }

        if (collision.TryGetComponent(out Portal portal))
            FindObjectOfType<GameManager>().Win();

        if (collision.TryGetComponent(out Shield shield))
        {
            FindObjectOfType<GameManager>().PlayShield();
            Destroy(shield.gameObject);
        }
    }

    [SerializeField] private Vector2 _nextPosition;
    [SerializeField] private bool _isNextMove = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector2.right;
        }

        TryPlayFirstAnimationShield();
        TryPlayAnimationShield();

        if (_direction == Vector2.zero)
            return;

        Debug.DrawLine(transform.position, _nextPosition, Color.red);

        float distance = Vector2.Distance(transform.position, _nextPosition);

        if (distance < 0.1f)
        {
            if (CheckAvailableDirection(_direction) == false)
            {
                if(_direction != movement.direction)
                    _nextPosition += movement.direction;

                return;
            }

            Debug.Log(CheckAvailableDirection(_direction));
            _nextPosition += _direction;
            movement.SetDirection(_direction, GetRotation(_direction), true);
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg * (-1), Vector3.forward * (-1));
    }

    private bool CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, _obstacleLayer);
        return hit.collider == null;
    }

    private void TryPlayFirstAnimationShield()
    {
        bool isMove = _direction != Vector2.zero;

        Debug.Log(isMove);

        if (isMove == false)
            _isUseShield = true;

        if (isMove && _isStartPlayAnimation)
        {
            _manager.PlayShield(_startAnimationTime);
            PlayShieldAnimation(_startAnimationTime, () => _isUseShield = false);
            _isStartPlayAnimation = false;
        }
    }

    private void TryPlayAnimationShield()
    {
        _shieldRenderer.enabled = _isPlayAnimation;

        if (_isPlayAnimation)
        {
            _expiredTime += Time.deltaTime;

            if (_expiredTime > _duration)
                _expiredTime = 0;

            float progress = _expiredTime / _duration;
            spriteRenderer.color = new Color(1, 1, 1, _animationCurve.Evaluate(progress));

            _shieldRenderer.transform.Rotate(0, 0, Time.deltaTime * _speedRotate);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public bool IsUseShield()
    {
        Debug.Log(_manager.IsPlayShield + "__" + _isUseShield);

        return _manager.IsPlayShield || _isUseShield;
    }

    public void PlayShieldAnimation(float duretion, UnityAction callback = null)
    {
        StartCoroutine(WaitPlayAnimationShield(duretion, callback));
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        //deathSequence.enabled = false;
        // deathSequence.spriteRenderer.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);

        _nextPosition = transform.position;
        _direction = Vector2.zero;
        _expiredTime = 0;
        _isStartPlayAnimation = true;
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        //deathSequence.enabled = true;
        //deathSequence.spriteRenderer.enabled = true;
        //deathSequence.Restart();
    }

    public Transform CreateBite()
    {
        return Instantiate(_biteTemplate, transform.position, transform.rotation).transform;
    }

    private void Swipe(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            _direction = Vector2.up;
        }
        else if (direction == Vector3.down)
        {
            _direction = Vector2.down;
        }
        else if (direction == Vector3.left)
        {
            _direction = Vector2.left;
        }
        else if (direction == Vector3.right)
        {
            _direction = Vector2.right;
        }
    }

    private Quaternion GetRotation(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return Quaternion.identity;

        return _directions[direction];
    }

    private IEnumerator WaitPlayAnimationShield(float duration, UnityAction callback = null)
    {
        _isPlayAnimation = true;
        yield return new WaitForSeconds(duration);
        _isPlayAnimation = false;
        callback?.Invoke();
    }
}
