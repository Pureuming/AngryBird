using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollisionMonster : MonoBehaviour
{
    private bool firstStructureHit = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!firstStructureHit)
        {
            if (other.gameObject.CompareTag("Structure"))
            {
                firstStructureHit = true;
                return; // 처음에는 firstStructureHit만 true로 바꾸고 멈춘다.
            }
        }
        
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Structure"))
        {
            GetComponentInParent<Monster>().hp--;
            if (gameObject != null)
                GetComponentInParent<Monster>().State?.Invoke();
        }
    }
}
