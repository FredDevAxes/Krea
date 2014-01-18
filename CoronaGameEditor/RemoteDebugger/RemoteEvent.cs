using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Krea.RemoteDebugger
{
    [ObfuscationAttribute(Exclude = true)]
    public class RemoteEvent
    {
        public string name;

        //------- ACCELROMETER FIELDS -----------
        public double xGravity;
        public double yGravity;
        public double zGravity;
        public double xInstant;
        public double yInstant;
        public double zInstant;
        public bool isShake;

        //------- COMPASS FIELDS -----------
        public double geographic;
        public double magnetic;

        //------- GYROSCOPE FIELDS -----------
        public double xRotation;
        public double yRotation;
        public double zRotation;

        //------- KEY FIELDS -----------
        public string keyName;
        public string phase;
       
        //------- GPS ----------------
        public double altitude;
        public double longitude;
        public double latitude;
        public long time;
        public double accuracy;
        public double directionAngle;
        public double speed;


        public RemoteEvent() { }

        public RemoteEvent(string name, double xGravity, double yGravity, double zGravity,
                        double xInstant, double yInstant, double zInstant, bool isShake)
        {
            this.name = name;

            this.xGravity = xGravity;
            this.yGravity = yGravity;
            this.zGravity = zGravity;

            this.xInstant = xInstant;
            this.yInstant = yInstant;
            this.zInstant = zInstant;

            this.isShake = isShake;
        }

        public RemoteEvent(string name, double altitude, double longitude, double latitude, double accuracy, double directionAngle, double speed, long time)
        {
            this.name = name;

            this.altitude = altitude;
            this.longitude = longitude;
            this.latitude = latitude;
            this.accuracy = accuracy;
            this.directionAngle = directionAngle;
            this.speed = speed;
            this.time = time;

        }
        public RemoteEvent(string name, double geographic, double magnetic)
        {
            this.name = name;

            this.geographic = geographic;
            this.magnetic = magnetic;
        }

        public RemoteEvent(string name, double xRotation, double yRotation, double zRotation)
        {
            this.name = name;

            this.xRotation = xRotation;
            this.yRotation = yRotation;
            this.zRotation = zRotation;

        }

        public RemoteEvent(string name,string keyName, string phase)
        {
            this.name = name;

            this.keyName = keyName;
            this.phase = phase;
           

        }

        public string serialize()
        {
            try
            {
                var jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 999999999;

                string jsonEvent = jss.Serialize(this);

                return jsonEvent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
