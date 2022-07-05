using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement))]
public class Guardin : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;

    public bool _isFollow { get; private set; } = false;
    public Collider2D Collider { get; private set; }
    public EnemyType EnemyType => _enemyType;
    public Movement movement { get; private set; }
    public GuardinHome home { get; private set; }
    public GuardinScatter scatter { get; private set; }
    public GuardinChase chase { get; private set; }
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
        home = GetComponent<GuardinHome>();
        scatter = GetComponent<GuardinScatter>();
        chase = GetComponent<GuardinChase>();
    }

    private void Start()
    {
        ResetState();
    }

    public void SetGuardinMode(string mode)
    {
        SaveData.Save(SaveData.GuardinMode, mode);
    }

    public void Follow(Transform target, bool isObstacle = false)
    {
        _isFollow = true;
        this.target = target;

        IsObstacle = isObstacle;
    }

    public bool IsObstacle { get; private set; }

    public void StopFollow()
    {
        _isFollow = false;
        this.target = null;
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Baxxxman"))
        {
            FindObjectOfType<GameManager>().HeroCaught();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Baxxxman"))
        {
            FindObjectOfType<GameManager>().HeroCaught();
        }

        if (collision.TryGetComponent(out Bite bite))
        {
            FindObjectOfType<GameManager>().StopBite();
            Destroy(bite.gameObject);
        }
    }
}
