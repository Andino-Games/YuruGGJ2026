using UnityEngine;

namespace Script.PowerUps.SecretKey
{
    public class SecretKeyPickup : MonoBehaviour
    {
        [SerializeField] private GameCapabilityState capabilityState;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            capabilityState.UnlockSecret();
            Destroy(gameObject);
        }
    }
}