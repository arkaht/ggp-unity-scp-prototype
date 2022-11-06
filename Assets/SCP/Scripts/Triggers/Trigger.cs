using UnityEngine;

[RequireComponent( typeof( BoxCollider ) )]
public class Trigger : MonoBehaviour
{
	public bool IsSingleUse = true;
	public bool IsPlayerOnly = true;

	protected new BoxCollider collider;
	protected Color color = Color.gray;

	protected virtual void Awake()
	{
		collider = GetComponent<BoxCollider>();
	}
	
	void OnDrawGizmos()
	{
		if ( collider == null )
		{
			Awake();
		}

		Gizmos.color = new( color.r, color.g, color.b, .25f );
		Gizmos.DrawCube( collider.bounds.center, collider.bounds.size );
	}

	protected virtual void OnTrigger( Collider other ) {}

	void OnTriggerEnter( Collider other )
	{
		if ( IsPlayerOnly && other.gameObject != Player.Instance.gameObject ) return;
		
		//  disable on single use
		if ( IsSingleUse )
		{
			collider.enabled = false;
		}

		OnTrigger( other );
	}
}