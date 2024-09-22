using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI sweepCd;

    [SerializeField] private GameObject resultWindow;
    [SerializeField] private TextMeshProUGUI resultWindowScore;
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetHp(int hpValue)
    {
        hp.text = $"HP: {hpValue}/100";
    }

    public void SetScore(int scoreValue)
    {
        score.text = $"Score: {scoreValue}";
        resultWindowScore.text = $"Score: {scoreValue}";
    }

    public void SetSweepCd(float cd)
    {
        sweepCd.text = $"SweepSkill CD: {cd}";
    }

    public void ShowResultWindow()
    {
        resultWindow.SetActive(true);
    }

    public void HideResultWindow()
    {
        resultWindow.SetActive(false);
    }

    public void Play()
    {
        GameRunner.Instance.StartGame();
    }
}