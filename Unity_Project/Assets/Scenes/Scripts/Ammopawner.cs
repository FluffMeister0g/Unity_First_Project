using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    public bool canSpawnAmmo = false;
    public float spawnTime = 60f;
    public int ammoCount = 0;
    public int ammoSpawn = 0;
    public Transform[] ammospawnLocations;
    public GameObject healthpickupPrefab;
    public List<GameObject> spawnedAmmo;
    private float spawnedAt;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Spawning()
    {
        while (canSpawnAmmo)
        {
            yield return new WaitForSeconds(spawnTime);
            for (int i = 0; i < ammoCount; i++)
            {
                GameObject obj = Instantiate(healthpickupPrefab, ammospawnLocations[Random.Range(0, ammospawnLocations.Length)].position, Quaternion.identity);
                spawnedAmmo.Add(obj);
            }
            spawnedAt = Time.time;
            spawnTime = 5f;
            yield return new WaitUntil(() => spawnedAmmo.TrueForAll((GameObject obj) => obj == null));
            spawnedAmmo.Clear();
        }
    }






}