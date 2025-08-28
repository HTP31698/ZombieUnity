using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text scoreText;
    public Text waveText;

    public GameObject gameOverUi;

    public void OnEnable()
    {
        SetAmmoText(0, 0);
        SetUpdateScore(0);
        SetWaveInfo(0,0);
        SetActiveGameOver(false);
    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo} / {remainAmmo}";
    }

    public void SetUpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void SetWaveInfo(int wave, int count)
    {
        waveText.text = $"Wave: {wave}\nEnemy Left: {count}";
    }

    public void SetActiveGameOver(bool active)
    {
        gameOverUi.SetActive(active);
    }

    public void OnclickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
