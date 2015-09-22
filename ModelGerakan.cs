using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Model gerakan.
/// </summary>
public class ModelGerakan
{
	
	int stateTahap1 = 0;
	int stateTahap2 = 0;
	int stepSalam = 0;
	float speed = 1.0f;
	public int stepTakbirToSujud = 0;
	public int stepKakiTakbirToSujud = 0;
	public int stepTahiyatToTakbir = 0;
	public AudioClip[] Bacaan;
	GameObject objectPlayBacaan = GameObject.Find("AudioBacaan");
		
	public int stateGerak = 1;
	KondisiAwal	kAwalObjek = new KondisiAwal();
	
	public ModelGerakan(){
	}
	
	float normalizeLimit(float limit){
		if(limit < 0){
			return 360+limit;
		}else{
			return limit;
		}
	}
	
	float normalizeAxisObject(float AxisValue,float limit){
		if(AxisValue !=normalizeLimit(limit)){
			if(limit < 0 && AxisValue <= 0 ){
				AxisValue =  360;
			}
			limit = normalizeLimit(limit);
			if(AxisValue > limit){
				return -1;
			}else{
				return 1;
			}
		}else{
			return 0;
		}
	}
	
	int Rotasi(string namaBagian,float limitX, float limitY, float limitZ){
		
		GameObject objek = GameObject.Find(namaBagian);
		float plusX,plusY,plusZ;
		Vector3 rotateValue;
		
		plusX = normalizeAxisObject(Mathf.Round(objek.transform.localEulerAngles.x),limitX);
		plusY = normalizeAxisObject(Mathf.Round(objek.transform.localEulerAngles.y),limitY);
		plusZ = normalizeAxisObject(Mathf.Round(objek.transform.localEulerAngles.z),limitZ);
		rotateValue = new Vector3(plusX,plusY,plusZ);
		
		if(rotateValue != Vector3.zero){
			objek.transform.localEulerAngles += rotateValue * speed;
			return 0;	
		}else{
			return 1;
		}
	}
	
	int Rotasi(string namaBagian,float limitX, float limitY, float limitZ,int loop){
		int temp = 0;
		for(int i=0; i< loop; i++){
			temp = Rotasi(namaBagian,limitX,limitY,limitZ);
		}
		return temp;
	}
	
	void doKondisiAwal(string NamaGerakan){
		GameObject temp;
	//	kAwalObjek.dg = KondisiAwal.Load(Path.Combine(Application.dataPath,"Resources/Data/"+NamaGerakan+".xml"));
		kAwalObjek.dg = KondisiAwal.Load(NamaGerakan);
	
		foreach (KondisiAwal.DataGerakan.BagianBadan a in kAwalObjek.dg.KondisiAwal) {
			temp = GameObject.Find(a.nama);	
			temp.transform.localEulerAngles = new Vector3(a.PotRot.RotX,a.PotRot.RotY,a.PotRot.RotZ);
			temp.transform.localPosition	= new Vector3(a.PotRot.x,a.PotRot.y,a.PotRot.z);
		}
		setAnimationSpeed(NamaGerakan);
		
		stateGerak = 2;
	}
	
	public void doDefaultPosition(){
		GameObject temp;
	//	kAwalObjek.dg = KondisiAwal.Load(Path.Combine(Application.dataPath,"Resources/Data/Takbiratulihram.xml"));
		kAwalObjek.dg = KondisiAwal.Load("Takbiratulihram");
	
		foreach (KondisiAwal.DataGerakan.BagianBadan a in kAwalObjek.dg.KondisiAwal) {
			temp = GameObject.Find(a.nama);	
			temp.transform.localEulerAngles = new Vector3(a.PotRot.RotX,a.PotRot.RotY,a.PotRot.RotZ);
			temp.transform.localPosition	= new Vector3(a.PotRot.x,a.PotRot.y,a.PotRot.z);
		}
		stateGerak = 0;
		if(objectPlayBacaan.audio.isPlaying){
			objectPlayBacaan.audio.Stop();
		}
		if(objectPlayBacaan.audio.clip != null){
			objectPlayBacaan.audio.clip = null;
		}
	}
	
