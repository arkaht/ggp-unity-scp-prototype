using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
	public float UseCooldown = 1.0f;
	float useCooldown = 0.0f;

	public UnityEvent useCallback;

	void OnTriggerEnter( Collider other )
	{
		var player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;

		player.UseEntity = this;
	}

	void OnTriggerExit( Collider other )
	{
		var player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;
		if ( player.UseEntity != this ) return;

		player.UseEntity = null;
	}

	void Update()
	{
		//  decrease cooldown
		if ( useCooldown > 0.0f )
		{
			useCooldown -= Time.deltaTime;
		}
	}

	public bool CanUse()
	{
		return useCooldown <= 0.0f;
	}

	public void Use()
	{
		if ( !CanUse() ) return;

		//  use
		OnUse();

		//  apply cooldown
		useCooldown = UseCooldown;
	}

	protected virtual void OnUse()
	{
		if ( useCallback == null ) 
		{
			Debug.LogError( "Button " + gameObject + " is not assigned to any events!" );
			return;
		}

		useCallback.Invoke();
	}
}
