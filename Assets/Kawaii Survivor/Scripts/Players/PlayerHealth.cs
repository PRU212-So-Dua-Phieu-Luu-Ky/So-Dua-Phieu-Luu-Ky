using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth;

    private int health;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;

    [SerializeField]
    private TextMeshProUGUI healthText;

    private void Start()
    {
        health = maxHealth;

        healthSlider.value = 1;

        UpdateUI();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= damage;

        UpdateUI();

        if (health <= 0)
        {
            PassAway();
        }
    }

    private void UpdateUI()
    {
        healthSlider.value = (float)health / maxHealth;
        healthText.text = health + " / " + maxHealth;
    }

    private void PassAway()
    {
        SceneManager.LoadScene(0);
    }
}