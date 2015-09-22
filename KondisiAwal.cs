using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;


public class KondisiAwal{
	
	[XmlRoot("DataGerakan")]
	public class DataGerakan
	{
		
		public struct DataTransform{
			// nilai Posisi
			public float x;
			public float y;
			public float z;
		
			// nilai Rotasi
			public float RotX;
			public float RotY;
			public float RotZ;
			
		}
		
		public struct BagianBadan{
			[XmlAttribute("nama")]
			public string nama;
			public DataTransform PotRot;
		}
		
	
		public List<BagianBadan> KondisiAwal = new List<BagianBadan>();
	}
	
	public DataGerakan dg = new DataGerakan();
	
	public static DataGerakan Load(string path)
 	{
		DataGerakan a = new DataGerakan();
		TextAsset file = (TextAsset) Resources.Load("Data/"+path);
		StringReader teks = new StringReader(file.ToString());
		XmlSerializer serializer = new XmlSerializer(typeof(DataGerakan));
		a = serializer.Deserialize(teks) as DataGerakan;
		return a;
		
 	}
}


