using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private Camera mainCamera;
    
    public ParticleSystem attackEffect;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_rigidbody != null)
        {
            //_rigidbody.drag = 1.0f;
        }
        Destroy(gameObject, 3.0f);
        
        var effectObject = Instantiate(attackEffect, transform.position, quaternion.identity);
        effectObject.Play();
    }

    private void LateUpdate() // Bird가 날아갈 때 카메라 이동
    {
        if (transform.position.x > mainCamera.transform.position.x)
        {
            var vector3 = mainCamera.transform.position;
            vector3.x = transform.position.x;
            mainCamera.transform.position = vector3;
        }
    }

    private void OnDestroy() // Bird가 사라지면 다시 카메라 원위치
    {
        if (!mainCamera.IsUnityNull())
            mainCamera.GetComponent<ReturnToPosition>().StartReturnToPosition();
        // Destroy할 때, UnityEngine에서는 null이지만 System에서는 null이 아니게 되는 Fake null 현상을 대비
    }
}
