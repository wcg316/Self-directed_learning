using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : MonoBehaviour
{
    public Transform bar;
    private float maxHealth = 100f;
    private float currentHealth = 100f;

    void Start()
    {
        currentHealth = maxHealth;
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
        float ratio = currentHealth / maxHealth;
        Vector3 scale = bar.localScale;
        scale.x = ratio;
        bar.localScale = scale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50f);
        }
    }

}
