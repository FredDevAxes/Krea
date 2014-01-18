using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using Krea.CoronaClasses;
using Krea.GameEditor.CollisionManager;
using System.Reflection;

namespace Krea.GameEditor.TilesMapping
{
     [ObfuscationAttribute(Exclude = true)]
    public class JSONTileMap
    {
        public int NbColumns;
        public int NbLines;
        public int TilesWidth;
        public int TilesHeight;
        public string TilesMapName;
        public bool IsInfinite = true;
        public bool IsPathFindingEnabled;
        //------ TIlES PHYSICS ---------
        public decimal Bounce = 0;
        public decimal Density = 0;
        public decimal Friction = 0;
        public int Radius = -1;
        public int[] TextureCountBySheet;
        public int[] ObjectCountBySheet;
        public bool isPhysicsEnabled = true;
        public int TextureCount;
        public int ObjectCount;
        public int TextureSheetCount;
        public int ObjectSheetCount;
        public int TextureSequenceCount;
        public int ObjectSequenceCount;
        public int TextureSequenceSheetCount;
        public int ObjectSequenceSheetCount;
        public int[] TextureSequenceCountBySheet;
        public int[] ObjectSequenceCountBySheet;
        public JSONTileSequence[] TextureSequences;
        public JSONTileSequence[] ObjectSequences;
        public int CollisionCategoryBits;
        public int CollisionMaskBits;

        public JSONTileMap() { }

        public JSONTileMap(TilesMap map,Scene sceneParent, float xRatio, float yRatio)
        {
            this.NbColumns = map.NbColumns;
            this.NbLines = map.NbLines;
            this.TilesHeight =  Convert.ToInt32(map.TilesHeight * yRatio);
            this.TilesWidth =  Convert.ToInt32(map.TilesWidth * xRatio);
            this.TilesMapName = map.TilesMapName;
            this.Bounce = map.Bounce;
            this.IsInfinite = map.isInfinite;
            this.Friction = map.Friction;
            this.Radius = Convert.ToInt32(map.Radius *((xRatio + yRatio) / 2));
            this.Density = map.Density;
            this.IsPathFindingEnabled = map.IsPathFindingEnabled;
            this.isPhysicsEnabled = map.isPhysicsEnabled;


            this.TextureCount = map.TileModelsTextureUsed.Count;
            this.ObjectCount = map.TileModelsObjectsUsed.Count;

            if (map.TextureCountBySheet != null)
            this.TextureCountBySheet = map.TextureCountBySheet.ToArray();

            if (map.ObjectCountBySheet != null)
            this.ObjectCountBySheet = map.ObjectCountBySheet.ToArray();


            this.TextureSheetCount = map.NbTextureSheets;
            this.ObjectSheetCount = map.NbObjectSheets;

            this.TextureSequenceCount = map.TextureSequences.Count;
            this.ObjectSequenceCount = map.ObjectSequences.Count;

            if(map.TextureSequenceCountBySheet != null)
                 this.TextureSequenceCountBySheet = map.TextureSequenceCountBySheet.ToArray();

            if(map.ObjectSequenceCountBySheet != null)
                this.ObjectSequenceCountBySheet = map.ObjectSequenceCountBySheet.ToArray();

            this.TextureSequenceSheetCount = map.NbTextureSequenceSheets;
            this.ObjectSequenceSheetCount = map.NbObjectSequenceSheets;

            this.TextureSequences = new JSONTileSequence[map.TextureSequences.Count];
            int textureFrameCount = TextureCount;
            for (int i = 0; i < map.TextureSequences.Count; i++)
            {
                TileSequence seq = map.TextureSequences[i];
                JSONTileSequence seqJson = new JSONTileSequence(seq.Name.Replace(" ",""), textureFrameCount + 1, seq.Frames.Count, seq.Lenght, seq.Iteration);
                this.TextureSequences[i] = seqJson;

                textureFrameCount = textureFrameCount + seq.Frames.Count;
            }

            this.ObjectSequences = new JSONTileSequence[map.ObjectSequences.Count];
            int objectFrameCount = this.ObjectCount;
            for (int i = 0; i < map.ObjectSequences.Count; i++)
            {
                TileSequence seq = map.ObjectSequences[i];
                JSONTileSequence seqJson = new JSONTileSequence(seq.Name.Replace(" ", ""), objectFrameCount + 1, seq.Frames.Count, seq.Lenght, seq.Iteration);
                this.ObjectSequences[i] = seqJson;

                objectFrameCount = objectFrameCount + seq.Frames.Count;
            }

            if (map.CollisionFilterGroupIndex + 1 <= sceneParent.CollisionFilterGroups.Count)
            {
                CollisionFilterGroup group = sceneParent.CollisionFilterGroups[map.CollisionFilterGroupIndex];
                this.CollisionCategoryBits = group.CategorieBit;
                this.CollisionMaskBits = group.getMaskBits();
            }
            else
            {
                this.CollisionCategoryBits = 0;
                this.CollisionMaskBits = 0;
            }
        }

