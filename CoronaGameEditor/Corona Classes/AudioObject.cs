using System;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class AudioObject
    {
        public String path {get; set;}
        public String name {get; set;}
        public Double volume {get; set;}
        public Boolean isPreloaded {get; set;}
        public int loops {get; set;}
        public String type { get; set; }
        public bool isEnabled = true;
        public AudioObject() { }

        public AudioObject(String _path, String _name, Double _volume, Boolean _isPreloaded, int _loops, String _type)
        {
            this.path = _path;
            this.name = _name;
            this.volume = _volume;
            this.isPreloaded = _isPreloaded;
            this.loops = _loops;
            this.type = _type;
        }

        public void Init(String _path, String _name, Double _volume, Boolean _isPreloaded, int _loops, String _type)
        {
            this.path = _path;
            this.name = _name;
            this.volume = _volume;
            this.isPreloaded = _isPreloaded;
            this.loops = _loops;
            this.type = _type;
        }

        public override string ToString()
        {
            return this.name.Split('.')[0] ;
        }

        public AudioObject cloneInstance()
        {
            return  new AudioObject(this.path, this.name, this.volume, this.isPreloaded, this.loops, this.type);

        }
    }
}
