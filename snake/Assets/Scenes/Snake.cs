using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private List<Transform> segments;
    public Transform segmentPreFab;

    public BoxCollider2D gridArea;
    public float portalDuration = 10f; // Duración de las paredes portales
    private bool arePortalsActive = false; // Controla si las paredes son portales

    private void Start() {
        segments = new List<Transform>();
        segments.Add(this.transform);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down) {
            direction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up) {
            direction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right) {
            direction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left) {
            direction = Vector2.right;
        }
    }

    private void FixedUpdate() {
        // Movimiento de la serpiente
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x + direction.x),
            Mathf.Round(this.transform.position.y + direction.y),
            0.0f
        );

        // Solo manejar portales si están activos
        if (arePortalsActive) {
            HandlePortals();
        }
    }

    private void Grow() {
        Transform segment = Instantiate(this.segmentPreFab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    private void ResetState() {
        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform);

        this.transform.position = Vector3.zero;
    }

private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Food")) {
        Grow();
    } else if (other.gameObject.CompareTag("Obstaculos") && !arePortalsActive) {
        // Las barreras solo causan daño cuando las paredes portales están desactivadas
        ResetState();
    } else if (other.gameObject.CompareTag("Antagonista")) {
        // Los antagonistas siempre causan daño
        Debug.Log("Chocaste con un antagonista. Game Over.");
        ResetState();
    } else if (other.gameObject.CompareTag("PortalPowerUp")) {
        Destroy(other.gameObject);
        StartCoroutine(ActivatePortals());
    }
}


    // Activa las paredes portales temporalmente
    private IEnumerator ActivatePortals() {
        arePortalsActive = true;
        Debug.Log("Paredes portales activadas.");
        yield return new WaitForSeconds(portalDuration);
        arePortalsActive = false;
        Debug.Log("Paredes portales desactivadas.");
    }

    // Maneja el comportamiento de las paredes como portales
    private void HandlePortals() {
        // Obtener los límites del GridArea
        Bounds bounds = gridArea.bounds; 
        Vector3 position = this.transform.position;

        // Verificar si la serpiente sale del GridArea
        if (position.x > bounds.max.x) {
            position.x = bounds.min.x; // Teletransportar al lado opuesto
        } else if (position.x < bounds.min.x) {
            position.x = bounds.max.x;
        }

        if (position.y > bounds.max.y) {
            position.y = bounds.min.y; // Teletransportar al lado opuesto
        } else if (position.y < bounds.min.y) {
            position.y = bounds.max.y;
        }

        // Actualizar la posición de la serpiente si se teletransporta
        this.transform.position = position;
    }

}
