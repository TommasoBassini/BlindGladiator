using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Script : MonoBehaviour 
{

    public int PlayerLife;
	public int Armor;

    public Text TextVita;
	public Text TextArmor;

	private PlayerStat playerStat;


	void Start ()
    {
		Armor = PlayerPrefs.GetInt ("Armatura");
		PlayerLife = PlayerPrefs.GetInt ("Vita");
		TextVita.text = PlayerLife.ToString();
		TextArmor.text = Armor.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		TextVita.text = PlayerLife.ToString();
		TextArmor.text = Armor.ToString();
	}
}
