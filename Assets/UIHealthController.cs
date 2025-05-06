using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : MonoBehaviour
{
    public Image fillImage;
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    private InputManager inputManager;

    void Start()
    {
        currentHealth = maxHealth;
        inputManager = InputManager.Instance;
        inputManager.OnPlayerHurt += TakeDamage;
        UpdateUIHealthController();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateUIHealthController();
    }

    private void UpdateUIHealthController()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.H))
    //     {
    //         TakeDamage(50f);
    //     }
    // }

}
