using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public void SetPause(bool isPause) => Time.timeScale = isPause ? 0 : 1;
}
