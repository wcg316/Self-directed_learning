using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HitHealthController : MonoBehaviour
{
    public Image fillImage;
    private float maxHealth = 100f;
    private float copyCurrentHealth = 100f;
    private float currentHealth = 100f;

    void Start()
    {
        copyCurrentHealth = maxHealth;
        currentHealth = maxHealth;
        UpdateHealthController();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(copyCurrentHealth, 0f, maxHealth);
    }
    public void Reducing()
    {
        copyCurrentHealth -= 1f;
        copyCurrentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthController();
    }

    private void UpdateHealthController()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = copyCurrentHealth / maxHealth;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(5f);
        }
        if (copyCurrentHealth > currentHealth)
        {
            Reducing();
        }
    }

}
