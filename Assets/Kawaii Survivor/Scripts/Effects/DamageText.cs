using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshPro damageText;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    [NaughtyAttributes.Button]
    public void Animate(string damage, bool isCriticalHit)
    {
        damageText.text = damage;
        damageText.color = isCriticalHit ? Color.red : Color.white;
        animator.Play("Damage Text Animation");
    }
}