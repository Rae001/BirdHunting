using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    [SerializeField] GameObject blood;

    Vector2[] curvePoint = new Vector2[4]; // Bird ������Ʈ�� �̵� ���� 4���� ���� �迭���� ����

    float currentTime = 0;
    float moveTime = 0;

    bool isDie;

    [SerializeField] AudioClip[] clip;

    AudioSource birdSound;

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

        float ab = Mathf.Lerp(a, b, t); // t�� ������ �Ǿ� ������ �̵� �ӵ��� ������ �� �ִ�.
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    public void DestroyBird() // Bird ������Ʈ�� ������ �̵����� ��ġ�� �����ҽ� �ı��ϰ� �����ð��� �������� �ʱ�ȭ�Ѵ�.
    {
        if (this.transform.position.x == curvePoint[3].x && this.transform.position.y == curvePoint[3].y)
        {
            Destroy(gameObject);
            currentTime = 0;
            moveTime = Random.Range(0.5f, 5.0f);
        }
        return;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject otherObject = other.gameObject;
        if (other.gameObject.tag == "Arrow")
        {
            Instantiate(blood, transform.position, transform.rotation);

            if (isDie == false)
            {
                GameObject sharedParent = new GameObject("Father");

                sharedParent.transform.position = this.transform.position;
                sharedParent.transform.rotation = this.transform.rotation;

                sharedParent.transform.SetParent(this.transform);

                otherObject.transform.SetParent(sharedParent.transform, true);

                isDie = true;
            }
            //else
            //{
            //    other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * 4, ForceMode2D.Impulse);
            //}

            




            birdSound.clip = clip[0];
            birdSound.Play();
            enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 5.0f;
        }

        if (other.gameObject.tag == "Ground")
        {
            birdSound.clip = clip[1];
            birdSound.Play();

            this.transform.SetParent(other.transform, true);

            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        }
    }
}
