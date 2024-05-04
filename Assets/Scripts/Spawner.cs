using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class Spawner : MonoBehaviour
{
    [SerializeField] Cube _prefabCube;
    [SerializeField] Floor _floor;
    [SerializeField] Collider _colliderSpawner;
    [SerializeField] private int _countSpawn;
    private ObjectPool<Cube> _cubePool;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            InstantiateCube,
            ActionOnGet, 
            ActionOnRelease, 
            Destroy, 
            false, 
            _countSpawn, 
            _countSpawn);
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, 0.001f);
    }

    public void OnEnable()
    {
        _floor.Collision += CollisionDetected;
    }

    private void OnDisable()
    {
        _floor.Collision -= CollisionDetected;
    }

    private void Spawn()
    {
        if (_cubePool.CountActive < _countSpawn)
            _cubePool.Get();
    }
    
    private void CollisionDetected(Cube cube)
    {
        if (cube.TryChangeColor(new Color(Random.value, Random.value, Random.value)))
            StartCoroutine(DespawnCube(cube));
    }
    
    private IEnumerator DespawnCube(Cube cube)
    {
        yield return new WaitForSeconds(cube.TimeLive);
        _cubePool.Release(cube);
    }
    
    private void ActionOnRelease(Cube cube)
    {
        cube.DisabledCube();
    }
    
    private void ActionOnGet(Cube cube)
    {
        cube.EnabledCube();
        cube.transform.position = RandomSpawnPoint();
    }
    
    private Cube InstantiateCube()
    {
        return Instantiate(_prefabCube, RandomSpawnPoint(), Quaternion.identity);
    }
    
    private Vector3 RandomSpawnPoint()
    {
        return new Vector3(
            Random.Range(_colliderSpawner.bounds.min.x, _colliderSpawner.bounds.max.x), 
            Random.Range(_colliderSpawner.bounds.min.y, _colliderSpawner.bounds.max.y), 
            Random.Range(_colliderSpawner.bounds.min.z, _colliderSpawner.bounds.max.z));
    }
}
