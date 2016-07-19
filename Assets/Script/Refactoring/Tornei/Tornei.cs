using UnityEngine;
using System.Collections;

public abstract class Tornei : MonoBehaviour
{
    public GameObject[] TipiGladiatori;
    public AudioClip[] AudioIntroduzione;
    public AudioClip[] AudioVittoria;
    public AudioClip[] AudioSconfitta;
    public int LivelloTorneo;
    protected AudioSource audioSource;
    protected EnemyControl enemy;
    protected PlayerControl playercontrol;
    protected Player_Script PlayerScript;
    public AudioSource AudioSpeaker;

    public abstract void Win();
    public abstract void Lose();
}
