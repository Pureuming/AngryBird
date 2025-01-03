using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private static readonly int IsDamaged = Animator.StringToHash("IsDamaged"); // Parameters를 id화
    public ParticleSystem destroyEffect;
    
    public int maxHp, hp = 3;

    public Action State;

    private Animator animator;

    private AudioSource audioSource;
    public AudioClip destroyAudio;

    private void Start()
    {
        State += ChangeState;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void ChangeState()
    {
        if (maxHp > hp && hp > 1)
        {
            animator.SetInteger(IsDamaged, 1); // Animation Parameters 변경
        }
        else if (hp == 1)
        {
            animator.SetInteger(IsDamaged, 2); // Animation Parameters 변경
        }
        else if (hp <= 0)
        {
            audioSource.PlayOneShot(destroyAudio);
            Destroy(gameObject, destroyAudio.length); // audio가 재생되기도 전에 파괴되는 현상 방지
            State = null; // 몬스터가 삭제되고 나서 불러오면 에러가 발생하니 이를 미연에 방지
            var effectObject = Instantiate(destroyEffect, transform.position, quaternion.identity);
            effectObject.Play();
        }
    }
}
