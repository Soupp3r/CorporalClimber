using UnityEngine;
using TMPro;

public class RankTextUpdater : MonoBehaviour
{
    [Header("Required Componenets")]
    public MainButton SO;
    public PointCollecter PC;

    [Header("TMP Objects")]
    public TMP_Text TillNextRankText;
    public TMP_Text CurrentRankText;

    void Update()
    {
        TillNextRankText.text = SO.ClicksTillNextLevel.ToString();

        CurrentRankText.text = (PC.currentIndex + 1).ToString();
    }
}
