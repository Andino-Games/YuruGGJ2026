using UnityEngine;

namespace Script.ReSpawn
{
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Si es true, este punto se activa automáticamente al iniciar el juego")]
        [SerializeField] private bool isStartingPoint;

        private void Start()
        {
            if (isStartingPoint)
            {
                ActivateSpawnPoint();
            }
        }

        // Este método puede ser llamado por un Trigger invisible al entrar a una nueva zona
        public void ActivateSpawnPoint()
        {
            if (RespawnManager.Instance != null)
            {
                RespawnManager.Instance.SetSpawnPoint(transform);
            }
        }

        // Opcional: Para visualizarlo en el editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up);
        }
    }
}