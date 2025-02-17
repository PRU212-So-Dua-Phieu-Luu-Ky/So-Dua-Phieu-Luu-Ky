//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class DamageText : MonoBehaviour
//{
//    [Header(" Elements ")]
//    [SerializeField] private TextMeshPro text;

//    [SerializeField] private Animator animator;

//    [Header(" Movement ")]
//    [SerializeField] private float moveAmplitude;

//    [SerializeField] private float moveSpeed;

//    [NaughtyAttributes.Button]
//    private void Start()
//    {
//        Configure(Random.Range(1, 500).ToString());
//    }

//    public void Configure(string textString)
//    {
//        text.text = textString;
//        animator.Play("Damage Text Animation");
//    }
//}