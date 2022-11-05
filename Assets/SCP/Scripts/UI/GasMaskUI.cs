using UnityEngine;
using UnityEngine.UI;

public class GasMaskUI : MonoBehaviour
{
	public static GasMaskUI Instance { get; private set; }

	Image image;

	void Awake()
	{
		image = GetComponent<Image>();

		Instance = this;

		Hide();
	}

	public void Show()
	{
		image.enabled = true;
	}

	public void Hide()
	{
		image.enabled = false;
	}
}