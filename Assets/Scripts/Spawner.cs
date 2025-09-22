using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private float _spawnOffset = 4f;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_cubePrefab),
        actionOnGet: (cube) => InitializeCube(cube),
        actionOnRelease: (cube) => ReleaseCube(cube),
        actionOnDestroy: (cube) => DestroyCube(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCube());
    }

    private void InitializeCube(Cube cube)
    {
        cube.Collided += HandleCollision;
        cube.transform.position = GetRandomPoint();
        cube.gameObject.SetActive(true);
    }

    private void ReleaseCube(Cube cube)
    {
        cube.ResetVelocity();
        cube.Collided -= HandleCollision;
        cube.gameObject.SetActive(false);
    }

    private void DestroyCube(Cube cube)
    {
        cube.Collided -= HandleCollision;
        Destroy(cube.gameObject);
    }

    private IEnumerator SpawnCube()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            _pool.Get();

            yield return wait;
        }
    }

    private Vector3 GetRandomPoint()
    {
        float positionX = _startPoint.x + Random.Range(-_spawnOffset, _spawnOffset);
        float positionZ = _startPoint.z + Random.Range(-_spawnOffset, _spawnOffset);

        return new Vector3(positionX, _startPoint.y, positionZ);
    }

    private void HandleCollision(Cube cube)
    {
        float lifetime = Random.Range(_minLifetime, _maxLifetime);

        cube.SetEventColor();
        cube.StartLifetime(lifetime, _pool.Release);
    }
}
