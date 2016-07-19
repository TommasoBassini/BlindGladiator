using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class AudioGestor
{
    public static AudioClip[] AudioClipArrayLoading(string pathFolder)
    {
        AudioClip[] clip = null;
        try
        {
            clip = Resources.LoadAll<AudioClip>(pathFolder);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Errore nel caricamento delle clips nel VocalGestor, path: " + pathFolder);
        }
        AudioClip[] clips = clip.OrderBy(go => go.name ).ToArray();
        return clips;
    }

}
