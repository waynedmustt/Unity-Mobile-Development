using UnityEngine;
using System.Collections;

public class ModelShalat
{
	public ModelGerakan human;
	DataShalat DS = new DataShalat();
	GameObject playSurat = GameObject.Find("AudioBacaan");
	public AudioClip[] Surat;
	int indexGerakan = 0;
	int indexSurat = 0;
	int indexShalat = 0;
	public int stateModel = 0;
	public string[] namaSurat = new string[2];
	
	public ModelShalat(){
		playSurat.audio.loop = false;
	}
	
	void getDataShalat(string JenisShalat){
		DS.WS = DataShalat.Load();
		human.stateGerak = 1;
		indexShalat = getIndeksShalat(JenisShalat);
		stateModel = 2;
		//UnityEngine.Debug.Log("Lewat");
	}
	
	public void doShalat(){
		switch (stateModel) {
			case 1 : getDataShalat("Shubuh");break;
			case 2 : doGerakanShalat(); break;
		}
	}
	
	
	void doGerakanShalat(){
		
		UnityEngine.Debug.Log(DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan]);
		switch (DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan]) {
			case 1 	: human.doGerakanShalat("Takbiratulihram"); break;
			case 2  : playAudioShalat(0);break; 
			case 3 	: playAudioShalat(1);break; // Membaca Al- Fatihah 
			case 4 	: playAudioShalat(getIndeksSurat(namaSurat[indexSurat]));break; // Membaca Surat Lain
			case 5 	: human.doGerakanShalat("Ruku"); break;
			case 6 	: human.doGerakanShalat("Itidal"); break;
			case 7 	: human.doGerakanShalat("Sujud"); break;
			case 8 	: human.doGerakanShalat("DudukDiantara2Sujud");break;
			case 9 	: human.doGerakanShalat("DudukToSujud"); break;
			case 10 : human.doGerakanShalat("SujudToTakbir"); break;
			case 11 : human.doGerakanShalat("TahiyatAwal"); break;
			case 12 : human.doGerakanShalat("TahiyatToTakbir"); break;
			case 13 : human.doGerakanShalat("TahiyatAkhir"); break;
			case 14 : human.doGerakanShalat("Salam"); break;
		}
		
		 if(DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan] != 2 && DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan]!= 3 && DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan]!= 4 && DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan]!= 0){
			if(human.stateGerak == 0){
				human.stateGerak = 1;
				if(DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan+1]!= 0){
					indexGerakan++;
				}else{
					indexGerakan = 0;
					stateModel = 0;
				}
			}
		}else if(DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan] == 2 || DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan] == 3){
			if(!playSurat.audio.isPlaying){
				indexGerakan++;
				playSurat.audio.clip = null;
			}
		}else if(DS.WS.JenisShalat[indexShalat].Tahapan[indexGerakan] == 4 ){
			if(!playSurat.audio.isPlaying){
				indexSurat++;
				indexGerakan++;
				playSurat.audio.clip = null;
			}
		}
		
		
	}
	
	
	void playAudioShalat(int indeksSurat){
		
		if(playSurat.audio.clip == null){
			playSurat.audio.clip = Surat[indeksSurat];
			playSurat.audio.Play();
		}
	}
	
	int getIndeksSurat(string NamaSurat){
		int indeks;
		switch (NamaSurat) {
		case "Al-Ikhlas" 	: indeks = 2;break;
		case "Al-Kautsar" 	: indeks = 3; break;
		default :	indeks = 0; break; // Surat Al- Fatihah
		}
		return indeks;
	}
	
	int getIndeksShalat(string JenisShalat){
		int indeks;
		switch (JenisShalat) {
		case "Dzuhur" 	: indeks = 1; break;
		case "Ashar" 	: indeks = 2; break;
		case "Magrib"  	: indeks = 3; break;
		case "Isya" 	: indeks = 4; break;
		default			: indeks = 0; break; // Shalat Shubuh
		}
		return indeks;
	}
	
	public void doReset(){
		stateModel = 0;
		human.doDefaultPosition();
	}
}

