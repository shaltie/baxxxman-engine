using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private GuardinScatter _guardinScatter;
    [SerializeField] private SpriteRenderer _target;
    [SerializeField] private float _accelerateSpeed;
    [SerializeField] private float _duration;

    private readonly Dictionary<Vector2, Quaternion> _directions = new Dictionary<Vector2, Quaternion>()
    {
        { Vector2.up, Quaternion.Euler(0, 0, 90) },
        { Vector2.down, Quaternion.Euler(0, 0, 270) },
        { Vector2.left, Quaternion.Euler(0, 0, 180) },
        { Vector2.right, Quaternion.Euler(0, 0, 0) }
    };
    private bool _isAccelerate = false;

    public SpriteRenderer Sprite => _target;
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public LayerMask obstacleLayerBox;
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction;// { get; private set; }
    public Vector2 nextDirection;// { get; private set; }
    public Vector2 nextDirection2;// { get; private set; }
    public Vector3 startingPosition { get; private set; }
    public Vector2 _Distination = new Vector2();
    public Vector2 PrewPosition = new Vector2();
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
        _Distination = (Vector2)transform.position+ initialDirection;
    }

    public void PlayAccelerate(UnityAction callback)
    {
        if(_isAccelerate == false)
            StartCoroutine(WaitAccelerate(callback));
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = direction;// Vector2.zero;
        if (GetComponent<Hero>() != null)
        {
            if ((Vector2)transform.position != GetComponent<Hero>()._prewPosition)
                transform.position = GetComponent<Hero>()._prewPosition;//startingPosition;
        }
        else
            transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        // Try to move in the next direction while it's queued to make movements
        // more responsive
       // MoveToDist();

        if (nextDirection != Vector2.zero && tag!="Player")
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
       /* Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);*/
       // MoveToDist();
        //  if (nextDirection != Vector2.zero && tag == "Player")
        //      MoveToDist();
        /* else
          {
              Vector2 position = rigidbody.position;
              Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

              rigidbody.MovePosition(position + translation);
          }*/
        if (/*nextDirection != Vector2.zero &&*/tag != "Player")
            MoveToDist();
      /* else
        {
            Vector2 position = rigidbody.position;
            Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

            rigidbody.MovePosition(position + translation);
        }*/

    }
    private void MoveToDist()
    {
        Debug.DrawLine(transform.position, _Distination, Color.red);
        Vector2 position = rigidbody.position;
        float DistN = Vector2.Distance(position, _Distination);
        double DistRoundF = Math.Round(DistN, 2, MidpointRounding.ToEven);

        if (Vector2.Distance(position, _Distination) < Mathf.Epsilon)
        {

            Debug.DrawLine(position, _Distination, Color.green);
            PrewPosition = position;
            _Distination = _Distination + direction;

        }
        else
        {
            transform.position = Vector2.MoveTowards(position, _Distination, speed * speedMultiplier * Time.fixedDeltaTime);

        }
        if (BoxOccupied(direction))
        {
            Vector2 DirAtas = _Distination - PrewPosition;
            direction = -direction;
            _Distination = PrewPosition;

            nextDirection = direction;
            Debug.DrawRay(position, DirAtas, Color.yellow);
        }
        if (Occupied(direction))
        {
            Vector2 DirAtas = _Distination - PrewPosition;
            if (!Occupied(-direction) && !BoxOccupied(-direction))
            {
                direction = -direction;
                _Distination = PrewPosition;
                nextDirection = direction;
            }
            else
            {
                Vector2 DirXX = new Vector2();                
                DirXX = new Vector2(1, 0);
                if (Occupied(DirXX) || BoxOccupied(DirXX))
                {
                    DirXX = new Vector2(-1, 0);
                    if (Occupied(DirXX) || BoxOccupied(DirXX))
                    {
                        DirXX = new Vector2(0, 1);
                        if (Occupied(DirXX) || BoxOccupied(DirXX))
                        {
                            DirXX = new Vector2(0, -1);
                        }
                    }
                }
                _Distination = position + DirXX;
                direction = DirXX;
                nextDirection = direction;
            }
            

          //  nextDirection = direction;
            Debug.DrawRay(position, DirAtas, Color.yellow);
        }
    }
    public void SetDirection(Vector2 direction, Quaternion rotation = default(Quaternion), bool isPlayerRotate = false)
    {
        // Only set the direction if the tile in that direction is available
        // otherwise we set it as the next direction so it'll automatically be
        // set when it does become available
       // nextDirection = direction;
       
        if (!Occupied(direction))
         {
             if(direction != Vector2.zero)
                 this.direction = direction;

             nextDirection = Vector2.zero;
         }
         else
         {
              /*  if(tag != "Player")
                {
                    int index = Random.Range(0, 3);
                    direction = RandomDir(index);
                    nextDirection = direction;
                }*/
           //     else
           //  if (Vector2.Distance(transform.position, _Distination) < Mathf.Epsilon)
            // {
                 //_Distination = (Vector2)transform.position + direction;
                 nextDirection = direction;
           //  }
         }

        if (isPlayerRotate == false)
        {
            if(_target != null)
                _target.transform.rotation = GetRotation(direction);
        }
    }
    Vector2 RandomDir(int X)
    {
        int index =  X;
        Vector2 VV = new Vector2();
        switch (X)
        {
            case 0:
                VV = Vector2.up;
                break;
            case 1:
                VV = Vector2.down;
                break;
            case 2:
                VV = Vector2.left;
                break;
            case 3:
                VV = Vector2.right;
                break;
        }
        return VV;
    }
    public bool Occupied(Vector2 direction)
    {
        // If no collider is hit then there is no obstacle in that direction
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, 0.5f, obstacleLayer);
      //  RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, obstacleLayer);
        return hit.collider != null;
    }
    private bool BoxOccupied(Vector2 direction)
    {
       // RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, 0.5f, obstacleLayerBox);
          RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 0.5f, obstacleLayerBox);
        return hit.collider != null;
    }
    private Quaternion GetRotation(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            _guardinScatter.GenerateNewDirection();
            return _directions[this.direction];
        }

        return _directions[direction];
    }

    private IEnumerator WaitAccelerate(UnityAction callback)
    {
        int accelerateCount = SaveData.GetInt(SaveData.Accelerate) - 1;
        SaveData.Save(SaveData.Accelerate, accelerateCount);
        callback?.Invoke();

        _isAccelerate = true;
        speed *= _accelerateSpeed;
        yield return new WaitForSeconds(_duration);
        speed /= _accelerateSpeed;
        _isAccelerate = false;
    }
}
