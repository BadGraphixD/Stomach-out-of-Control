using UnityEngine;

public class Toilet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask m_playerLayer;
    [SerializeField] private BoxCollider2D m_enterTrigger;
    [SerializeField] private Sprite m_playerAtToilet;
    [SerializeField] private AudioSource m_toiletFlushSound;

    private GameManager m_gameManager;

    private void Start()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        checkEnterTrigger();
    }

    private void checkEnterTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_enterTrigger.bounds.center, m_enterTrigger.bounds.size, 0f, m_playerLayer);

        if (colliders.Length > 0)
        {
            m_toiletFlushSound.Play();

            m_gameManager.AtToilet();
            GetComponentInChildren<SpriteRenderer>().sprite = m_playerAtToilet;
        }
    }
}
