using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game Architecture/Color Event Channel")]
public class ColorEventChannel : ScriptableObject
{
    // El color que está activo actualmente en el mundo
    public GameColor CurrentColor { get; private set; }

    // El evento al que se suscribirán los obstáculos y el fondo
    // Action es un delegado de C# optimizado para eventos
    public event Action<GameColor> OnColorChanged;

    // Método público para disparar el cambio
    public void RaiseColorChanged(GameColor newColor)
    {
        if (CurrentColor == newColor) return; // Evitar repeticiones innecesarias

        CurrentColor = newColor;
        
        // El signo '?' verifica si hay alguien escuchando antes de invocar
        OnColorChanged?.Invoke(newColor);
    }
    
    // Útil para reiniciar el estado al comenzar el juego
    public void ResetState()
    {
        CurrentColor = GameColor.None;
    }
}