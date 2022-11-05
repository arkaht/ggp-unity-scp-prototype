using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
	Image image;

	void Awake()
	{
		image = GetComponent<Image>();
	}

    void Update()
	{
		Player player = Player.Instance;
		image.color = new( image.color.r, image.color.g, image.color.b, 1.0f - (float) player.Health / (float) player.MaxHealth );
	}
}
