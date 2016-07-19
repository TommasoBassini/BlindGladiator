using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class Duelli : MonoBehaviour
{
    public abstract void Win();
    public abstract void Lose();

    protected PlayerControl playercontrol;
    protected AudioSource audioSource;
    public GameObject[] TipiGladiatori;
    protected EnemyControl enemy;
    public AudioClip[] AudioIntroduzione;
    public AudioClip AudioSpiegazione;
    public AudioClip[] AudioVittoria;
    public AudioClip[] AudioSconfitta;
    protected int i;
    public int MoneteIniziali;
    public Gestione_audio_speaker AudioSpeaker;
    public AudioSource audioSourceSpeaker;

    void Start()
    {

        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/Duel/Intr");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/English/Duel/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/English/Duel/Lose");
            AudioSpiegazione = Resources.Load<AudioClip>("Localization/English/Duel/Duel 1 2");
        }
        else
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/Duel/Intr");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/Italian/Duel/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/Italian/Duel/Lose");
            AudioSpiegazione = Resources.Load<AudioClip>("Localization/Italian/Duel/Duel 1 2");
        }

            playercontrol = GameObject.Find("Player").GetComponent<PlayerControl>();
        audioSource = GetComponent<AudioSource>();
        audioSourceSpeaker = AudioSpeaker.GetComponent<AudioSource>();
        StartCoroutine("provaEnemy");
        i = Random.Range(0, 2);
        Invoke("combattimentoIniziato", AudioIntroduzione[i].length);
    }

    void combattimentoIniziato()
    {
        playercontrol.CombattimentoIniziato = true;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("ScenaDaCaricare"));
    }
}
