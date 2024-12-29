using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlingShot : MonoBehaviour
{
    private Camera MainCamera;

    [SerializeField] private LineRenderer[] lineRenderers;
    [SerializeField] private Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;

    public float maxDistance;
    public float bottomBoundary;
    
    private bool isMouseDown;

    public GameObject birdPrefab;

    private Rigidbody2D bird;
    private Collider2D birdCollider;

    public float birdPositionOffset;

    public float force;
    
    public GameObject trajectory;
    private List<GameObject> trajectoryObjects = new List<GameObject>();
    public int maxStep = 20; // 궤적의 최대 점 수 --> 작을수록 궤적이 짧아진다
    public float timeStep = 0.1f; // 시간 간격 --> 작을수록 점이 촘촘해진다
    
    void Awake()
    {
        MainCamera = Camera.main;
    }

    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateBird();
    }

    void PredictTrajectory(Vector3 force)
    {
        ClearTrajectory();

        Vector3 position = currentPosition;
        Vector3 velocity = force / bird.mass; // 물체의 초기 속도 (F = ma --> v = F/m)

        for (int i = 0; i < maxStep; i++)
        {
            float timeElapsed = timeStep * i; // timeElapsed : 현재까지 경과 시간
            Vector3 gravity = Physics.gravity;
            // 등가속도 운동 --> 각 시간 간격마다 계산된 물체의 위치를 리스트에 추가
            Vector3 trajectoryPoint = position +
                                      velocity * timeElapsed +
                                      gravity * (0.5f * timeElapsed * timeElapsed);
            
            var trajectoryObject = Instantiate(trajectory, trajectoryPoint, Quaternion.identity);
            trajectoryObjects.Add(trajectoryObject);
        }
    }

    private void ClearTrajectory()
    {
        foreach (var obj in trajectoryObjects)
        {
            Destroy(obj);
        }
        trajectoryObjects.Clear();
    }

    void CreateBird()
    {
        bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        birdCollider = bird.GetComponent<Collider2D>();
        birdCollider.enabled = false;

        bird.isKinematic = true;
        
        ResetStrips();
    }

    void Update()
    {
        if (isMouseDown && MainCamera != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            currentPosition = MainCamera.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxDistance);

            currentPosition = ClampBoundary(currentPosition);
            
            SetStrips(currentPosition);

            if (birdCollider) // 안정성 --> Shoot()에서 null로 설정했기 때문에
            {
                birdCollider.enabled = true; // bird를 마우스로 당길 때, Collider가 필요
            }

            Vector3 birdForce = (currentPosition - center.position) * (force * -1);
            PredictTrajectory(birdForce);
        }
        else
        {
            ResetStrips();
            ClearTrajectory();
        }
    }
    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        Shoot();
        
    }

    private void Shoot()
    {
        bird.isKinematic = false;
        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        bird.velocity = birdForce;
        
        // 새가 발사된 이후엔 더 이상 참조가 필요하지 않으므로 이전 참조를 초기화
        bird = null;
        birdCollider = null;
        Invoke("CreateBird", 2);
    }

    public void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    public void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
    }
    
    private Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
}
