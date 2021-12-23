using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpVelocity;
    [SerializeField] private float m_vomitVelocity;

    [SerializeField] private float m_movementSmoothing;
    [SerializeField] private LayerMask m_walkable;

    [SerializeField] private float m_vomitIntervall;
    [SerializeField] private float m_pukeBallVelocity;

    [Header("References")]
    [SerializeField] private Rigidbody2D m_rigidBody;
    [SerializeField] private BoxCollider2D m_groundTrigger;
    [SerializeField] private Transform m_pukeBallSpawnPos;
    [SerializeField] private Animator m_animator;
    [SerializeField] private AudioSource m_runningSound;
    [SerializeField] private AudioSource m_splashSound;
    [SerializeField] private RandomSound m_jumpSound;

    [Header("Prefabs")]
    [SerializeField] private GameObject m_pukeBallPrefab;

    private float m_inputHorizontal = 0f;

    private bool m_jump;

    private float m_currentVelocity;
    private float m_vomitCooldown = 0f;
    private bool m_startedMoving = false;

    private bool m_runningSoundPlaying = false;

    private GameManager m_gameManager;

    public bool CanMove = false;
    public bool StartedMoving = false;

    private void Start()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (CanMove)
            recieveInput();
        updateAnimatorAndSounds();
    }

    private void recieveInput()
    {
        bool alreadyMovedBefore = m_startedMoving;

        m_inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            checkJump();
            m_startedMoving = true;
        }

        if (m_inputHorizontal != 0f)
            m_startedMoving = true;

        if (!alreadyMovedBefore && m_startedMoving)
            StartedMoving = true;

    }

    private void updateAnimatorAndSounds()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_groundTrigger.bounds.center, m_groundTrigger.bounds.size, 0f, m_walkable);

        bool running = Mathf.Abs(m_inputHorizontal) > 0.01f;
        bool onGround = colliders.Length > 0;

        m_animator.SetBool("running", running);
        m_animator.SetBool("runningRight", m_inputHorizontal > 0.01f);
        m_animator.SetBool("onGround", onGround);

        if (running && !m_runningSoundPlaying && onGround)
            playRunningSound();

        if ((!running && m_runningSoundPlaying) || !onGround)
            stopRunningSound();
    }

    private void stopRunningSound()
    {
        m_runningSound.Stop();
        m_runningSoundPlaying = false;
    }

    private void playRunningSound()
    {
        m_runningSound.Play();
        m_runningSoundPlaying = true;
    }

    private void FixedUpdate()
    {
        move();
        m_vomitCooldown -= Time.fixedDeltaTime;
    }

    private void move()
    {
        float horizontalTargetSpeed = m_inputHorizontal * m_speed;

        Vector2 rbVelocity = m_rigidBody.velocity;
        rbVelocity.x = Mathf.SmoothDamp(rbVelocity.x, horizontalTargetSpeed, ref m_currentVelocity, m_movementSmoothing);
        m_rigidBody.velocity = rbVelocity;
    }

    private void checkJump()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_groundTrigger.bounds.center, m_groundTrigger.bounds.size, 0f, m_walkable);

        if (colliders.Length > 0) // Jump
        {
            jump(m_jumpVelocity);
            m_jump = false;

            m_animator.SetTrigger("jump");
        }
        else // Vomit
        {
            if (m_vomitCooldown <= 0f)
            {
                jump(m_vomitVelocity);
                m_jump = false;

                m_animator.SetTrigger("vomit");

                spawnPukeBall();
                m_gameManager.VomitJump();

                m_vomitCooldown = m_vomitIntervall;
            }
        }
    }

    private void jump(float velocity)
    {
        Vector2 rbVelocity = m_rigidBody.velocity;
        rbVelocity.y = velocity;
        m_rigidBody.velocity = rbVelocity;

        m_jumpSound.Play();
    }

    private void spawnPukeBall()
    {
        Rigidbody2D pukeBallRb = Instantiate(m_pukeBallPrefab, m_pukeBallSpawnPos.position, Quaternion.identity).GetComponent<Rigidbody2D>();

        pukeBallRb.velocity = new Vector2(0f, -m_pukeBallVelocity);
    }

    public void AtToilet()
    {
        m_runningSound.enabled = false;
        m_splashSound.enabled = false;
    }

    public void VomitExplosion()
    {
        m_runningSound.enabled = false;
        m_splashSound.Play();
    }
}
