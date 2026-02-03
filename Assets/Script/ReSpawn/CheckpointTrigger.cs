using UnityEngine;

namespace Script.ReSpawn
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckpointTrigger : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Arrastra aquí el objeto SpawnPoint que corresponde a este nivel/zona")]
        [SerializeField] private SpawnPoint targetSpawnPoint;

        [Tooltip("Si es true, suena un efecto al activar el checkpoint")]
        [SerializeField] private bool playSound = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (targetSpawnPoint)
                {
                    // Activamos el nuevo punto de respawn
                    targetSpawnPoint.ActivateSpawnPoint();
                    
                    if (playSound && AudioManager.Instance != null)
                    {
                        
                    }
                    
                    Debug.Log($"Checkpoint activado: {targetSpawnPoint.name}");
                }
            }
        }

        private void OnDrawGizmos()
        {
            // Dibujar caja verde transparente para verlo en el editor
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position, transform.localScale);
            
            // Dibujar línea hacia el spawn point asociado
            if (targetSpawnPoint)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, targetSpawnPoint.transform.position);
            }
        }
    }
}