using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : PanelUI
{
	public static MenuUI Instance { get; private set; }

	public Text TitleText, MessageText;

	void Awake()
	{
		Instance = this;

		Hide();
	}

	public void SetTitle( string title )
	{
		TitleText.text = title;
	}

	public void SetMessage( string text )
	{
		MessageText.text = text;
	}

	public void Restart()
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
	}

	public void Quit()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
