using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 0.5f;

        private void Awake()
        {
            if (fadeImage)
            {
                fadeImage.gameObject.SetActive(true);
                // empieza transparente.
                Color c = fadeImage.color;
                c.a = 0f;
                fadeImage.color = c;
                fadeImage.raycastTarget = false; // Para no bloquear clicks cuando es transparente
            }
        }

        public IEnumerator FadeOut()
        {
            if (!fadeImage) yield break;
            
            fadeImage.raycastTarget = true; // Bloquear clicks
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                SetAlpha(alpha);
                yield return null;
            }
            SetAlpha(1f);
        }

        public IEnumerator FadeIn()
        {
            if (!fadeImage) yield break;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
                SetAlpha(alpha);
                yield return null;
            }
            SetAlpha(0f);
            fadeImage.raycastTarget = false; // Desbloquear clicks
        }

        private void SetAlpha(float alpha)
        {
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}