	void doTahap1Takbiratulihram(){
		int checkstate = 0;
			
			checkstate += Rotasi("LeftShoulder",0,0,0);
			checkstate += Rotasi("RightShoulder",0,0,359);
			checkstate += Rotasi("LeftArm",0,0,80);
			checkstate += Rotasi("RightArm",0,0,-80);
			checkstate += Rotasi("LeftForeArm",-15,115,5);
			checkstate += Rotasi("RightForeArm",-15,-115,-5);
			checkstate += Rotasi("LeftHand",85,60,0);
			checkstate += Rotasi("RightHand",85,-60,0);		
			if(checkstate == 8){
				stateTahap1 = 1;
			}
	}
	void doTahap1Ruku(){
		int checkState = 0;
		checkState += Rotasi("LeftArm",-1,0,80);
		checkState += Rotasi("LeftForeArm",-15,115,5);
		checkState += Rotasi("LeftHand",85,60,0);
		checkState += Rotasi("RightHand",85,-60,-1);
		checkState += Rotasi("LeftHandThumb1",0,-20,-20);
		checkState += Rotasi("RightHandThumb1",0,20,20);
		checkState += Rotasi("RightArm",-1,0,-80);
		checkState += Rotasi("RightForeArm",-15,-115,-5);
		
		if(checkState == 8){
			stateTahap1 = 1;
		}
	}
	void doTahap1Itidal(){
		int checkState = 0;
		checkState += Rotasi("Spine",0,0,0);
   		checkState += Rotasi("Spine1",0,0,0);
		checkState += Rotasi("LeftShoulder",-1,0,0);
		checkState += Rotasi("RightShoulder",-1,0,0);
		checkState += Rotasi("LeftArm",0,0,75);
		checkState += Rotasi("RightArm",0,0,-75);
		checkState += Rotasi("LeftForeArm",-15,115,5);
		checkState += Rotasi("RightForeArm",-15,-115,-5);
		checkState += Rotasi("LeftHand",85,60,0);
		checkState += Rotasi("RightHand",85,-60,0);

		if(checkState == 10)
		{
			stateTahap1 = 1;
		}
	}
	void doTahap1Sujud(){
		int checkState = 0;
		GameObject LeftLeg = GameObject.Find("LeftLeg");
		GameObject RightLeg = GameObject.Find("RightLeg");
		GameObject Hips = GameObject.Find("Hips");
		GameObject Hand = GameObject.Find("LeftHand");
		Vector3 batasSujud1 = new Vector3(30,180,180);

		Vector3 batasputar = new Vector3(80,180,180);
		float batasTurun = 4.8f;
	
		checkState += Rotasi("LeftShoulder",-30,15,1);
		checkState += Rotasi("RightShoulder",-30,-15,-1);
		checkState += Rotasi("LeftArm",5,0,85);
		checkState += Rotasi("RightArm",5,0,-85);
		checkState += Rotasi("LeftForeArm",-20,15,7);
		checkState += Rotasi("RightForeArm",-20,-15,-7);
		checkState += Rotasi("Spine",40,0,0);
		checkState += Rotasi("LeftUpLeg",-30,0,-5);
		checkState += Rotasi("RightUpLeg",-30,0,5);
		checkState += Rotasi("LeftFoot",-60,0,0,5);
		checkState += Rotasi("RightFoot",-60,0,0,5);
		checkState += Rotasi("LeftToeBase",-80,0,0,2);
		checkState += Rotasi("RightToeBase",-80,0,0,2);
		checkState += Rotasi ("LeftHand",85,25,0,5);
		checkState += Rotasi ("RightHand",85,-25,0,5);
		for(int i = 0; i<2; i++){
		// digunakan Library Unity karena rotasi manual yang dibuat tak bia lebih dari 90 derajat.
			if(LeftLeg.transform.localEulerAngles != batasSujud1){
				LeftLeg.transform.Rotate(1f,0,0);
				RightLeg.transform.Rotate(1f,0,0);
			}else{
				checkState++;
				break;
			}
		}
		if(Hips.transform.localPosition.y > batasTurun){
			Hips.transform.Translate(0,(-0.01f)/3.15f,0);
		}else{
			checkState++;
		}
		
		if(checkState == 17){
			stateTahap1 = 1;
		}
		
	}
	void doTahap1DudukDiantara2Sujud(){
		int checkState = 0;
		float batasTurun = 3f;
		GameObject Hips = GameObject.Find("Hips");
		
		if(Hips.transform.localPosition.y > batasTurun){
			Hips.transform.Translate(0,(-0.01f)/3,0);
		}else{
			checkState++;
		}
		
		checkState += Rotasi("Hips",1,0,0,3);
		checkState += Rotasi("Neck",-1,0,0);
		checkState += Rotasi("Spine",10,0,0,2);
		checkState += Rotasi("Spine1",1,0,0);
		checkState += Rotasi("Spine2",1,0,0);
		
		if(Hips.transform.localEulerAngles.x < 25){
			checkState += Rotasi("LeftShoulder",-30,15,0);
			checkState += Rotasi("RightShoulder",-30,-15,0);
		}
		checkState += Rotasi("LeftArm",5,0,85);
		checkState += Rotasi("RightArm",5,0,-85);
		checkState += Rotasi("LeftForeArm",-20,10,5,5);
		checkState += Rotasi("RightForeArm",-20,-10,-5,5);
		checkState += Rotasi("LeftHand",85,30,0);
		checkState += Rotasi("RightHand",-85,-30,-1);
		checkState += Rotasi("LeftLeg",15,180,180);
		checkState += Rotasi("RightLeg",1,180,195,2);
		checkState += Rotasi("LeftFoot",-1,80,0,8);
		checkState += Rotasi("RightFoot",-20,1,1,5);		
		
		if(checkState == 19)
		{
			stateTahap1 = 1;
		}
		
	}
	void doTahap1TahiyyatAwal(){
		int checkState = 0;
		float batasTurun = 3f;
		GameObject Hips = GameObject.Find("Hips");
		
		if(Hips.transform.localPosition.y > batasTurun){
			Hips.transform.Translate(0,(-0.01f)/3,0);
		}else{
			checkState++;
		}
		
		checkState += Rotasi("Hips",1,0,0,3);
		checkState += Rotasi("Neck",-1,0,0);
		checkState += Rotasi("Spine",10,0,0,2);
		checkState += Rotasi("Spine1",1,0,0);
		checkState += Rotasi("Spine2",1,0,0);
		
		if(Hips.transform.localEulerAngles.x < 25){
			checkState += Rotasi("LeftShoulder",-30,15,0);
			checkState += Rotasi("RightShoulder",-30,-15,0);
		}
		checkState += Rotasi("LeftArm",5,0,85,5);
		checkState += Rotasi("RightArm",5,0,-85,5);
		checkState += Rotasi("LeftForeArm",-20,10,5,4);
		checkState += Rotasi("RightForeArm",-20,-5,-5,4);
		checkState += Rotasi("LeftHand",85,30,0);
		checkState += Rotasi("RightHand",-60,-30,30);
		checkState += Rotasi("LeftLeg",15,180,180);
		checkState += Rotasi("RightLeg",1,180,195,2);
		checkState += Rotasi("LeftFoot",-1,80,0,8);
		checkState += Rotasi("RightFoot",-20,1,1);		
		
		// Gerakan Jari Pada Tangan Kanan
		
		checkState += Rotasi("RightHandMiddle1",0,0,-40,5);
		checkState += Rotasi("RightHandRing1",0,0,-40,5);
		checkState += Rotasi("RightHandPinky1",0,0,-40,5);
		checkState += Rotasi("RightHandThumb1",30,0,-15,5);
		
		checkState += Rotasi("RightHandMiddle2",0,0,-60,5);
		checkState += Rotasi("RightHandRing2",0,0,-60,5);
		checkState += Rotasi("RightHandPinky2",0,0,-60,5);
		checkState += Rotasi("RightHandThumb2",60,0,0,5);
		
		checkState += Rotasi("RightHandMiddle3",0,0,-60,5);
		checkState += Rotasi("RightHandRing3",0,0,-60,5);
		checkState += Rotasi("RightHandPinky3",0,0,-60,5);
		checkState += Rotasi("RightHandThumb3",30,0,-15,5);
		
		if(checkState == 31)
		{
			stateTahap1 = 1;
		}
	}
	void doTahap1TahiyyatAkhir(){
		int checkState = 0;
		float batasTurun = 3f;
		GameObject Hips = GameObject.Find("Hips");
		
		if(Hips.transform.localPosition.y > batasTurun){
			Hips.transform.Translate(0,(-0.01f)/3,0);
		}else{
			checkState++;
		}
		
		checkState += Rotasi("Hips",1,0,0,3);
		checkState += Rotasi("Neck",-1,0,0);
		checkState += Rotasi("Spine",10,0,0,2);
		checkState += Rotasi("Spine1",1,0,0);
		checkState += Rotasi("Spine2",1,0,0);
		
		if(Hips.transform.localEulerAngles.x < 25){
			checkState += Rotasi("LeftShoulder",-30,15,0);
			checkState += Rotasi("RightShoulder",-30,-15,0);
		}
		checkState += Rotasi("LeftArm",5,0,85,5);
		checkState += Rotasi("RightArm",5,0,-85,5);
		checkState += Rotasi("LeftForeArm",-20,10,5,4);
		checkState += Rotasi("RightForeArm",-20,-5,-5,4);
		checkState += Rotasi("LeftHand",85,30,0);
		checkState += Rotasi("RightHand",-85,-30,45);
		checkState += Rotasi("LeftLeg",20,180,200,4);
		checkState += Rotasi("RightLeg",1,180,195,2);
		checkState += Rotasi("LeftFoot",-5,80,45,8);
		checkState += Rotasi("RightFoot",-20,1,1);		
		checkState += Rotasi ("LeftToeBase",-1,0,0);
		checkState += Rotasi("LeftUpLeg",-65,-10,0,3);
		checkState += Rotasi("RightUpLeg",-63,10,0,3);
		// Gerakan Jari Pada Tangan Kanan
		
		checkState += Rotasi("RightHandMiddle1",0,0,-40,5);
		checkState += Rotasi("RightHandRing1",0,0,-40,5);
		checkState += Rotasi("RightHandPinky1",0,0,-40,5);
		checkState += Rotasi("RightHandThumb1",30,0,-15,5);
		
		checkState += Rotasi("RightHandMiddle2",0,0,-60,5);
		checkState += Rotasi("RightHandRing2",0,0,-60,5);
		checkState += Rotasi("RightHandPinky2",0,0,-60,5);
		checkState += Rotasi("RightHandThumb2",60,0,0,5);
		
		checkState += Rotasi("RightHandMiddle3",0,0,-60,5);
		checkState += Rotasi("RightHandRing3",0,0,-60,5);
		checkState += Rotasi("RightHandPinky3",0,0,-60,5);
		checkState += Rotasi("RightHandThumb3",30,0,-15,5);
		
		if(checkState == 32)
		{
			stateTahap1 = 1;
		}
	}
	void doTahap1Salam(){
		
		int checkState = 0;
		checkState += Rotasi ("Neck",0,80,0);
	
		if(checkState == 1){
			stepSalam = 0;
			stateTahap1 =1;
		}
	}
	void doTahap1SujudToTakbiratulihram(){
		int checkState= 0;
		GameObject Hips = GameObject.Find ("Hips");
		// bagian tubuh digerakan lebih cepat
		checkState += Rotasi("Spine",10,0,0,2);
		checkState += Rotasi("Hips",1,0,0);
		checkState += Rotasi("Neck",-1,0,0);
		checkState += Rotasi("Spine1",1,0,0);
		checkState += Rotasi("Spine2",1,0,0);
		
		if(Hips.transform.localEulerAngles.x < 25){
			checkState += Rotasi("LeftShoulder",-30,15,0);
			checkState += Rotasi("RightShoulder",-30,-15,0);
		}
		checkState += Rotasi("LeftArm",5,0,85);
		checkState += Rotasi("RightArm",5,0,-85);
		checkState += Rotasi("LeftForeArm",-20,15,5,2);
		checkState += Rotasi("RightForeArm",-20,-15,-5,2);
		checkState += Rotasi("LeftUpLeg",-75,0,-5);
		checkState += Rotasi("RightUpLeg",-75,0,5);
		checkState += Rotasi("LeftUpLeg",-35,0,-5);
		checkState += Rotasi("RightUpLeg",-35,0,5);
		checkState += Rotasi("LeftFoot",-30,0,0);
		checkState += Rotasi("RightFoot",-30,0,0);
		checkState += Rotasi ("RightHand",-85,-30,45);
		//UnityEngine.Debug.Log("T1 - "+checkState);
		if(checkState == 15)
		{
			stepTakbirToSujud = 0;
			stateTahap1 = 1;
		}
	}
	void doTahap1TahiyyatAwalToTakbiratullihram(){
		int checkState = 0,temp = 0;
		GameObject RightHandThumb1 = GameObject.Find("RightHandThumb1");
		GameObject Hips = GameObject.Find("Hips");
		float batasNaik = 4.6f;
		GameObject LeftLeg = GameObject.Find("LeftLeg");
		GameObject RightLeg = GameObject.Find("RightLeg");

		if(RightHandThumb1.transform.eulerAngles.z > 0 && stepTahiyatToTakbir == 0){
			checkState += Rotasi("RightHandThumb1",0,20,-1);
			
		}else{
			checkState = 0;
			stepTahiyatToTakbir = 1;
		}
		if(stepTahiyatToTakbir == 1){
			checkState += Rotasi("RightHandThumb1",0,20,20);
		}
		
		checkState += Rotasi("RightFoot",-35,0,0);
		checkState += Rotasi("LeftFoot",-35,0,0,5);
		checkState += Rotasi("LeftUpLeg",-45,0,-1);
		checkState += Rotasi("RightUpLeg",-45,0,1);
		checkState += Rotasi("RightLeg",30,180,180,2);
		checkState += Rotasi("LeftLeg",30,180,180);
		checkState += Rotasi("Hips",5,0,0,2);
		checkState += Rotasi ("RightToeBase",-80,0,0,5);

		// meluruskan jari tangan kanan
		checkState += Rotasi("RightHandThumb3",0,0,-1);
		checkState += Rotasi("RightHandMiddle1",0,0,-1,3);
		checkState += Rotasi("RightHandRing1",0,0,-1,3);
		checkState += Rotasi("RightHandPinky1",0,0,-1,3);
		checkState += Rotasi("RightHandMiddle2",0,0,-1,3);
		checkState += Rotasi("RightHandRing2",0,0,-1,3);
		checkState += Rotasi("RightHandPinky2",0,0,-1,3);
		checkState += Rotasi("RightHandThumb2",0,0,0,3);
		checkState += Rotasi("RightHandMiddle3",0,0,-1,3);
		checkState += Rotasi("RightHandRing3",0,0,-1,3);
		checkState += Rotasi("RightHandPinky3",0,0,-1,3);
		checkState += Rotasi("RightShoulder",-20,-15,-5);
		checkState += Rotasi("LeftShoulder",-20,15,5);
		checkState += Rotasi("RightArm",5,0,-85);
		checkState += Rotasi("LeftArm",5,0,85);
		checkState += Rotasi("RightHand",85,-30,0);
		// bagian pinggang, punggung, dada dan leher
		
		// Kaki
		if(Hips.transform.localPosition.y < batasNaik){
			Hips.transform.Translate(0,(0.01f)/3f,0);
		}else{
			checkState++;
		}
		if(checkState == 26){
			stepTahiyatToTakbir = 0;
			stateTahap1 = 1;
		}
	}
	
