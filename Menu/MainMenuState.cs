using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using ArabicSupport;

/// <summary>
/// Main menu state.
/// class yang berisi dan menampilkan pop-up menu yang ada dalam Main Menu dan sebagai tampilan awal aplikasi saat dijalankan
/// window yang akan ditampilkan yaitu :
/// 1. Menu Tata Cara Shalat
/// 2. Menu Syarat Sah Shalat
/// 3. Menu Tentang
/// 
/// Ditampilkan pula button sebagai access ke Menu Simulasi
/// </summary>
public class MainMenuState : Menu
{
	private ControllMenu controllMenu;			//	variabel penyimpan jenis state
	int indexMenu = 1;							//  variabel penyimpan index untuk menu yang ditampilkan
	float height,width,halfwidth,halfheight;	//  height 		: berisi nilai ukuran panjang layar
												//  width 		: berisi nilai ukuran lebar layar	
												//  halfheight  : berisi nilai ukuran panjang layar * 1/2
												//  halfwidth   : berisi nilai ukuran lebar layar * 1/2
	Vector2 scrollPosition = Vector2.zero;
	bool playAudioButton = false;
	/// <summary>
	/// Initializes a new instance of the <see cref="MainMenuState"/> class.
	/// </summary>
	/// <param name='controllMenuRef'>
	/// Controll menu reference.
	/// </param>
	public MainMenuState(ControllMenu controllMenuReff){
		controllMenu = controllMenuReff;
		height = Screen.height /8;
		width = height * 2;
		halfwidth = width/2;
		halfheight = height/2;
	}
	
	/// <summary>
	/// States the update.
	/// </summary>
	public void StateUpdate ()
	{
		if(playAudioButton == true){
			playAudioButtonClick();
			playAudioButton = false;
		}	
		if(indexMenu != 1){
			scanInputMobile();
		}
	}
	
	/// <summary>
	/// Shows the UI.
	/// </summary>
	public void ShowUI ()
	{
		GUI.skin = controllMenu.skin;
		switch (indexMenu) {
			case 1: drawMainMenu(); break;					
			case 2: drawInfoMenu("Tata Cara Shalat"); break;
			case 3: drawInfoMenu("Syarat Sah Shalat"); break;
			case 4: drawInfoMenu("Tentang"); break;
		}
	}
	
	/// GUI Section
	
	/// <summary>
	/// Draws the main menu.
	/// </summary>
	public void drawMainMenu(){
		// button Tata Cara Shalat
		if(GUI.Button(new Rect(width/25,height/50, width,height),"Tata Cara")){
			indexMenu = 2; playAudioButton = true;
		}
		// Syarat Sah
		if(GUI.Button(new Rect(width + 2 + width/25,height/25, width,height),"Syarat Sah")){
			indexMenu = 3; playAudioButton = true;
		}
		// About
		if(GUI.Button(new Rect(Screen.width - width - width/25,height/25, width,height),"About")){
			indexMenu = 4; playAudioButton = true;
		}
		if(GUI.Button(new Rect(Screen.width/2 - halfwidth,Screen.height/2 - halfheight, width,height),"Simulasi Shalat")){
			playAudioButton = true;
			swithToSimulationState();
		}
	}
	
	/// <summary>
	/// Draws the info menu.
	/// </summary>
	/// <param name='fileName'>
	/// File name.
	/// </param>
	public void drawInfoMenu(string fileName)
	{
		//GameObject Teks = GameObject.Find("Kampret");
		TextAsset contentFile = (TextAsset)Resources.Load("Text/"+fileName);
		string textContent = contentFile.text;
		Rect box = new Rect(Screen.width/2 - (Screen.width * 8/10)/2,Screen.height/2 - (Screen.height * 7/10)/2,Screen.width * 8/10,Screen.height * 7/10 );
		Rect box2 = new Rect(box.width * 0.07f ,box.height * 0.15f,Screen.width * 7/10,Screen.height * 6/10);
		
		GUILayout.BeginArea(box);
			GUILayout.Box(fileName,GUILayout.Width(Screen.width * 8/10),GUILayout.Height(Screen.height * 7/10));
			GUILayout.BeginArea(box2);
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width * 7/10), GUILayout.Height(Screen.height * 5.2f/10));
	        	GUILayout.Label(ArabicFixer.Fix(textContent,true,false));
	    	    GUILayout.EndScrollView();
			GUILayout.EndArea();
		GUILayout.EndArea();
		
		if(GUI.Button(new Rect(Screen.width - width - width/25,Screen.height -  height - height/25, width,height),"Kembali")){
			playAudioButtonClick();
			indexMenu = 1;
		}
	}
	
	
	// System Section
	
	public void playAudioButtonClick(){
		controllMenu.audio.Play();
	}
	
	public void swithToSimulationState(){
		controllMenu.SwitchMenu(new SimulationMenuState(controllMenu));
	}

	public void scanInputMobile(){
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
			scrollPosition += new Vector2(0,Input.GetTouch(0).deltaPosition.y);
		}
	}
}

