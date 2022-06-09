using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private List<Guardin> _guardins;

    public IReadOnlyList<Guardin> Guardins => _guardins;
}
