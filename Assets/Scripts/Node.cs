using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private bool _isObstacle;

    private readonly List<Vector2> _directions = new List<Vector2>()
    {
        new Vector2(1, 1),
        new Vector2(-1, 1),
        new Vector2(1, -1),
        new Vector2(-1, -1),
    };

    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections;// { get; private set; }

    public bool IsObstacle => _isObstacle;

    public bool IsBesideFire { get; private set; }

    private void Start()
    {
        IsBesideFire = ExistBesideFire();

        if (_isObstacle)
            return;

        availableDirections = new List<Vector2>();

        // We determine if the direction is available by box casting to see if
        // we hit a wall. The direction is added to list if available.
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void Update()
    {
        if (_isObstacle == false)
        {
            availableDirections = new List<Vector2>();

            CheckAvailableDirection(Vector2.up);
            CheckAvailableDirection(Vector2.down);
            CheckAvailableDirection(Vector2.left);
            CheckAvailableDirection(Vector2.right);
        }
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // If no collider is hit then there is no obstacle in that direction
        if (hit.collider == null)
        {
            if (_isObstacle)
            {
                if (availableDirections.Contains(direction))
                {
                    availableDirections[availableDirections.IndexOf(direction)] = direction;
                }
                else
                {
                    availableDirections.Add(direction);
                }
            }
            else
            {
                availableDirections.Add(direction);
            }
        }
    }

    private bool ExistBesideFire()
    {
        foreach (var direction in _directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f, obstacleLayer);

            if (hit)
            {
                if (hit.collider.TryGetComponent(out Fire fire))
                    return true;
            }
        }

        return false;
    }
}
