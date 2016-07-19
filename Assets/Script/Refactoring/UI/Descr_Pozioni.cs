using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Descr_Pozioni : MonoBehaviour 
{
	
	public Text text;
	public Text descrizione_pozione;
	
	public string ripristino_vita;
	public string costo;
	public string descrizione;

	public int Ripristino_Vita;
	public int Costo; 

	public void Descrizione ()
	{
		text.text = ("Ripristino Vita "+ ripristino_vita + "%" + "\n" + "Costo " + costo + "\n") ;
		text.text.Replace ("\n", "\n");
		descrizione_pozione.text = descrizione;
	}
}