	void doTahap1DudukToSujud(){
		int checkState = 0;float batasNaik = 5f;
		GameObject Hips = GameObject.Find("Hips");
		if(Hips.transform.localPosition.y < batasNaik){
			Hips.transform.Translate(0,(0.01f)/6,0);
		}else{
			checkState++;
		}
		
		
		checkState += Rotasi("Neck",-45,0,0);
		checkState += Rotasi("Spine",80,0,0);
		checkState += Rotasi("Spine1",15,0,0);
		checkState += Rotasi("Spine2",15,0,0);
		checkState += Rotasi("Hips",45,0,0);
			
		checkState += Rotasi("LeftShoulder",-60,1,1);
		checkState += Rotasi("RightShoulder",-60,-1,-1);
		checkState += Rotasi("LeftArm",0,0,70);
		checkState += Rotasi("RightArm",0,0,-70);
		
		checkState += Rotasi("LeftForeArm",-1,120,5,3);
		checkState += Rotasi("RightForeArm",-1,-120,-5,3);
		checkState += Rotasi("LeftHand",85,60,0,3);
		checkState += Rotasi("RightHand",85,-60,-1,3);
		
		checkState += Rotasi("LeftUpLeg",-75,0,-5);
		checkState += Rotasi("RightUpLeg",-75,0,5);
		
		checkState += Rotasi("LeftLeg",30,180,180,2);
		checkState += Rotasi("RightLeg",30,180,180,2);
		checkState += Rotasi ("LeftFoot",-20,0,0,2);
		
		
		if(checkState == 19){
			stateTahap1 = 1;
		}
	}
	void doTahap1(string NamaGerakan){
		switch (NamaGerakan) {
			case "Takbiratulihram" 			: doTahap1Takbiratulihram();
										  	  break;
			case "Ruku"			   			: doTahap1Ruku();
										  	  break;
			case "Itidal" 		   			: doTahap1Itidal();
										  	  break;
			case "Sujud"		   			: doTahap1Sujud();
										 	  break;
			case "DudukDiantara2Sujud"		: doTahap1DudukDiantara2Sujud();
											  break;
			case "TahiyatAwal"   			: doTahap1TahiyyatAwal();
											  break;
			case "TahiyatAkhir"  			: doTahap1TahiyyatAkhir();
											  break;
			case "Salam"		   			: doTahap1Salam();
											  break;
			case "SujudToTakbir"			: doTahap1SujudToTakbiratulihram();	
											  break;
			case "TahiyatToTakbir"			: //stepTahiyatToTakbir = 0;
											  doTahap1TahiyyatAwalToTakbiratullihram();	
											  break;
			case "DudukToSujud" 			: doTahap1DudukToSujud();
											  break;
		}
		if(stateTahap1 == 1){
			stateTahap1 = 0;
			stateGerak = 3;
		}
	}
	
