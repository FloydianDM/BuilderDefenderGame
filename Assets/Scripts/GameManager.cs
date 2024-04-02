using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(0);
    }
}
