using UnityEngine;

public class SunshineCircle : MonoBehaviour
{
    [SerializeField] private float SpeedScale, speedRotate;
    private RectTransform circle;
    void Start()
    {
        circle = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        circle.Rotate(0, 0, 20f * Time.deltaTime);
        circle.localScale = new Vector3(circle.localScale.x + SpeedScale, circle.localScale.y + SpeedScale, circle.localScale.z + SpeedScale);
        if (circle.localScale.x < 0.7 || circle.localScale.x > 1.3)
        {
            SpeedScale *= -1;
        }
    }
}
