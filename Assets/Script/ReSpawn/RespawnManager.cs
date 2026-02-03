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
            
            if (screenFader)
            {
                screenFader.gameObject.SetActive(false);
            }
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

            // 1. Activar el Fader
            if (screenFader) screenFader.gameObject.SetActive(true);

            // 2. Audio de Muerte/Caída
            if (AudioManager.Instance != null) AudioManager.Instance.Play("PlayerDeath");

            // 3. Congelar al jugador
            if (_playerRb) _playerRb.linearVelocity = Vector2.zero;

            // 4. Fade Out (Pantalla a Negro)
            if (screenFader) yield return screenFader.FadeOut();
            else yield return new WaitForSeconds(0.5f);

            // 5. Mover al jugador
            _player.transform.position = _currentSpawnPoint.position;
            if (_playerRb) _playerRb.linearVelocity = Vector2.zero;

            // 6. Audio de Respawn
            if (AudioManager.Instance != null) AudioManager.Instance.Play("PlayerRespawn");

            // 7. Pequeña pausa en negro
            yield return new WaitForSeconds(0.2f);

            // 8. Fade In (Pantalla visible)
            if (screenFader) yield return screenFader.FadeIn();

            // 9. Desactivar el Fader para limpiar el editor
            if (screenFader) screenFader.gameObject.SetActive(false);

            _isRespawning = false;
        }
    }
}