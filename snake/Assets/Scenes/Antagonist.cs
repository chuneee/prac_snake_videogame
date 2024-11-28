using UnityEngine;

public class Antagonist : MonoBehaviour
{
    public BoxCollider2D gridArea; // Área en la que puede moverse el antagonista
    private Vector3 targetPosition;
    public float moveSpeed = 2f; // Velocidad de movimiento

    private void Start() {
        SetRandomPosition();
    }

    private void Update() {
        // Moverse hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Si llega a la posición objetivo, establece una nueva posición aleatoria
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            SetRandomPosition();
        }
    }

    private void SetRandomPosition() {
        Bounds bounds = gridArea.bounds;
        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
        targetPosition = new Vector3(x, y, 0.0f);
    }
}
