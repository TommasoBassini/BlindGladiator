using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Gestione_audio_speaker : MonoBehaviour 
{
	public AudioSource audioSource;
	public AudioSource audioSource2;
	public AudioClip[] colpito;
	public AudioClip[] scudo;
	public AudioClip[] attaccoPieno;
	public AudioClip[] attaccoParato;
	public AudioClip[] findivita;
	public AudioClip[] uncolpomorto;
	public AudioClip[] enemyFinDiVita;
	private bool FinVitaDetto = false;
	private bool UnColpoMortoDetto = false;
	private bool EnemyFinDiVita = false;

    public bool isEnea = false;

    void Awake()
    {
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            colpito = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/Colpito");
            scudo = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/Scudo");
            attaccoPieno = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/AttaccoPieno");
            attaccoParato = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/AttaccoParato");
            findivita = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/FinDiVita");
            uncolpomorto = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/UnColpoMorto");
            enemyFinDiVita = AudioGestor.AudioClipArrayLoading("Localization/English/Speaker/EnemyFinDiVita");

        }
        else
        {
            colpito = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/Colpito");
            scudo = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/Scudo");
            attaccoPieno = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/AttaccoPieno");
            attaccoParato = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/AttaccoParato");
            findivita = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/FinDiVita");
            uncolpomorto = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/UnColpoMorto");
            enemyFinDiVita = AudioGestor.AudioClipArrayLoading("Localization/Italian/Speaker/EnemyFinDiVita");
        }
    }

	public void Colpito ()
	{
        if (!isEnea)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(colpito[Random.Range(0, colpito.Length)]);
            }
        }
	}

	public void Scudo ()
	{
        if (!isEnea)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(scudo[Random.Range(0, scudo.Length)]);
            }
        }
	}

	public void Attacco_pieno ()
	{
        if (!isEnea)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(attaccoPieno[Random.Range(0, attaccoPieno.Length)]);
            }
        }
	}

	public void Attacco_parato ()
	{
        if (!isEnea)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(attaccoParato[Random.Range(0, attaccoParato.Length)]);
            }
        }
	}

	public void Fin_di_vita ()
	{
        if (!isEnea)
        {
            if (!FinVitaDetto)
            {
                int i = Random.Range(0, findivita.Length);
                audioSource.mute = true;
                audioSource2.PlayOneShot(findivita[i]);
                Invoke("unmute", findivita[i].length);
                FinVitaDetto = true;
            }
        }
	}

	public void Un_colpo_morto ()
	{
        if (!isEnea)
        {
            if (!UnColpoMortoDetto)
            {
                int i = Random.Range(0, uncolpomorto.Length);
                audioSource2.PlayOneShot(uncolpomorto[i]);
                Invoke("unmute", uncolpomorto[i].length);
                UnColpoMortoDetto = true;
            }
        }
	}

	public void Enemy_Fin_di_vita ()
    {
        if (!isEnea)
        {
            if (!EnemyFinDiVita)
            {
                int i = Random.Range(0, enemyFinDiVita.Length);
                audioSource.mute = true;
                audioSource2.PlayOneShot(enemyFinDiVita[i]);
                EnemyFinDiVita = true;
                Invoke("unmute", enemyFinDiVita[i].length);
            }
        }
	}

	void unmute()
	{
		audioSource.mute = false;
	}
}
