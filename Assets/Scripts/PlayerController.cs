using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {

    [Header ("Player Components")]
    public Rigidbody2D myRB2D;
    public Animator animator;
    public GameObject utiles;

    [Header("Player Audio")]
    public AudioSource CachimboFoley;
    public AudioClip[] FoleysGritos;
    public AudioSource Claxon;

    [Header("Player stats")]
    public float velocity; //5
    public bool Finish; //false
    public int deadCount = 5; //5
    public bool Paused; //false
    public static bool Dificil; //false
    public bool dead = false; //false

    // Use this for initialization
    void Start () {
        myRB2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //FIX ROTATION
        if (dead || Finish || Paused) return;
        animator.SetFloat("Velocity", myRB2D.velocity.magnitude);

        if (Dificil)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                myRB2D.transform.Rotate(Vector3.forward * -Input.GetAxisRaw("Horizontal") * 5);
            if (Input.GetAxisRaw("Vertical") != 0)
                myRB2D.AddForce(transform.up * Input.GetAxisRaw("Vertical") * velocity*2);
            else
                myRB2D.velocity = Vector2.zero;
        }else
        {
            //keyboard
            float y = velocity * Input.GetAxisRaw("Vertical");
            float x = velocity * Input.GetAxisRaw("Horizontal");
            
            myRB2D.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            if (y != 0) myRB2D.velocity = new Vector2(0, y);
            else myRB2D.velocity = new Vector2(x, 0);
            //myRB2D.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * 0.25f, Input.GetAxisRaw("Vertical") * 0.25f, 0f);
        }

        if (this.transform.position.x < -9f || this.transform.position.x > 9f) StartCoroutine(Respawning());
    }

    void Testing()
    {
        myRB2D.transform.position += new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            StartCoroutine(Respawning());
            
        }
        if (other.tag == "Finish")
        {
            //CameraController cam = ;
            GameObject.FindWithTag("MainCamera").GetComponent<CameraController>().FinishLevel();
            Finish = true;
        }
    }

    public IEnumerator Respawning()
    {
        //yield return StartCoroutine(Camera.main.GetComponent<CameraController>().Shaking());
        this.dead = true;
        CachimboFoley.clip = FoleysGritos[Random.Range(0, 1)];
        CachimboFoley.Play();
        Claxon.Play();
        myRB2D.velocity = Vector2.zero;
        utiles.gameObject.transform.position = this.transform.position;
        myRB2D.angularVelocity = 0f;
        GameObject partyclesCreate = Instantiate(utiles, transform.position, Quaternion.identity) as GameObject;
        animator.SetTrigger("Dead");
        myRB2D.velocity = Vector2.zero; 
        yield return new WaitForSeconds(0.5f);
        dead = false;
        this.transform.position = new Vector2(0f, -6f);
        this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Destroy(partyclesCreate);
    }
}
