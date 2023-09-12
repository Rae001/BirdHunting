using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class ShootArrow : MonoBehaviour
{
    #region ������
    
    [SerializeField] GameObject arrowPrefeb = null; // ȭ�� ������ ���� ����
    [SerializeField] GameObject alreadyArrow; // ȭ�� ������ ���� ����
    [SerializeField] Transform tfArrow = null; // ȭ�� ��ġ ���� ���� ����
    [SerializeField] RectTransform distace_Text_Rect; // ȭ�� �Ÿ� ������ ǥ���� �ؽ�Ʈ ���� ����
    [SerializeField] TMP_Text distance_Text;
    [SerializeField] RectTransform angle_Text_Rect; // ȭ�� ���� ������ ǥ���� �ؽ�Ʈ ���� ����
    [SerializeField] TMP_Text angle_Text;
    [SerializeField] LineRenderer drawLine;

    public AudioSource shootSound;


    Camera _camera = null; // ī�޶� �⺻��ǥ������� �����ϸ� ��


    // ���콺 Ŭ�� ��ǥ ��ġ�� ���� ���ͺ��� ����
    Vector2 startClickPosition;
    Vector2 endClickPosition;
    
    float distancePower; // �� ���콺 ��ǥ ����� ���� ���� ����
    #endregion

    void Start()
    {
        //endClickPosition = Vector3.MoveTowards(startClickPosition, endClickPosition, 30.0f);
        alreadyArrow.SetActive(false);
        distace_Text_Rect.gameObject.SetActive(false);
        angle_Text_Rect.gameObject.SetActive(false);
        drawLine.gameObject.SetActive(false);
        drawLine.positionCount = 2;
        _camera = Camera.main;
        InitCursor();     
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMousePosition();
        Reload();
        Fire();
    }

    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = start - end;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    public void Reload()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startClickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            alreadyArrow.SetActive(true);
            distace_Text_Rect.gameObject.SetActive(true);
            angle_Text_Rect.gameObject.SetActive(true);
            drawLine.gameObject.SetActive(true); ;
            endClickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3[] points = new Vector3[2];
            points[0] = startClickPosition;
            points[1] = Vector3.MoveTowards(startClickPosition, endClickPosition, 30.0f);

            drawLine.SetPositions(points);

            
            Vector2 disTextPos = _camera.ScreenToWorldPoint(distace_Text_Rect.transform.position); // �Ÿ������� ������ RectTransform�� ��ũ������ ������ǥ�� ��ȯ�ϴ�.

            if (Vector2.Distance(disTextPos, points[1]) > 1) // DrawLine�� 1��° �ε��� ���� ���� disTextPos�� ���Ͱ� ������ �Ÿ��� 1�� ������ 
            {
                distace_Text_Rect.transform.position = _camera.WorldToScreenPoint(points[1]); // DrawLine�� 1��° �ε��� ���� ���� �Ҵ��Ѵ�.
            }
        }
      
        distancePower = (startClickPosition - endClickPosition).magnitude;
        if (distancePower >= 30)
        {
            distancePower = 30;
        }

        tfArrow.transform.rotation = Quaternion.Euler(0, 0, GetAngle(startClickPosition, endClickPosition));
        alreadyArrow.transform.rotation = Quaternion.Euler(0, 0, GetAngle(startClickPosition, endClickPosition));
    }

    public void Fire()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject arrow = Instantiate(arrowPrefeb, tfArrow.position, tfArrow.rotation);
            arrow.GetComponent<Rigidbody2D>().velocity = tfArrow.transform.right * distancePower * 3;
            alreadyArrow.SetActive(false);
            distace_Text_Rect.gameObject.SetActive(false);
            angle_Text_Rect.gameObject.SetActive(false);
            drawLine.gameObject.SetActive(false);
            shootSound.Play();
        }
    }

    public void InitCursor()
    {
        angle_Text_Rect.pivot = new Vector2(0.6f, -0.5f);
        distace_Text_Rect.pivot = new Vector2(0.6f, 1.0f);
        
        if (distace_Text_Rect.GetComponent<Graphic>())
            distace_Text_Rect.GetComponent<Graphic>().raycastTarget = false;

        if (angle_Text_Rect.GetComponent<Graphic>())
            angle_Text_Rect.GetComponent<Graphic>().raycastTarget = false;
    }

    public void UpdateMousePosition()
    {
        Vector2 anglePos = _camera.WorldToScreenPoint(startClickPosition);
        Vector2 distancePos = _camera.WorldToScreenPoint(endClickPosition);

        angle_Text_Rect.position = anglePos;
        distace_Text_Rect.position = distancePos;

        distance_Text.text = distancePower.ToString();
        if (distancePower >= 30.0f)
            distance_Text.text = "30.0";
        
        angle_Text.text = tfArrow.transform.rotation.eulerAngles.z.ToString();

    }
}
