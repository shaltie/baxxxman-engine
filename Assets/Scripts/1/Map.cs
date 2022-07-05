using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<Guardin> _guardins;
    public Transform mapCoins;
    public Transform _wall;

    public IReadOnlyList<Guardin> Guardins => _guardins;
    public Transform Wall => _wall;
}
