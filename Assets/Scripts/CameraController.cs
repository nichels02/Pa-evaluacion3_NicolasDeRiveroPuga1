using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    public Camera cam;

    [Header("Canvas/Menu")]
    //public GameObject Canvas;
    public GameObject CambDif;
    public GameObject Salir;
    public GameObject Fac;
    public GameObject Dif;
    public GameObject Winner;
    public GameObject Imag;
    public GameObject MuteButton;
    public GameObject SgtNivel;
    public GameObject MenuButton;
    public float shakeAmount;

    [Header("Audio PLayer")]
    public AudioSource OSTPlayer;
    public AudioClip Intro;
    public AudioClip Loop;
    public AudioClip win;

    [Header("Game States")]
    public bool Ended;
    public bool Paused;
    public bool mute;

    [Header("Game Objects")]
    public GameObject Player;
    public GameObject Inicio;
    public GameObject Final;
    public string LevelToLoad;

    float YMin;
    float YMax;
    bool destroyedDificulties;
    void Start()
    {
        //Encontrar los Game Objects
        Inicio = GameObject.FindGameObjectWithTag("Inicio");
        Final = GameObject.FindGameObjectWithTag("Final");
        Player = GameObject.FindGameObjectWithTag("Player");

        //Definir los límites de movimiento de cámara
        YMin = Inicio.transform.position.y - 0.5f;
        YMax = Final.transform.position.y - 4f;

        //Verificar que el canvas/menú esté desactivado
        //Canvas.SetActive(false);

        //Verificar que el AudioSource esté corriendo
        OSTPlayer.Play();

        //Verificar que el Game empieze sin pausa
        Paused = false;

        //Eliminar para Android
        destroyedDificulties = true;
        Destroy(CambDif);
        Destroy(Dif);
        Destroy(Fac);
    }

    void FixedUpdate()
    {
        float y = Mathf.Clamp(Player.transform.position.y, YMin, YMax);
        this.transform.position = new Vector3(0, y, this.transform.position.z);
        
        if (Player.GetComponent<PlayerController>().dead) StartCoroutine(Shaking());
    }

    void Update()
    {
        if (Ended) return;
        if (Input.GetButtonDown("Pause")) PauseMenu();

        if (OSTPlayer.clip == Intro && !OSTPlayer.isPlaying)
        {
            Debug.Log("Intro Played");
            OSTPlayer.clip = Loop;
            OSTPlayer.Play();
            OSTPlayer.loop = true;
        }
    }

    IEnumerator Shaking()
    {
        //Vector3 iniPos = this.transform.position;
        Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
        transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
        yield return new WaitForSeconds(0.5f);
        //Player.GetComponent<PlayerController>().dead = false;
    }

    public void PauseMenu()
    {
        Paused = !Paused;
        Salir.SetActive(Paused);
        Imag.SetActive(Paused);
        MuteButton.SetActive(Paused);
        if(!destroyedDificulties){
            Fac.SetActive(false);
            Dif.SetActive(false);
            CambDif.SetActive(Paused);
        }
        
        Player.GetComponent<PlayerController>().Paused = Paused;
    }

    //Cuando el jugador llega a la meta
    public void FinishLevel()
    {
        Ended = true;
        MenuButton.SetActive(false);

        //OSTPlayer.Stop();
        mute = false;
        OSTPlayer.volume = 1.0f;
        OSTPlayer.clip = win;
        OSTPlayer.Play();
        OSTPlayer.loop = false;
        Winner.SetActive(true);
        Salir.SetActive(true);
        SgtNivel.SetActive(true);
    }

    void CheckVolume()
    {
        OSTPlayer.mute = mute;
        if (mute) MuteButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "OFF";
        else MuteButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "ON";
    }

    //Funciones para Canvas/Menu

    public void FuncCambiarDif()
    {
        Fac.SetActive(true);
        Dif.SetActive(true);
    }

    public void FuncSalir()
    {
        SceneManager.LoadScene(0);
    }

    public void CambiarDificultad(bool easy)
    {
        PlayerController.Dificil = easy;
        Fac.SetActive(false);
        Dif.SetActive(false);
        PauseMenu();
    }

    public void MuteSound()
    {
        mute = !mute;
        CheckVolume();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
