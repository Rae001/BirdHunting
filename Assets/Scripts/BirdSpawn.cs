using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawn : MonoBehaviour
{

    [SerializeField] GameObject[] BirdPrefab; // Bird ������Ʈ�� ���� GameObject �迭

    [SerializeField] GameObject destination; // ������ ��̵��� ��������

    public float distanceFromStart = 5.0f; // ��� �������κ��� �Ÿ�
    public float distanceFromEnd = 5.0f; // ���� �������κ��� �Ÿ�


    public int randomNumber;
    private float spawnTime;

    void Start()
    {
        spawnTime = 2;
        randomNumber = Random.Range(0, 2);
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            CreateBird();
            spawnTime = Random.Range(2.0f, 4.0f);
            randomNumber = Random.Range(0, 2);
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
