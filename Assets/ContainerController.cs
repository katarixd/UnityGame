using UnityEngine;
using TMPro;

public class ContainerController : MonoBehaviour
{
    [Header("Основные настройки")]
    public string targetColor;
    public TextMeshProUGUI counterText;

    [Header("Визуальный фидбек")]
    public Material highlightMaterial; // Материал для подсветки
    public ParticleSystem successParticles; // Система частиц

    [Header("Аудио")]
    public AudioClip successSound; // Звук при успехе
    public float soundVolume = 1f; // Громкость звука

    [Header("Анимация")]
    public Animator counterAnimator; // Аниматор для текста
    public string animationTrigger = "Pop"; // Название триггера анимации

    private Material originalMaterial;
    private Renderer containerRenderer;
    private AudioSource audioSource;
    private int count = 0;

    public CrystalSpawner crystalSpawner;

    private void Start()
    {
        // Инициализация компонентов
        containerRenderer = GetComponent<Renderer>();
        originalMaterial = containerRenderer.material;
        audioSource = GetComponent<AudioSource>();

        // Автоматическое создание AudioSource если отсутствует
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CrystalColor crystal = other.GetComponent<CrystalColor>();
        if (crystal != null && crystal.crystalColor == targetColor)
        {
            // Визуальный фидбек
            StartCoroutine(HighlightEffect());

            // Аудио фидбек
            PlaySuccessSound();

            // Частицы
            PlayParticles();

            // Анимация текста
            TriggerCounterAnimation();

            // Обновление счетчика
            UpdateCounter();

            // Уничтожение кристалла
            Destroy(other.gameObject);

            crystalSpawner.RemoveCrystal(other.gameObject);
        }
    }

    private System.Collections.IEnumerator HighlightEffect()
    {
        containerRenderer.material = highlightMaterial;
        yield return new WaitForSeconds(0.5f);
        containerRenderer.material = originalMaterial;
    }

    private void PlaySuccessSound()
    {
        if (successSound != null)
        {
            audioSource.PlayOneShot(successSound, soundVolume);
        }
    }

    private void PlayParticles()
    {
        if (successParticles != null)
        {
            successParticles.Play();
        }
    }

    private void TriggerCounterAnimation()
    {
        if (counterAnimator != null && !string.IsNullOrEmpty(animationTrigger))
        {
            counterAnimator.SetTrigger(animationTrigger);
        }
    }

    private void UpdateCounter()
    {
        count++;
        counterText.text = $"{targetColor}: {count}";
    }
}