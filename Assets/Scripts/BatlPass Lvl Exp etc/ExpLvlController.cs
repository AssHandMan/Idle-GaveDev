using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ExpLvlController : MonoBehaviour
{
    [SerializeField] private GameObject ExpMenu;
    [SerializeField] private UpgreadeSceneMeneger BuyMeneger;
    [SerializeField] private Image ExpBar;
    public int lvl, CountExP, MaxExPLvl;
    [SerializeField] private Text textLvl, textprogressBar;

    [SerializeField] private GameObject starPrefab; // Префаб звезды
    [SerializeField] private RectTransform starSpawnPoint; // Центр экрана (например, пустой объект в Canvas)
    [SerializeField] private RectTransform targetUIElement; // Куда летят звезды (например, иконка уровня)
    [SerializeField] private Canvas parentCanvas; // Ссылка на Canvas, чтобы UI позиционировался правильно


    void Start()
    {
        MaxExPLvl = 3;

    }

    void Update()
    {

    }


    public void InscreaseExP(GameObject station)
    {
        if ((station.GetComponent<ObjectShowHider>() != null && station.name == "AddSpace0") || (station.GetComponent<ObjectShowHider>() == null))
        {
            CountExP += 3;
            if (CountExP >= MaxExPLvl)
            {
                CountExP = MaxExPLvl;
                StartCoroutine(PlayLevelUpStars(() =>
                {
                    lvl++;
                    MaxExPLvl = BuyMeneger.GetMaxLvl() * 3;
                    textLvl.text = lvl.ToString();
                }));
            }
            textprogressBar.text = CountExP.ToString() + "/" + MaxExPLvl.ToString();
            if (!ExpMenu.activeSelf)
            {
                ExpMenu.SetActive(true);
            }
        }
        ExpBar.fillAmount = (float)CountExP / MaxExPLvl;
    }

    private IEnumerator PlayLevelUpStars(Action onComplete)
    {
        int starsCount = 3;
        float explodeDistance = 100f;
        float explodeDuration = 0.3f;
        float flyDuration = 0.5f;

        int starsFinished = 0;

        for (int i = 0; i < starsCount; i++)
        {
            GameObject star = Instantiate(starPrefab, parentCanvas.transform);
            RectTransform starRect = star.GetComponent<RectTransform>();
            starRect.position = starSpawnPoint.position;
            starRect.localScale = Vector3.one;

            // Выбираем случайное направление для "взрыва"
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 explodeTarget = starSpawnPoint.position + (Vector3)(randomDirection * explodeDistance);

            StartCoroutine(AnimateStar(starRect, explodeTarget, targetUIElement.position, explodeDuration, flyDuration, targetUIElement.localScale, () =>
            {
                Destroy(star);
                starsFinished++;

                if (starsFinished == starsCount)
                    onComplete?.Invoke();
            }));

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AnimateStar(RectTransform star, Vector3 explodeTarget, Vector3 finalTarget, float explodeDuration, float flyDuration, Vector3 finalScale, Action onArrive)
    {
        Vector3 startPos = star.position;
        Vector3 startScale = star.localScale;

        // Этап 1: разлет
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / explodeDuration;
            float curved = Mathf.SmoothStep(0, 1, t);
            star.position = Vector3.Lerp(startPos, explodeTarget, curved);
            yield return null;
        }

        // Этап 2: сбор и уменьшение
        t = 0;
        Vector3 midPos = star.position;
        Vector3 midScale = star.localScale;
        while (t < 1f)
        {
            t += Time.deltaTime / flyDuration;
            float curved = Mathf.SmoothStep(0, 1, t);
            star.position = Vector3.Lerp(midPos, finalTarget, curved);
            star.localScale = Vector3.Lerp(midScale, finalScale, curved);
            yield return null;
        }

        star.position = finalTarget;
        star.localScale = finalScale;

        onArrive?.Invoke();
    }
}
