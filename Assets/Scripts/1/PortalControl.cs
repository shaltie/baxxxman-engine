using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalControl : MonoBehaviour
{
    [SerializeField] private Portal _portal;
    [SerializeField] private Vector2 _maxRangePosition;
    [SerializeField] private Vector2 _minRangePosition;

    private void Awake()
    {
        GenerateRandomPosition();
    }

    public void Show()
    {
        _portal.gameObject.SetActive(true);
    }

    private void GenerateRandomPosition()
    {
        float x = Random.RandomRange(_minRangePosition.x, _maxRangePosition.x);
        float y = Random.RandomRange(_minRangePosition.y, _maxRangePosition.y);

        _portal.transform.position = new Vector2(x, y);
    }
}
