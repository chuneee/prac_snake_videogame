using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject portalPowerUpPrefab; // Prefab del Power-Up
    public BoxCollider2D gridArea; // Área de aparición del Power-Up
    public float spawnInterval = 20f; // Tiempo entre cada aparición

    private void Start() {
        // Inicia el ciclo de generación del Power-Up
        InvokeRepeating(nameof(SpawnPowerUp), spawnInterval, spawnInterval);
    }

    private void SpawnPowerUp() {
        // Calcula una posición aleatoria dentro del GridArea
        Bounds bounds = gridArea.bounds;
        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        // Genera el Power-Up en la posición calculada
        Instantiate(portalPowerUpPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
    }
}
