using UnityEngine;
using System.Collections;

[AddComponentMenu("Hack And Slash Tutorial/Objects/Spawn Point")]
public class SpawnPoint : MonoBehaviour {
	public void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 2);
	}
}
