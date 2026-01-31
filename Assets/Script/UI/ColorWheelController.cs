using UnityEngine;
using UnityEngine.InputSystem;

public class ColorWheelController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ColorEventChannel _colorChannel; // Tu ScriptableObject
    [SerializeField] private GameObject _wheelVisuals; // El objeto padre de la UI (Círculo)
    
    [Header("Configuración de Colores")]
    // Asigna aquí los colores que corresponden a cada tercio del círculo
    // Orden sugerido: Arriba, Derecha-Abajo, Izquierda-Abajo (depende de tu arte)
    [SerializeField] private GameColor _colorTop;   
    [SerializeField] private GameColor _colorRight; 
    [SerializeField] private GameColor _colorLeft;

    private GameControls _controls;
    private Vector2 _screenPosition;
    private bool _isSelecting;

    private void Awake()
    {
        _controls = new GameControls();
        _wheelVisuals.SetActive(false); // Empezar oculto
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
        // Suscribirse a los eventos del Input System
        _controls.Gameplay.OpenColorWheel.started += OnClickStarted;
        _controls.Gameplay.OpenColorWheel.canceled += OnClickReleased;
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
            ProcessSelection();
        }
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        _isSelecting = true;
        
        // 1. Obtener posición del mouse
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        
        // 2. Mover la UI a esa posición
        _wheelVisuals.transform.position = mousePos;
        _wheelVisuals.SetActive(true);
        
        // Opcional: Ocultar el cursor del sistema si usas uno personalizado
        // Cursor.visible = false; 
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        _isSelecting = false;
        _wheelVisuals.SetActive(false);
        // Cursor.visible = true;

        // 3. Confirmar la selección final
        GameColor selected = CalculateColorFromMouse();
        if (selected != GameColor.None)
        {
            _colorChannel.RaiseColorChanged(selected);
            Debug.Log($"Color seleccionado: {selected}");
        }
    }

    private void ProcessSelection()
    {
        // Aquí podrías añadir feedback visual (iluminar la sección seleccionada)
        // GameColor hoveredColor = CalculateColorFromMouse();
        // HighlightSection(hoveredColor);
    }

    private GameColor CalculateColorFromMouse()
    {
        // Vector desde el centro de la rueda hasta el mouse
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        Vector2 center = _wheelVisuals.transform.position;
        Vector2 direction = mousePos - center;

        // Evitar cálculos si estamos muy cerca del centro (zona muerta)
        if (direction.magnitude < 10f) return GameColor.None;

        // Calcular ángulo en grados (0 a 360)
        // Atan2 devuelve radianes, convertimos a grados.
        // El +90 es para ajustar el 0 al norte (arriba), dependiendo de tu sprite.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        // Lógica de cuadrantes (ajustar según tu imagen)
        // Ejemplo: 
        // 90° es Arriba.
        // 330° a 210° es Abajo-Derecha aprox.
        // 210° a 150° es Abajo-Izquierda aprox.
        
        // Simplificación: Dividir 360 en 3 partes de 120 grados
        // 0-120, 120-240, 240-360 (Ajusta estos rangos según tu arte exacto)
        
        if (angle >= 45 && angle < 135) return _colorTop;       // Zona Superior
        if (angle >= 135 && angle < 225) return _colorLeft;     // Zona Izquierda
        else return _colorRight;                                // Zona Derecha
    }
}