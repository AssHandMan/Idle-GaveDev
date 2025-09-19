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

    public float minX, maxX;
    public float minZ, maxZ;
    private bool isDragging = false;
    [Header("Movement Settings")]
    public float dragSpeed = 5f;
    public float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    [Header("Zoom Settings")]
    public float minHeight = 5f;
    public float maxHeight = 30f;
    public float zoomSpeed = 10f;
    public float zoomSmoothTime = 0.1f;
    private float zoomVelocity;

    [Header("Camera Bounds")]
    public Vector2 mapMinBounds = new Vector2(-50f, -50f); // Минимальные X и Z карты
    public Vector2 mapMaxBounds = new Vector2(50f, 50f);    // Максимальные X и Z карты
    public float edgePadding = 5f; // Отступ от краёв карты

    private Vector3 dragOrigin;

    [Header("Audio")]
    public AudioSource _audio;
    public AudioSource _audio2;
    public AudioClip _clipBtn;

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
                // === 1. Начало перетаскивания ===
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    isDragging = true;
                    return;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }

                if (isDragging && Input.GetMouseButton(0))
                {
                    Vector3 mouseDelta = (Input.mousePosition - dragOrigin) * dragSpeed * Time.deltaTime;

                    // Плавное уменьшение скорости при маленьких движениях
                    float sensitivity = Mathf.Clamp01(mouseDelta.magnitude / 10f);
                    mouseDelta *= sensitivity;

                    Vector3 move = transform.right * mouseDelta.x + transform.forward * mouseDelta.y;
                    move.y = 0;

                    Vector3 targetPosition = transform.position - move;
                    targetPosition = ClampCameraPosition(targetPosition);

                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                    dragOrigin = Input.mousePosition;
                }


            }
        }
    }
    private Vector3 ClampCameraPosition(Vector3 targetPosition)
    {
        float heightFactor = Mathf.InverseLerp(minHeight, maxHeight, targetPosition.y);
        float padding = Mathf.Lerp(edgePadding * 0.5f, edgePadding, heightFactor);

        targetPosition.x = Mathf.Clamp(
            targetPosition.x,
            mapMinBounds.x + padding,
            mapMaxBounds.x - padding
        );

        targetPosition.z = Mathf.Clamp(
            targetPosition.z,
            mapMinBounds.y + padding,
            mapMaxBounds.y - padding
        );

        return targetPosition;
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
        _audio2.Stop();
        _audio.PlayOneShot(_clipBtn,0.4f);
        _audio.PlayDelayed(0.4f);
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
            plr.GetComponent<PlayerMovement>().BlockMoving();
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
        plr.GetComponent<PlayerMovement>().BlockMoving();
        isPlay = true;
    }
     #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Визуализация границ в редакторе
        Vector3 center = new Vector3(
            (mapMinBounds.x + mapMaxBounds.x) * 0.5f,
            0,
            (mapMinBounds.y + mapMaxBounds.y) * 0.5f
        );
        
        Vector3 size = new Vector3(
            mapMaxBounds.x - mapMinBounds.x,
            0.1f,
            mapMaxBounds.y - mapMinBounds.y
        );
        
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(center, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
    #endif
}
