using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    [SerializeField] GameObject blood;
    [SerializeField] AudioClip[] clip;
    AudioSource birdSound;


    Vector2[] curvePoint = new Vector2[4]; // Bird 오브젝트의 이동 지점 4곳을 담을 배열변수 선언

    float currentTime = 0;
    float moveTime = 0;
    float bloodCount = 5;
    float bloodSpeed = 5;
    bool isDie;

    void Start()
    {
        birdSound = GetComponent<AudioSource>();
        moveTime = Random.Range(1.0f, 10.0f);
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= moveTime)
        {
            currentTime = moveTime;
        }

        transform.position = new Vector2(
            BezierCurve(curvePoint[0].x, curvePoint[1].x, curvePoint[2].x, curvePoint[3].x),
            BezierCurve(curvePoint[0].y, curvePoint[1].y, curvePoint[2].y, curvePoint[3].y));

        DestroyBird();
    }

    public void CurvePointInit(Transform start, Transform end, float DistanceFromStart, float DistanceFromEnd)
    {


        curvePoint[0] = start.position;

        curvePoint[1] = start.position +
            (DistanceFromStart * start.right) +
            (DistanceFromStart * Random.Range(-5.0f, 5.0f) * start.up);

        curvePoint[2] = end.position +
            (DistanceFromEnd * end.right) +
            (DistanceFromEnd * Random.Range(-5.0f, 5.0f) * end.up);

        curvePoint[3] = end.position;
    }

    private float BezierCurve(float a, float b, float c, float d)
    {
        float t = currentTime / moveTime;

        float ab = Mathf.Lerp(a, b, t); // t가 변수가 되어 새들의 이동 속도를 조절할 수 있다.
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    public void DestroyBird() // Bird 오브젝트가 마지막 이동지점 위치에 도달할시 파괴하고 생성시간을 랜덤으로 초기화한다.
    {
        if (this.transform.position.x == curvePoint[3].x && this.transform.position.y == curvePoint[3].y)
        {
            Destroy(gameObject);
            currentTime = 0;
            moveTime = Random.Range(0.5f, 5.0f);
        }
        return;
    }

    public void BloodSplash()
    {
        for (int i = 0; i < bloodCount; i++)
        {
            GameObject b = Instantiate(blood, transform.position, transform.rotation);
            b.transform.localScale = new Vector2(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f));
            Rigidbody2D brb = b.GetComponent<Rigidbody2D>();
            Vector2 dir = transform.rotation * Vector2.up * Random.Range(-0.1f,0.1f);
            Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-30f, 30f);
            brb.velocity = (dir + pdir) * bloodSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject otherObject = other.gameObject;
        if (other.gameObject.tag == "Arrow")
        {

            GameObject sharedParent = new GameObject("Father");

            sharedParent.transform.position = this.gameObject.transform.position;
            sharedParent.transform.rotation = this.gameObject.transform.rotation;

            sharedParent.transform.SetParent(this.gameObject.transform);

            otherObject.transform.SetParent(this.gameObject.transform, true);

            if (isDie == false)
            {
                BloodSplash();
            }

            isDie = true;

            birdSound.clip = clip[0];
            birdSound.Play();
            enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 5.0f;
            this.gameObject.tag = "Arrow";
        }

        if (other.gameObject.tag == "Ground")
        {
            birdSound.clip = clip[1];
            birdSound.Play();

            this.transform.SetParent(other.transform, true);

            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            Destroy(this.gameObject.GetComponent<AudioSource>());
        }
    }
}
