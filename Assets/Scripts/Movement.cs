using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private GuardinScatter _guardinScatter;
    [SerializeField] private SpriteRenderer _target;
    [SerializeField] private float _accelerateSpeed;
    [SerializeField] private float _duration;

    private readonly Dictionary<Vector2, Quaternion> _directions = new Dictionary<Vector2, Quaternion>()
    {
        { Vector2.up, Quaternion.Euler(0, 0, 90) },
        { Vector2.down, Quaternion.Euler(0, 0, 270) },
        { Vector2.left, Quaternion.Euler(0, 0, 180) },
        { Vector2.right, Quaternion.Euler(0, 0, 0) }
    };
    private bool _isAccelerate = false;

    public SpriteRenderer Sprite => _target;
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public LayerMask doorLayer;

    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void PlayAccelerate(UnityAction callback)
    {
        if(_isAccelerate == false)
            StartCoroutine(WaitAccelerate(callback));
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        // Try to move in the next direction while it's queued to make movements
        // more responsive
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, Quaternion rotation = default(Quaternion), bool isPlayerRotate = false)
    {
        // Only set the direction if the tile in that direction is available
        // otherwise we set it as the next direction so it'll automatically be
        // set when it does become available

        if (!Occupied(direction))
        {
            if(direction != Vector2.zero)
                this.direction = direction;

            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }

        if (isPlayerRotate == false)
        {
            if(_target != null)
                _target.transform.rotation = GetRotation(direction);
        }
    }

    public bool Occupied(Vector2 direction)
    {
        // If no collider is hit then there is no obstacle in that direction
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.0f, obstacleLayer);

        return hit.collider != null;
    }

    private Quaternion GetRotation(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            _guardinScatter.GenerateNewDirection();
            return _directions[this.direction];
        }

        return _directions[direction];
    }

    private IEnumerator WaitAccelerate(UnityAction callback)
    {
        int accelerateCount = SaveData.GetInt(SaveData.Accelerate) - 1;
        SaveData.Save(SaveData.Accelerate, accelerateCount);
        callback?.Invoke();

        _isAccelerate = true;
        speed *= _accelerateSpeed;
        yield return new WaitForSeconds(_duration);
        speed /= _accelerateSpeed;
        _isAccelerate = false;
    }
}
