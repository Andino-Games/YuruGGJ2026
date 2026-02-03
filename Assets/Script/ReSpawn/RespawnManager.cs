using System.Collections;
using Script.UI;
using UnityEngine;

namespace Script.ReSpawn
{
    public class RespawnManager : MonoBehaviour
    {
        public static RespawnManager Instance;

        [Header("Referencias")]
        [SerializeField] private ScreenFader screenFader;
        
        private Transform _currentSpawnPoint;
        private GameObject _player;
        private Rigidbody2D _playerRb;
        private bool _isRespawning;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player) _playerRb = _player.GetComponent<Rigidbody2D>();
        }

        public void SetSpawnPoint(Transform newSpawnPoint)
        {
            _currentSpawnPoint = newSpawnPoint;
            Debug.Log("Nuevo punto de respawn establecido: " + newSpawnPoint.name);
        }

        public void RespawnPlayer()
        {
            if (_isRespawning || !_currentSpawnPoint || !_player) return;
            
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            _isRespawning = true;

            // 1. Audio de Muerte/Caída (Opcional)
            if (AudioManager.Instance != null) AudioManager.Instance.Play("PlayerDeath");

            // 2. Congelar al jugador (Opcional: Desactivar controles aquí si tuvieras referencia)
            if (_playerRb) _playerRb.linearVelocity = Vector2.zero; // Detener caída
            // _playerRb.simulated = false; // Opcional: Evitar físicas durante el fade

            // 3. Fade Out (Pantalla a Negro)
            if (screenFader) yield return screenFader.FadeOut();
            else yield return new WaitForSeconds(0.5f); // Espera de seguridad si no hay fader

            // 4. Mover al jugador
            _player.transform.position = _currentSpawnPoint.position;
            if (_playerRb) _playerRb.linearVelocity = Vector2.zero; // Asegurar velocidad cero de nuevo

            // 5. Audio de Respawn
            if (AudioManager.Instance != null) AudioManager.Instance.Play("PlayerRespawn");

            // 6. Pequeña pausa en negro
            yield return new WaitForSeconds(0.2f);

            // 7. Fade In (Pantalla visible)
            if (screenFader) yield return screenFader.FadeIn();

            // _playerRb.simulated = true;
            _isRespawning = false;
        }
    }
}