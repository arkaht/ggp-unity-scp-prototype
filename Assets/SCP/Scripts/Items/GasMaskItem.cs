using UnityEngine;

public class GasMaskItem : Item
{
	public override void OnEquiped()
	{
		//  play drop sound
		AudioNotification.PlayAudioAt( transform.position, DropSound, 0.5f );

		GasMaskUI.Instance.Show();
	}

	public override void OnUnEquiped()
	{
		//  play drop sound
		AudioNotification.PlayAudioAt( transform.position, DropSound, 0.5f );

		GasMaskUI.Instance.Hide();
	}
}