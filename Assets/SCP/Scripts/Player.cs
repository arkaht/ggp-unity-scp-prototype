using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }
	
	public Button UseEntity { get; set; }

	void Awake()
	{
		Instance = this;
	}

	public void OnUse( InputValue input )
	{
		if ( UseEntity == null ) return;

		UseEntity.Use();
	}
}
