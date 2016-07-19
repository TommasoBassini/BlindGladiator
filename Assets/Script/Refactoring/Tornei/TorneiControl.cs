using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TorneiControl : MonoBehaviour 
{
	public Button[] ButtonMenu;
	private int numButton = -1;
	private AudioSource audioSource;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip Comandi;
	public AudioClip[] AudioIntroduzione;
	public AudioClip porta;

	AudioClip[] numeri;
	public List<AudioClip> audioClips = new List<AudioClip>();
	public AudioClip Hai;
	public AudioClip VitaSu100;
	public AudioClip ArmorSu;
	private bool stop = false;
	void Start () 
	{
		numeri = AudioClipArrayLoading("Localization/Italian/Numeri");
		audioSource = GetComponent<AudioSource>();
        CheckScene();

        StartCoroutine ("Audio");
    }
	
    void CheckScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/MenuTornei/AudioButton");
            Comandi = Resources.Load<AudioClip>("Localization/English/MainMenu/Comandi");
            Hai = Resources.Load<AudioClip>("Localization/English/MenuTornei/You have");
            VitaSu100 = Resources.Load<AudioClip>("Localization/English/MenuTornei/Hp");
            ArmorSu = Resources.Load<AudioClip>("Localization/English/MenuTornei/Armour");

            if (scene.name == "Torneo_Di_Segobrida_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/MenuTornei/IntrSag");
            }
            if (scene.name == "Torneo_Di_Terragona_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/MenuTornei/IntrTer");
            }
            if (scene.name == "Torneo_Di_Treviri_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/MenuTornei/IntrTre");
            }
            if (scene.name == "Torneo_Di_Pola_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/MenuTornei/IntrPol");
            }
            if (scene.name == "Torneo_Di_Roma_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/MenuTornei/IntrRom");
            }
        }
        else
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/MenuTornei/AudioButton");
            Comandi = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Comandi");
            Hai = Resources.Load<AudioClip>("Localization/Italian/MenuTornei/You have");
            VitaSu100 = Resources.Load<AudioClip>("Localization/Italian/MenuTornei/Hp");
            ArmorSu = Resources.Load<AudioClip>("Localization/Italian/MenuTornei/Armour");

            if (scene.name == "Torneo_Di_Segobrida_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/MenuTornei/IntrSag");
            }
            if (scene.name == "Torneo_Di_Terragona_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/MenuTornei/IntrTer");
            }
            if (scene.name == "Torneo_Di_Treviri_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/MenuTornei/IntrTre");
            }
            if (scene.name == "Torneo_Di_Pola_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/MenuTornei/IntrPol");
            }
            if (scene.name == "Torneo_Di_Roma_menu")
            {
                AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/MenuTornei/IntrRom");
            }
        }
    }
	
	void Update () 
	{

		if(((Input.GetKeyDown (KeyCode.Escape))))
		{
			SceneManager.LoadScene("Menu_Principale");
		}

		if(((Input.GetKeyDown (KeyCode.LeftAlt)) || (Input.GetKeyDown (KeyCode.AltGr))))
		{
			audioSource.Stop ();
			TakeMoney();
		}

		if(Input.GetKeyDown (KeyCode.Tab))
		{
			stop = true;
			int n = numButton + 1;
			audioSource.Stop ();
			if (numButton < 5)
			{
				if(ButtonMenu[n].interactable == true && n != 0)
				{
				numButton++;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
				else
				{
					numButton = 0;
					ButtonMenu[numButton].Select ();
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
			}
			else 
			{
				numButton = 0;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
		}
		
		if(Input.GetKeyDown (KeyCode.LeftShift))
		{
			stop = true;
			int n = ButtonMenu.Length - 1;
			bool trovato = false;
			audioSource.Stop ();
			if (numButton > 0)
			{
				numButton--;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				
			}
			else 
			{
				if (ButtonMenu[n].interactable == true)
				{
					numButton = 5;
					ButtonMenu[numButton].Select ();
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
				else
					while (!trovato)
				{
					n--;
					if (ButtonMenu[n].interactable == true)
					{
						trovato = true;
						numButton = n;
						ButtonMenu[numButton].Select ();
						audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
					}
				}
			}
		}
	}

    public void SwipeDownMenu()
    {
        stop = true;
        int n = numButton + 1;
        audioSource.Stop();
        if (numButton < 5)
        {
            if (ButtonMenu[n].interactable == true && n != 0)
            {
                numButton++;
                ButtonMenu[numButton].Select();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
            else
            {
                numButton = 0;
                ButtonMenu[numButton].Select();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
        }
        else
        {
            numButton = 0;
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
    }

    public void SwipeUpMenu()
    {
        stop = true;
        int n = ButtonMenu.Length - 1;
        bool trovato = false;
        audioSource.Stop();
        if (numButton > 0)
        {
            numButton--;
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);

        }
        else
        {
            if (ButtonMenu[n].interactable == true)
            {
                numButton = 5;
                ButtonMenu[numButton].Select();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
            else
                while (!trovato)
                {
                    n--;
                    if (ButtonMenu[n].interactable == true)
                    {
                        trovato = true;
                        numButton = n;
                        ButtonMenu[numButton].Select();
                        audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
                    }
                }
        }
    }

    public void ConfirmButton()
    {
        ButtonMenu[numButton].onClick.Invoke();
    }

    IEnumerator Audio()
    {
        if (Application.loadedLevelName == "Torneo_Di_Segobrida_menu")
        {
            if (!(PlayerPrefs.HasKey("LivelloSagobrida")))
            {
                audioSource.PlayOneShot(AudioIntroduzione[0]);
				yield return new WaitForSeconds(AudioIntroduzione[0].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
            }
            else
            {
                audioSource.PlayOneShot(AudioIntroduzione[PlayerPrefs.GetInt("LivelloSagobrida")]);
				yield return new WaitForSeconds(AudioIntroduzione[PlayerPrefs.GetInt("LivelloSagobrida")].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
            }
        }

        if (Application.loadedLevelName == "Torneo_Di_Terragona_menu")
        {
            if (!(PlayerPrefs.HasKey("LivelloTerragona")))
            {
                audioSource.PlayOneShot(AudioIntroduzione[0]);
				yield return new WaitForSeconds(AudioIntroduzione[0].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
            }
            else
            {
                audioSource.PlayOneShot(AudioIntroduzione[PlayerPrefs.GetInt("LivelloTerragona")]);
				yield return new WaitForSeconds(AudioIntroduzione[PlayerPrefs.GetInt("LivelloTerragona")].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
            }
        }

		if (Application.loadedLevelName == "Torneo_Di_Treviri_menu")
		{
			if (!(PlayerPrefs.HasKey("LivelloTreviri")))
			{
				audioSource.PlayOneShot(AudioIntroduzione[0]);
				yield return new WaitForSeconds(AudioIntroduzione[0].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
			}
			else
			{
				audioSource.PlayOneShot(AudioIntroduzione[PlayerPrefs.GetInt("LivelloTreviri")]);
				yield return new WaitForSeconds(AudioIntroduzione[PlayerPrefs.GetInt("LivelloTreviri")].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
			}
		}

		if (Application.loadedLevelName == "Torneo_Di_Pola_menu")
		{
			if (!(PlayerPrefs.HasKey("LivelloPola")))
			{
				audioSource.PlayOneShot(AudioIntroduzione[0]);
				yield return new WaitForSeconds(AudioIntroduzione[0].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
			}
			else
			{
				audioSource.PlayOneShot(AudioIntroduzione[PlayerPrefs.GetInt("LivelloPola")]);
				yield return new WaitForSeconds(AudioIntroduzione[PlayerPrefs.GetInt("LivelloPola")].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
			}
		}

		if (Application.loadedLevelName == "Torneo_Di_Roma_menu")
		{
			if (!(PlayerPrefs.HasKey("LivelloRoma")))
			{
				audioSource.PlayOneShot(AudioIntroduzione[0]);
				yield return new WaitForSeconds(AudioIntroduzione[0].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
			}
			else
			{
				audioSource.PlayOneShot(AudioIntroduzione[PlayerPrefs.GetInt("LivelloRoma")]);
				yield return new WaitForSeconds(AudioIntroduzione[PlayerPrefs.GetInt("LivelloPola")].length + 0.5f);
				if(!stop)
				{
					audioSource.PlayOneShot(Comandi);
				}
			}
		}
    }
	public void LoadScene (string i)
	{
		SceneManager.LoadScene (i);
	}
	

	public void Spada ()
	{
		StartCoroutine ("CoSpada");
	}

	IEnumerator CoSpada ()
	{
		audioSource.PlayOneShot (porta);
		yield return new WaitForSeconds (porta.length);
        SceneManager.LoadScene("Menu_Armi");
	}
	
	public void Armatura ()
	{
		StartCoroutine ("CoArmatura");
	}
	IEnumerator CoArmatura ()
	{
		audioSource.PlayOneShot (porta);
		yield return new WaitForSeconds (porta.length);
        SceneManager.LoadScene("Menu_Armature");
	}

	public void Pozioni ()
	{
		StartCoroutine ("CoPozioni");
	}

	IEnumerator CoPozioni ()
	{
		audioSource.PlayOneShot (porta);
		yield return new WaitForSeconds (porta.length);
        SceneManager.LoadScene("Menu_Pozioni");
	}

	public void Scudo ()
	{
		StartCoroutine ("CoScudo");
	}
	IEnumerator CoScudo ()
	{
		audioSource.PlayOneShot (porta);
		yield return new WaitForSeconds (porta.length);
        SceneManager.LoadScene("Menu_Scudi");
	}

	#region pronuncia numeri
	private AudioClip getNumberClip(string name)
	{
		AudioClip clip = null;
		name = "Number" + name;
		foreach (AudioClip a in numeri)
		{
			if (a.name == name)
			{
				clip = a;
				break;
			}

		}
		return clip;
	} 

	private AudioClip[] AudioClipArrayLoading(string pathFolder)
	{
		AudioClip[] clips = null;
		try
		{
			clips = Resources.LoadAll<AudioClip>(pathFolder);
		}
		catch (System.Exception e)
		{
			Debug.LogError("Errore nel caricamento delle clips nel VocalGestor, path: "+pathFolder);
		}
		return clips;
	}

	private List<AudioClip> NumberGestor(int n)
	{
		List<AudioClip> clips = new List<AudioClip>();
		string nString = n.ToString();
		int type = nString.Length;
		if (type > 6)
		{
			//nel caso il numero sia uguale o maggiore ad un milione
			clips.Add(getNumberClip("OverMilion"));
		}
		else
		{
			switch (type)
			{
			case 6:
				{
					if (nString[0] != '1')
					{
						clips.Add(getNumberClip(nString[0]+""));
					}
					clips.Add(getNumberClip("100"));
					n -= (int) (int.Parse(nString[0] + "") * Mathf.Pow(10, type-1));
					Debug.Log(n);
					if (n != 0)
					{
						foreach (AudioClip a in NumberGestor(n))
						{
							clips.Add(a);
						}
					}
				}
				break;
			case 5:
				{
					if (nString[0] != '2' && nString[0]!='1' && nString[0] != '0')
					{
						clips.Add(getNumberClip(nString[0] + "0"));
						n -= (int)(int.Parse(nString[0] + "0") * Mathf.Pow(10, type-2));
					}
					else
					{
						if (nString[0] != '0')
						{
							if (nString[0]=='1')
							{

								clips.Add(getNumberClip(nString[0].ToString()+ nString[1].ToString()+""));
								n -= (int)(int.Parse(nString[0].ToString() + nString[1].ToString()+ "") * Mathf.Pow(10, type-2));
								clips.Add(getNumberClip("Mila"));
							} 
							else
							{
								clips.Add(getNumberClip(nString[0] +"0"));
								clips.Add(getNumberClip("Mila"));
								n -= (int)(int.Parse(nString[0] + "") * Mathf.Pow(10, type-1));     
							}
						}
					}
					if (n != 0)
					{
						foreach (AudioClip a in NumberGestor(n))
						{
							clips.Add(a);
						}
					}
					Debug.Log(n);
				}
				break;
			case 4:
				{

					if (nString[0] != '0')
					{
						clips.Add(getNumberClip(nString[0] + ""));
					}
					clips.Add(getNumberClip("Mila"));
					n -= (int)(int.Parse(nString[0] + "") * Mathf.Pow(10, type - 1));
					Debug.Log(n);
					if (n != 0)
					{
						foreach (AudioClip a in NumberGestor(n))
						{
							clips.Add(a);
						}
					}
				}
				break;
			case 3:
				{
					if (nString[0] != '1')
					{
						clips.Add(getNumberClip(nString[0] + ""));
					}
					clips.Add(getNumberClip("100"));
					n -= (int)(int.Parse(nString[0] + "") * Mathf.Pow(10, type-1));
					Debug.Log(n);
					if (n != 0)
					{
						foreach (AudioClip a in NumberGestor(n))
						{
							clips.Add(a);
						}
					}
				}
				break;
				//caso 1 e 2
			case 2:
			case 1:
				{
					if (n <= 20)
					{
						//se è un numero fino a venti
						clips.Add(getNumberClip(nString));
					}
					else
					{
						nString = nString[0] + "0";
						clips.Add(getNumberClip(nString));
						n -= int.Parse(nString);                            
						if (n != 0)
						{
							foreach (AudioClip a in NumberGestor(n))
							{
								clips.Add(a);
							}
						}
					}
				}
				break;
			}            
		}
		return clips;
	}
	#endregion

	void TakeMoney()
	{
		foreach (AudioClip a in NumberGestor(PlayerPrefs.GetInt("Vita")))
		{
			audioClips.Add(a);
		}
		StartCoroutine("PlayAudioNumberVita");
	}

	IEnumerator PlayAudioNumberVita()
	{
		audioSource.PlayOneShot(Hai);
		yield return new WaitForSeconds (Hai.length + 0.1f);

		for (int i = 0; i<audioClips.Count;i++)
		{
			audioSource.PlayOneShot(audioClips[i]);
			yield return new WaitForSeconds (audioClips[i].length);
		}

		audioSource.PlayOneShot(VitaSu100);
		audioClips.Clear();
		yield return new WaitForSeconds (VitaSu100.length + 0.3f);

		foreach (AudioClip a in NumberGestor(PlayerPrefs.GetInt("Armatura")))
		{
			audioClips.Add(a);
		}

		StartCoroutine("PlayAudioNumberArmor");
	}
	IEnumerator PlayAudioNumberArmor()
	{
		audioSource.PlayOneShot(Hai);
		yield return new WaitForSeconds (Hai.length + 0.1f);

		for (int i = 0; i<audioClips.Count;i++)
		{
			audioSource.PlayOneShot(audioClips[i]);
			yield return new WaitForSeconds (audioClips[i].length);
		}
		audioSource.PlayOneShot(ArmorSu);
		audioClips.Clear();
		yield return new WaitForSeconds (ArmorSu.length + 0.3f);
		foreach (AudioClip a in NumberGestor(PlayerPrefs.GetInt("ArmaturaMax")))
		{
			audioClips.Add(a);
		}
		for (int i = 0; i<audioClips.Count;i++)
		{
			audioSource.PlayOneShot(audioClips[i]);
			yield return new WaitForSeconds (audioClips[i].length);
		}
		audioClips.Clear();
	}
}


