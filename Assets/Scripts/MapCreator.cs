using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MapCreator : MonoBehaviour {

    [Serializable]
    public class Speed
    {
        public int min;
        public int max;

        public Speed(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    public int Roads;
    public static int level;
    public GameObject Veredas;
    public GameObject Grass;
    public GameObject Arbol;
    public GameObject Reja;
    public GameObject Pistas;
    public GameObject Inicio;
    public GameObject Final;
    public GameObject[] Coches;
    public GameObject Shawarmero;
    public GameObject Camera;
    int NumRoads;
    // Use this for initialization
    void Start () {
        CreateMapa();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateMapa()
    {
        GameObject Ini = Instantiate(Inicio, new Vector3(0, -3.5f, 0), Quaternion.identity) as GameObject;
        bool dummy = false;
        NumRoads = Random.Range(2, 26);
        for (int i = 0; i < NumRoads; i++)
        {
            if (i == 0 || i == NumRoads - 1) dummy = true;
            else dummy = false;
            GameObject Pista = Instantiate(Pistas, new Vector3(0, -3.5f + i, 0), Quaternion.identity) as GameObject;
            PutObject(-3.5f + i, dummy);
        }
        GameObject Fin = Instantiate(Final, new Vector3(0, -3.5f + 1*NumRoads + 2.5f, 0), Quaternion.identity) as GameObject;
        Instantiate(Camera, new Vector3(0f,0f,-10f), Quaternion.identity);
    }

    void PutObject(float y, bool can)
    {
        int ran = Random.Range(0, 6);
        switch (ran)
        {
            case 0:
                return;
            case 1: case 2:
                GameObject CarDer = Coches[Random.Range(0, Coches.Length)];
                CarDer.GetComponent<EnemyController>().GoingRight = true;
                CarDer.transform.localScale = new Vector3(1f, 1f, 1f);
                Instantiate(CarDer, new Vector3(-9.5f, y, 0), Quaternion.identity);
                break;
            case 3: case 4:
                GameObject CarIzq = Coches[Random.Range(0, Coches.Length)];
                CarIzq.GetComponent<EnemyController>().GoingRight = false;
                CarIzq.transform.localScale = new Vector3(-1f, 1f, 1f);
                Instantiate(CarIzq, new Vector3(9.5f, y, 0), Quaternion.identity);
                break;
            case 5:
                if (can) return;
                CreateSafePoint(y);
                break;
        }
    }

    void CreateSafePoint(float y)
    {
        Transform VeredaHolder = new GameObject("Vereda Hollder").transform;
        VeredaHolder.transform.position = new Vector3(0, y, 0);
        int shawrmaCreated = 0;
        for (int i = 0; i < 24; i++)
        {
            
            GameObject dummy;
            if (Random.Range(0, 2) <= 0.5f)
            {
                dummy = Instantiate(Veredas, new Vector3(-11.5f + i, y, -1), Quaternion.identity) as GameObject;
                if (Random.Range(0, 2) <= 0.5f) Instantiate(Reja, new Vector3(-11.5f + i, y, 0), Quaternion.identity);
            }
            else
            {
                dummy = Instantiate(Grass, new Vector3(-11.5f + i, y, 0), Quaternion.identity) as GameObject;
                if (Random.Range(0, 2) <= 0.75f) Instantiate(Arbol, new Vector3(-11.5f + i, y, 0), Quaternion.identity);
            }
            if(Random.Range(0,3) <= 0.25f && shawrmaCreated < 2)
            {
                GameObject Shawarma = Shawarmero;
                Shawarma.transform.position = new Vector3(-11.5f + i, y, 0);
                Shawarma.GetComponent<ShawarmeroController>().obj = new Vector2(0, y);
                Instantiate(Shawarma, Shawarma.transform.position, Quaternion.identity);
                shawrmaCreated++;
            }
            dummy.transform.SetParent(VeredaHolder.transform);
        }
    }
}
