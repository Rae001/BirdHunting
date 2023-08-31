using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    Vector2[] curvePoint = new Vector2[4];

    //float moveSpeed = 10.0f;
    float moveTimeMax = 0;
    float moveTimeCurrent = 0;
    void Start()
    {
        
        //moveSpeed = Random.Range(1.0f, 5.0f);
    }

    void Update()
    {
        //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        moveTimeCurrent += Time.deltaTime;


        transform.position = new Vector2(
            BezierCurve(curvePoint[0].x, curvePoint[1].x, curvePoint[2].x, curvePoint[3].x),
            BezierCurve(curvePoint[0].y, curvePoint[1].y, curvePoint[2].y, curvePoint[3].y));

        DestroyBird();
    }

    public void DestroyBird()
    {
        if (this.transform.position.x == curvePoint[3].x && this.transform.position.y == curvePoint[3].y)
        {
            Destroy(gameObject);
            moveTimeMax = Random.Range(99.0f, 100.0f);
            moveTimeCurrent = 0;
        }
        return;
    }

    public void CurvePointInit(Transform start, Transform end, float DistanceFromStart, float DistanceFromEnd)
    {

        moveTimeMax = Random.Range(3.0f, 6.0f);

        curvePoint[0] = start.position;

        curvePoint[1] = start.position +
            (DistanceFromStart * Random.Range(0.0f, 6.0f) * start.right) +
            (DistanceFromStart * Random.Range(-2.0f, 3.5f) * start.up);

        curvePoint[2] = end.position +
            (DistanceFromEnd * Random.Range(0.0f, 6.0f) * end.right) +
            (DistanceFromEnd * Random.Range(-2.0f, 3.5f) * end.up);

        curvePoint[3] = end.position;
    }


    private float BezierCurve(float a, float b, float c, float d)
    {
        // 바로 이동하는 문제는 여기서 발생
        float t = moveTimeCurrent / moveTimeMax;

        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 5.0f;
           
        }

        if (other.gameObject.tag == "Ground")
        {
            this.transform.SetParent(other.transform, true);

            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        }


        if (other.gameObject.tag == "DestroyZone")
        {
            Destroy(gameObject);
            
        }
    }


}
