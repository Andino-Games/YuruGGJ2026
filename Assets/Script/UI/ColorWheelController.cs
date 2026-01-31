using UnityEngine;
using UnityEngine.InputSystem;

public class ColorWheelController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameObject _wheelVisuals;

    [Header("Configuración de Colores")]
    [SerializeField] private GameColor _colorTop;
    [SerializeField] private GameColor _colorRight;
    [SerializeField] private GameColor _colorLeft;

    private GameControls _controls;
    private bool _isSelecting;

    private void Awake()
    {
        _controls = new GameControls();
        
        // Verificación de seguridad
        if (_wheelVisuals == null) Debug.LogError("¡ERROR! No has asignado '_wheelVisuals' en el inspector.");
        else _wheelVisuals.SetActive(false);
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
        // Suscripción
        _controls.Gameplay.OpenColorWheel.started += OnClickStarted;
        _controls.Gameplay.OpenColorWheel.canceled += OnClickReleased;
        Debug.Log("Sistema de Input Activado y Escuchando...");
    }

    private void OnDisable()
    {
        _controls.Gameplay.OpenColorWheel.started -= OnClickStarted;
        _controls.Gameplay.OpenColorWheel.canceled -= OnClickReleased;
        _controls.Gameplay.Disable();
    }

    private void Update()
    {
        if (_isSelecting)
        {
            // Opcional: Para ver en tiempo real coordenadas del mouse
            // Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
            // Debug.Log($"Mouse en: {mousePos}"); 
        }
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        Debug.Log(">>> CLIC DETECTADO: Iniciando selección <<<"); // ESTO DEBE SALIR AL CLICAR
        
        _isSelecting = true;
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        
        if (_wheelVisuals != null)
        {
            _wheelVisuals.transform.position = mousePos;
            _wheelVisuals.SetActive(true);
        }
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        Debug.Log(">>> CLIC SOLTADO: Finalizando selección <<<");
        
        _isSelecting = false;
        if (_wheelVisuals != null) _wheelVisuals.SetActive(false);

        GameColor selected = CalculateColorFromMouse();
        
        if (selected != GameColor.None)
        {
            Debug.Log($"Color Enviado al Evento: {selected}");
            _colorChannel.RaiseColorChanged(selected);
        }
        else
        {
            Debug.Log("Ningún color seleccionado (Zona muerta o centro)");
        }
    }

    private GameColor CalculateColorFromMouse()
    {
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        Vector2 center = _wheelVisuals.transform.position;
        Vector2 direction = mousePos - center;

        if (direction.magnitude < 10f) return GameColor.None;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        Debug.Log($"Ángulo calculado: {angle}"); // DEBUG DEL ÁNGULO

        if (angle >= 45 && angle < 135) return _colorTop;
        if (angle >= 135 && angle < 225) return _colorLeft;
        else return _colorRight;
    }
}