using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Guardin : MonoBehaviour
{
  public Movement movement { get; private set; }
  public GuardinHome home { get; private set; }
  public GuardinScatter scatter { get; private set; }
  public GuardinChase chase { get; private set; }
  public GuardinFrightened frightened { get; private set; }
  public GuardinBehavior initialBehavior;
  public Transform target;
  public int points = 200;

  private void Awake()
  {
      movement = GetComponent<Movement>();
      home = GetComponent<GuardinHome>();
      scatter = GetComponent<GuardinScatter>();
      chase = GetComponent<GuardinChase>();
      frightened = GetComponent<GuardinFrightened>();
  }

  private void Start()
  {
      ResetState();
  }

  public void ResetState()
  {
      gameObject.SetActive(true);
      movement.ResetState();

      frightened.Disable();
      chase.Disable();
      scatter.Enable();

      if (home != initialBehavior) {
          home.Disable();
      }

      if (initialBehavior != null) {
          initialBehavior.Enable();
      }
  }

  public void SetPosition(Vector3 position)
  {
      // Keep the z-position the same since it determines draw depth
      position.z = transform.position.z;
      transform.position = position;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Baxxxman"))
      {
          if (frightened.enabled) {
              FindObjectOfType<GameManager>().GuardinCaught(this);
          } else {
              FindObjectOfType<GameManager>().HeroCaught();
          }
      }
  }
}
