using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawn : MonoBehaviour
{

    [SerializeField] GameObject[] BirdPrefab; // Bird ������Ʈ�� ���� GameObject �迭

    [SerializeField] GameObject destination; // ������ ��̵��� ��������

    public float distanceFromStart = 0.0f; // ���� ������ �������� �󸶳� ������.
    public float distanceFromEnd = 0.0f; // ���� ������ �������� �󸶳� ������.


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

        if (bird.transform.position.x < 0)
        {
            bird.GetComponent<SpriteRenderer>().flipX = true;
        }

        bird.GetComponent<Bird>().CurvePointInit(this.transform, destination.transform, distanceFromStart, distanceFromEnd);

        return bird;
    }
}