	void doTahap2Takbiratulihram(){
		int checkState = 0;
		// melakukan transformasi 2 kali lebih cepat
		checkState += Rotasi("LeftArm",-25,0,75,2);
		checkState += Rotasi("LeftForeArm",-10,85,75,2);
		checkState += Rotasi("LeftHand",10,0,45,2);
		checkState += Rotasi("RightHand",5,359,-42,2);
		checkState += Rotasi("RightArm",-25,0,-75);
		checkState += Rotasi("RightForeArm",-12,-85,-65);
		
		if(checkState == 6){
			stateTahap2 = 1;
		}
	}
	void doTahap2Ruku(){
		int checkState=0;
		
		checkState += Rotasi("LeftForeArm",-1,1,1,2);
		checkState += Rotasi("RightForeArm",-1,-1,-1,2);
		checkState += Rotasi("LeftHand",85,20,0,2);
		checkState += Rotasi("RightHand",85,-20,-1,2);
		checkState += Rotasi("LeftHandThumb1",0,340,340,2);
		checkState += Rotasi("RightHandThumb1",0,20,20,2);
		checkState += Rotasi("Spine",86,0,0);
		checkState += Rotasi("Spine1",14,0,0);
		checkState += Rotasi("LeftShoulder",-60,0,0);
		checkState += Rotasi("RightShoulder",-60,0,0);
		checkState += Rotasi("LeftArm",-1,0,98);
		checkState += Rotasi("RightArm",-1,0,-98);
		if(checkState == 12){	
			stateTahap2 = 1;
		}
	}
	void doTahap2Itidal(){
		int checkState = 0;
		
		checkState += Rotasi("LeftShoulder",-1,0,10);
		checkState += Rotasi("RightShoulder",-1,0,-10);
		checkState += Rotasi("LeftArm",0,0,75);
		checkState += Rotasi("RightArm",0,0,-75);
		checkState += Rotasi("LeftForeArm",-1,1,10,2);
		checkState += Rotasi("RightForeArm",-1,-1,-10,2);
		checkState += Rotasi("LeftHand",1,1,0);
		checkState += Rotasi("RightHand",1,-1,0);
	
		if(checkState == 8){
			stateTahap2 =1;
		}
	}
	void doTahap2Sujud(){
		int checkState=0;
		
		checkState += Rotasi("Neck",-45,0,0);
		checkState += Rotasi("Spine",80,0,0);
		checkState += Rotasi("Spine1",15,0,0);
		checkState += Rotasi("Spine2",15,0,0);
		checkState += Rotasi("Hips",45,0,0);
			
		checkState += Rotasi("LeftShoulder",-60,1,1);
		checkState += Rotasi("RightShoulder",-60,-1,-1);
		checkState += Rotasi("LeftArm",0,0,70);
		checkState += Rotasi("RightArm",0,0,-70);
		
		checkState += Rotasi("LeftForeArm",-1,120,5,3);
		checkState += Rotasi("RightForeArm",-1,-120,-5,3);
		checkState += Rotasi("LeftHand",85,60,0,3);
		checkState += Rotasi("RightHand",85,-60,-1,3);		

		checkState += Rotasi("LeftUpLeg",-75,0,-5);
		checkState += Rotasi("RightUpLeg",-75,0,5);
				
		if(checkState == 15)
		{
			stateTahap2 = 1;
		}
		
		
	}
	void doTahap2DudukDiantara2Sujud(){
		stateTahap2 =1;
	}
	void doTahap2TahiyyatAwal(){
		stateTahap2 =1;
	}
	void doTahap2TahiyyatAkhir(){
		stateTahap2 = 1;
	}
	void doTahap2Salam(){
		int checkState = 0;
		if(stepSalam == 0){
			checkState += Rotasi ("Neck",0,0,0);
			if(checkState == 1){
				stepSalam = 1;
				checkState = 0;
			}
		}else{
			checkState += Rotasi ("Neck",0,-80,0);
		}
		if(checkState == 1){
			stateTahap2 = 1;
		}
		
	}
	void doTahap2SujudToTakbiratulihram(){
		int checkState = 0,i = 0 ;
		GameObject LeftArm = GameObject.Find("LeftArm");
		GameObject RightArm = GameObject.Find("RightArm");
		GameObject Hips = GameObject.Find("Hips");
		GameObject LeftLeg = GameObject.Find("LeftLeg");
		GameObject RightLeg = GameObject.Find("RightLeg");
		float batasNaik = 9.4f;
		// nilai rotasi X pada left Arm dan Right Arm
		// dibuat menjadi 0 terlebih dahulu. 
		// hal ini dilakukan karena bila memasukan input bernilai minus, rotasi pada sumbu x 
		// akan dilakukan dengan arah yang terbalik, nilai minus yang diinputkan akan berubah menjadi 360 + (nilai minus)
		// sehingga nilai rotasi yang sebelumnya dimaksudkan melakakukan decrement malah terjadi sebaliknya.
		
		if(LeftArm.transform.localEulerAngles.x > 0 || RightArm.transform.localEulerAngles.x > 0){
			checkState += Rotasi("LeftArm",0,0,75);
			checkState += Rotasi("RightArm",0,0,-75);
		}
		if(checkState == 2)
		{
			checkState = 0;
			stepTakbirToSujud = 1;
		}
		
		if(stepTakbirToSujud == 1){
			// melakukan transformasi 2 kali lebih cepat
			do{	
				checkState = 0;
				checkState += Rotasi("LeftArm",-25,0,75);
				checkState += Rotasi("LeftForeArm",-10,85,75);
				checkState += Rotasi("LeftHand",10,0,45);
				checkState += Rotasi("RightHand",5,358,-42);
			
				checkState += Rotasi("LeftUpLeg",-1,0,-1);
				checkState += Rotasi("RightUpLeg",-1,0,1);
				i++;
			}while(i< 2 && checkState < 6);
			checkState += Rotasi("RightArm",-25,0,-75);
			checkState += Rotasi("RightForeArm",-12,-85,-65);
			checkState += Rotasi("LeftShoulder",-1,1,1);
			checkState += Rotasi("RightShoulder",-1,-1,-1);
				
			if(Hips.transform.localPosition.y < batasNaik){
				Hips.transform.Translate(0,(0.01f)/2,0);
			}else{
				checkState++;
			}
			
			if(LeftLeg.transform.localEulerAngles.x > 0){
				LeftLeg.transform.Rotate(-10 * Time.maximumDeltaTime ,0,0);
				RightLeg.transform.Rotate(-10 * Time.maximumDeltaTime,0,0);
			}else{
				checkState++;
			}
				checkState += Rotasi("Spine",0,0,0);
				checkState += Rotasi("LeftFoot",-1,0,0);
				checkState += Rotasi("RightFoot",-1,0,0);
				checkState += Rotasi("LeftToeBase",-1,0,0);
				checkState += Rotasi ("RightToeBase",-1,0,0);
			// asal 17 jadi 16
			if(checkState == 17){
				stepTakbirToSujud = 2;
			}
			
		}
		UnityEngine.Debug.Log("c - "+checkState);
		if(stepTakbirToSujud == 2){
			stateTahap2 = 1;
		}
		
	}
	void doTahap2TahiyyatAwalToTakbiratullihram(){
		int checkState = 0;
		float batasMundur = -0.6f;
		float batasNaik = 9.5f;
		GameObject Hips = GameObject.Find("Hips");
		GameObject LeftLeg = GameObject.Find("LeftLeg");
		GameObject RightLeg = GameObject.Find("RightLeg");
		
		
		if(stepTahiyatToTakbir == 0){
			if(Hips.transform.localPosition.z > batasMundur){
				Hips.transform.Translate(0,0,-0.01f/3.15f);
			}else{
				checkState++;
			}
			checkState += Rotasi("LeftUpLeg",-80,0,-1,4);
			checkState += Rotasi("RightUpLeg",-80,0,0,4);
			checkState += Rotasi("LeftForeArm",-20,35,5,2);
			checkState += Rotasi("RightForeArm",-20,-35	,-5,2);
			checkState += Rotasi("Hips",0,0,0,2);
			
			if(checkState == 6){
				checkState = 0;
				stepTahiyatToTakbir = 1;
			}
		}
		if(stepTahiyatToTakbir == 1){
			if(Hips.transform.localPosition.y < batasNaik){
				Hips.transform.Translate(0,0.062139551f/2f * Time.maximumDeltaTime ,0);
			}else{
				checkState++;
			}
			checkState += Rotasi ("Spine",0,0,0);
			checkState += Rotasi ("RightShoulder",-1,-1,-1);
			checkState += Rotasi ("LeftShoulder",-1,0,0);
			checkState += Rotasi("LeftArm",0,0,80,2);
			checkState += Rotasi("RightArm",0,0,-80,2);
			checkState += Rotasi("LeftForeArm",-15,115,5,2);
			checkState += Rotasi("RightForeArm",-15,-115,-5,2);
			checkState += Rotasi("LeftHand",85,60,0);
			checkState += Rotasi("RightHand",85,-60,0);		
			
			
			checkState += Rotasi("LeftUpLeg",-1,0,-1,3);
			checkState += Rotasi("RightUpLeg",-1,0,0,3);
			checkState += Rotasi ("RightToeBase",-1,0,0,3);
			checkState += Rotasi("LeftToeBase",-1,0,0,3);
			checkState += Rotasi ("LeftFoot",-1,0,0);
			checkState += Rotasi ("RightFoot",-1,0,0);
			
			
			for(int i = 0; i<3; i++){
				if(LeftLeg.transform.localEulerAngles.x < 358){
					LeftLeg.transform.Rotate(-1.948051948f,0,0);
					RightLeg.transform.Rotate(-1.948051948f,0,0);
				}else{
					checkState++;
				}
			}
			
			if(checkState == 17 ){
				stepTahiyatToTakbir = 3;
				checkState = 0;
			}
			
		}
		
		if(stepTahiyatToTakbir == 3){
			checkState += Rotasi("LeftArm",-25,0,75);
			checkState += Rotasi("RightArm",-25,0,-75);
			checkState += Rotasi("LeftForeArm",-10,85,75,3);
			checkState += Rotasi("RightForeArm",-12,-85,-65,2);
			checkState += Rotasi("LeftHand",10,0,45,2);
			checkState += Rotasi("RightHand",5,359,-42,2);
			
		}
		
//		if(checkState == 5)
//		stateTahap2 = 1;
	}
	void doTahap2DudukToSujud(){
		stateTahap2 = 1;
	}
	
