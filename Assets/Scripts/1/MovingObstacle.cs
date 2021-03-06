using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform _targetStart;
    [SerializeField] private Transform _targetEnd;
    [SerializeField] private Transform _targetRotate;
    [SerializeField] private float _speedRotate;

    private Guardin _guardin;

    private void Awake()
    {
        _guardin = GetComponent<Guardin>();
    }

    private void Start()
    {
        _guardin.Follow(_targetStart, true);
    }

    private void Update()
    {
        _targetRotate.Rotate(new Vector3(0, 0, _speedRotate));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == _targetStart.name || collision.name == _targetEnd.name)
            _guardin.Follow(GetNextPath(collision), true);
    }

    private Transform GetNextPath(Collider2D collision)
    {
        return collision.name == _targetStart.name ? _targetEnd : _targetStart;
    }
}
