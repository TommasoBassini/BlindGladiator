using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Player_Tutorial : MonoBehaviour 
{
	public AudioClip[] AudioSpiegazioni;
	public AudioSource audioSource;
	public AudioSource audioSource2;

	private bool tutorial1_iniziato = false;

	private bool AzioneInCorso = false;
	public bool AttaccoNemicoDestro = false;									//Serie di Bool per attacchi nemico dalla destra
	private bool SchivataDestraWork = false;
	public bool AttaccoDestroSchivato = false;
	
	public bool AttaccoNemicoSinistro = false;									//Serie di Bool per attacchi nemico dalla sinistra
	public bool AttaccoSinistroSchivato = false;
	private bool SchivataSinistraWork = false;
	
	public bool AttaccoNemicoCentrale = false;									//Serie di Bool per attacchi nemico centrali
	public bool AttaccoCentraleSchivato = false;
	private bool SchivataCentraleWork = false;

	private int NSchivate = 0;
	public AudioClip OK;
	
	public float TempoDanno;													//Il tempo che hai per reagire
	public float Tempo_danno;

	public AudioClip SuoniDodgeLaterale;
	public AudioClip SuoniDodgeCentrale;

	public AudioClip[] Attacco;
	public AudioClip[] SuoniAttaccoNemico;
	public AudioClip[] SuonoDanno;

	private int random;
	private int NAttacchiSchivati = 0;

	public bool ScudoAlzato = false;											//Bool per sapere se lo scudo e' alzato
	public AudioClip[] HitScudo;
	private int NParate = 0;

	public AudioClip[] SuoniColpito;


	private float CurrAction = 0;												//Float crescente, a 100 si puo' fare un azione
	public float SpeedAction;													//La velocita' con cui cresce l'action Bar
	public Text UiText;															// GameObject per UI
	public Image ActionBar; 													// GameObject per UI
	public AudioClip BarraFull;
	private int NAttacchi = 0;

	public Image AttaccoSinistro;
	public Image AttaccoCentrale;
	public Image AttaccoDestro;
	public Image panelSinistro;
	public Image panelDestro;
	public Image panelCentrale;

	public Image schivataSinistra;
	public Image schivataCentrale;
	public Image schivataDestra;

	public AudioSource audioSource1;
	public AudioClip Affaticamento;
	private bool affaticato = false;

	private bool una_volta1 = false;
	private bool una_volta2 = false;
	private bool una_volta3 = false;
	private bool una_volta4 = false;
	private bool una_volta5 = false;


	public bool PlayerMorto = false;
	public bool AttaccoPossibile = false;
	public AudioClip pausaSpiegazioni;

	void Start () 
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/Tutorial/Parte 1");
            pausaSpiegazioni = Resources.Load<AudioClip>("Localization/Italian/Tutorial/Tut 3");
        }
        else
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/Tutorial/Parte 1");
            pausaSpiegazioni = Resources.Load<AudioClip>("Localization/Italian/Tutorial/Tut 3");
        }

        SpeedAction = PlayerPrefs.GetFloat ("Velocita");
		audioSource.PlayOneShot (AudioSpiegazioni[0]);
		Invoke ("Tutorial1",AudioSpiegazioni[0].length + 1);
		InvokeRepeating ("DecreaseActionBar",0.01f,0.01f);						//Invoca la funzione che fa crescere l'action Bar

	}
	
	void Update () 
	{
		ActionBar.fillAmount = CurrAction / 100;								//Funzione per UI
		UiText.text = CurrAction.ToString ("0");								//Funzione per UI
		
		if (CurrAction >= 100 && !una_volta4)										//Controlla quando CurrAction arriva a 100
		{
			una_volta4 = true;											
			ActionReady ();																									
		}

		if (NSchivate >= 5 && !una_volta1)
		{
			una_volta1 = true;
			Tutorial2 ();
		}

		if (NAttacchiSchivati >= 5 && !una_volta2)
		{
			una_volta2 = true;
			CancelInvoke ("AttaccoTutorial");
			Tutorial3 ();
		}

		if (NParate >= 3 && !una_volta3)
		{
			una_volta3 = true;
			CancelInvoke ("AttaccoTutorial");
			Tutorial4 ();
		}

		if (NAttacchi >= 10 && !una_volta5)
		{
			una_volta5 = true;
			Tutorial5 ();
		}

		if(Input.GetKey (KeyCode.UpArrow) )											//Momentaneo Pulsante di azione
		{
			ScudoAlzato = true;
		}
		else 
			ScudoAlzato = false;

		if(Input.GetKey (KeyCode.RightArrow) && !SchivataSinistraWork && !AzioneInCorso && tutorial1_iniziato)			//Comando Per schivata sinistra	
		{
			StartCoroutine ("SchivataSinistra");
		}
		
		if(Input.GetKey (KeyCode.LeftArrow) && !SchivataDestraWork && !AzioneInCorso && tutorial1_iniziato)			//Comando per schivata destra
		{
			StartCoroutine ("SchivataDestra");
		}
		
		if(Input.GetKey (KeyCode.DownArrow) && !SchivataCentraleWork && !AzioneInCorso && tutorial1_iniziato)			//Comando schivata centrale
		{
			StartCoroutine ("SchivataCentrale");
		}

		if(Input.GetKeyDown (KeyCode.Space) && AttaccoPossibile)						//Momentaneo Pulsante di azione
		{
			if (CurrAction < 81)
			{ 
				StartCoroutine ("AttaccoVeloce");
				affaticato = false;
			}
			else 
			{
				if (!affaticato)
				{
					audioSource1.PlayOneShot (Affaticamento);
					affaticato = true;
				}
			}
		}
	}

	void Tutorial1 ()
	{
		audioSource.PlayOneShot (pausaSpiegazioni); // audio introduzione e spiegazione schivata
		tutorial1_iniziato = true;
	}

	void Tutorial2 ()
	{
		audioSource.PlayOneShot (AudioSpiegazioni[1]); // audio schivata e direzioni attacchi
		Invoke ("AttaccoTutorial",AudioSpiegazioni[1].length + 0.5f);
	}

	void Tutorial3 ()
	{
		audioSource.PlayOneShot (AudioSpiegazioni[2]); // audio Tutorial Scudo
		Invoke ("AttaccoTutorial", AudioSpiegazioni[2].length + 0.5f);
	}

	void Tutorial4 ()
	{
		audioSource.PlayOneShot (AudioSpiegazioni[3]); 							// audio tutorial attacco
		Invoke ("attaccoPossibile",AudioSpiegazioni[3].length);
	}

	void Tutorial5 ()
	{

		Invoke ("ChangeScene",3.0f);
	}

	void attaccoPossibile ()
	{
		AttaccoPossibile = true;
	}

	IEnumerator SchivataSinistra ()												//Coroutines per la schivata sinistra		
	{
		AzioneInCorso = true;
		SchivataSinistraWork = true;
		if (NSchivate < 5)
		{
			NSchivate++;
			audioSource.PlayOneShot (OK);
		}
		Color schivata_sinistra = schivataSinistra.color;
		schivata_sinistra.a = 255;
		schivataSinistra.color = schivata_sinistra;
		audioSource.PlayOneShot (SuoniDodgeLaterale);
		AttaccoSinistroSchivato = true;

		yield return new WaitForSeconds (1.5f);

		Color schivata_sinistra1 = schivataSinistra.color;
		schivata_sinistra1.a = 0;
		schivataSinistra.color = schivata_sinistra1;
		SchivataSinistraWork = false;
		AttaccoSinistroSchivato = false;
		AzioneInCorso = false;
	}
	
	IEnumerator SchivataCentrale ()												//Coroutines per la schivata centrale
	{
		AzioneInCorso = true;
		SchivataCentraleWork = true;
		if (NSchivate < 5)
		{
			NSchivate++;
			audioSource.PlayOneShot (OK);
		}
		Color schivata_centrale = schivataCentrale.color;
		schivata_centrale.a = 255;
		schivataCentrale.color = schivata_centrale;
		audioSource.PlayOneShot (SuoniDodgeCentrale);
		AttaccoCentraleSchivato = true;
		yield return new WaitForSeconds (1.5f);

		Color schivata_centrale1 = schivataCentrale.color;
		schivata_centrale1.a = 0;
		schivataCentrale.color = schivata_centrale1;

		SchivataCentraleWork = false;
		AttaccoCentraleSchivato = false;
		AzioneInCorso = false;
	}
	
	IEnumerator SchivataDestra ()												//Coroutines per la schivata destra
	{
		AzioneInCorso = true;
		SchivataDestraWork = true;
		if (NSchivate < 5)
		{
			NSchivate++;
			audioSource.PlayOneShot (OK);
		}
		Color schivata_destra = schivataDestra.color;
		schivata_destra.a = 255;
		schivataDestra.color = schivata_destra;
		audioSource.PlayOneShot (SuoniDodgeLaterale);
		AttaccoDestroSchivato = true;

		yield return new WaitForSeconds (1.5f);

		Color schivata_destra1 = schivataDestra.color;
		schivata_destra1.a = 0;
		schivataDestra.color = schivata_destra1;
		SchivataDestraWork = false;
		AttaccoDestroSchivato = false;
		AzioneInCorso = false;
	}


	public void AttaccoNemico ()
	{
		Invoke ("TogliVita",TempoDanno);
	}
	
	void TogliVita ()
	{
		if(AttaccoNemicoSinistro)												//Capisce se il nemico attacca da sinistra
		{
			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				NParate++;
			}

			if (!ScudoAlzato && !AttaccoSinistroSchivato)
			{
				SuonoDanni();
			}

			if (AttaccoSinistroSchivato)
			{
				NAttacchiSchivati++;
				audioSource.PlayOneShot (OK);
			}
			AttaccoNemicoSinistro = false;
		}
		
		if(AttaccoNemicoDestro)													//Capisce se il nemico attacca da destra
		{
			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				NParate++;
			}

			if (!ScudoAlzato && !AttaccoDestroSchivato)
			{
				SuonoDanni();
			}

			if (AttaccoDestroSchivato)
			{
				NAttacchiSchivati++;
				audioSource.PlayOneShot (OK);
			}
			AttaccoNemicoDestro = false;
		}
		
		if(AttaccoNemicoCentrale)												//Capisce se il nemico attacca centralmente
		{
			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				NParate++;
			}

			if (!ScudoAlzato && !AttaccoCentraleSchivato)
			{
				SuonoDanni();
			}

			if (AttaccoCentraleSchivato)
			{
				NAttacchiSchivati++;
				audioSource.PlayOneShot (OK);
			}
			AttaccoNemicoCentrale = false;				
		}
	}

	void AttaccoTutorial ()
	{
		random = Random.Range (0,3);
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
		}

	}

	void AttackLeftAnim ()
	{
		audioSource2.panStereo = -1.0f;
		SuonoAttaccoNemico();
		Color attaccoSinistro = AttaccoSinistro.color;
		attaccoSinistro.a = 255;
		AttaccoSinistro.color = attaccoSinistro;
		Color panelsinistro = panelSinistro.color;
		panelsinistro.a = 255;
		panelSinistro.color = panelsinistro;
		Invoke ("ResetAlphaSinistra",1);
		AttaccoNemicoSinistro = true;
		TempoDanno = Tempo_danno;
		AttaccoNemico ();
		if (random == 0)
		{
			Invoke ("AttaccoTutorial",Random.Range(4.0f,5.0f));	
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
		audioSource2.panStereo = 0.0f;
		SuonoAttaccoNemico();
		Color attaccoCentrale = AttaccoCentrale.color;
		attaccoCentrale.a = 255;
		AttaccoCentrale.color = attaccoCentrale;
		Color panelcentrale = panelCentrale.color;
		panelcentrale.a = 255;
		panelCentrale.color = panelcentrale;
		Invoke ("ResetAlphaCentrale",1);
		AttaccoNemicoCentrale = true;
		TempoDanno = Tempo_danno;
		AttaccoNemico ();
		if (random == 2)
		{
			Invoke ("AttaccoTutorial",Random.Range(4.0f,5.0f));	
		}	
	}

	void ResetAlphaCentrale()
	{
		Color attaccoCentrale = AttaccoCentrale.color;
		attaccoCentrale.a = 0;
		AttaccoCentrale.color = attaccoCentrale;
		Color paneldestro = panelCentrale.color;
		paneldestro.a = 0;
		panelCentrale.color = paneldestro;
	}
	
	void AttackRightAnim ()
	{
		audioSource2.panStereo = 1.0f;
		SuonoAttaccoNemico();
		Color attaccoDestro = AttaccoDestro.color;
		attaccoDestro.a = 255;
		AttaccoDestro.color = attaccoDestro;
		Color paneldestro = panelDestro.color;
		paneldestro.a = 255;
		panelDestro.color = paneldestro;
		Invoke ("ResetAlphaDestra",1);
		AttaccoNemicoDestro = true;
		TempoDanno = Tempo_danno;
		AttaccoNemico ();
		if (random == 1)
		{
			Invoke ("AttaccoTutorial",Random.Range(4.0f,5.0f));	
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

	void SuonoAttaccoNemico()
	{
		int SuonoRandom = Random.Range (0,SuoniAttaccoNemico.Length);
		audioSource2.PlayOneShot (SuoniAttaccoNemico[SuonoRandom]);
	}

	void DecreaseActionBar ()													//Funzione incremento action bar
	{
		if (CurrAction > 0)
			CurrAction = CurrAction - SpeedAction;
		else 
			CurrAction = 0;
	}

	void ActionReady ()															//Funzione che viene chiamata quando CurrAction arriva a 100
	{
		CancelInvoke("IncreaseActionBar");										//cancella il precedente Invoke, setta CurrAction a 100
		audioSource.PlayOneShot (BarraFull);
		CurrAction = 100;
	}

	IEnumerator AttaccoVeloce()
	{
		NAttacchi ++;
		SuonoAttacco();
		CurrAction += 20.0f;
		yield return new WaitForSeconds (0.15f);
		SuonoColpito();
	}

	void SuonoScudo()
	{
		int SuonoRandom = Random.Range (0,HitScudo.Length);
		audioSource.PlayOneShot (HitScudo[SuonoRandom]);
	}

	void SuonoAttacco()
	{
		int SuonoRandom = Random.Range (0,Attacco.Length);
		audioSource.PlayOneShot (Attacco[SuonoRandom]);
	}

	void SuonoDanni()
	{
		int SuonoRandom = Random.Range (0,SuonoDanno.Length);
		audioSource.PlayOneShot (SuonoDanno[SuonoRandom]);
	}

	void ChangeScene ()
	{
		PlayerPrefs.SetInt ("ScenaDaCaricare",4);
		SceneManager.LoadScene ("Tutorial_combattimento");
	}

	public void SuonoColpito ()
	{
		audioSource.panStereo = 0.0f;
		int SuonoRandom = Random.Range (0,SuoniColpito.Length);
		audioSource.PlayOneShot (SuoniColpito[SuonoRandom]);
	}
}
