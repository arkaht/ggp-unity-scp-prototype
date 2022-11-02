using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	bool isOpen = false;

	Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>(); 
	}

	public void SetToggle( bool is_open )
	{
		isOpen = is_open;
		animator.SetTrigger( is_open ? "Open" : "Close" );
	}

	public void Toggle()
	{
		SetToggle( !isOpen );
	}
}
