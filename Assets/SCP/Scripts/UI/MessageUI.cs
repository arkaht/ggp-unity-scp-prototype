using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
	public static MessageUI Instance { get; private set; }

	public float ShowTime = 3.0f;

	float showTime = 0.0f;

    Text MessageText;

    void Awake()
	{
		Instance = this;

		MessageText = GetComponent<Text>();
	}


	void Update()
	{
		if ( showTime > 0.0f && ( showTime -= Time.deltaTime ) <= 0.0f )
		{
			MessageText.enabled = false;
		}
	}

	public void ShowText( string text )
	{
		MessageText.text = text;
		MessageText.enabled = true;

		showTime = ShowTime;
	}
}
