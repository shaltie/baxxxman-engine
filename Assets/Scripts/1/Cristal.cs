using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cristal : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleLayer;

    private Wall _wall;

    private readonly List<Vector2> _directions = new List<Vector2>()
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Update()
    {
        if (_wall != null)
            return;

        if (_wall == null && CheckAvailableDirection())
        {
            Debug.Log("Crystal upd");
            var walls = FindObjectsOfType<Wall>();

            foreach (var wall in walls.Reverse())
            {
                if (wall.IsHide)
                {
                    _wall = wall;
                    return;
                }
            }
        }
    }

    public void HideWall() => _wall?.gameObject.SetActive(false);

    private bool CheckAvailableDirection()
    {
        List<bool> results = new List<bool>();

        foreach (var direction in _directions)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, _obstacleLayer);
            results.Add(hit.collider == null);
        }

        return results.Any(result => result);
    }
}
