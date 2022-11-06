using UnityEngine;

public class EndingTrigger : Trigger
{
	[TextArea( 3, 10 )]
	public string Message;

	protected override void Awake()
	{
		base.Awake();

		color = new( 0.5f, 0.0f, 1.0f );
	}

	protected override void OnTrigger( Collider other )
	{
		MenuUI menu = MenuUI.Instance;
		menu.SetTitle( "YOU ESCAPED" );
		menu.SetMessage( "Subject: D-9341\n\n" + Message );
		menu.Show();

		Player.Instance.StopMovement();
	}
}