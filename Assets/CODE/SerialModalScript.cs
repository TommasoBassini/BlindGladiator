/**
 * 
 * DRM TiconBlu
 * ver. 1.0
 * 
 ****/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
 


public class SerialModalScript : MonoBehaviour {
	#region Properties
		enum AUTHSTATE{OFF,READY,WAITING,RESPONSE,SUCCESS,CONNECTION_ERROR,SERIAL_ERROR,USERCODE_ERROR}
		const int MAXCHARS = 15;
		private string PLAYERPREFST_AUTHKEY="AUTHCHECK";
		private string PLAYERPREFST_AUTHDOUBLECHECK_KEY="AUTHDOUBLECHECK";
		private string PLAYERPREFST_AUTHDOUBLECHECK_VALUE="A921E1z141g101et1c4ksd6660wn";
	
		private string authURL="";
		private AUTHSTATE state=AUTHSTATE.OFF;
	
		//string (input, msg etc..)
		private string responseMSG;
		private string inputString="";
		private string inputStringClipBoard="";	
		private string labelString="";
		private string hintString="";
		private bool _ctrlpressed = false;

		//Main scene
		public string mainScene = "_main";

		public string PRODUCTCHAR="E";
		public string PRODUCTCODE="8";
		public string PRODUCTVERSION="2";
		public string KEY="3,6,7,1,4,8,3,6,5,0";
		public bool ForzaLinguaITA = false;
		public bool ForzaLinguaENG = false;

		public bool audiogame = true;
		public bool AbilitaMsgVoiceover = true;
		public bool AbilitaMsgCuffie = true;

		//inspector properties
		public TextMesh textInput3D;
		private float   textInputPulsar=0;
	
		public TextMesh label3D;
	
		public Texture  yellowTexture;
		public Texture  redTexture;
	
		//flags
		private bool noInternet=false;	
		private bool _auth_ok = false;
		
		private string Language = "English";
		private List<string> talkQueue = null;
	#endregion
	
	#region Methods
	
		/// <summary>
		///  START
		/// </summary>
		public void Start () {	
				Debug.Log(Application.systemLanguage.ToString());

				SecurityUtils.SetProductInfo(PRODUCTCHAR,PRODUCTVERSION,PRODUCTCODE,KEY);

				if (GetComponent<AudioSource>()==null) this.gameObject.AddComponent<AudioSource>();
				Language = Application.systemLanguage.ToString ();
				if ((Language != "Italian") &&
				    (Language != "English")) 
				{
					Language = "English";
				}
				if (ForzaLinguaITA) Language = "Italian";
				if (ForzaLinguaENG) Language = "English";
				talkQueue = new List<string>();
#if UNITY_STANDALONE_OSX || UNITY_EDITOR
				if (AbilitaMsgVoiceover) Talk ("voiceover");
#endif
				if (AbilitaMsgCuffie) Talk ("headphones",false);
				//PlayerPrefs.DeleteKey(PLAYERPREFST_AUTHKEY); //TEMP x reset test//

				//if authceck return true... change state to success...
				if(CheckAuthentication(false)){
					state = AUTHSTATE.SUCCESS;
				}else{
					//if connection, get the auth URL from server
					if(Application.internetReachability !=NetworkReachability.NotReachable){
						//request at launcher server the URL of authentication PHP page
						LauncherUtils launcherUtils = new LauncherUtils();
						launcherUtils.SetUp("SOFTWARE\\NOME_GIOCO","http://www.eymerich.it/launcher/launcherServer.php");
						launcherUtils.GetServerValue("EymerichAuthURL",hReady);
					}else{
						//if no connection, use copycode
						state = AUTHSTATE.CONNECTION_ERROR;
					}
				}
				
				
		}
	
