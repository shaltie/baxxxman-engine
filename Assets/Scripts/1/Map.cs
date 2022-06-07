using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private List<Guardin> _guardins;
    [SerializeField] private Transform _walls;
    [SerializeField] private Transform _money;
    [SerializeField] private Transform _nodes;

    public IReadOnlyList<Guardin> Guardins => _guardins; 
    public Transform Walls => _walls;
    public Transform Money => _money;
    public Transform Nodes => _nodes;
}
