using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawn : MonoBehaviour
{

    [SerializeField] GameObject[] BirdPrefab; // Bird 오브젝트를 담을 GameObject 배열

    [SerializeField] GameObject destination; // 베지어 곡선이동의 도착지점

    public float distanceFromStart = 0.0f; // 시작 지점을 기준으로 얼마나 꺾일지.
    public float distanceFromEnd = 0.0f; // 도착 지점을 기준으로 얼마나 꺾일지.


    public int randomNumber;
    private float spawnTime;

    void Start()
    {
        spawnTime = 2;
        randomNumber = Random.Range(0, 2);

        distanceFromStart = Random.Range(0.0f, 5.0f);
        distanceFromEnd = Random.Range(0.0f, 5.0f);

    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            CreateBird();
            spawnTime = Random.Range(2.0f, 4.0f);
            randomNumber = Random.Range(0, 2);

            distanceFromStart = Random.Range(0.0f, 5.0f);
            distanceFromEnd = Random.Range(0.0f, 5.0f);
        }
    }

    public GameObject CreateBird()
    {
        GameObject bird;
        bird = Instantiate(BirdPrefab[randomNumber], transform.position, transform.rotation);
        bird.GetComponent<Bird>().CurvePointInit(this.transform, destination.transform, distanceFromStart, distanceFromEnd);

        return bird;
    }
}
