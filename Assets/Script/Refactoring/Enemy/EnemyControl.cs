using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyControl : MonoBehaviour 
{
	private PlayerControl playerControl;
	private Win_Lose win_Lose;
	 
	public int DannoMin;
	public int DannoMax;
	public float Tempo_danno;
	public int EnemyLife;
	public bool morto = false;
	private bool oneTime = false;
	private int random;
	public bool DannoPieno = false;
	public int VitaIniziale;

	public float DannoPienoTime;
	public float TimeInCombo;
	public float MinAtkTime;
	public float MaxAtkTime;

	private AudioSource audioSource;
	public AudioClip[] SuoniAttacco;
	public AudioClip[] SuoniColpitoPieno;
	public AudioClip[] SuoniColpitoParati;

	private Image AttaccoSinistro;
	private Image AttaccoCentrale;
	private Image AttaccoDestro;
	public Image panelSinistro;
	public Image panelDestro;
	public Image panelCentrale;

	public AudioClip SuonoMorte;

	private Tornei gestione;
	private Duelli duello;

	public int monete;

	void Start () 
	{
		VitaIniziale = EnemyLife;
        if (GameObject.FindGameObjectWithTag("Gestione"))
            gestione = GameObject.FindGameObjectWithTag("Gestione").GetComponent<Tornei>();
        if (GameObject.FindGameObjectWithTag("Duello"))
            duello = GameObject.FindGameObjectWithTag("Duello").GetComponent<Duelli>();

        AttaccoCentrale = GameObject.FindGameObjectWithTag ("Centrale").GetComponent<Image>();
		AttaccoSinistro = GameObject.FindGameObjectWithTag ("Sinistra").GetComponent<Image>();
		AttaccoDestro = GameObject.FindGameObjectWithTag ("Destra").GetComponent<Image>();
		panelCentrale = GameObject.FindGameObjectWithTag ("PanelCentrale").GetComponent<Image>();
		panelSinistro = GameObject.FindGameObjectWithTag ("PanelSinistro").GetComponent<Image>();
		panelDestro = GameObject.FindGameObjectWithTag ("PanelDestro").GetComponent<Image>();
		audioSource = GetComponent<AudioSource>();
		playerControl = GameObject.Find ("Player").GetComponent<PlayerControl>();
	}

    void Update ()
	{
		if (EnemyLife <= 0 && !oneTime)
		{
			morto = true;
			Morto ();
			audioSource.PlayOneShot (SuonoMorte);
			oneTime = true;
		}
	}

	public void IniziaBattaglia(float t)
	{
		Invoke ("Attacco",t);
		Debug.Log ("inizio Battaglia");
	}

	public void SuonoColpito ()
	{
		audioSource.panStereo = 0.0f;
		if (DannoPieno)
		{
			int SuonoRandom = Random.Range (0,SuoniColpitoPieno.Length);
			audioSource.PlayOneShot (SuoniColpitoPieno[SuonoRandom]);
		}
		else
		{
			int SuonoRandom = Random.Range (0,SuoniColpitoParati.Length);
			audioSource.PlayOneShot (SuoniColpitoParati[SuonoRandom]);
		}
	}

	void Attacco ()
	{
		DannoPieno = false;
		if(!morto && !playerControl.PlayerMorto)
		{
			random = Random.Range (0,8);
			switch(random)
			{
			case 0:
				AttackLeftAnim ();
				break;
			case 1:
				AttackRightAnim ();
				break;
			case 2:
				AttackCentralAnim ();
				break;
			case 3:
				StartCoroutine ("Combo1");
				break;
			case 4:
				StartCoroutine ("Combo2");
				break;
			case 5:
				StartCoroutine ("Combo3");
				break;
			case 6:
				StartCoroutine ("Combo4");
				break;
			case 7:
				StartCoroutine ("Combo5");
				break;
			}
		}

	}

	void AttackLeftAnim ()
	{
		audioSource.panStereo = -1.0f;
		SuonoAttacco();
		Color attaccoSinistro = AttaccoSinistro.color;
		attaccoSinistro.a = 255;
		AttaccoSinistro.color = attaccoSinistro;
		Color panelsinistro = panelSinistro.color;
		panelsinistro.a = 255;
		panelSinistro.color = panelsinistro;
		Invoke ("ResetAlphaSinistra",1);
		playerControl.AttaccoNemicoSinistro = true;
		playerControl.DannoNemico = Random.Range (DannoMin,DannoMax);
		playerControl.TempoDanno = Tempo_danno;
		playerControl.AttaccoNemico ();
		if (random == 0)
		{
			DannoPieno = true;
			Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));	
		}
	}

	void ResetAlphaSinistra()
	{
		Color attaccoSinistro = AttaccoSinistro.color;
		attaccoSinistro.a = 0;
		AttaccoSinistro.color = attaccoSinistro;
		Color panelsinistro = panelSinistro.color;
		panelsinistro.a = 0;
		panelSinistro.color = panelsinistro;
	}

	void AttackCentralAnim ()
	{
		audioSource.panStereo = 0.0f;
		SuonoAttacco();
		Color attaccoCentrale = AttaccoCentrale.color;
		attaccoCentrale.a = 255;
		AttaccoCentrale.color = attaccoCentrale;
		Color panelcentrale = panelCentrale.color;
		panelcentrale.a = 255;
		panelCentrale.color = panelcentrale;
		Invoke ("ResetAlphaCentrale",1);
		playerControl.AttaccoNemicoCentrale = true;
		playerControl.DannoNemico = Random.Range (DannoMin,DannoMax);
		playerControl.TempoDanno = Tempo_danno;
		playerControl.AttaccoNemico ();
		if (random == 2)
		{
			DannoPieno = true;
			Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));	
		}	
	}

	void ResetAlphaCentrale()
	{
		Color attaccoCentrale = AttaccoCentrale.color;
		attaccoCentrale.a = 0;
		AttaccoCentrale.color = attaccoCentrale;
		Color panelcentrale = panelCentrale.color;
		panelcentrale.a = 0;
		panelCentrale.color = panelcentrale;
	}

	void AttackRightAnim ()
	{
		audioSource.panStereo = 1.0f;
		SuonoAttacco();
		Color attaccoDestro = AttaccoDestro.color;
		attaccoDestro.a = 255;
		AttaccoDestro.color = attaccoDestro;
		Color paneldestro = panelDestro.color;
		paneldestro.a = 255;
		panelDestro.color = paneldestro;
		Invoke ("ResetAlphaDestra",1);
		playerControl.AttaccoNemicoDestro = true;
		playerControl.DannoNemico = Random.Range (DannoMin,DannoMax);
		playerControl.TempoDanno = Tempo_danno;
		playerControl.AttaccoNemico ();
		if (random == 1)
		{
			DannoPieno = true;
			Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));	
		}
	}

	void ResetAlphaDestra()
	{
		Color attaccoDestro = AttaccoDestro.color;
		attaccoDestro.a = 0;
		AttaccoDestro.color = attaccoDestro;
		Color paneldestro = panelDestro.color;
		paneldestro.a = 0;
		panelDestro.color = paneldestro;
	}

	void SuonoAttacco()
	{
		int SuonoRandom = Random.Range (0,SuoniAttacco.Length);
		audioSource.PlayOneShot (SuoniAttacco[SuonoRandom]);
	}

	IEnumerator Combo1 ()
	{
		AttackLeftAnim ();
		yield return new WaitForSeconds(TimeInCombo);
		if(!playerControl.PlayerMorto)
			AttackRightAnim ();
		yield return new WaitForSeconds(TimeInCombo);
		if(!playerControl.PlayerMorto)
			AttackCentralAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
		}
		yield return new WaitForSeconds(DannoPienoTime);
		Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));
	}

	IEnumerator Combo2 ()
	{
		AttackRightAnim ();
		yield return new WaitForSeconds(TimeInCombo);
		if(!playerControl.PlayerMorto)
			AttackCentralAnim ();
		yield return new WaitForSeconds(TimeInCombo);
		if(!playerControl.PlayerMorto)
			AttackLeftAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
		}
		yield return new WaitForSeconds(DannoPienoTime);
		Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));
	}

	IEnumerator Combo3 ()
	{
		AttackCentralAnim ();
		yield return new WaitForSeconds(TimeInCombo);
		if(!playerControl.PlayerMorto)
			AttackCentralAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
		}
		yield return new WaitForSeconds(DannoPienoTime);
		Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));
	}

	IEnumerator Combo4 ()
	{
		AttackRightAnim ();
		yield return new WaitForSeconds(TimeInCombo);		
		if(!playerControl.PlayerMorto)
			AttackLeftAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
		}
		yield return new WaitForSeconds(DannoPienoTime);
		Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));
	}

	IEnumerator Combo5 ()
	{
		AttackCentralAnim ();
		yield return new WaitForSeconds(TimeInCombo);		
		if(!playerControl.PlayerMorto)
			AttackLeftAnim ();
		yield return new WaitForSeconds(TimeInCombo);
		if(!playerControl.PlayerMorto)
			AttackRightAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
		}
		yield return new WaitForSeconds(DannoPienoTime);
		Invoke ("Attacco",Random.Range(MinAtkTime,MaxAtkTime));
	}

	public void AttaccoSpeciale ()
	{
		StopAllCoroutines ();
		StartCoroutine ("AttaccoSpecialeCoroutines");
	}
	
	IEnumerator AttaccoSpecialeCoroutines ()
	{
		CancelInvoke ("Attacco");
		DannoPieno = true;
		yield return new WaitForSeconds(5.0f);
		DannoPieno = false;
		Invoke ("Attacco",0.0f);
	}
	
	public void Morto ()
	{
		if(GameObject.FindGameObjectWithTag("Gestione"))
			gestione.Win ();
		if(GameObject.FindGameObjectWithTag("Duello"))
			duello.Win ();
		morto = true;
		StopAllCoroutines ();
		CancelInvoke ("Attacco");
	}
}
