using UnityEngine;
using UnityEngine.UI;

public class ObjectData : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public Image heathBar;

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        if (heathBar != null)
            heathBar.fillAmount = (float)health / maxHealth;
    }
}
