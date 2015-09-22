using UnityEngine;
using System.Collections;

public class SimulationMenuState : Menu
{
	
	private ControllMenu controllMenu;
	
	/// <summary>
	/// The index menu.
	/// </summary>
	int indexMenu = 1;	
	bool DoShalat = false;
	bool DoGerakan = false;
	bool playAudioButton = false;
	bool resetShalat = false;
	Vector2 scrollPosition = Vector2.zero;
	string namaGerakan = "";
	string typeSideMenu = "";
	float height = 0f;
	string StateButtonAnimation = "Play";
	bool playAnimation = false;
	GameObject Suara = GameObject.Find("AudioBacaan");
	CameraMovement CM = new CameraMovement();
	ModelGerakan human = new ModelGerakan();
	ModelShalat MS = new ModelShalat();
	
	public SimulationMenuState(ControllMenu controllMenuReff){
		controllMenu = controllMenuReff;
		human.Bacaan = controllMenu.Bacaan;
		MS.Surat = controllMenu.Surat;
		MS.human = human;
		MS.stateModel = 1;
	}
	
	
	public void StateUpdate ()
	{
		// membca input dari user berupa sentuhan pada layar
		CM.scanInput();
		
		if(Screen.height != height)
		{
			height = Screen.height/8;
		}
		// Play Audio untuk button bila ditekan	
		if(playAudioButton == true){
			playAudioButtonClick();
			playAudioButton = false;
		}
	
		if(indexMenu == 4){
			scanInputForScrolling();
		}
		//UnityEngine.Debug.Log(MS.stateModel+"-"+DoShalat);
		// menangkap status gerakan shalat yang sedang dilakukan
		// default nilai adalah 0, tidak melakukan apapun
		if(DoGerakan == true)
		{
			doGerakanShalat();
		}
		if(DoShalat == true){
			doShalat();
		}
		
		if(resetShalat == true){
			MS.doReset();
			playAnimation = false;
			indexMenu = 5;
			resetShalat = false;
		}
	UnityEngine.Debug.Log(human.stateGerak);
	}

	public void ShowUI ()
	{
		GUI.skin = controllMenu.skin;
		switch (indexMenu) {
			case 1: drawDecisionMenu(); break;
			case 2: drawPengenalanGerakanMenu(); break;
			case 3: drawPengenalanShalatMenu(); 
					MS.namaSurat[0] = "Al-Ikhlas";
					MS.namaSurat[1] = "Al-Kautsar";break;
			case 4: drawSideMenu(namaGerakan,typeSideMenu); break;
			case 5: human.doDefaultPosition(); indexMenu = 1; break;
		}
	}
	
		
	
	// GUI
	
	/// <summary>
	/// Draws the decision menu.
	/// Menampilkan 2 pilihan menu simulasi
	/// 1. simulasi gerakan shalat
	/// 2. simulasi shalat
	/// </summary>
	public void drawDecisionMenu(){
		
		if(GUI.Button(new Rect(Screen.width/2 - height * 4,Screen.height/2,height * 3, height),"Pengenalan Gerakan Shalat"))
		{
			indexMenu = 2;	 playAudioButton = true;		
		}
		if(GUI.Button(new Rect(Screen.width/2 + height,Screen.height/2,height * 3, height),"Pengenalan Shalat"))
		{
			indexMenu = 3;	 playAudioButton = true;		
		}
		
		if(GUI.Button(new Rect(Screen.width -  (height * 2)- (height * 2)/25,Screen.height -  (height ) - (height)/25, (height * 2),(height)),"Kembali")){
			playAudioButton = true;
			swithToMainMenuState();
		}
	}
	
