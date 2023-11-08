using UnityEngine;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
    public TextMeshProUGUI loadingText;

    //[SerializeField]
    private float updateInterval = 0.4f; // Update interval in seconds
    private float lastUpdateTime;

    private string loadingTextBase = "Loading";
    private int maxDots = 3;
    private int currentDots = 0;
    private bool startAnimation;

    private void OnEnable()
    {
        lastUpdateTime = Time.time;
        loadingText.text = loadingTextBase;
        startAnimation = true;
    }
    private void OnDisable()
    {
        startAnimation = false;
        lastUpdateTime = 0f;
        loadingText.text = "";
        currentDots = 0;
    }

    private void Update()
    {
        if (!startAnimation)
        {
            return;
        }
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            lastUpdateTime = Time.time;
            currentDots = (currentDots + 1) % (maxDots + 1);

            string dots = new string('.', currentDots);
            // dots = $"<size=120%>{dots}</size>";
            // loadingText.text = $"<b><uppercase>{loadingTextBase}</uppercase></b>{dots}";
            loadingText.text = loadingTextBase + dots;
        }
    }
}
