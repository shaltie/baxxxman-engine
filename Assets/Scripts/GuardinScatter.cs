
using UnityEngine;

public class GuardinScatter : GuardinBehavior
{
    private Node _node;

    private void OnDisable()
    {
        guardin.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened

        //Debug.Log((node != null) + "_" + this.enabled + "_" + !guardin.frightened.enabled);

        if (_node != null && !guardin.frightened.enabled)
        {
            // Pick a random available direction
            int index = Random.Range(0, _node.availableDirections.Count);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction

            if (_node.availableDirections[index] == guardin.movement.direction && _node.availableDirections.Count > 1)
            {
                index++;

                // Wrap the index back around if overflowed
                if (index >= _node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            guardin.movement.SetDirection(_node.availableDirections[index]);
        }
    }

    public void GenerateNewDirection()
    {
        // Do nothing while the ghost is frightened

        //Debug.Log((node != null) + "_" + this.enabled + "_" + !guardin.frightened.enabled);

        if (_node != null && !guardin.frightened.enabled)
        {
            // Pick a random available direction
            int index = Random.Range(0, _node.availableDirections.Count);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction

            if (_node.availableDirections[index] == guardin.movement.direction && _node.availableDirections.Count > 1)
            {
                index++;

                // Wrap the index back around if overflowed
                if (index >= _node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            guardin.movement.SetDirection(_node.availableDirections[index]);
        }
    }
}