	void doTahap2(string NamaGerakan){
		switch (NamaGerakan) {
			case "Takbiratulihram" 			: doTahap2Takbiratulihram();
										  	  break;
			case "Ruku"			   			: doTahap2Ruku();
										  	  break;
			case "Itidal" 		   			: doTahap2Itidal();
										  	  break;
			case "Sujud"		   			: doTahap2Sujud();
										 	  break;
			case "DudukDiantara2Sujud"		: doTahap2DudukDiantara2Sujud();
											  break;
			case "TahiyatAwal"   			: doTahap2TahiyyatAwal();
											  break;
			case "TahiyatAkhir"  			: doTahap2TahiyyatAkhir();
											  break;
			case "Salam"		   			: doTahap2Salam();
											  break;		
			case "SujudToTakbir"			: doTahap2SujudToTakbiratulihram();	
											  break;
			case "TahiyatToTakbir"			: doTahap2TahiyyatAwalToTakbiratullihram();	
											  break;
			case "DudukToSujud"				: doTahap2DudukToSujud();
											  break;
		}
		if(stateTahap2 == 1){
			stateTahap2 = 0;
			stateGerak = 4;
		}
	}
	
	public void doGerakanShalat(string NamaGerakan){
		
		switch(stateGerak){
		case 1:	doKondisiAwal(NamaGerakan);
				playAudio(setAudioAwalGerakan(NamaGerakan));
				break;
		case 2: doTahap1(NamaGerakan);
				break;
		case 3: doTahap2 (NamaGerakan);
				if(!objectPlayBacaan.audio.isPlaying){
					objectPlayBacaan.audio.clip = null;
				}
				break;
		case 4 : playBacaan(NamaGerakan);
			    break;
		}
			
	}
	
