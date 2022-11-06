using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
	public float SmoothSpeed = 5.0f;

	Image image;

	void Awake()
	{
		image = GetComponent<Image>();
	}

    void Update()
	{
		Player player = Player.Instance;

		Color color = image.color;
		color.a = Mathf.Lerp( color.a, 1.0f - (float) player.Health / player.MaxHealth, Time.deltaTime * SmoothSpeed );
		image.color = color;
	}
}