	/// <summary>
	/// Draws the pengenalan gerakan menu.
	/// </summary>
	public void drawPengenalanGerakanMenu(){
		
		drawButtonListShalat();
		drawExtraButton();
		if(namaGerakan != ""){
			drawButtonInfoDanKeterangan();
		}
		if(GUI.Button(new Rect(Screen.width -  (height * 2)- (height * 2)/25,Screen.height -  (height ) - (height)/25, (height * 2),(height)),"Kembali")){
			playAudioButton = true;
			indexMenu = 5;
		}
	
	}
	
	
	/// <summary>
	/// Draws the button list shalat.
	/// </summary>
	public void drawButtonListShalat(){
		if(GUI.Button(new Rect((height * 2)/25,(height)/50,(height * 3),(height)),"Takbiratulihram")){
			namaGerakan = "Takbiratulihram"; DoGerakan = true; human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height),(height * 3),(height)),"Ruku'")){
			namaGerakan = "Ruku"; DoGerakan = true;human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height) * 2,(height * 3),(height)),"I\'tidal")){
			namaGerakan = "Itidal"; DoGerakan = true;	human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height) * 3,(height * 3),(height)),"Sujud")){
			namaGerakan = "Sujud"; DoGerakan = true;human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height) * 4,(height * 3),(height)),"Duduk diantara 2 Sujud")){
			namaGerakan = "DudukDiantara2Sujud";DoGerakan = true;human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height) * 5,(height * 3),(height)),"Tahiyat Awal")){
			namaGerakan = "TahiyatAwal"; DoGerakan = true;human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height) * 6,(height * 3),(height)),"Tahiyat Akhir")){
			namaGerakan = "TahiyatAkhir"; DoGerakan = true;human.stateGerak = 1;
		}
		if(GUI.Button(new Rect((height * 2)/25,(height)/50 + (height) * 7,(height * 3),(height)),"Salam")){
			namaGerakan = "Salam"; DoGerakan = true;human.stateGerak = 1;
		}
		
	}
	
	/// <summary>
	/// Draws the button info dan keterangan.
	/// </summary>
	public void drawButtonInfoDanKeterangan(){
		if(GUI.Button(new Rect(Screen.width - (height * 2) - (height * 2)/25,(height)/25,(height * 2),(height)),"Info")){
			typeSideMenu = "Info"; indexMenu = 4;
		}
		if(GUI.Button(new Rect(Screen.width - (height * 2) - (height * 2)/25,(height) + (height)/25,(height * 2),(height)),"Ket.")){
			typeSideMenu = "Keterangan"; indexMenu = 4;
		}
		
	}
	
	public void drawExtraButton(){
		if(GUI.Button(new Rect(Screen.width - (height * 2) - (height * 2)/25,(height)*2 + (height)/25,(height * 2),(height)),"Sujud-Takbir")){
			namaGerakan = "SujudToTakbir"; DoGerakan = true;human.stateGerak = 1; 
		}
		if(GUI.Button(new Rect(Screen.width - (height * 2) - (height * 2)/25,(height)*3 + (height)/25,(height * 2),(height)),"TahiyyatAwal-Takbir")){
			namaGerakan = "TahiyatToTakbir"; DoGerakan = true;human.stateGerak = 1; human.stepTahiyatToTakbir = 0;
		}
		if(GUI.Button(new Rect(Screen.width - (height * 2) - (height * 2)/25,(height)*4 + (height)/25,(height * 2),(height)),"TahiyyatAwal-Takbir")){
			namaGerakan = "DudukToSujud"; DoGerakan = true;human.stateGerak = 1; 
		}
		
		if(GUI.Button(new Rect(Screen.width - (height * 2) - (height * 2)/25,(height)*5 + (height)/25,(height * 2),(height)),"TahiyyatAwal-Takbir")){
			if(playAnimation == false)
			{
				playAnimation = true;
				DoGerakan = false;
				Suara.audio.Pause();
			}else{
				
				Suara.audio.Play();
				DoGerakan = true;
				playAnimation = false;
			}
		}
	}
	
	/// <summary>
	/// Draws the side menu.
	/// </summary>
	/// <param name='namaGerakan'>
	/// Nama gerakan.
	/// </param>
	/// <param name='typeSideMenu'>
	/// Type side menu. 1. Info, 2. Keterangan
	/// </param>
	public void drawSideMenu(string namaGerakan,string typeSideMenu){
		TextAsset contentFile = (TextAsset)Resources.Load("Text/"+string.Concat(namaGerakan,typeSideMenu));
		string textContent = contentFile.text;
		Rect boxSideMenu = new Rect(Screen.width - (Screen.width/2),(height)/25,(Screen.width/2),Screen.height);
		float heighTextArea = Screen.height - (height * 3);
		GUI.BeginGroup(boxSideMenu);
		GUI.Box(new Rect(0,0,boxSideMenu.width,boxSideMenu.height),typeSideMenu);
		if(typeSideMenu.Equals("Info")){
			if(GUI.Button(new Rect(height/25 * 2, height + height/10,boxSideMenu.width - (height/25 * 2) ,height),"Mainkan Suara Bacaan")){
				playAudio(namaGerakan);	
			}
			heighTextArea -= height;
		}
		GUILayout.BeginArea(new Rect(height	- height/3,height * 2 + height/10,boxSideMenu.width,heighTextArea));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(boxSideMenu.width - height), GUILayout.Height(heighTextArea));
		GUILayout.Label(textContent);
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		if(GUI.Button(new Rect(boxSideMenu.width - (height * 2.2f),boxSideMenu.height - height * 1.3f,height * 2, height),"Kembali")){
			indexMenu = 2;
		}
		GUI.EndGroup();
	}
	
	/// <summary>
	/// Draws the pengenalan shalat menu.
	/// </summary>
	public void drawPengenalanShalatMenu(){
		
		GUI.Box(new Rect((height*2)/25,(height*2)/50 ,height * 2.5f, height * 5),"");
		GUI.Label(new Rect((height*2)/25 * 3,(height*2)/50 	,height * 2 ,height /2),"Shalat");
		GUI.Button(new Rect((height*2)/25 * 3,(height*2)/50 + height/2 ,height * 2,height),"Shubuh");
		GUI.Label(new Rect((height*2)/25 * 3,(height*2)/50 + height + height/2 ,height * 2,height/2),"Raka\'at 1");
		GUI.Button(new Rect((height*2)/25 * 3,(height*2)/50 + height * 2,height *2,height),"Al-Ikhlas");
		GUI.Label(new Rect((height*2)/25 * 3,(height*2)/50 + height * 3 ,height * 2,height/2),"Raka\'at 2");
		GUI.Button(new Rect((height*2)/25 * 3,(height*2)/50 + height * 3 + height/2,height *2,height),"Al-Ikhlas");
		
		if(GUI.Button(new Rect(0 + (height * 2)/25,Screen.height -  (height ) - (height)/25, (height * 2),(height)),StateButtonAnimation)){
			playAudioButton = true;
			if(playAnimation == true)
			{
				playAnimation = false;
				DoShalat = false;
				Suara.audio.Pause();
			}else{
				playAnimation = true;
				DoShalat = true;
				Suara.audio.Play();
			}
		}
		
		if(GUI.Button(new Rect(Screen.width -  (height * 2)- (height * 2)/25,Screen.height -  (height ) - (height)/25, (height * 2),(height)),"Kembali")){
			playAudioButton = true;
			resetShalat = true;
		}
	}
	
	// System
	
	public void playAudioButtonClick(){
		controllMenu.audio.Play();
	}
	public void swithToMainMenuState(){
		controllMenu.SwitchMenu(new MainMenuState(controllMenu));
	}
	public void scanInputForScrolling(){
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
			scrollPosition += new Vector2(0,Input.GetTouch(0).deltaPosition.y);
		}
	}	
	public void doGerakanShalat(){
		human.doGerakanShalat(namaGerakan);
		if (human.stateGerak == 0) {
			DoGerakan = false;
		}
		//UnityEngine.Debug.Log(human.stateGerak);
	}
	void playAudio(string NamaGerakan){
		int index = 0;
		switch (NamaGerakan) {
			case "Takbiratulihram" : index = 10; break;
			case "Ruku": index = 3; break;
			case "Sujud" : index = 5; break;
			case "Itidal" : index = 4; break;
			case "DudukDiantara2Sujud" : index = 6; break;
			case "TahiyatAwal" : index = 7; break;
			case "TahiyatAkhir" : index = 8; break;
			case "Salam" : index = 9; break;
		}
		if(!Suara.audio.isPlaying)
		{
			Suara.audio.PlayOneShot(controllMenu.Bacaan[index]);
		}
	}
	public void doShalat(){
		MS.doShalat();
		if(MS.stateModel == 0){
			DoShalat = false;
			MS.stateModel = 1;
		}
		
	}
}

