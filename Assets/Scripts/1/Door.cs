using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;

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
                guardin.Collider.isTrigger = true;
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
