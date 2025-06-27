using Mono.Cecil.Cil;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HitHealthController : MonoBehaviour
{
    public Image fillImage;
    private float maxHealth = 100f;
    private float redHealth = 100f;
    private float currentHealth = 100f;
    private float healthToReduce = 0f;
    private bool damageTakingDone = true;
    private PlayerController playerController;
    private InputManager inputManager;

    void Start()
    {
        redHealth = maxHealth;
        currentHealth = maxHealth;
        playerController = PlayerController.Instance;
        inputManager = InputManager.Instance;
        playerController.stabbed += TakeDamage;
        inputManager.OnPlayerHurt += TakeDamage;
        UpdateHealthController();
    }

    public void TakeDamage(float damage)
    {
        if (damageTakingDone)
        {
            healthToReduce = damage;
            damageTakingDone = false;
        }
        else
        {
            healthToReduce = Mathf.Max(healthToReduce, damage);
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
    public void Reducing()
    {
        redHealth -= healthToReduce * 0.6f;
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
        if (redHealth > currentHealth)
        {
            Reducing();
        }
        else
        {
            damageTakingDone = true;
        }
    }

}
