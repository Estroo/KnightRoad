using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    GamePadController.Controller gamePad;
    public Image HealthImage;
    public GameObject CameraObject;

    public static bool inDialog = false;

    private int dashCount;
    bool isJumping = false;
    private float moveSpeed = 8f;
    public float jumpSpeed = 16f;
    private float jumpTimeCounter;
    public float jumpTime;

    private bool unarmed = true;
    private bool haveSword = false;
    public bool bossKilled = false;

    private Rigidbody2D rigidBody2D;

    public int deathHeight = -20;
    public bool dead = false;
    bool left = false;
    bool right = false;

    //RAYCAST
    public float raycastMaxDistance = 1.5f;

    private Animator Anim;

    private GameObject PlayerSword;
    private GameObject PlayerModel;

    //CHEST
    private string[] chestItem = new string[] { "Sword", "Life" };
    private bool isOnChest = false;
    private Collider2D chestCol = null;

    //SOUND
    [Header("Sound Settings")]
    public AudioSource spawnSound;
    public AudioSource attackSoundUnarmed;
    public AudioSource attackSoundSword;
    public AudioSource hitWallSound;
    public AudioSource deathSoundPlayer;
    public AudioSource deathSoundBlood;
    public AudioSource chestOpeningSound;
    public AudioSource UnsheatSwordSound;
    public AudioSource TouchGrassSound;
    public AudioSource EnterWater;
    public AudioSource ExitWater;
    public AudioSource GetHealthSound;
    public AudioSource DashSound;

    [Header("Footsteps Grass Settings")]
    public AudioClip[] FootstepsGrass;
    public AudioSource GrassFootstep;

    [Header("Footsteps Snow Settings")]
    public AudioClip[] FootstepsSnow;
    public AudioSource SnowFootstep;

    [Header("Footsteps Snow Settings")]
    public AudioClip[] FootstepsChest;
    public AudioSource ChestFootstep;

    [Header("Footsteps Water Settings")]
    public AudioClip[] FootstepsWater;
    public AudioSource WaterFootstep;

    [Header("Particle")]
    public GameObject SplashWater;
    public GameObject JumpParticle;
    public GameObject dashEffect;
    public GameObject BloodParticle;
    public GameObject SpawningParticle;

    [Header("Shake")]
    public float camShakeAmount = 0.1f;
    Shake camShake;

    [Header("Life")]
    private int Health = 3;

    [HideInInspector]
    public bool isGrounded = false;
    public bool onGrass = false;
    public bool onChest = false;
    public bool onSnow = false;
    public bool inWater = false;
    public bool onPause = false;
    public bool inCinematic = false;

    public GameObject pauseMenu;

    private TakeScreenshot screenshot;

    public bool Relic1 = false, Relic2 = false, Relic3 = false;

    private void Start()
    {
        Time.timeScale = 1;
        PlayerSword = GameObject.FindGameObjectWithTag("Sword");
        PlayerModel = GameObject.FindGameObjectWithTag("PlayerModel");
        screenshot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TakeScreenshot>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        transform.position = GameObject.FindWithTag("Spawn").transform.position;
        Anim = transform.Find("model").GetComponent<Animator>();
        camShake = GameObject.FindGameObjectWithTag("Effect").GetComponent<Shake>();
        gamePad = GamePadController.GamePadOne;
        Flip(false);
    }

    public IEnumerator showPauseMenu()
    {
        onPause = true;
        pauseMenu.SetActive(true);
        for (float f = 0; f <= 0.20f; f += Time.deltaTime)
        {
            pauseMenu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, f / 0.20f);
            yield return null;
        }
        pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
    }

    public IEnumerator hidePauseMenu()
    {
        for (float f = 0; f <= 0.20f; f += Time.deltaTime)
        {
            pauseMenu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, f / 0.20f);
            yield return null;
        }
        pauseMenu.GetComponent<CanvasGroup>().alpha = 0;
        pauseMenu.SetActive(false);
        onPause = false;

    }

    void Update()
    {
        if (Input.GetButtonDown("Screenshot"))
            screenshot.TakeShot();
        if (Input.GetButtonDown("Start"))
        {
            if (onPause == false)
                StartCoroutine(showPauseMenu());
            else
                StartCoroutine(hidePauseMenu());
        }
        if (onPause == true && Input.GetJoystickNames().Length == 0)
            Cursor.visible = true;
        if (onPause == false)
        {
            Cursor.visible = false;
            if (inDialog == true)
                Anim.Play("Idle");
            if (dead == false && inDialog == false && inCinematic == false)
            {
                HandleWeapon();
                HandleMovement();
                HandleChest(chestCol);
            }
            if (dead == true)
            {
                if (isGrounded == true)
                {
                    gameObject.GetComponent<Rigidbody2D>().simulated = false;
                    gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
                }
            }
            HandleHealth();
            HandleDeath();
        }
    }

    void HandleHealth()
    {
        float newWidth = 20 * Health;
        HealthImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    void HandleMovement()
    {
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("AttackMoving") && !Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (Input.GetAxisRaw("Horizontal") != 0 && isGrounded == true)
                    Anim.Play("AttackMoving");
                else
                    Anim.Play("Attack");
                if (haveSword == true)
                    attackSoundSword.Play();
                if (unarmed == true)
                    attackSoundUnarmed.Play();
            }
            else if (dead == false)
            {
                if (Input.GetAxisRaw("Horizontal") == 0 && isGrounded == true)
                    Anim.Play("Idle");
                else if (Input.GetAxisRaw("Horizontal") != 0 && isGrounded == true)
                    Anim.Play("Run");
                else if (isGrounded == false)
                    Anim.Play("Jump");
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.transform.Translate(Vector2.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime);
            if (Input.GetAxisRaw("Horizontal") > 0)
                Flip(false);
        }

        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.transform.Translate(Vector2.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime);
            if (Input.GetAxisRaw("Horizontal") < 0)
                Flip(true);
        }
        if ((Input.GetButtonDown("Dash") || Input.GetAxis("Dash") > 0) && isGrounded == false && dashCount == 0)
        {
            gamePad.SetVibration(100, 100, 0.15f);
            isJumping = false;
            Destroy(Instantiate(dashEffect, transform.position, Quaternion.identity), 1.0f);
            DashSound.Play();
            dashCount = 1;
            moveSpeed = 0;
            rigidBody2D.velocity = new Vector2(0, 0);
            if (right == true)
                rigidBody2D.velocity = Vector2.right * 20;
            if (left == true)
                rigidBody2D.velocity = Vector2.left * 20;
            rigidBody2D.gravityScale = 0.0f;
            camShake.Shakee(camShakeAmount, 0.1f);
            (CameraObject.GetComponent("MotionBlur") as MonoBehaviour).enabled = true;
            StartCoroutine(DashGravity(0.20f));
        }
        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            Anim.Play("Jump");
            if (inWater == false)
                Destroy(Instantiate(JumpParticle, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.identity), 1.0f);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 1 * jumpSpeed);
        }
        if (Input.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 1 * jumpSpeed);
                jumpTimeCounter -= Time.deltaTime * 2;
            }
            else
                isJumping = false;
        }
        if (Input.GetButtonUp("Jump"))
            isJumping = false;
    }

    void HandleWeapon()
    {
        if (haveSword == false)
            PlayerSword.SetActive(false);
        else
            PlayerSword.SetActive(true);
    }

    void HandleDeath()
    {
        if (transform.position.y <= deathHeight)
        {
            dead = true;
            Health--;
            moveSpeed = 8f;
            rigidBody2D.velocity = Vector3.zero;
            deathSoundPlayer.Play();
            deathSoundBlood.Play();
            if (Health > 0)
            {
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
                transform.position = GameObject.FindWithTag("Spawn").transform.position;
                Destroy(Instantiate(SpawningParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity), 1.0f);
                dead = false;
                Flip(false);
                spawnSound.Play();
            }
            else
            {
                transform.position = GameObject.FindWithTag("Spawn").transform.position;
                PlayerModel.SetActive(false);
                dead = false;
                gameObject.GetComponent<Rigidbody2D>().simulated = false;
                gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void HandleChest(Collider2D col)
    {
        if (isOnChest && col.gameObject.tag != "OpenChest")
        {
            if (Input.GetButton("Action"))
            {
                chestOpeningSound.Play();
                col.gameObject.tag = "OpenChest";
                var tmp = col.gameObject.name.Split(char.Parse("_"))[1];
                var item = chestItem[int.Parse(tmp)];
                StartCoroutine(HandleChestItem(item, 0.75f));
            }
        }
    }

    void HandleTakingDamage()
    {
        dead = true;
        Destroy(Instantiate(BloodParticle, transform.position, Quaternion.identity), 1.0f);
        camShake.Shakee(camShakeAmount, 0.1f);
        Health--;
        moveSpeed = 8f;
        rigidBody2D.velocity = Vector3.zero;
        Anim.Play("Die");
        deathSoundPlayer.Play();
        deathSoundBlood.Play();
        if (Health > 0)
            StartCoroutine(HandleRespawn(0.6f));
        else
            PlayerModel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            isOnChest = true;
            chestCol = collision;
        }
        if (collision.gameObject.tag == "Fire_Boss" && dead == false)
        {
            GameObject[] fireballs = GameObject.FindGameObjectsWithTag("Fire_Boss");

            GameObject.Find("Boss-Child").GetComponent<Animator>().Play("Idle");

            for (var i = 0; i < fireballs.Length; i++) {
                Destroy(fireballs[i]);
            }
            Destroy(GameObject.FindWithTag("Boss"), 1f);
            HandleTakingDamage();
        }
        if (collision.gameObject.tag == "Enemy" && dead == false)
            HandleTakingDamage();
        if (collision.gameObject.CompareTag("Water"))
        {
            onGrass = false;
            inWater = true;
            Destroy(Instantiate(SplashWater, transform.position, Quaternion.identity), 1.0f);
            EnterWater.Play();
        }
        if (collision.gameObject.CompareTag("Relic1"))
        {
            Relic1 = true;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (collision.gameObject.CompareTag("Relic2"))
        {
            Relic2 = true;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (collision.gameObject.CompareTag("Relic3"))
        {
            Relic3 = true;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            isOnChest = false;
            chestCol = collision;
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            onGrass = true;
            inWater = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap" && dead == false)
            HandleTakingDamage();
        if (collision.gameObject.CompareTag("Ground"))
        {
            rigidBody2D.velocity = Vector3.zero;
            TouchGrassSound.Play();
            if (inWater == false)
                Destroy(Instantiate(JumpParticle, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.identity), 1.0f);
            moveSpeed = 8f;
        }
        if (collision.gameObject.layer == 17) // Grass
        {
            onGrass = true;
            onSnow = false;
            onChest = false;
        }
        if (collision.gameObject.layer == 18) // Snow
        {
            onGrass = false;
            onSnow = true;
            onChest = false;
        }
        if (collision.gameObject.layer == 20) // Snow
        {
            onGrass = false;
            onSnow = false;
            onChest = true;

        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckIfGrounded();
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
            moveSpeed = 4f;
    }

    void Flip(bool bLeft)
    {
        if (bLeft == true)
        {
            left = true;
            right = false;
        }
        if (bLeft == false)
        {
            left = false;
            right = true;
        }
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit;

        LayerMask layermask = LayerMask.GetMask("Grass", "Snow", "ChestPlatform");
        Vector3 positionToCheck = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        float thickness = 0.5f;
        hit = Physics2D.CircleCast(positionToCheck, thickness, new Vector3(0, -0.5f, transform.position.z), 0.01f, layermask);
        if (hit == false)
            isGrounded = false;
        else if (hit.collider.CompareTag("Ground"))
        {
            dashCount = 0;
            isGrounded = true;
        }
    }

    IEnumerator HandleChestItem(string item, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (item == "Sword")
        {
            haveSword = true;
            unarmed = false;
            UnsheatSwordSound.Play();
        }
        if (item == "Life")
        {
            Health++;
            GetHealthSound.Play();
        }
    }

    IEnumerator HandleRespawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.GetComponent<Rigidbody2D>().simulated = true;
        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        transform.position = GameObject.FindWithTag("Spawn").transform.position;
        Destroy(Instantiate(SpawningParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity), 1.0f);
        dead = false;
        Flip(false);
        spawnSound.Play();
    }

    IEnumerator DashGravity(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        (CameraObject.GetComponent("MotionBlur") as MonoBehaviour).enabled = false;
        rigidBody2D.gravityScale = 3.0f;
        rigidBody2D.velocity = new Vector2(0, 0);
        moveSpeed = 8f;
    }
}