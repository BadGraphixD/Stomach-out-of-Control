using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLink : MonoBehaviour
{
    private Player m_player;
    private GameManager m_gameManager;

    private void Start()
    {
        m_player = FindObjectOfType<Player>();
        m_gameManager = FindObjectOfType<GameManager>();

        Time.timeScale = 1f;
    }

    public void CanMove()
    {
        m_player.CanMove = true;
    }

    public void LoadNextLevel()
    {
        m_gameManager.LoadNextLevel();
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public void ReloadLevel()
    {
        m_gameManager.ReloadCurrentLevel();
    }

    public void ExitGame()
    {
        Debug.Log("MANAGER: Quit Application!");
        Application.Quit();
    }
}
