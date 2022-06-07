using UnityEngine;

public class GuardinChase : GuardinBehavior
{
    private void OnDisable()
    {
        this.guardin.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        return;
        Node node = other.GetComponent<Node>();

        // Do nothing while the guardin is frightened
        if (node != null && enabled && !guardin.frightened.enabled)
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

            Debug.Log(direction);
            guardin.movement.SetDirection(direction);
        }
    }
}
