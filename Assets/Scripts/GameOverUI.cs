using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameOverInfoText;
    [SerializeField] private EnemyWaveManager _enemyWaveManager;

    private void Start()
    {
        HideGameOverUI();
    }

    private void HideGameOverUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        int waveNumber = _enemyWaveManager.WaveNumber;

        _gameOverInfoText.text = $"You survived {waveNumber} waves!";
        
        gameObject.SetActive(true);
    }
    
}
