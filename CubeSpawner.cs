using System.Collections;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float spawnInterval = 2f;
    public float moveSpeed = 3f;
    public float spawnHeight = 5f;
    public float spawnRange = 10f;

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        while (true)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRange, spawnRange), spawnHeight, Random.Range(-spawnRange, spawnRange));
            GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(MoveCube(cube));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator MoveCube(GameObject cube)
    {
        Vector3 targetPosition = Vector3.zero; // The position where the player is (could be the center)
        while (cube != null && cube.transform.position != targetPosition)
        {
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(cube); // Destroy the cube if it reaches the player
    }
}