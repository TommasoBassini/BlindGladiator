using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class GestioneDuelloTerragona : Duelli
{
	IEnumerator provaEnemy()
	{
		Instantiate (TipiGladiatori[Random.Range (0,TipiGladiatori.Length)]);
		enemy = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<EnemyControl>();
		yield return new WaitForSeconds (0.15f);
		if (!(PlayerPrefs.HasKey ("DuelloTerragona")))
		{
			audioSource.PlayOneShot (AudioSpiegazione);
			enemy.IniziaBattaglia (AudioSpiegazione.length + 2.0f);
			PlayerPrefs.SetInt ("DuelloSagobrida",0);
		}
		else
		{
			audioSource.PlayOneShot (AudioIntroduzione[i]);
			enemy.IniziaBattaglia (AudioIntroduzione[i].length + 2.0f);
			PlayerPrefs.SetInt ("DuelloTerragona",0);
		}
	}
	public override void Win()
	{
		audioSourceSpeaker.Stop();
		i = Random.Range (0,2);
		if( PlayerPrefs.GetInt ("DuelloTerragona") < 10)
		{
			PlayerPrefs.SetInt ("DuelloTerragona",PlayerPrefs.GetInt ("DuelloTerragona") + 1);
			PlayerPrefs.SetInt("Monete",PlayerPrefs.GetInt("Monete") + ((MoneteIniziali  * ((100-(PlayerPrefs.GetInt ("DuelloSagobrida")*10))/100))));
		}
		audioSource.PlayOneShot (AudioVittoria[i]);
		Invoke ("ChangeScene", AudioVittoria[i].length );
	}

	public override void Lose()
	{
		audioSourceSpeaker.Stop();
		i = Random.Range (0,2);
		audioSource.PlayOneShot (AudioSconfitta[i]);
		Invoke ("ChangeScene", AudioSconfitta[i].length );
	}

}
