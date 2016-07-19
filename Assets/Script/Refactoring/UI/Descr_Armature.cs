using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Descr_Armature : MonoBehaviour
{
	
	public Text text;
	public Text descrizione_armatura;
	
	public string armatura;
	public string peso;
	public string costo;
	public string descrizione;
	public string playerprefs;
	public float Peso;
	public int Armatura; 
	public int Costo;


	public string GruppoSblocco;
	private Button button;

	void Start ()
	{
		button = this.gameObject.GetComponent<Button>();
		if (PlayerPrefs.HasKey (GruppoSblocco))
			button.interactable = true;
		playerprefs = this.gameObject.name;
	}

	public void Descrizione ()
	{
		text.text = ("Armour "+ armatura + "\n" + "weight " + peso + "\n" + "price " + costo + "\n") ;
		text.text.Replace ("\n", "\n");
		descrizione_armatura.text = descrizione;
	}
}