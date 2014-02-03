using UnityEngine;
using UnityEditor;
 
[InitializeOnLoad]
public class GA_Autorun
{
    static GA_Autorun()
    {
		GA_Inspector.CheckForUpdates();
    }
}