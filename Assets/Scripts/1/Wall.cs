using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool _isHide;

    public bool IsHide => _isHide;
}