		/// <summary>
		/// UPDATE
		/// </summary>
		public void Update(){
			if (_auth_ok)
			{
				if (!GetComponent<AudioSource>().isPlaying && talkQueue.Count==0)
				{
					Application.LoadLevel(mainScene);
					_auth_ok = false;
					return;
				}
			}
			//Handle talk messages queue
			if (talkQueue != null) 
			{
				if (talkQueue.Count>0)
				{
					if (!GetComponent<AudioSource>().isPlaying)
					{
						Talk (talkQueue[0],false);
						talkQueue.RemoveAt(0);
						return;
					}
				}
			}
			//update label text according to inputstring
			label3D.text=labelString;
		
			switch(state){
				//connections states
				case AUTHSTATE.OFF:{	labelString="Connecting";	break;	}
				case AUTHSTATE.READY:{	labelString=responseMSG;
			
					//read input string: char KEYs
					if(inputString.Length<MAXCHARS){
						if (!_ctrlpressed) inputString+=FilterAllowedChar(Input.inputString);					
					}

					//read input string: delete KEYs
					if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)){
						if(inputString.Length>0) inputString = inputString.Substring(0,inputString.Length-1);
						Debug.Log(inputString);
					}
			
					//clamp string			
					inputString = inputString.Length<MAXCHARS?inputString:inputString.Substring(0,MAXCHARS);
			
					//reset string on click
					if(Input.GetMouseButtonUp(0)) inputString="";
			
					//ON ENTER/RETURN/OK
					if(Input.GetKeyUp(KeyCode.Return) || inputString.Contains("\n")){
						inputString = inputString.Replace("\n","");						
						PressButton();
						//inputString = "";
					}
			
					//text cursor animation
					textInput3D.text=inputString+((((int)textInputPulsar)%2==0)?"|":"");
					textInputPulsar+=Time.deltaTime*3;					
					break;
				}
				case AUTHSTATE.WAITING:{
					textInput3D.text=inputString+"";
					labelString="Please wait";
					break;
				}
				case AUTHSTATE.RESPONSE:{
					labelString = responseMSG;
					if(Delay(2)){
						state = AUTHSTATE.READY;
					}
					break;
				}
				case AUTHSTATE.CONNECTION_ERROR:{
					label3D.GetComponent<Renderer>().material.SetTexture("_Diffuse",redTexture);
					labelString = responseMSG;
					hintString  = "Non e' possibile autenticare il prodotto online\nRichiedi il tuo USERCODE a [...].\nIl tuo COPYCODE e': "+SecurityUtils.GetUserUniqueID();
					if(Delay(2)){
						label3D.GetComponent<Renderer>().material.SetTexture("_Diffuse",yellowTexture);
						responseMSG="Connection Error \n Insert your USER CODE *";
						state = AUTHSTATE.READY;
					}
					break;
				}
				case AUTHSTATE.SERIAL_ERROR:{
					label3D.GetComponent<Renderer>().material.SetTexture("_Diffuse",redTexture);
					labelString = responseMSG;
					hintString  = "Non conosci il tuo USERCODE ? [...]";
					if(Delay(2)){
						label3D.GetComponent<Renderer>().material.SetTexture("_Diffuse",yellowTexture);
						responseMSG="Inserisci il tuo USERCODE";
						state = AUTHSTATE.READY;
					}
					break;
				}
				case AUTHSTATE.USERCODE_ERROR:{
					label3D.GetComponent<Renderer>().material.SetTexture("_Diffuse",redTexture);
					labelString = "INVALID USER CODE";
					hintString  = "Non conosci il tuo USERCODE? richiedilo a [...].\nIl tuo COPYCODE e': "+SecurityUtils.GetUserUniqueID();
					if(Delay(2)){
						label3D.GetComponent<Renderer>().material.SetTexture("_Diffuse",yellowTexture);
						responseMSG="Connection Error \n Insert your USER CODE **";
						state = AUTHSTATE.READY;
					}
					break;
				}
			}
		
		
			if(noInternet){
				//todo
			}
		
			if(Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
		}
		



		/// <summary>
		/// when server response "URL of authentication PHP page"
		/// </summary>
		public void hReady(WWWResponse response){
			if(response.error!=""){
				responseMSG = "ERRORE: "+response.error;
				noInternet  = true;
				state = AUTHSTATE.CONNECTION_ERROR;
				return;
			}	
			responseMSG="Insert your USER CODE";
			authURL = response.Value;
			state = AUTHSTATE.READY;

			Talk("insertUserCode",false);
		}
	
		/// <summary>
		/// when server response the authentication result
		/// </summary>
		public void hResponse(WWWResponse response){
			state = AUTHSTATE.RESPONSE;
			if(response.error!=""){
				responseMSG = "ERRORE: "+response.error;	
				if(noInternet){
					state = AUTHSTATE.USERCODE_ERROR;
				}else{	
					state = AUTHSTATE.SERIAL_ERROR;
				}
				PlayAudio("userCodeKO");
				return;
			}

		Debug.Log(response.dataDictionary["usercode"]);
			
			responseMSG = "USER CODE: OK";
			PlayerPrefs.SetString(PLAYERPREFST_AUTHDOUBLECHECK_KEY,PLAYERPREFST_AUTHDOUBLECHECK_VALUE);
			//PlayerPrefs.SetString(PLAYERPREFST_AUTHKEY,response.dataDictionary["usercode"].Trim().ToLower());
			CheckAuthentication();	

		//PlayAudio("userCodeOK");
		//GoToGame();
		}
	
	


	/// <summary>
		/// ONGUI
		/// </summary>
		private bool clipboardToggle=false;
		public void OnGUI(){
			switch(state){

				case AUTHSTATE.READY:{
					if(Event.current.type == EventType.keyDown)
					{
						string s = Event.current.keyCode.ToString();
						if (s!="None")
						{
							if (!audiogame) return;
							Talk ("key_"+s);
						}
					}

					//Debug.Log(Event.current.type+"  "+Event.current.keyCode);
					if((Event.current.keyCode == KeyCode.LeftControl) || Event.current.keyCode ==KeyCode.RightControl){
					    if(Event.current.type     == EventType.keyDown) clipboardToggle = true;
						if(Event.current.type     == EventType.keyUp)   clipboardToggle = false;
					}
#if UNITY_STANDALONE_OSX
					_ctrlpressed = false;					
					if(Input.GetKey(KeyCode.LeftApple)||Input.GetKey(KeyCode.LeftControl)) 
			   		{	
						_ctrlpressed = true;
						if(Input.GetKeyUp(KeyCode.V))
						{
							inputStringClipBoard=ClipboardHelper.clipBoard ;
							
						}
					}
#endif
					if(clipboardToggle){
						GUI.SetNextControlName ("InputTextField");
						inputStringClipBoard=GUI.TextArea(new Rect(Screen.width/2-100,Screen.height+120,200,30),inputStringClipBoard);
						GUI.FocusControl("InputTextField");
					}else{
						if(inputStringClipBoard!=""){
							inputString = inputStringClipBoard;
							inputStringClipBoard="";
							PlayCharToChar(inputString);
						}
					}
					break;
				}
			
			}
		
			//GUI.Label(new Rect(5,5,Screen.width,100),hintString);
		}

	private void Talk(string namefile, bool stopPrevious = true)
	{		
		if (stopPrevious) 
		{
			talkQueue.Clear ();
			GetComponent<AudioSource>().Stop ();
		} else 
		{
			if (GetComponent<AudioSource>().isPlaying) 
			{
				talkQueue.Add(namefile);
				return;
			}
		}
		GetComponent<AudioSource>().clip = (AudioClip)Resources.Load ("Localization/" + Language + "/Dubbing/" + namefile, typeof(AudioClip));
		GetComponent<AudioSource>().Play ();
	}

		private void PressButton(){
 #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
			String inputStringToSend = StripInput(inputString);
			if(inputStringToSend.Trim()=="") return;
		
			PlayerPrefs.SetString(PLAYERPREFST_AUTHKEY,inputStringToSend);				
			if(CheckAuthentication()){		
				//if the input string is THE valid USERCODE.... 
				state = AUTHSTATE.SUCCESS;
				
			}else{
				//ELSE... input string is a SERIAL, so... ask to server if SERIAL is OK: if OK it will respond with the USERCODE (see hResponse)
				WWWForm wwwFormData = new WWWForm();
				wwwFormData.AddField("serial"      ,inputStringToSend);
				wwwFormData.AddField("copycode"    ,SecurityUtils.GetUserUniqueID());
				wwwFormData.AddField("platform"	   ,SystemInfo.operatingSystem);
				wwwFormData.AddField("systeminfo"  ,
													"ID="+SystemInfo.deviceUniqueIdentifier+","+
													"OS="+SystemInfo.operatingSystem+","+
													"MEM="+SystemInfo.systemMemorySize+","+
													"DEVICE="+SystemInfo.deviceModel+" - "+SystemInfo.deviceName+" - "+SystemInfo.deviceType+","+
													"GFX="+SystemInfo.graphicsDeviceVendor+" - "+SystemInfo.graphicsDeviceName
					
									 );
				
				WWWUtils.CreateRequest(authURL,wwwFormData,hResponse);
				state = AUTHSTATE.WAITING;
			}	
		
			inputString = "";
#endif
		}
	
		private bool CheckAuthentication(bool enablemsg=true){

		
			String realUserCode  = SecurityUtils.GetUserCode(SecurityUtils.GetUserUniqueID());
			String inputUserCode = PlayerPrefs.GetString(PLAYERPREFST_AUTHKEY,"");		
			if(realUserCode.ToLower()==inputUserCode.ToLower()
			||
			//aggiunta doublecheck per evitare problemi con macaddress wifi sempre diversi
			(PlayerPrefs.GetString(PLAYERPREFST_AUTHDOUBLECHECK_KEY,"")==PLAYERPREFST_AUTHDOUBLECHECK_VALUE
		 	&& PlayerPrefs.HasKey(PLAYERPREFST_AUTHKEY))
			){
				if (enablemsg) PlayAudio("userCodeOK");
				GoToGame();
				return true;
			}
			return false;
		}

		private void PlayAudio(string fileName){
			GetComponent<AudioSource>().clip = (AudioClip)Resources.Load ("Localization/" + Language + "/Dubbing/" + fileName, typeof(AudioClip));
			GetComponent<AudioSource>().Play ();
		}
		private void PlayCharAudio(string inputString){
			/* ADD LOGIC HERE */
			Debug.Log(inputString);
		}

		private void PlayCharToChar(string inputString){
			if (!audiogame) return;
			if (inputString.Length>0) 
			{				
				Talk ("key_"+inputString.Substring(0,1),true);
				Talk ("key_Alpha"+inputString.Substring(0,1),false);
			}
			for(int i=1;i<inputString.Length;i++)
			{
				Talk ("key_"+inputString.Substring(i,1),false);
				Talk ("key_Alpha"+inputString.Substring(i,1),false);
			}
		}

		private void GoToGame(){
			Debug.Log("AUTH OK");
			/* CHANGE SCENE HERE */
			_auth_ok = true;			
		}
	#endregion
	
	
	#region AUX
		/// <summary>
		/// Strips the input from escape chars
		/// </summary>
		private String StripInput(String input){
			input = input.Replace(" ","");
			input = input.Replace("-","");
			input = input.Replace("/","");
			input = input.Replace(".","");
			input = input.ToUpper();	
			return input;
		}
	
		/// <summary>
		/// Filters the allowed char.
		/// </summary>
		private String FilterAllowedChar(String input){
				if(input.Length>0){
					int charInt = (int)input[0];
					if(charInt==27) return "";
					if(charInt==8) return "";
					if(charInt<48 && charInt!=32) return "";
					//if(charInt>=48 && charInt<58) return "";
				}
				return input;
		}
	
	#endregion

	#region Delayer
		private float delayer=0;
		
		private bool Delay(float delay){
			delayer+=Time.deltaTime;
			if(delayer>delay){
				delayer=0;
				return true;
			}
			return false;
		}
	#endregion
}
