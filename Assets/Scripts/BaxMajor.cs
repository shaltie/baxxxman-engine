using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaxMajor : Bax
{
  public float duration = 8.0f;

  protected override void Eat()
  {
      FindObjectOfType<GameManager>().BaxMajorCollected(this);
  }
}
