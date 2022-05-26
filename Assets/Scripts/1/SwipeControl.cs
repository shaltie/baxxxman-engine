using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeControl : MonoBehaviour
{
    [SerializeField] private float _minSwipeLength;

    private Vector2 _firstPressPosition;
    private Vector2 _secondPressPosition;
    private Vector2 _currentSwipe;

    public event UnityAction<Vector3> Swiped;

    private void Update() => DetectSwipe();

    private void DetectSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                _firstPressPosition = new Vector2(touch.position.x, touch.position.y);

            if (touch.phase == TouchPhase.Ended)
            {
                _secondPressPosition = new Vector2(touch.position.x, touch.position.y);
                _currentSwipe = new Vector3(_secondPressPosition.x - _firstPressPosition.x, _secondPressPosition.y - _firstPressPosition.y);

                if (_currentSwipe.magnitude < _minSwipeLength)
                    return;

                _currentSwipe.Normalize();

                if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    Swiped?.Invoke(Vector3.up);
                else if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    Swiped?.Invoke(Vector3.down);
                else if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    Swiped?.Invoke(Vector3.left);
                else if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    Swiped?.Invoke(Vector3.right);
            }
        }
    }
}
