using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleLayer;//--
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private float _speed;
    private float _speedMultiplier;
    private Vector2 _NextPos;
    private Vector2 _nextPosition;
    private bool _isMove = false;
    public bool waalOK = false;
    private bool _isMoveNew = false;
   // public Hero Her = null;
    private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();
   // [SerializeField] private LayerMask _obstacleLayer;
    private void FixedUpdate()
    {
     /*   if (_isMove == false)
            return;

        Vector2 position =  _rigidbody.position;
        Vector2 translation = _direction * _speed * Time.fixedDeltaTime;
        */
        if (_isMoveNew)
        {
            _isMoveNewStart();
           
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Hero hero))
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
           
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Hero hero))
        {           
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            hero.KUB = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
        if (collision.collider.TryGetComponent(out Hero hero))
        {
            _rigidbody.bodyType = RigidbodyType2D.Static;           
        }
    }*/
    public void Move(Vector2 direction, float speed)
    {
        _isMove = true;
        _direction = direction;
        _speed = speed;
    }

    public void StopMove(Vector2 pos)
    {
        transform.position = pos;
        _isMove = false;
        //_isMoveNew = false;
    }
    public void StopMoveNew()
    {
        _isMoveNew = false;
    }
    public void MoveNewTriggers(Vector2 NextPos, Vector2 dir, float speed, float speedMultiplier)
    {
        _direction = dir;
        _speed = speed;
        _speedMultiplier = speedMultiplier;
        _isMoveNew = true;
        _NextPos = NextPos;
    }
  public void _isMoveNewStart()
    {
        Vector2 position = transform.position;
        _nextPosition = _NextPos + _direction;
        transform.position = Vector2.MoveTowards(position, _nextPosition, _speed * _speedMultiplier * Time.fixedDeltaTime);
        
       
    }
    private bool CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, 1, _obstacleLayer);
        //  RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1, _obstacleLayer);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
           /* Debug.Log("NAME COOLAI=" + hit.collider.gameObject.name + "pos=" + hit.collider.transform.position);
            if (hit.collider.tag == "Player")
                return false;
            */
            return true;
        }
        //   return hit.collider == null;
    }
}
