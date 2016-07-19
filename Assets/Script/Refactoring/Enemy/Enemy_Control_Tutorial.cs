using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy_Control_Tutorial : MonoBehaviour 
{
	private Player_Tutorial_Combattimento player_Tutorial;
	
	public int Danno;
	public float Tempo_danno;
	public int EnemyLife;
	private bool morto = false;
	private int random;
	public bool DannoPieno = false;

	private AudioSource audioSource;
	public AudioClip[] SuoniAttacco;
	public AudioClip[] SuoniColpitoParati;
	public AudioClip[] SuoniColpitoPieno;
	public AudioClip SuonoFineCombattimento;
	public AudioClip MomentoAttacco;

	public Image AttaccoSinistro;
	public Image AttaccoCentrale;
	public Image AttaccoDestro;
	public Image panelSinistro;
	public Image panelDestro;
	public Image panelCentrale;
	
	void Start () 
	{

		audioSource = GetComponent<AudioSource>();
		player_Tutorial = GameObject.Find ("Player").GetComponent<Player_Tutorial_Combattimento>();
		Invoke ("Attacco",13.5f);
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
		if(!morto)
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
		player_Tutorial.AttaccoNemicoSinistro = true;
		player_Tutorial.TempoDanno = Tempo_danno;
		player_Tutorial.AttaccoNemico ();
		if (random == 0)
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
			Invoke ("Attacco",Random.Range(4.0f,4.5f));	
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
		player_Tutorial.AttaccoNemicoCentrale = true;
		player_Tutorial.TempoDanno = Tempo_danno;
		player_Tutorial.AttaccoNemico ();
		if (random == 2)
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
			Invoke ("Attacco",Random.Range(4.0f,4.5f));	
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
		audioSource.panStereo = 1.0f;
		SuonoAttacco();
		Color attaccoDestro = AttaccoDestro.color;
		attaccoDestro.a = 255;
		AttaccoDestro.color = attaccoDestro;
		Color paneldestro = panelDestro.color;
		paneldestro.a = 255;
		panelDestro.color = paneldestro;
		Invoke ("ResetAlphaDestra",1);
		player_Tutorial.AttaccoNemicoDestro = true;
		player_Tutorial.TempoDanno = Tempo_danno;
		player_Tutorial.AttaccoNemico ();
		if (random == 1)
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
			Invoke ("Attacco",Random.Range(4.0f,4.5f));	
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
		yield return new WaitForSeconds(2.7f);
		if(!player_Tutorial.PlayerMorto)
			AttackRightAnim ();
		yield return new WaitForSeconds(2.7f);
		if(!player_Tutorial.PlayerMorto)
			AttackCentralAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
		}
		yield return new WaitForSeconds(1.7f);
		Invoke ("Attacco",Random.Range(3.0f,4.0f));
	}
	
	IEnumerator Combo2 ()
	{
		AttackRightAnim ();
		yield return new WaitForSeconds(2.7f);
		if(!player_Tutorial.PlayerMorto)
			AttackCentralAnim ();
		yield return new WaitForSeconds(2.7f);
		if(!player_Tutorial.PlayerMorto)
			AttackLeftAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
		}
		yield return new WaitForSeconds(1.7f);
		Invoke ("Attacco",Random.Range(3.0f,4.0f));
	}
	
	IEnumerator Combo3 ()
	{
		AttackCentralAnim ();
		yield return new WaitForSeconds(2.7f);
		if(!player_Tutorial.PlayerMorto )
			AttackCentralAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
		}
		yield return new WaitForSeconds(1.7f);
		Invoke ("Attacco",Random.Range(2.0f,3.0f));
	}
	
	IEnumerator Combo4 ()
	{
		AttackRightAnim ();
		yield return new WaitForSeconds(2.7f);		
		if(!player_Tutorial.PlayerMorto )
			AttackLeftAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
		}
		yield return new WaitForSeconds(1.7f);
		Invoke ("Attacco",Random.Range(2.0f,3.0f));
	}
	
	IEnumerator Combo5 ()
	{
		AttackCentralAnim ();
		yield return new WaitForSeconds(2.7f);		
		if(!player_Tutorial.PlayerMorto)
			AttackLeftAnim ();
		yield return new WaitForSeconds(2.7f);
		if(!player_Tutorial.PlayerMorto)
			AttackRightAnim ();
		yield return new WaitForSeconds(1.0f);
		{
			DannoPieno = true;
			audioSource.PlayOneShot (MomentoAttacco);
		}
		yield return new WaitForSeconds(1.7f);
		Invoke ("Attacco",Random.Range(3.0f,4.0f));
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
		morto = true;
		audioSource.PlayOneShot (SuonoFineCombattimento);
		StopAllCoroutines ();
		CancelInvoke ("Attacco");
	}
}