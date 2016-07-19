using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CambiaTesto : MonoBehaviour 
{

	void Start () 
	{
		Text text = this.gameObject.GetComponent<Text>();
		if (PlayerPrefs.HasKey ("LivelloSagobrida"))
		{
			text.text = "Next fight";
		}
	}

}
