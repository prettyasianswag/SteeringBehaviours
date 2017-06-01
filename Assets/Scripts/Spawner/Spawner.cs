using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab; // Prefab of object that we want to spawn
    public float spawnRate = 1f; // Spawn an object every spawn rate interval (in seconds)
    [HideInInspector] public List<GameObject> objects = new List<GameObject>(); // List of gameobjects

    private float spawnTimer = 0f; // Counts up every frame in seconds

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    // Generates a random point within the transform's scale
    Vector3 GenerateRandomPoint()
    {
        Vector3 scale;
        Vector3 halfScale;

        // SET halfscale to half of transform's scale
        scale = transform.localScale;
        halfScale = scale / 2f;

        // SET randomPoint to zero
        Vector3 randomPoint = Vector3.zero;

        // SET randomPoint x, y & z to Random Range betweeen - halfScale to halfScale (HINT: can do individually)
        randomPoint.x = Random.Range(-halfScale.x, halfScale.x);
        randomPoint.y = 0;
        randomPoint.z = Random.Range(- halfScale.z, halfScale.z);

        // RETURN randomPoint
        return randomPoint;
    }

    public void Spawn (Vector3 position, Quaternion rotation)
    {
        // SET clone to new instance of prefab
        GameObject clone = Instantiate(prefab);

        // ADD clone to objects list
        objects.Add(clone);

        // SET clone's position to spawner position + position
        clone.transform.position = transform.position + position;

        // SET clones rotation to rotation
        clone.transform.rotation = rotation;

    }

    // Update is called once per frame
    void Update()
    {
        // SET spawnTimer to spawnTimer + delta time
        spawnTimer = spawnTimer + Time.deltaTime;

        // IF spawnTimer > spawnRate
        if (spawnTimer > spawnRate)
        {
            // SET randomPoint to GenerateRandomPoint()
            Vector3 randomPoint = GenerateRandomPoint();

            // CALL Spawn() and pass GenerateRandomPoint(), Quaternion identity
            Spawn(randomPoint, Quaternion.identity);

            // SET spawnTimer to zero
            spawnTimer = 0f;
        }
    }
}
