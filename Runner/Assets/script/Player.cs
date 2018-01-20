using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class Player : MonoBehaviour {

    private Rigidbody rb;
    [SerializeField]
    private float speed;

    private Animator anim;
    float lengtinZaxis = 7.6f;

    [SerializeField]
    Text scoreUI;


    [SerializeField]
    GameObject platform;

    [SerializeField]
    GameObject robot;

    [SerializeField]
    Transform firstObject;

    [SerializeField]
    AudioClip jumpsound;

    [SerializeField]
    GameObject runSteps;

    [SerializeField]
    GameObject attackSound;

    [SerializeField]
    GameObject gameOver;


    //pozycja ostatniej platformy
    Vector3 lastposition;
    //Pozycja pierwszej platformy
    Vector3 firstPlatform;
    //Licznik platform
    int counterPlatform = 4;
    //wynik
    int score = 0;
    float _score = 0f;
    bool doublejump = true;
    bool jump = false;
    bool attack = false;
    bool nextAttack = true;
    bool destroy = false;
    bool startcounter = false;
    int speedlv = 0;
    bool gameover = false;
    float temp;
    AudioSource sounds;

    void Start () {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        sounds = GetComponent<AudioSource>();
        runSteps.GetComponent<AudioSource>().Play();
        rb.velocity = new Vector3(0f, 0f, speed);
        lastposition = firstObject.transform.position;
        firstPlatform = GameObject.Find("Ground" + 2).transform.position;
        //InvokeRepeating("PlatformUpdate", 0f, 0.3f);

        //Rysowanie pierwszych 20 platform
        for (int x = 4; x < 20; x++)
        {
            SpawningPlatform(x);
            SpawningRobot(x);
        }
    }

    private void SpawningPlatform(int numberPlatform) {
        GameObject _object = Instantiate(platform) as GameObject;      
        _object.name = "Ground" + numberPlatform;    
        int _randomGap = Random.Range(0, 7);
        
        if(_randomGap <=4)
        {
            _object.transform.position = lastposition + new Vector3(0f, 0f, lengtinZaxis);
        }
        else
        {
            _object.transform.position = lastposition + new Vector3(0f, 0f, 11f);
            
            
        }

        lastposition = _object.transform.position;
        
    }

    private void SpawningRobot(int numberRobot)
    {
        GameObject _robot = Instantiate(robot) as GameObject;
        _robot.name = "Robot" + numberRobot;
        int _randomRobot = Random.Range(1, 9);
        if (_randomRobot == 1)
        {
            _robot.transform.position = lastposition + new Vector3(-1.24f, 0f, -0.85f);
        }
        if (_randomRobot == 2)
        {
            _robot.transform.position = lastposition + new Vector3(-1.24f, 0f, -3.5f);
        }
        if (_randomRobot == 3)
        {
            _robot.transform.position = lastposition + new Vector3(-1.24f, 0f, -6f);
        }
        if (_randomRobot == 4)
        {
            _robot.transform.position = lastposition + new Vector3(1.24f, 0f, -0.85f);
        }
        if (_randomRobot == 5)
        {
            _robot.transform.position = lastposition + new Vector3(1.24f, 0f, -3.5f);
        }
        if (_randomRobot == 6)
        {
            _robot.transform.position = lastposition + new Vector3(1.24f, 0f, -6f);
        }
        if (_randomRobot == 7)
        {
            _robot.transform.position = lastposition + new Vector3(0f, 0f, -0.85f);
        }
        if (_randomRobot == 8)
        {
            _robot.transform.position = lastposition + new Vector3(0f, 0f, -3.5f);
        }
        if (_randomRobot == 9)
        {
            _robot.transform.position = lastposition + new Vector3(0f, 0f, -6f);
        }
        counterPlatform++;
        if (counterPlatform == 20)
        {
            counterPlatform = 1;
        }
    }

    private void PlatformUpdate()
    {
        if ( rb.transform.position.z > firstPlatform.z+1f)
        {
            Destroy(GameObject.Find("Ground" + counterPlatform));
            Destroy(GameObject.Find("Robot" + counterPlatform));
            if (startcounter)
            {
                SocerUpdate();
            }
            speedlv++;
            if (speedlv == 5)
                {
                rb.velocity = new Vector3(0f, 0f, speed);
                }
            if (speedlv > 10) {
                speed++;
                rb.velocity = new Vector3(0f, 0f, speed);
                speedlv = 0;
            }

            SpawningPlatform(counterPlatform);
            SpawningRobot(counterPlatform);
            firstPlatform = GameObject.Find("Ground" + (counterPlatform)).transform.position;
            startcounter = true;
            }
    }

    void SocerUpdate()
    {
        _score++;
        score = Mathf.RoundToInt(_score);
        scoreUI.text = score.ToString();
    }

    public void Jump()
    {
        if (doublejump && gameover == false)
        {
            jump = true;
        }
    }

    void playAttackSound()
    {
        attackSound.GetComponent<AudioSource>().Play();
    }

    public void Attack()
    {
        if (nextAttack && gameover == false)
        {
            playAttackSound();
            Invoke("playAttackSound", 0.5f);
            nextAttack = false;
            attack = true;
            destroy = true;
        }

    }

    void delayAttack()
    {
        nextAttack = true;
        destroy = false;
    }
    
    void Update () {
        if (jump && doublejump)
        {
            doublejump = false;
            jump = false;
            runSteps.GetComponent<AudioSource>().Stop();
            rb.AddForce(0f, 5f, 0f, ForceMode.Impulse);
            anim.Play("Jumping");
            
        }
        if (attack)
        {
            attack = false;
            anim.Play("Attack");
            playAttackSound();
            Invoke("playAttackSound", 0.5f);
            Invoke("delayAttack", 0.9f);

        }
        if (gameover == false)
        {
            temp = Input.acceleration.x / 4;
            transform.Translate(temp, 0, 0);
        }
        PlatformUpdate();
    }





    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Touch" && gameover == false)
        {   if (doublejump == false)
            {
                sounds.PlayOneShot(jumpsound);
                runSteps.GetComponent<AudioSource>().Play();
            }
            doublejump = true;
        }
        if (col.gameObject.tag == "Water" && gameover == false)
        {
            gameover = true;
            rb.velocity = new Vector3(0f, 0f, 0f);
            runSteps.GetComponent<AudioSource>().Stop();
            sounds.Stop();
            gameOver.GetComponent<AudioSource>().Play();
            anim.Play("Dead");
            Invoke("GameOver", 3.204f);
        }
        if (col.gameObject.tag == "Robot" && destroy == false && gameover == false)
        {
            gameover = true;
            rb.velocity = new Vector3(0f, 0f, 0f);
            runSteps.GetComponent<AudioSource>().Stop();
            sounds.Stop();
            gameOver.GetComponent<AudioSource>().Play();
            anim.Play("Dead");
            Invoke("GameOver", 3.204f);
        }
        else if(col.gameObject.tag == "Robot" && destroy == true && gameover == false)
        {
            Destroy(col.gameObject);
        }
    }

    private void GameOver()
    {
        PlayerPrefs.SetString("Score", score.ToString());
        SceneManager.LoadScene("EndGame");
    }
    int getScore()
    {
        return score;
    }
}
