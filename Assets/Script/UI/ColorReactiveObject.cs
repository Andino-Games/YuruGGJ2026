using UnityEngine;

public class ColorReactiveObject : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí tu ColorEventChannel creado en la carpeta de assets")]
    [SerializeField] private ColorEventChannel _colorChannel;

    [Tooltip("El color al que pertenece este obstáculo")]
    [SerializeField] private GameColor _objectColor;

    private void OnEnable()
    {
        if (_colorChannel != null)
        {
            // Nos suscribimos al evento
            _colorChannel.OnColorChanged += HandleColorChanged;
            
            // Inicializamos el estado actual por si el objeto se activa tarde
            HandleColorChanged(_colorChannel.CurrentColor); 
        }
    }

    private void OnDisable()
    {
        // IMPORTANTE: Siempre desuscribirse para evitar errores de memoria (Memory Leaks)
        if (_colorChannel != null)
        {
            _colorChannel.OnColorChanged -= HandleColorChanged;
        }
    }

    private void HandleColorChanged(GameColor newWorldColor)
    {
        // Lógica solicitada:
        // Si el color del mundo COINCIDE con mi color -> Desaparezco (false)
        // Si el color del mundo es DIFERENTE -> Aparezco (true)
        bool shouldBeActive = newWorldColor != _objectColor;

        // Podríamos usar una animación aquí en el futuro, 
        // por ahora desactivamos el objeto completo (física + visual)
        gameObject.SetActive(shouldBeActive);
    }
}