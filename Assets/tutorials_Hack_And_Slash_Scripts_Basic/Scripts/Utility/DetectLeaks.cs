/// <summary>
/// DetectLeaks.cs
/// Updated Feb 16, 2011 by Peter Laliberte @ http://www.burgzergarcade.com
/// 
/// This script was updated to get rid of the obsolete calls in Unity 3.2
/// Original script was from the Unity Wiki: http://www.unifycommunity.com/wiki/index.php?title=DetectLeaks
/// </summary>


using UnityEngine;
using System.Collections;

public class DetectLeaks : MonoBehaviour {

    void OnGUI () {
        GUILayout.Label("All " + Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length);
        GUILayout.Label("Textures " + Resources.FindObjectsOfTypeAll(typeof(Texture)).Length);
        GUILayout.Label("AudioClips " + Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length);
        GUILayout.Label("Meshes " + Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length);
        GUILayout.Label("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
        GUILayout.Label("GameObjects " + Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length);
        GUILayout.Label("Components " + Resources.FindObjectsOfTypeAll(typeof(Component)).Length);
    }
}
