using UnityEngine;
using TMPro;

public class ContainerController : MonoBehaviour
{
    [Header("�������� ���������")]
    public string targetColor;
    public TextMeshProUGUI counterText;

    [Header("���������� ������")]
    public Material highlightMaterial; // �������� ��� ���������
    public ParticleSystem successParticles; // ������� ������

    [Header("�����")]
    public AudioClip successSound; // ���� ��� ������
    public float soundVolume = 1f; // ��������� �����

    [Header("��������")]
    public Animator counterAnimator; // �������� ��� ������
    public string animationTrigger = "Pop"; // �������� �������� ��������

    private Material originalMaterial;
    private Renderer containerRenderer;
    private AudioSource audioSource;
    private int count = 0;

    public CrystalSpawner crystalSpawner;

    private void Start()
    {
        // ������������� �����������
        containerRenderer = GetComponent<Renderer>();
        originalMaterial = containerRenderer.material;
        audioSource = GetComponent<AudioSource>();

        // �������������� �������� AudioSource ���� �����������
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
            // ���������� ������
            StartCoroutine(HighlightEffect());

            // ����� ������
            PlaySuccessSound();

            // �������
            PlayParticles();

            // �������� ������
            TriggerCounterAnimation();

            // ���������� ��������
            UpdateCounter();

            // ����������� ���������
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