using System;
using UnityEngine;
using TrafficReport;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using TrafficReport.Util;

namespace TrafficReport
{	
	
	[Serializable]
	public class VehicleDisplay {
		public string id;
		public string display;
		public bool onOff;
        public Color32 color;
	}

    public delegate void ConfigChangeDelegate();

	[Serializable]
	public class Config {

        public event ConfigChangeDelegate eventConfigChanged;

        public static int CONFIG_VERSION = 1;

        public int configVersion = -1;

        public static Config instance = Config.Load();
        public Vector3 newButtonPos = new Vector3(-1.670f, 0.985f);

		public Vector2 buttonPosition  = new Vector2(80, 5);
		public KeyCode keyCode = KeyCode.Slash;

		public VehicleDisplay[] vehicleTypes = {
			new VehicleDisplay { id =  "citizen", display = "Pedestrian", onOff=true, color = new Color32(1,255,216,255) },
			
			new VehicleDisplay { id =  "Residential/ResidentialLow", display = "Car", onOff=true , color = new Color32(35,100,90,255) },
			
			new VehicleDisplay { id =  "Industrial/IndustrialGeneric", display = "Cargo truck", onOff=true , color = new Color32(154,92,59,255)  },
			new VehicleDisplay { id =  "Industrial/IndustrialOil", display = "Oil Tanker", onOff=true , color = new Color32(46,39,35,255) },
			new VehicleDisplay { id =  "Industrial/IndustrialOre", display = "Ore Truck", onOff=true  , color = new Color32(150,144,141,255)},
			new VehicleDisplay { id =  "Industrial/IndustrialForestry", display = "Log Truck", onOff=true , color = new Color32(140,39,16,255) },
			new VehicleDisplay { id =  "Industrial/IndustrialFarming", display = "Tractor", onOff=true , color = new Color32(148,86,52,255) },
			
			new VehicleDisplay { id =  "HealthCare/None", display = "Ambulance", onOff=true  , color = new Color32(30,255,0,255) },
            new VehicleDisplay { id =  "DeathCare/None", display = "Herse", onOff=true  , color = new Color32(30,180,0,255) },
			new VehicleDisplay { id =  "Garbage/None", display = "Garbage Truck", onOff=true , color = new Color32(255,240,0,255)  },
			new VehicleDisplay { id =  "PoliceDepartment/None", display = "Police Car", onOff=true , color = new Color32(24,19,249,255)   },
			new VehicleDisplay { id =  "FireDepartment/None", display = "Fire truck", onOff=true , color = new Color32(255,0,0,255)  },
			
			
			new VehicleDisplay { id =  "PublicTransport/PublicTransportBus", display = "Bus", onOff=true , color = new Color32(170,57,249,255)  },
			new VehicleDisplay { id =  "PublicTransport/Train", display = "Train / Subway", onOff=true , color = new Color32(113,24,176,255)  },
			
		};


		[XmlIgnore]
		public Rect buttonRect {
			get {
				return new Rect(buttonPosition.x,buttonPosition.y,80,80);
			}
		}

        public void NotifyChange()
        {
            Save();
            if(eventConfigChanged!=null)
                eventConfigChanged();
        }
		
		public static Config Load() {
			try {
				XmlSerializer xml = new XmlSerializer (typeof(Config));
				FileStream fs = new FileStream("TrafficReport.xml", FileMode.Open, FileAccess.Read);
				Config config =  xml.Deserialize(fs) as Config;
                fs.Close();
                if (config.configVersion != CONFIG_VERSION)
                {
                    Config c = new Config();
                    c.configVersion = CONFIG_VERSION;
                    return c;
                }
				
				return config;
			} catch {
				return new Config();
			} 
		}
		
		public void Save() {
			try 
			{
				XmlSerializer xml = new XmlSerializer (GetType());
				FileStream fs = new FileStream ("TrafficReport.xml", FileMode.Create, FileAccess.Write);
				xml.Serialize (fs,this);
				fs.Close();
			} catch(Exception e) {
				Log.error("Error saving config" + e.ToString());

			}
		}

        internal bool IsTypeVisible(string p)
        {
            foreach(VehicleDisplay v in vehicleTypes) {
                if(v.id == p) {
                    return v.onOff;
                }
            }
            return true;
        }
    }
}

