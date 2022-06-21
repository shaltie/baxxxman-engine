using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] public List<Guardin> _guardins;
    [SerializeField] public Transform mapCoins;

    public IReadOnlyList<Guardin> Guardins => _guardins;
}
