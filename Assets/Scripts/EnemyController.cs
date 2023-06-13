using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {

    public int MoveVelocity;
    public bool GoingRight;
    public Rigidbody2D myRB2D;
    public float y;
    public float x;
    float speed;
    public float Velocity;
	// Use this for initialization
	void Start () {
        myRB2D = GetComponent<Rigidbody2D>();
        y = this.transform.position.y;
        x = this.transform.position.x;
        ChangeVelocity();
        if (GoingRight) speed = 1;
        else speed = -1;
        Velocity = Random.Range(2 * speed * MoveVelocity, 6 * speed * MoveVelocity);
        if (GoingRight) transform.localScale = new Vector3(1f, 1f, 1f);
        else transform.localScale = new Vector3(-1f, 1f, 1f);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        myRB2D.velocity = new Vector2(Velocity, myRB2D.velocity.y);
        ChangeVelocity();
        //Debug.Log(this.transform.position.x);
	}

    void ChangeVelocity()
    {
        if((GoingRight && this.transform.position.x <= 10.5f) || (!GoingRight && this.transform.position.x >= -10.5f)) return;

        myRB2D.transform.position = new Vector2(x, y);
        myRB2D.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        myRB2D.velocity = Vector3.zero;
        myRB2D.angularVelocity = 0f;
        Velocity = Random.Range(2 * speed * MoveVelocity, 6 * speed * MoveVelocity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            StartCoroutine(other.GetComponent<PlayerController>().Respawning());
    }
}
