using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D _rb;

    [SerializeField] float _speed = 10f;
    int angle;
    int maxAngle = 20;
    int minAngle = -60;
    public Score score;
    public GameManager gameManager;
    bool touchedGround;
    public Sprite fishDied;
    SpriteRenderer sp;
    Animator anim;
    [SerializeField] private AudioSource swim, hit, point;



    public ObstacleSpawner obstacleSpawner;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FishSwim();
    }

    private void FixedUpdate()
    {

        FishRotation();
    }


    void FishSwim()
    {

        if (Input.GetMouseButtonDown(0) && !GameManager.gameOver)
        {
            if (!swim.isPlaying) swim.Play();
            if (!GameManager.gameStarted)
            {
                _rb.gravityScale = 4f;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
                obstacleSpawner.InstantiateObstacle();
                gameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
            }
        }
    }


    void FishRotation()
    {
        var verticalVel = _rb.velocity.y;

        if (verticalVel > 0)
        {
            if (angle <= maxAngle)
            {
                angle += 4;
            }

        }
        else if (verticalVel < -1.2)
        {
            if (angle > minAngle) angle -= 4;
        }
        if (!touchedGround)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            score.Scored();
            if (!point.isPlaying) point.Play();
        }
        else if (other.CompareTag("Column") && !GameManager.gameOver)
        {
            FishDieEffect();
            gameManager.GameOver();
        }
    }

    void FishDieEffect()
    {
        if (!hit.isPlaying) hit.Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!GameManager.gameOver)
            {
                //game over
                FishDieEffect();
                gameManager.GameOver();
                GameOver();
            }
            else
            {
                gameManager.GameOver();
                GameOver();
            }
        }

    }

    void GameOver()
    {
        touchedGround = true;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        sp.sprite = fishDied;
        anim.enabled = false;
    }


}
