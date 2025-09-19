using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private float _spawnOffset = 4f;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(GameObject obj)
    {
        CollisionNotifier collision = obj.GetComponent<CollisionNotifier>();

        if (collision != null)
            collision.Collide += ActionCollision;

        obj.transform.position = GetRandomPoint();
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private IEnumerator DalayOnRelease(GameObject obj)
    {
        CollisionNotifier collision = obj.GetComponent<CollisionNotifier>();

        float wait = Random.Range(_minLifetime, _maxLifetime);

        yield return new WaitForSeconds(wait);

        if (collision != null)
            collision.Collide -= ActionCollision;

        _pool.Release(obj);
    }

    private Vector3 GetRandomPoint()
    {
        float positionX = _startPoint.x + Random.Range(-_spawnOffset, _spawnOffset);
        float positionZ = _startPoint.z + Random.Range(-_spawnOffset, _spawnOffset);

        return new Vector3(positionX, _startPoint.y, positionZ);
    }

    private void ActionCollision(GameObject obj)
    {
        SetColor(obj);
        StartCoroutine(DalayOnRelease(obj));
    }

    private void SetColor(GameObject obj)
    {
        ColorSetter colorSetter = obj.GetComponent<ColorSetter>();
        if (colorSetter != null)
            colorSetter.SetEventColor();
    }
}
