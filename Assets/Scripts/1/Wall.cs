using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private readonly List<Vector2> _directions = new List<Vector2>()
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Guardin guardin))
        {
            var freeDirection = _directions.Where(direction => direction != guardin.movement.direction).ToList();

            int randomIndex = Random.Range(0, freeDirection.Count);
            guardin.movement.SetDirection(freeDirection[randomIndex]);
        }
    }
}
