using UnityEngine;

public class PanelUI : MonoBehaviour
{
	public bool IsVisible => gameObject.activeSelf;
	
	public void Show()
	{
		gameObject.SetActive( true );

		Player.SetCursorLocked( false );
	}

	public void Hide()
	{
		gameObject.SetActive( false );

		Player.SetCursorLocked( true );
	}

	public void Toggle()
	{
		if ( IsVisible )
			Hide();
		else
			Show();
	}
}