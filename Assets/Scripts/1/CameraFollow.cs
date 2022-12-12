using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed;

    private void Update()
    {
        Vector2 smoothPosition = Vector2.Lerp(transform.position, _target.position, Time.deltaTime * _followSpeed);
        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, transform.position.z);
    }
}
