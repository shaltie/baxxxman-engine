using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Door : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;

    private readonly List<Vector2> _directions = new List<Vector2>()
    {
        Vector2.up,
        Vector2.down,
    };

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Hero hero))
        {
            if (hero.EnemyType == _enemyType)
                hero.collider.isTrigger = true;
        }

        if (collision.transform.TryGetComponent(out Guardin guardin))
        {
            if (guardin.EnemyType == _enemyType)
            {
                guardin.Collider.isTrigger = true;
            }
            else
            {
                var freeDirection = _directions.Where(direction => direction != guardin.movement.direction).ToList();

                int randomIndex = Random.Range(0, freeDirection.Count);
                Vector2 ranomDirection = _directions[randomIndex];

                guardin.movement.SetDirection(ranomDirection, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Hero hero))
        {
            if (hero.EnemyType == _enemyType)
                hero.collider.isTrigger = false;
        }

        if (collision.TryGetComponent(out Guardin guardin))
        {
            if (guardin.EnemyType == _enemyType)
                guardin.Collider.isTrigger = false;
        }
    }
}
