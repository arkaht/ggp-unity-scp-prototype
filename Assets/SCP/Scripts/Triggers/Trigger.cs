using UnityEngine;

[RequireComponent( typeof( BoxCollider ) )]
public class Trigger : MonoBehaviour
{
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
}