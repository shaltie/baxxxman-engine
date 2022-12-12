using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Movement))]
public class Hero : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private SwipeControl _swipeControl;
    [SerializeField] private int _maxCristalCount;
    [SerializeField] private Bite _biteTemplate;
    [SerializeField] private float _startAnimationTime;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private SpriteRenderer _shieldRenderer;
    [SerializeField] private float _speedRotate;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private LayerMask _obstacleLayerBox;
    [SerializeField] private LayerMask _obstacleLayerGuardBoxer;
    public GameObject PosPoint = null;
    Obstacle OBSKUB = null;
    //public bool Kub = false;
    private readonly Dictionary<Vector2, Quaternion> _directions = new Dictionary<Vector2, Quaternion>()
    {
        { Vector2.up, Quaternion.Euler(0f, 0f, 270f) },
        { Vector2.down, Quaternion.Euler(0f, 0f, 90f) },
        { Vector2.left, Quaternion.Euler(0f, 0f, 0f) },
        { Vector2.right, Quaternion.Euler(0f, 0f, 180f) }
    };
    private GameManager _manager;
    private int _currentCristalCount;
    public Vector2 _direction;
    private float _expiredTime;
    private float _duration = 4;
    private bool _isPlayAnimation = false;
    private bool _isStartPlayAnimation = true;

    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public EnemyType EnemyType => _enemyType;
    private bool _isUseShield = false;
    private Rigidbody2D _rigidbody;
    public float distance;

    public Vector2 PosZero = new Vector2();
    public bool KUB = false;
    private Obstacle _obstacle;
    private Node _node;
    Vector2 DirectionMove = new Vector2();
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
        _manager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        _nextPosition = transform.position;
    }

    private void OnEnable()
    {
        _swipeControl.Swiped += Swipe;
    }

    private void OnDisable()
    {
        _swipeControl.Swiped -= Swipe;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.Move(movement.direction, movement.speed);
            Debug.Log("BOX1");
        }
       
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
       /* if (collision.collider.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.StopMove();           
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Fire fire))
            FindObjectOfType<GameManager>().HeroCaught();

        if (collision.TryGetComponent(out Cristal cristal))
        {
            cristal.HideWall();

            _currentCristalCount++;
            Destroy(cristal.gameObject);

            if (_currentCristalCount == _maxCristalCount)
            {
                FindObjectOfType<PortalControl>().Show();
            }
        }

        if (collision.TryGetComponent(out Portal portal))
            FindObjectOfType<GameManager>().Win();

        if (collision.TryGetComponent(out Shield shield))
        {
            FindObjectOfType<GameManager>().PlayShield();
            Destroy(shield.gameObject);
        }
        if (collision.TryGetComponent(out Node node))
        {
            //obstacle.Move(_direction, movement.speed);
            Debug.Log("BOX2");
         /*   _node = node;
            KUB = true;*/
        }
    }

    [SerializeField] public Vector2 _nextPosition;
    [SerializeField] public Vector2 _prewPosition;
    [SerializeField] private bool _isNextMove = false;
   
    private void Update()
    {
        if(PosPoint!=null)
            PosPoint.transform.position = _nextPosition;
        RotateHeroNew();
    }
    private void FixedUpdate()
    {
        MoveToDist();
    }
    private void LateUpdate()
    {
       
    }
    void RotateHero()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector2.right;

        }

        TryPlayFirstAnimationShield();
        TryPlayAnimationShield();

        if (_direction == Vector2.zero)
        {
            
            return;
        }
        Debug.DrawLine(transform.position, _nextPosition, Color.red);
       // _prewPosition = _nextPosition;
        //  _nextPosition = transform.position* _direction;
        distance = Vector2.Distance(transform.position, _nextPosition);
        PosZero = transform.position;
        //поворот через центр
      /*  if (Vector2.Distance(transform.position, _nextPosition) < 0.1f)//0,1
       {
            PosZero = _nextPosition;
            _prewPosition = _nextPosition; //1
            Debug.Log("_nextPosition Direction = ok 1");
            movement.SetDirection(_direction, GetRotation(_direction), true);
         
            if (CheckAvailableDirection(_direction) == true)
             {
                Debug.Log("_nextPosition Direction COLIDER = ok 1");
                //   if (_direction != movement.direction)
                //   {
                //_prewPosition = (Vector2)transform.position;//_nextPosition; 1
                    _prewPosition += _direction;// movement.direction; 1
                    Vector2 dir = _prewPosition - (Vector2)transform.position;
                    if (CheckAvailableDirection(dir) == false)
                    {
                    _nextPosition += _direction;// movement.direction;1
                        movement.SetDirection(dir, GetRotation(dir), true);
                    Debug.Log("_nextPosition Direction COLIDER = false 2");
                }
                    else
                    {

                    if (CheckAvailableDirection(movement.direction) == false)
                    {
                            _nextPosition += movement.direction;// transform.position;
                                                                //_direction = movement.direction;
                        PosZero = _nextPosition;
                        movement.SetDirection(_direction, GetRotation(_direction), true);
                        Debug.Log("_nextPosition Direction COLIDER = false 3");
                    }
                    else
                    {
                        Debug.Log("_nextPosition Direction COLIDER = return 4");
                        // return;
                        PosZero = _nextPosition;
                        movement.SetDirection(_direction, GetRotation(_direction), true);
                        return;
                    }
                    }
                Debug.Log("_nextPosition Direction COLIDER = HZ 5");
                return;
             }
            _nextPosition += _direction;
            movement.SetDirection(_direction, GetRotation(_direction), true);
            Debug.Log("_nextPosition Direction COLIDER = HZ6 2");
        }*/
        //--
       /* if (NextChekColider(movement.direction) == true)
        {
            // PosZero = _nextPosition;
            _nextPosition = PosZero- movement.direction;                                        
           // movement.SetDirection(_direction, GetRotation(_direction), true);
            Debug.Log("_nextPosition Direction COLIDER = true 0101");
            
        }*/
        //--**
       /* float dist2 = Vector2.Distance(transform.position, _nextPosition);
        if (dist2 > 1f)
        {
            _nextPosition = PosZero + movement.direction;
            KUB = false;
        }
        if (dist2 > 0.1f)
        {
            if(KUB)
                _nextPosition = PosZero + _direction;
            movement.SetDirection(_direction, GetRotation(_direction), true);
        }*/
        //--
        //_direction
        //float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        //_nextPosition = PosZero + _direction;
        movement.SetDirection(_direction/*, GetRotation(_direction), true*/);
        float angle = Mathf.Atan2(_direction.y, _direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg * (-1), Vector3.forward * (-1));
        Debug.Log("_nextPosition Direction COLIDER = false 7");
    }

    //-------------------------
    private void RotateHeroNew()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _direction = Vector2.down;
            
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
            
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector2.right;
            
        }
        

        TryPlayFirstAnimationShield();
        TryPlayAnimationShield();

        float angle = Mathf.Atan2(_direction.y, _direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg * (-1), Vector3.forward * (-1)); 
    }
    private void MoveToDist()
    {
        
        Debug.DrawLine(transform.position, _nextPosition, Color.red);
        Vector2 position = transform.position;
        Vector2 position2 = position + _direction;
        //  _nextPosition = position + _direction;
        //Math.Round((Vector2.Distance(position.normalized, _nextPosition.normalized)), 1, MidpointRounding.ToEven);
        double dist1 = Math.Round((Vector2.Distance(position, _nextPosition)), 2, MidpointRounding.ToEven); //Vector2.Distance(position.normalized, _nextPosition.normalized);
       
        if (dist1 < Mathf.Epsilon)
        {
            Debug.DrawLine(transform.position, position2, Color.green);
            
            if (!CheckAvailableDirection(_direction))
            {
                _nextPosition = position + _direction;
                DirectionMove = _direction;
                
            }
            else
            {
                if (CheckAvailableDirection(DirectionMove))
                {
                    _nextPosition = position;
                   
                }
                else
                {
                    _nextPosition = position + DirectionMove;
                    
                }
            }
            CheckBoxMoveCollider();
        }
        else
        {
            //Задний ход
            if(_direction== -DirectionMove)
            {
                _nextPosition = _nextPosition + _direction;
                DirectionMove = _direction;
                if (OBSKUB)
                {
                    OBSKUB.StopMoveNew();
                    Debug.Log("OBSKUB!");
                    OBSKUB = null;
                }
            }

            if (!KUB)
            {
                transform.position = Vector2.MoveTowards(position, _nextPosition, movement.speed * movement.speedMultiplier * Time.fixedDeltaTime);
               
            }
            else
                _nextPosition = position;
            //
            Vector2 DirMove = _nextPosition - position;
            Debug.DrawLine(transform.position, _nextPosition, Color.blue);            
        }
    }
    //----------------------
    private void CheckBoxMoveCollider()
    {
        RaycastHit2D hitDir = Physics2D.Raycast((Vector2)transform.position, _direction.normalized, 2f, _obstacleLayer);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, DirectionMove, 2f, _obstacleLayer);
        //Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1, _obstacleLayer);
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, DirectionMove, 1f, _obstacleLayerBox);//на куб
        //  Physics2D.Raycast((Vector2)transform.position, DirectionMove, 1f, _obstacleLayerBox);//на куб
        RaycastHit2D hitBoxGuard = Physics2D.Raycast((Vector2)transform.position, DirectionMove, 2f, _obstacleLayerGuardBoxer);//на охранника
                                                                                                                // _obstacleLayerGuardBoxer
        KUB = false;
        if (hitBox.collider != null)
        {
            if (hitBox.collider.gameObject.GetComponentInParent<Obstacle>() != null)
            {
                OBSKUB = hitBox.collider.gameObject.GetComponentInParent<Obstacle>();
                if (hit.collider == null)
                {
                    //hitBox.collider.gameObject.GetComponentInParent<Obstacle>().MoveNewTriggers(_nextPosition /*+ DirectionMove*/, DirectionMove, movement.speed, movement.speedMultiplier);
                    // KUB = false;
                    if (hitBoxGuard.collider == null)
                    {
                        hitBox.collider.gameObject.GetComponentInParent<Obstacle>().MoveNewTriggers(_nextPosition /*+ DirectionMove*/, DirectionMove, movement.speed, movement.speedMultiplier);
                        KUB = false;

                    }
                    else
                    {
                        KUB = true;
                    }
                }
                else
                {
                    KUB = true;
                }

            }
        }
    }

    private bool CheckAvailableDirection(Vector2 direction)
    {
       RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, 1, _obstacleLayer);
      //  RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1, _obstacleLayer);
        if(hit.collider == null)
        {
            return false;
        }
        else
        {
            Debug.Log("NAME COOLAI=" + hit.collider.gameObject.name+ "pos="+hit.collider.transform.position);

            return true;
        }
    }

    private bool NextChekColider(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(PosZero, Vector2.one * 0.2f, 0f, direction, 1, _obstacleLayer);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            PosZero = hit.collider.transform.position;
            return true;
        }

    }
    private void TryPlayFirstAnimationShield()
    {
        bool isMove = _direction != Vector2.zero;

       // Debug.Log(isMove);

        if (isMove == false)
            _isUseShield = true;

        if (isMove && _isStartPlayAnimation)
        {
            _manager.PlayShield(_startAnimationTime);
            PlayShieldAnimation(_startAnimationTime, () => _isUseShield = false);
            _isStartPlayAnimation = false;
        }
    }

    private void TryPlayAnimationShield()
    {
        _shieldRenderer.enabled = _isPlayAnimation;

        if (_isPlayAnimation)
        {
            _expiredTime += Time.deltaTime;

            if (_expiredTime > _duration)
                _expiredTime = 0;

            float progress = _expiredTime / _duration;
            spriteRenderer.color = new Color(1, 1, 1, _animationCurve.Evaluate(progress));

            _shieldRenderer.transform.Rotate(0, 0, Time.deltaTime * _speedRotate);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public bool IsUseShield()
    {
     //   Debug.Log(_manager.IsPlayShield + "__" + _isUseShield);

        return _manager.IsPlayShield || _isUseShield;
    }

    public void PlayShieldAnimation(float duretion, UnityAction callback = null)
    {
        StartCoroutine(WaitPlayAnimationShield(duretion, callback));
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        //deathSequence.enabled = false;
        // deathSequence.spriteRenderer.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);

        _nextPosition = transform.position;
        _direction = Vector2.zero;
        _expiredTime = 0;
        _isStartPlayAnimation = true;
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        //deathSequence.enabled = true;
        //deathSequence.spriteRenderer.enabled = true;
        //deathSequence.Restart();
    }

    public Transform CreateBite()
    {
        return Instantiate(_biteTemplate, transform.position, transform.rotation).transform;
    }

    private void Swipe(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            _direction = Vector2.up;
        }
        else if (direction == Vector3.down)
        {
            _direction = Vector2.down;
        }
        else if (direction == Vector3.left)
        {
            _direction = Vector2.left;
        }
        else if (direction == Vector3.right)
        {
            _direction = Vector2.right;
        }
    }

    private Quaternion GetRotation(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return Quaternion.identity;

        return _directions[direction];
    }

    private IEnumerator WaitPlayAnimationShield(float duration, UnityAction callback = null)
    {
        _isPlayAnimation = true;
        yield return new WaitForSeconds(duration);
        _isPlayAnimation = false;
        callback?.Invoke();
    }
}
