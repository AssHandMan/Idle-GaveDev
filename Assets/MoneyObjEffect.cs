
using UnityEngine;

public class MoneyObjEffect : MonoBehaviour
{
    [SerializeField] private float duration;
    private RectTransform rectTransform;
    private Vector2 endPos = new Vector2(421, 1022);
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(AnimateMove());
    }

    private System.Collections.IEnumerator AnimateMove()
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        rectTransform.anchoredPosition = endPos;
        Destroy(rectTransform.gameObject);
    }
}
