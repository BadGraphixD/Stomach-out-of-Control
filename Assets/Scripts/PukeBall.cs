using UnityEngine;

public class PukeBall : MonoBehaviour
{
    [SerializeField] private GameObject[] m_splashPrefabs;
    [SerializeField] private string m_floorTag;
    [SerializeField] private float m_maxLifetime;

    private void Start()
    {
        Invoke("splash", m_maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(m_floorTag))
        {
            CancelInvoke();
            splash();
        }
    }

    private void splash()
    {
        Instantiate(m_splashPrefabs[Random.Range(0, m_splashPrefabs.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
