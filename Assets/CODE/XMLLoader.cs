using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection; 
using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;



/// <summary>
/// Load Object from XML
/// </summary>
public class XMLLoader : IDataLoader
{
	static protected bool TRACE_FILES=false;
	
	static public bool CACHE_FILES=true;
	/*
	/// <summary>
	/// ===============================================================================
	/// Load via XMLDocument
	/// ===============================================================================
	/// </summary>
	public T Load<T>(String filePath)
	{
		filePath = Application.dataPath.Substring(0,
		                                          Application.dataPath.LastIndexOf("/") + 1)
												  +filePath;
        
		Trace.Log("Load File:"+filePath);
		//------------------------------------------------------------------------------
		XmlDocument xmlDoc = new XmlDocument();
 		xmlDoc.Load(filePath);
		
		
		String str = CommonUtils.HTMLEscapeCharEncode(xmlDoc.OuterXml);
		StringReader textReader = new StringReader(str);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //------------------------------------------------------------------------------
		object result = xmlSerializer.Deserialize(textReader);
		return (T)result;
	}
	*/
	
	/// <summary>
	/// Load & Convert an XML file into an object of T class
	/// </summary>
	public T Load<T>(String filePath)
	{
		if(TRACE_FILES) Debug.Log("Load File:"+filePath);
		//load the file
		String str = LoadPlainText(filePath);
		//convert the xmlString into object of T
		StringReader textReader = new StringReader(str);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //------------------------------------------------------------------------------
		System.Object result = xmlSerializer.Deserialize(textReader);
		SetFileProperty(result,filePath);
		return (T)result;
	}
	
	public System.Object Load(Type classType, String filePath)
	{
		if(TRACE_FILES) Debug.Log("Load File:"+filePath);
		//load the file
		String str = LoadPlainText(filePath);
		//convert the xmlString into object of T
		StringReader textReader = new StringReader(str);
        XmlSerializer xmlSerializer = new XmlSerializer(classType);
        //------------------------------------------------------------------------------
		System.Object result = xmlSerializer.Deserialize(textReader);
		SetFileProperty(result,filePath);
		return result;		
	}
	

	
	/// <summary>
	/// Convert an XML string into an object of T class
	/// </summary>
	public T LoadFromString<T>(String xmlString){
		if(TRACE_FILES) Debug.Log("Deserializing "+xmlString);
		StringReader textReader = new StringReader(xmlString);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //------------------------------------------------------------------------------
		object result = xmlSerializer.Deserialize(textReader);
		return (T)result;	
	}
	

	
	
	
	/// <summary>
	/// Loads a text file
	/// </summary>
	public String LoadPlainText(String filePath){		
		filePath = filePath.Replace("Assets/Resources/","");
		filePath = filePath.Replace(".xml","");
		filePath = filePath.Replace(".txt","");
		filePath = filePath.Replace(".zs","");

		if(!CACHE_FILES){
			return GetTextAssetAsString(filePath);
		}else{
			if(_plainTextCache.ContainsKey(filePath)) return _plainTextCache[filePath];
			String str =  GetTextAssetAsString(filePath);
			_plainTextCache[filePath]=str;
			return str;
		}
	}
	static private Dictionary<String,String> _plainTextCache = new Dictionary<String,String>();
	
	public String GetTextAssetAsString(String filePath){
		//Debug.Log(filePath);
		TextAsset textAsset = Resources.Load(filePath) as TextAsset;
		return textAsset.text;	
	}
	
	
	
	protected void SetFileProperty(System.Object result,String filePath){
		FieldInfo fieldInfo = result.GetType().GetField("FileName");
		if(fieldInfo!=null) fieldInfo.SetValue(result,filePath);
	}
	
	public XmlDocument LoadXmlDocument(String filePath){
		XmlDocument doc = null;
		filePath = filePath.ToLower(); 
		// get resource relative path
		string relativePath = ""; 
		if (filePath.Contains("resources") || filePath.Contains(":"))
		{
			string resourcesPath = Application.dataPath+"/resources/"; 
			string ext = Path.GetExtension(filePath); 
			relativePath = filePath.Replace(resourcesPath,"");
			if (ext!="") relativePath = relativePath.Replace(ext,"");
		}
		else relativePath = filePath; 
#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR)
		// try loading the file directly, without using Resources
		// get absolute path
		if (!filePath.Contains("resources") && !filePath.Contains(":")) filePath = Application.dataPath+"/resources/"+filePath+".xml";
		if (File.Exists(filePath))
		{
			// create reader and settings
		    XmlReaderSettings Settings = new XmlReaderSettings();
		    Settings.XmlResolver = null;
		    Settings.ProhibitDtd = false;
		    XmlTextReader Reader = (XmlTextReader) XmlTextReader.Create(filePath,Settings);
		    Reader.EntityHandling = EntityHandling.ExpandCharEntities;
			// load doc
			if (Reader!=null) 
			{
				doc = new XmlDocument();
			    doc.Load(Reader);
			    Reader.Close();								
			}
			return doc; 
		}
#endif		
		// try loading from Resources
		TextAsset textAsset = Resources.Load(relativePath) as TextAsset;
		if (textAsset!=null)
		{		    
			doc = new XmlDocument();
			try
			{
				doc.LoadXml(GetTextWithoutBOM(textAsset)); 
				return doc; 
			}
			catch
			{
				Debug.LogError("Error loading Xml: "+filePath); 
			}
		}
		return null; 
	}
	
	public string GetTextWithoutBOM(TextAsset textAsset){
		MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
		StreamReader streamReader = new StreamReader(memoryStream,true); 
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		memoryStream.Close();
		return result;
		
	}
	
	#region other filesystem loaders.. to test / switch..
	
	/*
	/// <summary>
	/// ===============================================================================
	/// Load via Resources.Load
	/// ===============================================================================
	/// </summary>
	public T Load<T>(String filePath)
	{
		filePath = filePath.Replace("Assets/Resources/","");
		filePath = filePath.Replace(".xml","");
        //------------------------------------------------------------------------------
		Trace.Log("::LoadXML["+filePath+"]");
		
		TextAsset textAsset = Resources.Load(filePath) as TextAsset;
		String str = textAsset.text;
		StringReader textReader = new StringReader(str);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //------------------------------------------------------------------------------
		object result = xmlSerializer.Deserialize(textReader);
		return (T)result;
		
	}
	*/
	
	/*
	/// <summary>
	/// ===============================================================================
	/// Load via FileStream
	/// ===============================================================================
	/// </summary>
	public T Load<T>(String filePath)
	{
        //------------------------------------------------------------------------------
        FileStream fileStream       = new FileStream(filePath, FileMode.Open);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //------------------------------------------------------------------------------
		object result = xmlSerializer.Deserialize(fileStream);
		fileStream.Close();
		return (T)result;
		
	}
	*/
	
	#endregion
}