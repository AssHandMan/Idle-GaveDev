using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float OffsetX, OffsetY, OffsetZ, FixX, FixZ;
    public Vector3 PlatRotate, SkinRotate, SkinPos, GlobalMapPos, GlobalMapRotate;
    public bool isPlay, isGloblaMap;
    private GameObject plr;
    private Vector3 target;
    public GameObject UiShowHide, Money, Diamonds, Skins, GlobalMapUI;
    public float dragSpeed = 10f;

    public float minX, maxX;
    public float minZ, maxZ;
    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Start()
    {
        isPlay = true;
        plr = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        target = new Vector3(plr.transform.position.x - OffsetX, plr.transform.position.y + OffsetY, plr.transform.position.z - OffsetZ);
        if(isPlay)
        {
            if (plr != null)
            {
                if(plr.transform.position.x > FixX)
                {
                    transform.position = new Vector3(transform.position.x, target.y, target.z);
                }
                else if(plr.transform.position.z < FixZ)
                {
                    transform.position = new Vector3(target.x, target.y, transform.position.z);
                }
                else 
                {
                    transform.position = new Vector3(target.x, target.y, target.z);   
                }
            }
        }
        else
        {
            if(isGloblaMap)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    isDragging = true;
                }

                // Отпускание ЛКМ
                if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }

                if (isDragging)
                {
                    Vector3 mouseDelta = Input.mousePosition - dragOrigin;
                    Vector3 deltaView = Camera.main.ScreenToViewportPoint(mouseDelta);

                    // Меняем местами оси: движение по X мышью влияет на Z, по Y — на X
                    float moveX = -deltaView.y * dragSpeed; // вертикальный свайп → X
                    float moveZ = deltaView.x * dragSpeed; // горизонтальный свайп → Z

                    Vector3 newPosition = transform.localPosition + new Vector3(moveX, 0, moveZ);

                    // Ограничения
                    newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                    newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
                    newPosition.y = transform.localPosition.y; // Y остаётся прежним

                    transform.localPosition = newPosition;

                    dragOrigin = Input.mousePosition;
                }
            }
        }
    }

    public void SetPlayMood()
    {
        isPlay=true;
        isGloblaMap = false;
        transform.rotation = Quaternion.Euler(PlatRotate);
    }
    public void SetSkinMood()
    {
        isPlay = false;
        transform.position = SkinPos;
        transform.rotation = Quaternion.Euler(SkinRotate);
    }
    public void SetGlobalMapMod()
    {
        UiShowHide.SetActive(false);
        Money.SetActive(false);
        Diamonds.SetActive(false);
        Skins.SetActive(false);
        isPlay = false;
        transform.localPosition = GlobalMapPos;
        transform.rotation = Quaternion.Euler(GlobalMapRotate);
        GlobalMapUI.SetActive(true);
        isGloblaMap = true;
    }
    public void SetBuySpacePos(GameObject space)
    {
        if (isPlay)
        {
            isPlay = false;
            Vector3 targetBuy = new Vector3(space.transform.position.x - OffsetX, transform.position.y, space.transform.position.z - OffsetZ);
            StartCoroutine(MoveToLustBuytSpace(targetBuy));
        }
    }

    private IEnumerator MoveToLustBuytSpace(Vector3 pos)
    {
        while(Vector3.Distance(transform.position, pos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 27 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 34 * Time.deltaTime);
            yield return null;
        }
        isPlay = true;
    }
}
