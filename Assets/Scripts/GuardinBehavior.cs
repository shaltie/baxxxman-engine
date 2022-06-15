using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Guardin))]
public class GuardinBehavior : MonoBehaviour
{
  public Guardin guardin { get; private set; }

  private void Awake()
  {
      guardin = GetComponent<Guardin>();
  }
}
