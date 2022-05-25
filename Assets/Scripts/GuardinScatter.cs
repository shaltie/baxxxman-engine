
using UnityEngine;

public class GuardinScatter : GuardinBehavior
{

    private void OnDisable()
    {
        guardin.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened

        //Debug.Log((node != null) + "_" + this.enabled + "_" + !guardin.frightened.enabled);

        Debug.Log("OnTriggerEnter2D. Enabled: " + enabled);

        if (node != null && !guardin.frightened.enabled)
        {
            Debug.Log("Avaible derections count: " + node.availableDirections.Count);
            // Pick a random available direction
            int index = Random.Range(0, node.availableDirections.Count);

            Debug.Log("Avaible derections choosed index: " + index);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction

            Debug.Log("Movement direction: " + node.availableDirections[index] + "__" + -guardin.movement.direction);

            if (node.availableDirections[index] == guardin.movement.direction && node.availableDirections.Count > 1)
            {
                index++;

                // Wrap the index back around if overflowed
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            guardin.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
