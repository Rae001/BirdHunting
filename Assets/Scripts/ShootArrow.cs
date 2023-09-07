using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class ShootArrow : MonoBehaviour
{
    #region 변수들
    
    [SerializeField] GameObject arrowPrefeb = null; // 화살 프리팹 변수 선언
    [SerializeField] GameObject alreadyArrow; // 화살 프리팹 변수 선언
    [SerializeField] Transform tfArrow = null; // 화살 위치 정보 변수 선언
    [SerializeField] RectTransform distace_Text_Rect; // 화살 거리 정보를 표현할 텍스트 변수 선언
    [SerializeField] TMP_Text distance_Text;
    [SerializeField] RectTransform angle_Text_Rect; // 화살 거리 정보를 표현할 텍스트 변수 선언
    [SerializeField] TMP_Text angle_Text;
    [SerializeField] LineRenderer drawLine;

    public AudioSource shootSound;


    Camera _camera = null; // 카메라가 기본좌표지도라고 생각하면 됨


    // 마우스 클릭 좌표 위치를 담을 벡터변수 선언
    Vector2 startClickPosition;
    Vector2 endClickPosition;
    
    float distancePower; // 두 마우스 좌표 계산을 담을 변수 선언
    #endregion

    void Start()
    {
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
            drawLine.SetPosition(0, new Vector3(startClickPosition.x, startClickPosition.y, 0f));
            drawLine.SetPosition(1, new Vector3(endClickPosition.x, endClickPosition.y, 0f));
        }

        // firstClickPosition - endClickPosition의 거리 계산
        if (distancePower >= 20.0f)
            distancePower = 20.0f;
        else
            distancePower = 0;
            distancePower = (startClickPosition - endClickPosition).magnitude;

        tfArrow.transform.rotation = Quaternion.Euler(0, 0, GetAngle(startClickPosition, endClickPosition));
        alreadyArrow.transform.rotation = Quaternion.Euler(0, 0, GetAngle(startClickPosition, endClickPosition));
        //Vector3 angle = new Vector3(0, 0, GetAngle(firstClickPosition, endClickPosition));
        //Debug.Log("두 좌표 사이의 각도 : " + angle.z);
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
        distace_Text_Rect.pivot = new Vector2(0.6f, 0.5f);
        
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
        angle_Text.text = tfArrow.transform.rotation.eulerAngles.z.ToString();

    }
}
