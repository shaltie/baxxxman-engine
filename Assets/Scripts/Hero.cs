using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Hero : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private SwipeControl _swipeControl;

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

    private void OnEnable()
    {
        _swipeControl.Swiped += Swipe;
    }

    private void OnDisable()
    {
        _swipeControl.Swiped -= Swipe;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision.name);

        if (collision.TryGetComponent(out Fire fire))
            FindObjectOfType<GameManager>().HeroCaught();
    }

    private void Update()
    {
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up, rotation: Quaternion.Euler(0f, 0f, 270f), isPlayerRotate: true);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down, rotation: Quaternion.Euler(0f, 0f, 90f), isPlayerRotate: true);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left, rotation: Quaternion.Euler(0f, 0f, 0f), isPlayerRotate: true);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right, rotation: Quaternion.Euler(0f, 0f, 180f), isPlayerRotate: true);
        }

        // Rotate pacman to face the movement direction
        //float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        //transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
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

    private void Swipe(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            movement.SetDirection(Vector2.up, rotation: Quaternion.Euler(0, 0, 270), isPlayerRotate: true);
        }
        else if (direction == Vector3.down)
        {
            movement.SetDirection(Vector2.down, rotation: Quaternion.Euler(0, 0, 90), isPlayerRotate: true);
        }
        else if (direction == Vector3.left)
        {
            movement.SetDirection(Vector2.left, rotation: Quaternion.Euler(0, 0, 0), isPlayerRotate: true);
        }
        else if (direction == Vector3.right)
        {
            movement.SetDirection(Vector2.right, rotation: Quaternion.Euler(0, 0, -180), isPlayerRotate: true);
        }
    }
}
