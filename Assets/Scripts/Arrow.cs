using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    bool hit;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (hit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.right = GetComponent<Rigidbody2D>().velocity;
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject otherObject = other.gameObject;

        if (other.gameObject.tag == "Bird")
        {
            hit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());

            GameObject sharedParent = new GameObject("Father");
            sharedParent.transform.position = otherObject.transform.position;
            sharedParent.transform.rotation = otherObject.transform.rotation;
            sharedParent.transform.SetParent(otherObject.gameObject.transform);


            //other.gameObject.transform.parent = this.gameObject.transform;
            this.gameObject.transform.SetParent(sharedParent.transform, true);
        }

        if (other.gameObject.tag == "Ground")
        {
            hit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        }

        if (other.gameObject.tag == "DestroyZone")
        {
            Destroy(this.gameObject);
        }

    }

}
