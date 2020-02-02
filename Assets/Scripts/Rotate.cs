using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{
	public Vector3 rotation = Vector3.zero;
	public bool startRotating = true;
	void Update () 
	{
		if( startRotating )
			transform.Rotate( rotation * Time.deltaTime );
	}

	public void EnableRotation()
	{
		startRotating = true;
	}
	public void DisableRotation()
	{
		startRotating = false;
	}
}
