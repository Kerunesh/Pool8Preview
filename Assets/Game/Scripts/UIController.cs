using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public static UIController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    private int score = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    
    void Update()
    {
        float time = Time.time;

        int minutes = (int)time / 60;
        int secodns = (int)time % 60;

        timerText.text = minutes.ToString("00") + " " + secodns.ToString("00");
    }

    public void AddScore (int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score " + score;
    }
}
