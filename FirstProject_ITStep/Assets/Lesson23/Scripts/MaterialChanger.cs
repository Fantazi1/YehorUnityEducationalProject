using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Components")]
    [Tooltip("Компонент Renderer вашої 3D моделі (MeshRenderer або SkinnedMeshRenderer)")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Health Materials")]
    [Tooltip("Матеріал для 76% - 100% здоров'я (Ідеальний стан)")]
    [SerializeField] private Material fullHealthMaterial;

    [Tooltip("Матеріал для 51% - 75% здоров'я (Легкі пошкодження)")]
    [SerializeField] private Material goodHealthMaterial;

    [Tooltip("Матеріал для 26% - 50% здоров'я (Середні пошкодження)")]
    [SerializeField] private Material damagedHealthMaterial;

    [Tooltip("Матеріал для 0% - 25% здоров'я (Критичний стан)")]
    [SerializeField] private Material criticalHealthMaterial;

    void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        currentHealth = maxHealth;
        UpdateHealthVisuals();
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthVisuals();
    }

    private void UpdateHealthVisuals()
    {
        if (targetRenderer == null)
        {
            Debug.LogError("Renderer не знайдено! Призначте його в інспекторі.", this);
            return;
        }

        float healthPercentage = currentHealth / maxHealth;

        Material selectedMaterial;

        if (healthPercentage > 0.75f)     // 76% - 100%
        {
            selectedMaterial = fullHealthMaterial;
        }
        else if (healthPercentage > 0.50f) // 51% - 75%
        {
            selectedMaterial = goodHealthMaterial;
        }
        else if (healthPercentage > 0.25f) // 26% - 50%
        {
            selectedMaterial = damagedHealthMaterial;
        }
        else                               // 0% - 25%
        {
            selectedMaterial = criticalHealthMaterial;
        }

        if (selectedMaterial != null)
        {
            targetRenderer.material = selectedMaterial;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHealth(-15f); // Шкода
            Debug.Log($"Поточне здоров'я: {currentHealth} ({currentHealth / maxHealth * 100f}%)");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeHealth(15f); // Лікування
            Debug.Log($"Поточне здоров'я: {currentHealth} ({currentHealth / maxHealth * 100f}%)");
        }
    }
}
