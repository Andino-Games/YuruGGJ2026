using UnityEngine;
using Script.UI; // Importante: Necesario para ver GameColor

namespace Script.PowerUps
{
    public class MaskPickup : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private ColorCapabilityState colorCapabilities;
        [SerializeField] private GameColor colorToUnlock;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            // Desbloquear el color
            colorCapabilities.UnlockColor(colorToUnlock);

            // Feedback de Audio (Usamos el sistema existente)
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play("KeyPickup"); // Reutilizamos el sonido de pickup o puedes crear "MaskPickup"
            }

            // Destruir el objeto
            Destroy(gameObject);
        }
    }
}