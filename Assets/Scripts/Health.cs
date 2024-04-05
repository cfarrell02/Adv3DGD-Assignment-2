using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public bool isPlayer = false;

    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isPlayer = gameObject.CompareTag("Player");
        
        if (isPlayer)
        {
            int health = PlayerPrefs.GetInt("health", maxHealth);
            currentHealth = health != 0 ? health : maxHealth;
        }

        
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

        var ddManager = GameObject.FindObjectOfType<DynamicDifficultyManager>();
        if(ddManager != null)
        {
            if (isPlayer)
                ddManager.userSkillLevel--;
            else
                ddManager.userSkillLevel++;
            
        }

    }
    
    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
