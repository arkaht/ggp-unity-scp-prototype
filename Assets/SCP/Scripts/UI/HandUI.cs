using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
	public float ScreenToCenterWeight = 0.4f;
	public int ClampMargin = 200;

	public Sprite ItemSprite, ActionSprite;

	Image image;

	Vector2 screenSize = new( Screen.width, Screen.height );

	void Awake()
	{
		image = GetComponent<Image>();
	}

	void Update()
	{
		Player player = Player.Instance;
		if ( player == null ) return;

		//  check use entity
		UseableEntity entity = player.UseEntity;
		if ( entity == null || !entity.CanUse( player ) )
		{
			image.enabled = false;
			return;
		};

		//  show image
		image.enabled = true;
		image.sprite = entity is Item ? ItemSprite : ActionSprite;

		//  get screen pos
		Vector2 screen_pos = Camera.main.WorldToScreenPoint( entity.transform.position );

		//  clamp screen pos
		screen_pos.x = Mathf.Clamp( screen_pos.x, ClampMargin, screenSize.x - ClampMargin );
		screen_pos.y = Mathf.Clamp( screen_pos.y, ClampMargin, screenSize.x - ClampMargin );

		//  lerp hand pos from screen pos to screen center
		transform.position = Vector2.Lerp( screen_pos, screenSize / 2.0f, ScreenToCenterWeight );
	}
}
