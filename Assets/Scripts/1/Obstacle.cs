using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Obstacle : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private float _speed;
    private bool _isMove = false;

    private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (_isMove == false)
            return;

        Vector2 position = _rigidbody.position;
        Vector2 translation = _direction * _speed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(position + translation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (true)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (true)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (true)
        {
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        _isMove = true;
        _direction = direction;
        _speed = speed;
    }

    public void StopMove()
    {
        _isMove = false;
    }
}
