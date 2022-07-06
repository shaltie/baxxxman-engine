using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Hero : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private SwipeControl _swipeControl;
    [SerializeField] private int _maxCristalCount;
    [SerializeField] private Bite _biteTemplate;

    private int _currentCristalCount;

    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public EnemyType EnemyType => _enemyType;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
    }

    private void Start()
    {
        if (SaveData.Has(SaveData.Speed))
            movement.speed = SaveData.GetFloat(SaveData.Speed);
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Node node))
        {
            Debug.Log(Vector2.Distance(transform.position, node.transform.position));
        }
    }

    private void Update()
    {
        // Set the new direction based on the current inputaaa
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up, Quaternion.Euler(0f, 0f, 270f), true);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down, Quaternion.Euler(0f, 0f, 90f), true);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left, Quaternion.Euler(0f, 0f, 0f), true);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right, Quaternion.Euler(0f, 0f, 180f), true);
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg * (-1), Vector3.forward * (-1));
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
            movement.SetDirection(Vector2.up, Quaternion.Euler(0, 0, 270), true);
        }
        else if (direction == Vector3.down)
        {
            movement.SetDirection(Vector2.down, Quaternion.Euler(0, 0, 90), true);
        }
        else if (direction == Vector3.left)
        {
            movement.SetDirection(Vector2.left, Quaternion.Euler(0, 0, 0), true);
        }
        else if (direction == Vector3.right)
        {
            movement.SetDirection(Vector2.right, Quaternion.Euler(0, 0, -180), true);
        }
    }
}