	public void playAudio(int index){
		objectPlayBacaan.audio.clip = Bacaan[index];
		objectPlayBacaan.audio.Play();
	}
	void playBacaan(string NamaGerakan){
		if(NamaGerakan != "Takbiratulihram" && NamaGerakan != "Itidal" && NamaGerakan != "SujudToTakbir" && NamaGerakan != "TahiyatToTakbir" && NamaGerakan !="Salam"){
			if(objectPlayBacaan.audio.clip == null)
			{
				playAudio(setAudioAkhirGerakan(NamaGerakan));
			}
			if(!objectPlayBacaan.audio.isPlaying)
			{
				stateGerak = 0;
				objectPlayBacaan.audio.clip = null;
			}
		}else{
			stateGerak = 0;
			objectPlayBacaan.audio.clip = null;
		}
		
	}
	
	public int setAudioAwalGerakan(string NamaGerakan){
		int index = 0;
		if(NamaGerakan == "Itidal"){
			index = 4;
		}else if( NamaGerakan == "Salam"){
			index = 9;
		}else if(NamaGerakan == "Takbiratulihram"){
			index = 10;
		}
		return index;
	}
	public int setAudioAkhirGerakan(string NamaGerakan){
		int indeks = 0;
		switch (NamaGerakan) {
			case "Ruku"			   			: indeks = 3;
										  	  break;
			case "Sujud"		   			: indeks = 5;
										 	  break;
			case "DudukDiantara2Sujud"		: indeks = 6;
											  break;
			case "TahiyatAwal"   			: indeks = 7;
											  break;
			case "TahiyatAkhir"  			: indeks = 8;
											  break;
			case "DudukToSujud"				: indeks = 5;
											  break;
		}
		return indeks;
	}
	public void setAnimationSpeed(string NamaGerakan){
		
		if(NamaGerakan == "Itidal" || NamaGerakan =="SujudToTakbir" || NamaGerakan =="TahiyatToTakbir" ){
			speed = 1.0f;
		}else if(NamaGerakan =="Ruku" || NamaGerakan == "Takbiratulihram"){
			speed = 0.75f;
		}else{
			speed = 0.4f;
		}
//		if(NamaGerakan == "Sujud" || NamaGerakan == "DudukDiantara2Sujud" || NamaGerakan == "TahiyatAwal" || NamaGerakan =="TahiyatAkhir")
//		{
//			speed = 0.5f;
//		}else{
//			speed = 1.0f;
//		}
	}
}