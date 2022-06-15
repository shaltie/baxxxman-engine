using UnityEngine;

public class GuardinChase : GuardinBehavior
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        string guardinMode = SaveData.GetString(SaveData.GuardinMode);
        if (node != null && guardinMode == "chase")
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

            guardin.movement.SetDirection(direction);
        }
    }
}
