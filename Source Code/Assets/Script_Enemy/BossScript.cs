using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private CompleteCameraController myCamera;

    public GameObject bloodParticle;

    public float moveSpeedForward;
    public float jumpingLandSpeed;
    public float returnToSpawnSpeed;
    public int healthPoints;
    public int numberJump;
    public float timeBetweenAttack;
    public Transform ForwardPoint;
    public Transform SpawnPoint;
    public GameObject FireBall;
    public int nbFireballsPerWave;
    public Transform DMPoint;
    public int fireballSpeed;

    public BoxCollider2D invisibleWall;

    private GameObject player;
    private GameObject plateform;
    private GameObject sawDoor;
    private GameObject escapePlateform;
    private AudioSource Audio;

    private Animator bossAnim;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer sprite_render;
    private Vector3 downPlateformPos;
    private Vector3 upPlateformPos;

    private bool isRushing = false;
    private bool upPlateform = false;

    private bool isReturningSpawn;

    private int attackNb;

    private Vector3 jumpPosition;
    private bool isJumping;
    private int jumpCount;
    private bool isGrounded;
    private bool jumpAnimInProg;
    private float jump_x_pos;

    private Vector3 DivePoint;
    private Vector3 PlayerDivePoint;
    private int diveCount;
    private bool isDivingUp;
    private bool isDivingPlayer;

    private Dictionary<GameObject, Vector3> fireballArray = new Dictionary<GameObject, Vector3>();
    private bool bossDM_Point = false;
    public bool bossDM_attack = false;

    public bool isAgressive;

    private PlayerControl PlayerScript;
    Shake camShake;

    public AudioClip AttackDash;
    public AudioClip AttackDive;
    public AudioClip AttackJumping;
    public AudioClip TakingDamageSound;
    public AudioClip DeathSound;

    public AudioClip StompSound;

    void Start()
    {
        PlayerScript = FindObjectOfType<PlayerControl>();
        camShake = GameObject.FindGameObjectWithTag("Effect").GetComponent<Shake>();
        myCamera = FindObjectOfType<CompleteCameraController>();
        invisibleWall = GameObject.Find("InvisibleWallBoss").GetComponent<BoxCollider2D>();
        Audio = GetComponent<AudioSource>();

        isRushing = false;
        upPlateform = false;
        isReturningSpawn = false;
        attackNb = 1;
        jumpPosition = new Vector3(0, 0, 0);
        isJumping = false;
        jumpCount = 0;
        isGrounded = true;
        jumpAnimInProg = false;
        jump_x_pos = 0.0f;

        DivePoint = new Vector3(0, 0, 0);
        PlayerDivePoint = new Vector3(0, 0, 0);
        diveCount = 0;
        isDivingUp = false;
        isDivingPlayer = false;
        player = GameObject.FindWithTag("Player");
        plateform = GameObject.Find("BossPlateform");
        sawDoor = GameObject.Find("Saw_Door");
        escapePlateform = GameObject.Find("GrassContainerBoss");
        downPlateformPos = plateform.transform.position;
        upPlateformPos = new Vector3(plateform.transform.position.x, plateform.transform.position.y + 5.0f, plateform.transform.position.z);
        jumpCount = numberJump;
        bossAnim = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        rigidBody2D.gravityScale = jumpingLandSpeed;
    }

    public void StartAgain()
    {
        isAgressive = true;
        if (isAgressive)
            StartCoroutine(chooseAttack(timeBetweenAttack));
    }

    void Jump_Animation()
    {
        jumpCount = 0;
        bossAnim.Play("Attack_Jump");
    }

    void Forward_Animation()
    {
        upPlateform = true;
        bossAnim.Play("Attack_Forward");
    }

    IEnumerator Dive_Attack(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        DivePoint.x = transform.position.x;
        DivePoint.y = transform.position.y + 3.0f;
        DivePoint.z = transform.position.z;
        rigidBody2D.velocity = new Vector2(0, 0);
        rigidBody2D.gravityScale = 0.0f;
        isDivingUp = true;
    }

    void Invocation_Attack()
    {
        fireballArray.Clear();
        rigidBody2D.velocity = new Vector2(0, 0);
        rigidBody2D.gravityScale = 0.0f;
        bossDM_Point = true;
    }

    void invoke_multiple_fireballs()
    {
        for (int i = 0; i < nbFireballsPerWave; i++)
        {
            invoke_fireball();
        }
    }

    void invoke_fireball()
    {
        int x_pos_fireball = Random.Range(751, 776);
        Vector3 fireball_pos = new Vector3(x_pos_fireball, 4.5f, 0);

        fireballArray.Add(Instantiate(FireBall, transform.position, Quaternion.identity), fireball_pos);
    }

    void Update()
    {
        if (bossDM_Point)
        {
            transform.position = Vector3.MoveTowards(transform.position, DMPoint.position, Time.deltaTime * 30);
            if (transform.position == DMPoint.position)
            {
                bossDM_Point = false;
                bossDM_attack = true;
                bossAnim.Play("DM");
            }
        }
        if (bossDM_attack)
        {
            foreach (KeyValuePair<GameObject, Vector3> entry in fireballArray)
            {
                if (entry.Key != null)
                {
                    entry.Key.transform.position = Vector3.MoveTowards(entry.Key.transform.position, entry.Value, Time.deltaTime * fireballSpeed);
                    if (entry.Key.transform.position == entry.Value)
                        Destroy(entry.Key);
                }
            }
            if (fireballArray.Count == 0 && isReturningSpawn)
                bossDM_attack = false;
        }
        if (!isRushing || !isReturningSpawn)
        {
            FlipTowardsPlayer();
        }
        if (upPlateform)
        {
            plateform.transform.GetChild(0).gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
            plateform.transform.position = Vector3.MoveTowards(plateform.transform.position, upPlateformPos, Time.deltaTime * 9);

        }
        else
        {
            plateform.transform.GetChild(0).gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            plateform.transform.position = Vector3.MoveTowards(plateform.transform.position, downPlateformPos, Time.deltaTime * 9);
        }
            
        if (isReturningSpawn)
        {
            FlipRight();
            transform.position = Vector3.MoveTowards(transform.position, SpawnPoint.position, Time.deltaTime * returnToSpawnSpeed);
            if (transform.position == SpawnPoint.position)
            {
                FlipLeft();
                if (upPlateform)
                    upPlateform = false;
                isReturningSpawn = false;
                if (isAgressive)
                    StartCoroutine(chooseAttack(timeBetweenAttack));
            }
        }
        if (isRushing)
        {
            FlipLeft();
            transform.position = Vector3.MoveTowards(transform.position, ForwardPoint.position, Time.deltaTime * moveSpeedForward);
            if (transform.position == ForwardPoint.position)
            {
                camShake.Shakee(0.1f, 0.1f);
                Audio.clip = StompSound;
                Audio.Play();
                FlipRight();
                isRushing = false;
                returnToSpawn();
            }
        }
            
        JumpingAttackPhysics();
        DivingAttackPhysics();
    }

    void ForwardAttack()
    {
        isRushing = true;
    }

    void DivingAttackPhysics()
    {
        if (isDivingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, DivePoint, Time.deltaTime * 5);
            if (transform.position == DivePoint)
            {
                isDivingUp = false;
                bossAnim.Play("Prepare_Dive");
            }
        }
        if (isDivingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerDivePoint, Time.deltaTime * moveSpeedForward / 1.2f);
            if (transform.position == PlayerDivePoint)
            {
                isDivingPlayer = false;
                rigidBody2D.gravityScale = jumpingLandSpeed;
                diveCount++;
                camShake.Shakee(0.1f, 0.1f);
                Audio.clip = StompSound;
                Audio.Play();
                if (diveCount < jumpCount)
                    StartCoroutine(Dive_Attack(1.0f));
                else
                {
                    diveCount = 0;
                    returnToSpawn();
                }
            }
        }

    }

    void DiveToPlayer()
    {
        isDivingPlayer = true;
        PlayerDivePoint = player.transform.position;
        PlayerDivePoint.y = player.transform.position.y + 0.5f;
    }

    void JumpingAttackPhysics()
    {
        if (isJumping)
        {
            transform.position = Vector3.MoveTowards(transform.position, jumpPosition, Time.deltaTime * 80);
            if (transform.position == jumpPosition)
            {
                isJumping = false;
                transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                rigidBody2D.gravityScale = jumpingLandSpeed;
                jumpCount++;
                if (jumpCount >= numberJump)
                    returnToSpawn();
            }
        }
        if (isGrounded && jumpCount < numberJump && !isJumping && !jumpAnimInProg)
        {
            camShake.Shakee(0.1f, 0.1f);
            Audio.clip = StompSound;
            Audio.Play();
            jumpAnimInProg = true;
            jump_x_pos = transform.position.x;
            bossAnim.Play("Attack_Jump");
        }
        if (jumpAnimInProg)
            transform.position = new Vector3(jump_x_pos, transform.position.y, transform.position.z);
    }



    void JumpAttack()
    {   
        jumpPosition.y = transform.position.y + 20.0f;
        jumpPosition.z = transform.position.z;
        jumpPosition.x = transform.position.x;
        rigidBody2D.velocity = new Vector2(0, 0);
        rigidBody2D.gravityScale = 0.0f;
        isJumping = true;
        jumpAnimInProg = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == 17)
            isGrounded = true;
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            upPlateform = false;
            isJumping = false;
            isRushing = false;
            jumpCount = numberJump;
            isDivingPlayer = false;
            isDivingUp = false;
            diveCount = 0;
            rigidBody2D.gravityScale = jumpingLandSpeed;
            isAgressive = false;
            isReturningSpawn = true;
            bossDM_attack = false;
            bossDM_Point = false;
            foreach (KeyValuePair<GameObject, Vector3> entry in fireballArray)
            {
                Destroy(entry.Key);
            }
            Destroy(gameObject, 1f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword" && gameObject.tag != "Dying")
        {
            StartCoroutine(HitChangeColor());
            camShake.Shakee(0.1f, 0.1f);
            Audio.clip = TakingDamageSound;
            Audio.Play();
            healthPoints--;
            Destroy(Instantiate(bloodParticle, transform.position, Quaternion.identity), 1.0f);
            if (healthPoints <= 0)
            {
                foreach (KeyValuePair<GameObject, Vector3> entry in fireballArray)
                {
                    Destroy(entry.Key);
                }
                Audio.clip = DeathSound;
                Audio.Play();
                PlayerScript.bossKilled = true;
                invisibleWall.enabled = false;
                myCamera.Target = player;
                Destroy(sawDoor);
                escapePlateform.transform.position = new Vector3(escapePlateform.transform.position.x, escapePlateform.transform.position.y + 2.0f, escapePlateform.transform.position.z);
                bossAnim.Play("Die");
                gameObject.tag = "Dying";
                Destroy(gameObject, 0.8f);
            }
        }
    }

    public void FireballTouchPlayer()
    {
        print("enter");
        bossDM_attack = false;
        bossDM_Point = false;
        foreach (KeyValuePair<GameObject, Vector3> entry in fireballArray)
        {
            Destroy(entry.Key);
        }
        Destroy(gameObject, 1f);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 17)
            isGrounded = false;
    }

    void FlipLeft()
    {
        sprite_render.flipX = false;
    }

    void FlipRight()
    {
        sprite_render.flipX = true;
    }

    void FlipTowardsPlayer()
    {
        if (player.transform.position.x > transform.position.x)
            sprite_render.flipX = true;
        else
            sprite_render.flipX = false;
    }

    IEnumerator chooseAttack(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (isAgressive)
        {
            attackNb = Random.Range(0, 101);

            if (attackNb >= 0 && attackNb <= 25)
                Jump_Animation();
            else if (attackNb >= 26 && attackNb <= 50)
                StartCoroutine(Dive_Attack(1.0f));
            else if (attackNb >= 51 && attackNb <= 85)
                Forward_Animation();
            else
                Invocation_Attack();
        }
        
    }

    void returnToSpawn()
    {
        isReturningSpawn = true;
    }

    void endDMPhase()
    {
        rigidBody2D.gravityScale = jumpingLandSpeed;
        isReturningSpawn = true;
    }

    IEnumerator HitChangeColor()
    {
        for (float f = 0; f <= 0.1f; f += Time.deltaTime)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(255 / 255, 0 / 255, 0 / 255), f / 0.1f);
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255, 0 / 255, 0 / 255);
        StartCoroutine(HitBackColor());
    }

    IEnumerator HitBackColor()
    {
        for (float f = 0; f <= 0.1f; f += Time.deltaTime)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(255 / 255, 0 / 255, 0 / 255), Color.white, f / 0.1f);
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
