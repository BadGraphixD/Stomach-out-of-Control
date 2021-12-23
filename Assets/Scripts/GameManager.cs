using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int m_nextLevelSceneIndex;
    [SerializeField] private float m_vomitSpeed;
    [SerializeField] private float m_vomitjumpReset;
    [SerializeField] private float m_vomitBarSize;

    [Header("References")]
    [SerializeField] private RectTransform m_vomitBar;
    [SerializeField] private GameObject m_vomitExplosionPrefab;
    [SerializeField] private Animator m_blackFadeAnimator;
    [SerializeField] private GameObject m_pausedCanvas;
    [SerializeField] private GameObject m_clickToSkip;
    [SerializeField] private GameObject m_thoughtBubble;

    private float m_vomitLevel = 0f;
    private bool m_exploded = false;

    private bool m_gamePaused = false;

    private Player m_player;

    private void Start()
    {
        m_player = FindObjectOfType<Player>();

        m_gamePaused = false;
        Time.timeScale = 1f;
        m_pausedCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!m_exploded && m_player.StartedMoving && !m_gamePaused)
            checkVomitLevel();

        if (Input.GetKeyDown(KeyCode.R))
            ReloadCurrentLevel();

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            skip();
    }

    private void checkVomitLevel()
    {
        m_vomitLevel += m_vomitSpeed * Time.deltaTime;
        m_vomitLevel = Mathf.Clamp01(m_vomitLevel);

        Vector2 size = m_vomitBar.sizeDelta;
        size.x = m_vomitLevel * m_vomitBarSize;
        m_vomitBar.sizeDelta = size;

        if (m_vomitLevel >= 1f)
            VomitExplosion();
    }

    private void skip()
    {
        m_clickToSkip.SetActive(false);
        m_thoughtBubble.SetActive(false);

        m_player.CanMove = true;
        m_player.GetComponentInChildren<Animator>().SetTrigger("skip");
    }

    public void AtToilet()
    {
        m_vomitSpeed = 0f;
        m_player.CanMove = false;
        m_player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        m_player.GetComponent<Rigidbody2D>().simulated = false;

        m_player.AtToilet();

        m_blackFadeAnimator.SetTrigger("endLevel");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadSceneAsync(m_nextLevelSceneIndex);
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        m_gamePaused = !m_gamePaused;

        Time.timeScale = m_gamePaused ? 0f : 1f;
        m_pausedCanvas.SetActive(m_gamePaused);
    }

    public void VomitExplosion()
    {
        m_exploded = true;

        Instantiate(m_vomitExplosionPrefab, m_player.transform.position, Quaternion.identity);

        m_player.CanMove = false;
        m_player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        m_player.GetComponent<Rigidbody2D>().simulated = false;

        m_player.VomitExplosion();

        m_blackFadeAnimator.SetTrigger("reloadLevel");
    }

    public void VomitJump()
    {
        m_vomitLevel -= m_vomitjumpReset;
        m_vomitLevel = Mathf.Clamp01(m_vomitLevel);
    }
}
