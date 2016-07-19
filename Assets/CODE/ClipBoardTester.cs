using UnityEngine;
using System.Collections;

public class ClipBoardTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V) ||
		   Input.GetKey(KeyCode.A)){
			Debug.Log ( ClipboardHelper.clipBoard ) ;
		}
	}
}
