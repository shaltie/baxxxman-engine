
using UnityEngine;

public class GuardinScatter : GuardinBehavior
{
    private Node _node;

    private void Start() {
        // Debug.Log("Scatter started");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Node>() != null)
        {
            _node = other.GetComponent<Node>();

            // Debug.Log("Scatter: Guardin mode = " + SaveData.GetString(SaveData.GuardinMode));
            // Debug.Log("Scatter: Guardin NODE = " + _node);

            string guardinMode = SaveData.GetString(SaveData.GuardinMode);

            if (_node != null && guardinMode == "scatter")
            {

                // Pick a random available direction
                if (_node.availableDirections.Count == 0)
                    return;

                int index = Random.Range(0, _node.availableDirections.Count);

                // Prefer not to go back the same direction so increment the index to
                // the next available direction

                // Debug.Log("Scatter Index: " + index + ". Count is: " + _node.availableDirections.Count);

                if (_node.availableDirections.Count > 1 && _node.availableDirections[index] == guardin.movement.direction)
                {
                    index++;

                    // Wrap the index back around if overflowed
                    if (index >= _node.availableDirections.Count)
                    {
                        index = 0;
                    }
                }
                // Debug.Log("Scatter SetDirection " + _node.availableDirections[index]);
                guardin.movement.SetDirection(_node.availableDirections[index]);
            }
        }
    }

    public void GenerateNewDirection()
    {

        if (_node != null)
        {
            // Pick a random available direction
            int index = Random.Range(0, _node.availableDirections.Count);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction

            if (_node.availableDirections.Count > 1 && _node.availableDirections[index] == guardin.movement.direction)
            {
                index++;

                // Wrap the index back around if overflowed
                if (index >= _node.availableDirections.Count)
                {
                    index = 0;
                }
            }
            // Debug.Log("Scatter GenerateNewDirection guarding move");
            guardin.movement.SetDirection(_node.availableDirections[index]);
        }
    }
}
