using UnityEngine;

public class ActivableEntityTrigger : Trigger
{
	public ActivableEntity Entity;

	protected override void Awake()
	{
		base.Awake();

		color = new( 1.0f, 1.0f, 0.0f );
	}

	protected override void OnTrigger( Collider other )
	{
		Entity.Activate( Player.Instance, this );
	}
}