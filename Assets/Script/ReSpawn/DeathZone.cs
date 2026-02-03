using UnityEngine;

namespace Script.ReSpawn
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (RespawnManager.Instance != null)
                {
                    RespawnManager.Instance.RespawnPlayer();
                }
            }
        }

        // Visualización en editor para ver dónde está la muerte
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Rojo semitransparente
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}