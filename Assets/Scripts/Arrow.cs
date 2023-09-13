using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] AudioClip[] clip;
    AudioSource arrowSound;

    Rigidbody2D rb;
    bool isHit;

    void Start()
    {
        arrowSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (this.transform.position.y <= -20)
        {
            Destroy(this.gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject otherObject = other.gameObject;
        if (other.gameObject.tag == "Bird")
        {
            isHit = true;

            GameObject sharedParent = new GameObject("Father");

            sharedParent.transform.position = otherObject.transform.position;
            sharedParent.transform.rotation = otherObject.transform.rotation;

            sharedParent.transform.SetParent(otherObject.transform);

            this.transform.SetParent(sharedParent.transform, true);

            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        }

        if (other.gameObject.tag == "Ground")
        {
            arrowSound.clip = clip[0];
            arrowSound.Play();

            isHit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        }

        if (other.gameObject.tag == "Arrow")
        {
            arrowSound.clip = clip[1];
            arrowSound.Play();
        }
    }
}
