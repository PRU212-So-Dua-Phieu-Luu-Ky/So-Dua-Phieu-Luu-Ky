//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DamageTextManager : MonoBehaviour
//{
//    [Header(" Elements ")]
//    [SerializeField] private DamageText damageTextPrefab;

//    private void Start()
//    {
//    }

//    private void Update()
//    {
//    }

//    [NaughtyAttributes.Button]
//    private void InstantiateDamageText()
//    {
//        Vector3 spawnPosition = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(1f, 5f);
//        var damageTextInstance = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, transform);
//        damageTextInstance.Configure("13");
//    }
//}