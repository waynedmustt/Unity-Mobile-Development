using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;



public class DataShalat
{
	[XmlRoot("WaktuShalat")]
	public class WaktuShalat{
		public class Shalat{
			[XmlAttribute("nama")]
			public string nama;
			public int[] Tahapan;
		}
		
		public Shalat[] JenisShalat;
	}
	public WaktuShalat WS = new WaktuShalat();
	
	public static WaktuShalat Load()
	{
 		WaktuShalat a = new WaktuShalat();
		XmlSerializer serializer = new XmlSerializer(typeof(WaktuShalat));
 		TextAsset file = (TextAsset) Resources.Load("Data/DataShalat");
		StringReader teks = new StringReader(file.ToString());
		a = serializer.Deserialize(teks) as WaktuShalat;
		return a;
 	}
}
