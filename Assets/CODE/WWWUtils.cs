using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Xml.Serialization;

public class WWWUtils : MonoBehaviour {
	
	#region static Factory
		public delegate void WWWResponseCallback(WWWResponse response);
		
		static private int instanceCounter=0;
	
		static public WWWUtils CreateRequest(String url_,WWWForm data_,WWWResponseCallback callback_){
			GameObject gameObject = new GameObject("_WWWRequest"+instanceCounter++);
			WWWUtils wwwUtils     = gameObject.AddComponent<WWWUtils>();
			wwwUtils.url       = url_;
			wwwUtils.data      = data_;
			wwwUtils.callback  = callback_;
			wwwUtils.Request();
			return wwwUtils;
		}
	#endregion
	
	#region Properties
		public String url;
		public WWWForm data;
		public WWWResponseCallback callback;
	#endregion
	
	#region Methods
		public void Request(){
			WWW www = new WWW(url,data);	
			StartCoroutine(WaitForRequest(www));
		}
	
		IEnumerator WaitForRequest(WWW www)
	    {
	        yield return www;	
			//if(www.error==null){
		
				if(www.text!=null){
					Debug.Log("-->"+www.text);
					WWWResponse response = WWWResponse.Create(www.text);
					response.www = www;
				
			        if(callback!=null) callback(response); 
				}
			//}
	    }
	#endregion
}


#region WWW response
[XmlRoot("Response")]
public class WWWResponse{
	
	static public WWWResponse Create(String responseString){
		try{
			XMLLoader xmlLoader  = new XMLLoader();
			return xmlLoader.LoadFromString<WWWResponse>(responseString);	
		}catch(Exception error){
			Debug.LogError("ERROR: "+error);
			WWWResponse response = new WWWResponse();
			response.error = "Connection Error";
			return response;
		}
		
	}
	
	
	[XmlIgnore]
	public WWW www;
	
	[XmlAttribute("Error")]
	public String error;
	[XmlAttribute("Success")]
	public String success;
	[XmlArray("Data")]
	[XmlArrayItem("Key")]
	public WWWKeyValue[] data; 
	
	[XmlIgnore]
	private Dictionary<String,String> _dataDictionary;
	[XmlIgnore]
	public  Dictionary<String,String> dataDictionary{
		get{
			if(_dataDictionary!=null) return _dataDictionary;
			
			_dataDictionary = new Dictionary<String, String>();
			if(data!=null) foreach(WWWKeyValue kv in data) _dataDictionary[kv.ID]=kv.Value;					
			return _dataDictionary;
		}
	}
	
	
	public bool HasError(){
		return www.error!=null || !String.IsNullOrEmpty(error);	
	}
	
	public String Value {get{
		if(data==null) return "";
		if(dataDictionary.ContainsKey("value")) return dataDictionary["value"];
		return "";
	}}
	
}

[XmlType("Key")]
public class WWWKeyValue{
	[XmlAttribute("ID")]
	public String ID;
	
	[XmlText]
	public String Value;
}
#endregion
