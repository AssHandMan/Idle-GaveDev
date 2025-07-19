using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Amoga : MonoBehaviour
{
    private RectTransform rect;
    public Canvas canvas;
    public int number;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        Vector2 canvasPos = rect.position;
        gameObject.name = " FreeBlock " + number.ToString();
        StartCoroutine(Cre());
    }

    private IEnumerator Cre()
    {
        yield return new WaitForSeconds(0.1f);
        if (number <= 33)
        {
            GameObject obj = Instantiate(gameObject, canvas.transform);
            RectTransform newRect = obj.GetComponent<RectTransform>();
            Amoga newA = obj.GetComponent<Amoga>();
            newA.number = number + 1;
            newRect.anchoredPosition = rect.anchoredPosition + new Vector2(0, -360);
        }
    }

}