        public void deserialize(string folderDest, TilesMap map)
        {
            var jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 999999999;
            
            //get textures content
            string textureContentFilePath = folderDest + "\\" + map.TilesMapName + "textures.json";
            if(File.Exists(textureContentFilePath))
                map.textureContent = (int[])jss.Deserialize(File.ReadAllText(textureContentFilePath), typeof(int[]));
            
            //get objects content
            string objectContentFilePath = folderDest + "\\" + map.TilesMapName + "objects.json";
            if (File.Exists(objectContentFilePath))
             map.objectContent = (int[])jss.Deserialize(File.ReadAllText(objectContentFilePath), typeof(int[]));

            //get collisions content
            string objectCollisionFilePath = folderDest + "\\" + map.TilesMapName + "collisions.json";
            if (File.Exists(objectCollisionFilePath))
                map.collisionContent = (int[])jss.Deserialize(File.ReadAllText(objectCollisionFilePath), typeof(int[]));

            //get texture sequences content
            string textureSequenceContentFilePath = folderDest + "\\" + map.TilesMapName + "texturesequences.json";
            if (File.Exists(textureSequenceContentFilePath))
                map.textureSequenceContent = (int[])jss.Deserialize(File.ReadAllText(textureSequenceContentFilePath), typeof(int[]));

            //get object sequences content          
            string objectSequenceContentFilePath = folderDest + "\\" + map.TilesMapName + "objectsequences.json";
            if (File.Exists(objectSequenceContentFilePath))
                map.objectSequenceContent = (int[])jss.Deserialize(File.ReadAllText(objectSequenceContentFilePath), typeof(int[]));


            //get events content          
            string eventsContentFilePath = folderDest + "\\" + map.TilesMapName + "events.json";
            if (File.Exists(eventsContentFilePath))
                 map.eventContent = (int[])jss.Deserialize(File.ReadAllText(eventsContentFilePath), typeof(int[]));
            jss = null;
           
        }
        public void serialize(string folderDest, TilesMap map)
        {
                       
            var jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 999999999;

            var jsonConfig = jss.Serialize(this);
            string configFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "config.json";
            File.WriteAllText(configFilePath, jsonConfig);

            //Serialize Texture Content Tab
            map.textureContent = new int[this.NbLines * this.NbColumns];
            map.objectContent = new int[this.NbLines * this.NbColumns];

            for (int i = 0; i < this.NbLines; i++)
            {
                for (int j = 0; j < this.NbColumns; j++)
                {
                    Tile tile = map.TabTiles[i, j];
                    if (tile != null)
                    {
                        if (tile.TileModelTexture != null)
                        {
                            map.textureContent[i * this.NbColumns + j] = map.TileModelsTextureUsed.IndexOf(tile.TileModelTexture);
                        }
                        else
                            map.textureContent[i * this.NbColumns + j] = -1;

                        if (tile.TileModelImageObject != null)
                        {
                            map.objectContent[i * this.NbColumns + j] = map.TileModelsObjectsUsed.IndexOf(tile.TileModelImageObject);
                        }
                        else
                            map.objectContent[i * this.NbColumns + j] = -1;

                    }

                }
            }


            var jsonTextureContent = jss.Serialize(map.textureContent);
            string textureContentFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "textures.json";

            File.WriteAllText(textureContentFilePath, jsonTextureContent);

            //Serialize Object Content Tab
           
            var jsonObjectContent = jss.Serialize(map.objectContent);
            string objectContentFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "objects.json";

            File.WriteAllText(objectContentFilePath, jsonObjectContent);

            //Serialize Collision Content Tab
            var jsonCollisionContent = jss.Serialize(map.collisionContent);
            string objectCollisionFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "collisions.json";

            File.WriteAllText(objectCollisionFilePath, jsonCollisionContent);

            //Serialize Texture Sequence Content Tab
            var jsonTextureSequenceContent = jss.Serialize(map.textureSequenceContent);
            string textureSequencesFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "texturesequences.json";

            File.WriteAllText(textureSequencesFilePath, jsonTextureSequenceContent);

            //Serialize Object Sequence Content Tab
            var jsonObjectSequenceContent = jss.Serialize(map.objectSequenceContent);
            string objectSequencesFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "objectsequences.json";

            File.WriteAllText(objectSequencesFilePath, jsonObjectSequenceContent);

            //Serialize events Content Tab
            var jsonEventContent = jss.Serialize(map.eventContent);
            string eventsFilePath = folderDest + "\\" + map.TilesMapName.ToLower() + "events.json";

            File.WriteAllText(eventsFilePath, jsonEventContent);

            jss = null;
            jsonCollisionContent = null;
            jsonConfig = null;
            jsonObjectContent = null;
            jsonTextureContent = null;
            jsonObjectSequenceContent = null;
            jsonTextureSequenceContent = null;
            jsonEventContent = null;
        }
    }
}
