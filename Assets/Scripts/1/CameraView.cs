using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class CameraView : MonoBehaviour
{
    [SerializeField] private List<Wall> _walls;

    private Camera _camera;
    private bool _isFinal = false;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        StartCoroutine(WaitSetupSize());
    }

    private void Update()
    {
        if (_walls.Count > 0)
            return;

        _walls = FindObjectsOfType<Wall>().ToList();
    }

    private IEnumerator WaitSetupSize()
    {
        yield return new WaitUntil(() => _walls.Count > 0);

        while (IsAllWallVisible() == false)
            _camera.orthographicSize += 1;

        _camera.orthographicSize += 2;
    }

    private bool IsAllWallVisible()
    {
        return _walls.All(wall => GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), wall.GetComponent<Collider2D>().bounds));
    }
}
