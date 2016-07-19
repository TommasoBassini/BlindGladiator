using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Menu_Equipaggiamenti_sound : MonoBehaviour 
{
	private AudioSource audioSource;
	public AudioClip BenvenutoPrimaVolta;
	public AudioClip[] Benvenuto;
	public AudioClip fabbro;
	private string nomeScena;
	AudioClip[] numeri;
	public List<AudioClip> audioClips = new List<AudioClip>();
	public AudioClip Hai;
	public AudioClip Monete;
	public AudioClip VitaSu100;
	public AudioClip ArmorSu;
	public bool stop = false;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
		nomeScena = Application.loadedLevelName;
		//caricamento dei numeri


        Scene scene = SceneManager.GetActiveScene();
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            Hai = Resources.Load<AudioClip>("Localization/English/MenuTornei/You have");
            VitaSu100 = Resources.Load<AudioClip>("Localization/English/MenuTornei/Hp");
            ArmorSu = Resources.Load<AudioClip>("Localization/English/MenuTornei/Armour");
            numeri = AudioClipArrayLoading("Localization/English/Numeri");
            Monete = Resources.Load<AudioClip>("Localization/English/MainMenu/Coins");
            if (scene.name == "Menu_Armature")
            {
                Benvenuto = AudioClipArrayLoading("Localization/English/Armour/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/English/Armour/Farmature 1 2 3");
            }
            if (scene.name == "Menu_Armi")
            {
                Benvenuto = AudioClipArrayLoading("Localization/English/Weapon/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/English/Weapon/F arm 1 2 3");
            }
            if (scene.name == "Menu_Scudi")
            {
                Benvenuto = AudioClipArrayLoading("Localization/English/Shield/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/English/Armour/Fscud 1 2 3");
            }
            if (scene.name == "Menu_Pozioni")
            {
                Benvenuto = AudioClipArrayLoading("Localization/English/Inn/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/English/Inn/Loc01");
            }
        }
        else
        {
            numeri = AudioClipArrayLoading("Localization/Italian/Numeri");
            Hai = Resources.Load<AudioClip>("Localization/Italian/MenuTornei/You have");
            VitaSu100 = Resources.Load<AudioClip>("Localization/Italian/MenuTornei/Hp");
            ArmorSu = Resources.Load<AudioClip>("Localization/Italian/MenuTornei/Armour");
            Monete = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Coins");

            if (scene.name == "Menu_Armature")
            {
                Benvenuto = AudioClipArrayLoading("Localization/Italian/Armour/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/Italian/Armour/Farmature 1 2 3");
            }
            if (scene.name == "Menu_Armi")
            {
                Benvenuto = AudioClipArrayLoading("Localization/Italian/Weapon/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/Italian/Weapon/F arm 1 2 3");
            }
            if (scene.name == "Menu_Scudi")
            {
                Benvenuto = AudioClipArrayLoading("Localization/Italian/Shield/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/Italian/Shield/Fscud 1 2 3");
            }
            if (scene.name == "Menu_Pozioni")
            {
                Benvenuto = AudioClipArrayLoading("Localization/Italian/Inn/Benvenuto");
                BenvenutoPrimaVolta = Resources.Load<AudioClip>("Localization/Italian/Inn/Loc01");
            }
        }
        StartCoroutine("start");

    }

    IEnumerator  start()
	{
		if(nomeScena == "Menu_Armature" || nomeScena == "Menu_Armi" || nomeScena == "Menu_Scudi")
		{
			audioSource.PlayOneShot (fabbro);
			yield return new WaitForSeconds(fabbro.length - 0.2f);
		}
		if(!stop)
		{
			if (PlayerPrefs.HasKey (nomeScena))
			{
				int i = Random.Range (0,2);
				audioSource.PlayOneShot (Benvenuto[i]);
				Invoke("TakeMoney",Benvenuto[i].length + 1.0f);

			}	
			else
			{
				int i;
				Random.Range (0,1);
				audioSource.PlayOneShot (BenvenutoPrimaVolta);
				PlayerPrefs.SetInt (nomeScena,1);
				Invoke("TakeMoney",BenvenutoPrimaVolta.length + 1.0f);
			}
		}
	}

	void Update () 
	{
		if(((Input.GetKeyDown (KeyCode.LeftAlt)) || (Input.GetKeyDown (KeyCode.AltGr))))
		{
			stop = false;
			TakeMoney();
		}
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
		foreach (AudioClip a in NumberGestor(PlayerPrefs.GetInt("Monete")))
		{
			audioClips.Add(a);
		}
		StartCoroutine("PlayAudioNumber");
	}

	IEnumerator PlayAudioNumber()
	{
		if(!stop)
		{
			audioSource.PlayOneShot(Hai);
			yield return new WaitForSeconds (Hai.length + 0.1f);

			for (int i = 0; i<audioClips.Count;i++)
			{
				audioSource.PlayOneShot(audioClips[i]);
				yield return new WaitForSeconds (audioClips[i].length);
			}
			audioSource.PlayOneShot(Monete);
			audioClips.Clear();
			if(nomeScena == "Menu_Pozioni")
			{
				yield return new WaitForSeconds (Monete.length + 0.3f);
				foreach (AudioClip a in NumberGestor(PlayerPrefs.GetInt("Vita")))
				{
					audioClips.Add(a);
				}

			StartCoroutine("PlayAudioNumberVita");
			}
		}
	}

	IEnumerator PlayAudioNumberVita()
	{
		if(!stop)
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
	}
	IEnumerator PlayAudioNumberArmor()
	{
		if(!stop)
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
}