using UnityEngine;
using Lean.Pool;
using Cysharp.Threading.Tasks; // Ensure you have UniTask installed for delay handling

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] GameObject prefabToSpawn; // The prefab to spawn
    [SerializeField] float spawnDelay; // Delay between spawns

    private  bool isSpawning = true;

    
    public async void StartSpawning()
    {
  
        while (isSpawning)
        {
            SpawnObject();
            await UniTask.Delay(System.TimeSpan.FromSeconds(spawnDelay));
        }
    }

    public  void GO()
    {
        isSpawning = false;
    }

    private void SpawnObject()
    {
        // Spawn the object using Lean Pool at the spawner's position and rotation
        if (prefabToSpawn != null)
        {
           GameObject obj = LeanPool.Spawn(prefabToSpawn, transform.position, transform.rotation);
            LeanPool.Despawn(obj, 7.5f);
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned!");
        }
    }
}
