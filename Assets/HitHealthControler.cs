using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HitHealthController : MonoBehaviour
{
    public Image fillImage;
    private float maxHealth = 100f;
    private float redHealth = 100f;
    private float currentHealth = 100f;

    void Start()
    {
        redHealth = maxHealth;
        currentHealth = maxHealth;
        UpdateHealthController();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
    public void Reducing()
    {
        Debug.Log("123");
        redHealth -= 0.03f;
        redHealth = Mathf.Clamp(redHealth, 0f, maxHealth);
        UpdateHealthController();
    }

    private void UpdateHealthController()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = redHealth / maxHealth;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(5f);
            Debug.Log(currentHealth);
            Debug.Log(redHealth);
        }
        if (redHealth > currentHealth)
        {

            Reducing();
        }

    }

}
