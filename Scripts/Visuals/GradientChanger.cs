using UnityEngine;
using UnityEngine.UI;

public class GradientChanger : MonoBehaviour
{
    public Button UI;
    public Image[] imagelist;
    public PointCollecter PointCollector;

    private int lastindex;

    private Color[] targetColors;       // Stores the dynamic target color per image
    private bool isTransitioning = false;
    public float transitionSpeed = 2f;

    void Start()
    {
        lastindex = PointCollector.currentIndex;
        targetColors = new Color[imagelist.Length];
        
        // Initialize with current image colors
        for (int i = 0; i < imagelist.Length; i++)
        {
            targetColors[i] = imagelist[i].color;
        }
    }

    void Update()
    {
        GradientChange();

        if (isTransitioning)
        {
            bool allFinished = true;

            for (int i = 0; i < imagelist.Length; i++)
            {
                Image img = imagelist[i];
                img.color = Color.Lerp(img.color, targetColors[i], Time.deltaTime * transitionSpeed);

                if (Vector4.Distance(img.color, targetColors[i]) > 0.01f)
                {
                    allFinished = false;
                }
            }

            if (allFinished)
            {
                isTransitioning = false;
            }
        }
    }

    private void GradientChange()
    {
        if (PointCollector.currentIndex == lastindex + 1)
        {
            UI.onClick.Invoke();

            for (int i = 0; i < imagelist.Length; i++)
            {
                Color c = imagelist[i].color;
                targetColors[i] = GetAdjustedColor(c); // Save the color we want to lerp to
            }

            isTransitioning = true;
            lastindex = PointCollector.currentIndex;
        }
    }

    private Color GetAdjustedColor(Color color)
    {
        float r = AdjustChannel(color.r);
        float g = AdjustChannel(color.g);
        float b = AdjustChannel(color.b);
        return new Color(r, g, b, color.a);
    }

    private float AdjustChannel(float channel)
    {
        float value255 = channel * 255f;

        if (value255 >= 245f)
            value255 -= 10f;
        else
            value255 += 10f;

        return Mathf.Clamp01(value255 / 255f);
    }
}
