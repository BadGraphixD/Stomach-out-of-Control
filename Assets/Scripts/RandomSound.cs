using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_clips;
    [SerializeField] private AudioSource m_source;

    [SerializeField] private bool m_playOnAwake = false;

    private void Awake()
    {
        if (m_playOnAwake)
            Play();
    }

    public void Play()
    {
        m_source.Stop();
        m_source.clip = m_clips[Random.Range(0, m_clips.Length)];
        m_source.Play();
    }
}
