using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturnToPosition : MonoBehaviour
{
    [SerializeField] private float returnTime;
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position; // 처음 카메라 위치 저장
    }

    public void StartReturnToPosition()
    {
        StartCoroutine(ReturnToPositionCoroutine());
    }

    IEnumerator ReturnToPositionCoroutine()
    {
        float duration = 0.0f;
        Vector3 initPosition = transform.position; // 이동한 카메라 위치 저장

        while (true)
        {
            float lerp = Mathf.Min(duration, returnTime) / returnTime; // duration 값을 0~1로 보간
            // Mathf.Min(a, b) : a와 b 중 더 작은 값 반환
            Vector3 newPos = Vector3.Slerp(initPosition, startPosition, lerp); // Slerp : 구면 선형 보간
            // 이동한 카메라 위치로부터 처음 카메라 위치까지 부드럽게 보간
            transform.position = newPos; // 보간한 위치로 이동

            if (lerp >= 1.0f) // lerp가 1이면 처음 위치로 이동을 완료한 것이니 반복 탈출
                break;

            duration += Time.deltaTime;
            
            yield return new LateUpdate(); // LateUpdate까지 정지 후 재개
        }

        transform.position = startPosition;
        // Slerp를 하면 미세한 오차(ex. 1.00001)가 발생할 수 있으므로 다시 startPosition으로 위치 재설정
    }
}
