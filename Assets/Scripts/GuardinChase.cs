using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GuardinChase : GuardinBehavior
{
    [SerializeField] private bool _isObstacle;
    [SerializeField] private Vector2 _tempDirection = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GenerateNewPath(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //GenerateNewPath(other);
    }

    private void GenerateNewPath(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node == null)
            return;

        string guardinMode = SaveData.GetString(SaveData.GuardinMode);

        if (guardinMode == "chase" || _isObstacle)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Find the available direction that moves closet to hero
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is less than the current
                // min distance then this direction becomes the new closest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);

                float distance = minDistance;

                if (guardin.target != null)
                    distance = (guardin.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            if (_isObstacle == false && node.availableDirections.Count >= 2)
            {
                if (_tempDirection == direction)
                    direction = GetNewDirection(node.availableDirections, direction);

                _tempDirection = guardin.movement.direction;
            }

            guardin.movement.SetDirection(direction);
        }
    }

    private Vector2 GetNewDirection(IReadOnlyList<Vector2> directions, Vector2 currentDirection)
    {
        var sorteredDirections = directions.Where(direction => direction != currentDirection).ToList();
        int randomIndex = Random.Range(0, sorteredDirections.Count);
        return sorteredDirections[randomIndex];
    }
}
