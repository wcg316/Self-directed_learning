using UnityEngine;

public class HealthController
{
    int maxHealth;
    int currentHealth;

    public void Initialize(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }
}
