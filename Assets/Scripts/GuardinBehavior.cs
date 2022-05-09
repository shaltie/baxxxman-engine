using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Guardin))]
public class GuardinBehavior : MonoBehaviour
{
  public Guardin guardin { get; private set; }
  public float duration;

  private void Awake()
  {
      guardin = GetComponent<Guardin>();
  }

  public void Enable()
  {
      Enable(duration);
  }

  public virtual void Enable(float duration)
  {
      enabled = true;

      CancelInvoke();
      Invoke(nameof(Disable), duration);
  }

  public virtual void Disable()
  {
      enabled = false;

      CancelInvoke();
  }
}
