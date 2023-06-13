using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShawarmeroController : MonoBehaviour {


    public bool IsStatic;
    public Vector2 obj;
    public Vector2 ini;
    public GameObject Player;
    public Animator animator;
    public bool ObjISEnd;
    public float speed;
    public GameObject[] Mensajes;
    bool talking;

    void Start()
    {
        ini = this.transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    
    
	void FixedUpdate () {
        if (!IsStatic) MoveShawarma();
	}

    void RotateShawarma()
    {
        transform.LookAt(Player.transform);
    }

    void MoveShawarma()
    {
        if (!ObjISEnd) transform.position = Vector2.MoveTowards(transform.position, obj, speed);
        else transform.position = Vector2.MoveTowards(transform.position, ini, speed);
        Vector2 dummy = transform.position;
        if(dummy == ini || dummy == obj)
        {
            ObjISEnd = !ObjISEnd;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !talking)
            StartCoroutine(Hablar(Random.Range(1,Mensajes.Length)));
    }

    IEnumerator Hablar(int n)
    {
        talking = true;
        Mensajes[n].SetActive(true);
        yield return new WaitForSeconds(3.5f);
        Mensajes[n].SetActive(false);
        talking = false;
    }
}
