using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthController : MonoBehaviour
{
    public Image fillImage;
    private float maxHealth = 100;
    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        UpdateUIHealthController();
    }
    public void TakeDamage(float damge)
    {
        currentHealth -= damge;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateUIHealthController();
    }
    private void UpdateUIHealthController()
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}
