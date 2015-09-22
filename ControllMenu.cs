using UnityEngine;
using System.Collections;

public class ControllMenu : MonoBehaviour
{
	/// <summary>
	/// The active menu.
	/// </summary>
	private Menu activeMenu;
	
	public string namaMenu;
	public AudioClip[] Bacaan;
	public AudioClip[] Surat;
	int fontSize;
	public GUISkin skin;
	// Use this for initialization
	void Start () {
		activeMenu = new MainMenuState(this);
	}
	
	
	// Show GUI
	void OnGUI(){
		activeMenu.ShowUI();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( activeMenu != null)
			activeMenu.StateUpdate();
	}
	
	/// <summary>
	/// Switchs the menu.
	/// </summary>
	/// <param name='newMenu'>
	/// New menu.
	/// </param>
	public void SwitchMenu(Menu newMenu)
	{
		activeMenu = newMenu;
	}
	
}

