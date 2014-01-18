using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krea.Corona_Classes;
using Krea.CoronaClasses;
using Krea.GameEditor.FontManager;

namespace Krea.Asset_Manager
{
    [Serializable()]
    public class AssetsToSerialize
    {

        public List<DisplayObject> ListObjects;
        public List<CoronaSpriteSet> SpriteSets;
        public List<CoronaSpriteSheet> SpriteSheets;
        public List<AudioObject> Audios;
        public List<Snippet> Snippets;
        public List<FontItem> Fonts;
        public String ProjectName;
        public bool HasBeenModified = false;

        public AssetsToSerialize()
        {
            this.ListObjects = new List<DisplayObject>();
            this.SpriteSets = new List<CoronaSpriteSet>();
            this.SpriteSheets = new List<CoronaSpriteSheet>();
            this.Snippets = new List<Snippet>();
            this.Audios = new List<AudioObject>();

            this.Fonts = new List<FontItem>();
            this.HasBeenModified = false;
        }

        public void clean()
        {
            for (int i = 0; i < this.ListObjects.Count; i++)
            {
                DisplayObject obj = this.ListObjects[i];
                if (obj.Image != null)
                {
                    obj.Image.Dispose();
                    obj.Image = null;
                }
            }

            for (int i = 0; i < this.SpriteSets.Count; i++)
            {
                CoronaSpriteSet set = SpriteSets[i];
                for (int j = 0; j < set.Frames.Count; j++)
                {
                    SpriteFrame frame = set.Frames[j];
                    if (frame.Image != null)
                    {
                        frame.Image.Dispose();
                        frame.Image = null;
                    }
                }
            }

            for (int i = 0; i < this.SpriteSheets.Count; i++)
            {
                CoronaSpriteSheet sheet = this.SpriteSheets[i];
                if (sheet.ImageSpriteSheet != null)
                {
                    sheet.ImageSpriteSheet.Dispose();
                    sheet.ImageSpriteSheet = null;
                }
            }

           
        }
      
    }
}
