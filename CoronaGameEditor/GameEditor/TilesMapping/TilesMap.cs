using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Krea.Corona_Classes;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Krea.GameEditor.CollisionManager;
using Krea.CoronaClasses;

namespace Krea.GameEditor.TilesMapping
{
    [Serializable()]
   public class TilesMap
    {

        //---------------------------------------------------
        //-------------------Attributes----------------------
        //---------------------------------------------------
        public Size TilesMapSize;
        public int NbColumns;
        public int NbLines;
        public int TilesWidth;
        public int TilesHeight;
        public string TilesMapName;
        public bool isInfinite = true;
        public Point Location;

        //------ TIlES PHYSICS ---------
        public decimal Bounce = 0;
        public decimal Density = 0;
        public decimal Friction = 0;
        public int Radius = -1;

        //--------------------------------
        private int colMin;
        private int lineMin;
        private int colMax;
        private int lineMax;

        private int lastColMin;
        private int lastLineMin;
        private int lastColMax;
        private int lastLineMax;
        public bool isPhysicsEnabled = true;
        public CoronaLayer LayerParent;
        public bool isEnabled = true;
        public List<TileModel> TileModelsTextureUsed;
        public List<TileModel> TileModelsObjectsUsed;
        public List<TileSequence> TextureSequences;
        public List<TileSequence> ObjectSequences;

        public List<TileEvent> TileEvents;
       

        public bool IsPathFindingEnabled = false;
        
        public int CollisionFilterGroupIndex =0;

        [NonSerialized]
        public List<int> TextureCountBySheet;
        [NonSerialized]
        public List<int> ObjectCountBySheet;
        [NonSerialized]
        public int NbTextureSheets;
        [NonSerialized]
        public int NbObjectSheets;
        [NonSerialized]
        public Tile[,] TabTiles;
        [NonSerialized]
        public int[] textureContent;
        [NonSerialized]
        public int[] objectContent;
        [NonSerialized]
        public int[] textureSequenceContent;
        [NonSerialized]
        public int[] objectSequenceContent;

        [NonSerialized]
        public int[] eventContent;

        [NonSerialized]
        public List<int> TextureSequenceCountBySheet;
        [NonSerialized]
        public List<int> ObjectSequenceCountBySheet;
        [NonSerialized]
        public int NbTextureSequenceSheets;
        [NonSerialized]
        public int NbObjectSequenceSheets;
        [NonSerialized]
        public int[] collisionContent;

        //GORGON
        [NonSerialized]
        public GorgonLibrary.Graphics.Sprite TouchEventSprite;
        [NonSerialized]
        public GorgonLibrary.Graphics.Sprite CollisionEventSprite;
        [NonSerialized]
        public GorgonLibrary.Graphics.Sprite PostCollisionEventSprite;
        [NonSerialized]
        public GorgonLibrary.Graphics.Sprite PreCollisionEventSprite;

        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public TilesMap(string name,Point location,int nbLines, int nbColumns, int tilesWidth, int tilesHeight,CoronaLayer layerParent)
        {
            this.TilesMapName = name;
            this.Location = location;
            this.NbLines = nbLines;
            this.NbColumns = nbColumns;
            this.TilesWidth = tilesWidth;
            this.TilesHeight = tilesHeight;

            this.colMin = 0;
            this.lineMin = 0;
            this.colMax = nbColumns;
            this.lineMax = nbLines;

            this.TileModelsTextureUsed = new List<TileModel>();
            this.TileModelsObjectsUsed = new List<TileModel>();
            TextureSequences = new List<TileSequence>();
            ObjectSequences = new List<TileSequence>();
            TileEvents = new List<TileEvent>();
            this.LayerParent = layerParent;
            createTilesTab();

            this.UpdateTileMapGraphicsContent();
        }

        //---------------------------------------------------
        //-------------------Methods-------------------------
        //---------------------------------------------------
        public void createTilesTab()
        {
            this.TilesMapSize = new Size(this.NbColumns * this.TilesWidth, this.NbLines * this.TilesHeight);

            //Creer le tableau a 2D des tiles
            //Si un tableau existe deja, recuperer tous les tiles deja present
            if (this.TabTiles != null)
            {
                Tile[,] tabTilesTemp = new Tile[this.NbLines, this.NbColumns];
                
                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = null;
                        if (i < this.TabTiles.GetLength(0) && j < this.TabTiles.GetLength(1))
                        {
                            tile = this.TabTiles[i, j];
                        }
                        
                        if(tile == null)
                        {
                            //Creer une Tile
                            tile = new Tile(i, j, this.TilesWidth, this.TilesHeight, true, this);
                        }
                        

                        //L'inserer dans les tableau des tiles
                        tabTilesTemp[i, j] = tile;

                    }
                }

                this.TabTiles = tabTilesTemp;
            }
            else
            {
                this.TabTiles = new Tile[this.NbLines, this.NbColumns];

                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        //Creer une Tile
                        Tile tile = new Tile(i, j, this.TilesWidth, this.TilesHeight, true, this);

                        //L'inserer dans les tableau des tiles
                        this.TabTiles[i, j] = tile;

                    }
                }
            }
            
        }

        public void setTilesSize(Size size)
        {
            if(this.TabTiles != null)
            {
                this.TilesMapSize = new Size(this.NbColumns * this.TilesWidth, this.NbLines * this.TilesHeight);
                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = this.TabTiles[i, j];
                        tile.Width = size.Width;
                        tile.Height = size.Height;
                        //Definirla position de la tile
                        tile.Location = new Point(tile.Width * tile.ColumnIndex, tile.Height * tile.LineIndex);
                    }
                }
            }
             
        }

        public void applyTextureOnTilePointed(Point p,TileModel model)
        {

            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
               
                if (model.IsTexture == true)
                    tileTouched.setTexture(model);
                else
                    tileTouched.setObjectImage(model);
            }
        }

        public void applyTextureSequenceOnTilePointed(Point p, TileSequence seq)
        {

            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
                tileTouched.setTextureSequence(seq);
            }
        }

        public void applyObjectSequenceOnTilePointed(Point p, TileSequence seq)
        {

            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
                tileTouched.setObjectSequence(seq);
            }
        }

        public void applyEventOnTilePointed(Point p, TileEvent ev)
        {

            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
                tileTouched.setEvent(ev);
            }
        }

        public void removeTextureOnTilePointed(Point p)
        {
            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
                tileTouched.setTexture(null);
                tileTouched.setTextureSequence(null);
            }
               
        }

        public void removeObjectOnTilePointed(Point p)
        {
            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
                tileTouched.setObjectImage(null);
                tileTouched.setObjectSequence(null);
            }
                
        }

        public void removeEventOnTilePointed(Point p)
        {
            Tile tileTouched = this.getTileAtLocation(p);
            if (tileTouched != null)
            {
                tileTouched.setEvent(null);
            }

        }
        public void applyTextureOnAllTiles(TileModel model)
        {
            for (int i = 0; i < this.NbLines; i++)
            {
                for (int j = 0; j < this.NbColumns; j++)
                {
                    if (model.IsTexture == true)
                        this.TabTiles[i, j].setTexture(model);
                    else
                        this.TabTiles[i, j].setObjectImage(model);
                    
                }
            }
        }



        public void applyTextureSequenceOnAllTiles(TileSequence seq)
        {
            for (int i = 0; i < this.NbLines; i++)
            {
                for (int j = 0; j < this.NbColumns; j++)
                {
                    this.TabTiles[i, j].setTextureSequence(seq);

                }
            }
        }

        public void applyObjectSequenceOnAllTiles(TileSequence seq)
        {
            for (int i = 0; i < this.NbLines; i++)
            {
                for (int j = 0; j < this.NbColumns; j++)
                {
                    this.TabTiles[i, j].setObjectSequence(seq);

                }
            }
        }

        public void applyEventOnAllTiles(TileEvent ev)
        {
            for (int i = 0; i < this.NbLines; i++)
            {
                for (int j = 0; j < this.NbColumns; j++)
                {
                    this.TabTiles[i, j].setEvent(ev);

                }
            }
        }

        public void setSurfaceVisible(Rectangle displayRectangle,float xScale, float yScale)
        {
            if (xScale < 1)
            {
                //Definir les zones de recherches
                lastColMin = colMin;
                colMin = Convert.ToInt32((displayRectangle.X / this.TilesWidth) * xScale) - 2;
                lastLineMin = lineMin;
                lineMin = Convert.ToInt32((displayRectangle.Y / this.TilesHeight) * yScale) - 2;
                if (lineMin < 0) lineMin = 0;
                if (colMin < 0) colMin = 0;
                int cointInfDroitX = displayRectangle.X + displayRectangle.Width + this.TilesWidth;
                int cointInfDroitY = displayRectangle.Y + displayRectangle.Height + this.TilesHeight;

                lastColMax = colMax;
                colMax = Convert.ToInt32(cointInfDroitX / (this.TilesWidth * xScale)) + 2;
                lastLineMax = lineMax;
                lineMax = Convert.ToInt32(cointInfDroitY / (this.TilesHeight * yScale)) + 2;

                if (this.isInfinite == false)
                {
                    if (colMax > this.NbColumns) colMax = this.NbColumns;
                    if (lineMax > this.NbLines) lineMax = this.NbLines;
                }
            }
            else
            {
                //Definir les zones de recherches
                lastColMin = colMin;
                colMin = Convert.ToInt32(displayRectangle.X / (this.TilesWidth * xScale) - 2);
                lastLineMin = lineMin;
                lineMin = Convert.ToInt32(displayRectangle.Y / (this.TilesHeight * yScale) - 2);
                if (lineMin < 0) lineMin = 0;
                if (colMin < 0) colMin = 0;
                int cointInfDroitX = Convert.ToInt32((displayRectangle.X + displayRectangle.Width + this.TilesWidth) * xScale);
                int cointInfDroitY = Convert.ToInt32((displayRectangle.Y + displayRectangle.Height + this.TilesHeight) * yScale);

                lastColMax = colMax;
                colMax = Convert.ToInt32(cointInfDroitX / (this.TilesWidth * xScale)) + 2;
                lastLineMax = lineMax;
                lineMax = Convert.ToInt32(cointInfDroitY / (this.TilesHeight * yScale)) + 2;

                if (this.isInfinite == false)
                {
                    if (colMax > this.NbColumns) colMax = this.NbColumns;
                    if (lineMax > this.NbLines) lineMax = this.NbLines;
                }
            }
           
            


        }

        public void resetSurfaceVisible()
        {
            this.colMin = lastColMin;
            this.colMax = lastColMax;
            this.lineMin = lastLineMin;
            this.lineMax = lastLineMax;
        }

        public Tile getTileAtLocation(Point p)
        {
            int columnIndex = p.X / this.TilesWidth;
            int lineIndex = p.Y / this.TilesHeight;

            if (columnIndex >= this.NbColumns ) columnIndex = this.NbColumns -1;
            if (lineIndex >= this.NbLines) lineIndex = this.NbLines -1;

            if (columnIndex <0) columnIndex = 0;
            if (lineIndex < 0) lineIndex = 0;

            return this.TabTiles[lineIndex, columnIndex];
        }

        public List<Tile> getTilesInsideRectangle(Rectangle rect)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = lineMin; i < this.lineMax; i++)
            {
                for (int j = colMin; j < this.colMax; j++)
                {
                    Tile currentTile = this.TabTiles[i, j];
                    if(currentTile.isTouched(rect) == true)
                    {
                        tiles.Add(currentTile);
                    }
                }
            }

            return tiles;
        }


        public void DrawGorgon(float xScale, float yScale, Point offsetPoint, string layerToShow, bool showCollision)
        {

            try
            {
                if (this.TabTiles != null)
                {
                    for (int i = lineMin; i < this.lineMax; i++)
                    {
                        int iFinal = i;
                        int loopY = Convert.ToInt32(Math.Floor((double)i / this.NbLines));
                        int nbTilesYInLoop = this.NbLines * loopY;
                        if (i >= this.NbLines)
                        {
                            iFinal = i - nbTilesYInLoop;
                        }

                        for (int j = colMin; j < this.colMax; j++)
                        {

                            int jFinal = j;
                            if (this.isInfinite == true)
                            {
                                int loopX = Convert.ToInt32(Math.Floor((double)j / this.NbColumns));
                                int nbTilesXInLoop = this.NbColumns * loopX;
                                if (j >= this.NbColumns)
                                {

                                    jFinal = j - nbTilesXInLoop;

                                }

                                Point finalOffSet = new Point(offsetPoint.X + (loopX * (this.NbColumns * this.TilesWidth)),
                                          offsetPoint.Y + (loopY * (this.NbLines * this.TilesHeight)));

                                
                                this.TabTiles[iFinal, jFinal].DrawGorgon(finalOffSet, layerToShow, showCollision, xScale);
                            }
                            else
                            {
                                this.TabTiles[iFinal, jFinal].DrawGorgon(offsetPoint, layerToShow, showCollision, xScale);
                               
                            }




                        }
                    }
                }
            }


            catch (Exception ex)
            {

                MessageBox.Show("Error during tiles drawing!\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
           
            }
        }


        public bool DrawTilesInEditor(Graphics g,float xScale, float yScale,Point offsetPoint,string layerToShow, bool showCollision)
        {
            if (this.TabTiles != null)
            {
                Matrix m = new Matrix();
                m.Scale(xScale, yScale);
                g.Transform = m;


                for (int i = lineMin; i < this.lineMax; i++)
                {
                     int iFinal = i;
                     int loopY = Convert.ToInt32(Math.Floor((double)i / this.NbLines));
                     int nbTilesYInLoop = this.NbLines * loopY;
                    if(i >= this.NbLines)
                    {
                        iFinal = i - nbTilesYInLoop;
                    }

                    for (int j = colMin; j < this.colMax; j++)
                    {

                        try
                        {
                            int jFinal = j;
                            if (this.isInfinite == true)
                            {
                                int loopX = Convert.ToInt32(Math.Floor((double)j / this.NbColumns));
                                int nbTilesXInLoop = this.NbColumns * loopX;
                                if (j >= this.NbColumns)
                                {

                                    jFinal = j - nbTilesXInLoop;
                                  
                                }

                                Point finalOffSet = new Point(offsetPoint.X + (loopX * (this.NbColumns * this.TilesWidth)),
                                          offsetPoint.Y + (loopY * (this.NbLines * this.TilesHeight)));

                                this.TabTiles[iFinal, jFinal].Draw(g, finalOffSet,layerToShow,showCollision);
                            }
                            else
                            {
                                this.TabTiles[i, j].Draw(g, offsetPoint,layerToShow,showCollision);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            
                            MessageBox.Show("Error during tiles drawing!\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                            return false;
                        }
                           

                    }
                }

                return true;
            }
            return false;
            
        }

        public void DrawAllTiles(Graphics g, float xScale, float yScale,string showLayer)
        {
            Matrix m = new Matrix();
            m.Scale(xScale, yScale);
            g.Transform = m;

            for (int i = 0; i < this.NbLines; i++)
            {
                for (int j = 0; j < this.NbColumns; j++)
                {
                    this.TabTiles[i, j].Draw(g, new Point(0, 0), showLayer,false);
                }
            }
        }

        public void DisposeUnusedTileModels(List<TileModel> currentList, List<TileModel> newList)
        {
            for (int i = 0; i < currentList.Count; i++)
            {
                TileModel model = currentList[i];
                if (!newList.Contains(model))
                {
                    if (model.Image != null)
                        model.Image.Dispose();

                    if (model.GorgonSprite != null)
                        model.GorgonSprite.Image.Dispose();


                    model.Image = null;
                    model.GorgonSprite = null;
                }
            }
        }


        //--------------------- TextureSet Creation ------------------------------
        public void createTextureSet(string folderDest, string sheetName,string suffix, float xRatio, float yRatio)
        {
           
            this.TextureCountBySheet = new List<int>();
            if (TileModelsTextureUsed == null || TileModelsTextureUsed.Count == 0) return;

            int tileHeight = Convert.ToInt32(this.TilesHeight * yRatio);
            int tileWidth = Convert.ToInt32(this.TilesWidth * xRatio);

          
            double nbLignesParSheet = Math.Ceiling((double)768 / (double)tileHeight);
            double nbColonnesParSheet = Math.Ceiling((double)1024 / (double)tileWidth);

            this.NbTextureSheets =(int) Math.Ceiling((double)this.TileModelsTextureUsed.Count / (nbLignesParSheet * nbColonnesParSheet));

            int textureCount = this.TileModelsTextureUsed.Count;
            for (int k = 0; k < this.NbTextureSheets; k++)
            {
                //Creer la TextureSet finale
                Bitmap textureSheet = null;

                //Creer la dataSheet 
                TilesMapDataSheetGenerator dataSheetGen = new TilesMapDataSheetGenerator(sheetName + k);

                if (k == this.NbTextureSheets - 1)
                {
                    //Calculer la taile de la texture finale
                    int nbColonnes = (int)Math.Ceiling((double)1024 / (double)tileWidth);
                    int nbLignes = (int)Math.Ceiling(Convert.ToDouble(textureCount) / (double)nbColonnes);

                    if (TileModelsTextureUsed.Count < nbColonnes)
                        nbColonnes = TileModelsTextureUsed.Count;

                    textureSheet = new Bitmap(nbColonnes * tileWidth, nbLignes * tileHeight);

                    Graphics g = Graphics.FromImage(textureSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                            TileModel modelUsed = this.TileModelsTextureUsed[TileModelsTextureUsed.Count - textureCount];
                            if(modelUsed.GorgonSprite != null)
                            {
                                Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                Bitmap finalFrame = new Bitmap(imageFrame, new Size(tileWidth, tileHeight));
                                g.DrawImage(finalFrame, positionFrame);
                                finalFrame.Dispose();
                                imageFrame.Dispose();

                                dataSheetGen.addFrame((i+1)*j, positionFrame, new Size(tileWidth, tileHeight));
                                textureCount--;

                                imageFrame.Dispose();
                                imageFrame = null;
                            }
                           
                        }

                        if (textureCount == 0) break;

                    }

                    this.TextureCountBySheet.Add(dataSheetGen.textureCount);
                    g.Dispose();
                    textureSheet.Save(folderDest + "\\" + sheetName +k+ suffix+".png");
                    textureSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix,k);

                }
                else
                {
                    int nbColonnes = (int)Math.Ceiling(nbColonnesParSheet);
                    int nbLignes = (int)Math.Ceiling(nbLignesParSheet);
                    textureSheet = new Bitmap(nbColonnes*tileWidth, nbLignes*tileHeight);

                    
                    Graphics g = Graphics.FromImage(textureSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                            TileModel modelUsed = this.TileModelsTextureUsed[TileModelsTextureUsed.Count - textureCount];
                            if (modelUsed.GorgonSprite != null)
                            {
                                Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                Bitmap finalFrame = new Bitmap(imageFrame, new Size(tileWidth, tileHeight));
                                g.DrawImage(finalFrame, positionFrame);
                                finalFrame.Dispose();
                                imageFrame.Dispose();
                                dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                textureCount--;
                            }
                        }

                        if (textureCount == 0) break;

                    }

                    this.TextureCountBySheet.Add(dataSheetGen.textureCount);
                    g.Dispose();
                    textureSheet.Save(folderDest + "\\" + sheetName + k + suffix + ".png");
                    textureSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);
                }


            }

        }

        public void ExportTileMapModelsUsedToProjectResources(List<TileModel> modelList,bool disposeModels)
        {
            if (modelList != null)
            {

                for (int i = 0; i < modelList.Count; i++)
                {
                    TileModel model = modelList[i];
                    if (model != null)
                    {

                        this.ExportTileModelToProjectResources(model);
                        if (disposeModels == true)
                        {
                            this.CleanTileModel(model,true,false, false);
                        }

                    }
                }
            }
        }

        public void UpdateGorgonTileModel(TileModel model, bool shouldExport)
        {
            if (model != null)
            {
                if (shouldExport == true)
                {
                    this.ExportTileModelToProjectResources(model);
                }

                this.CleanTileModel(model, true, false, true);

                string projectPath = this.LayerParent.SceneParent.projectParent.ProjectPath;
                string filename = Path.Combine(projectPath + "\\Resources\\TileMaps\\" + this.TilesMapName
                                                   , model.Name + ".png");

                if (File.Exists(filename))
                {
                    GorgonLibrary.Graphics.Sprite sprite = new GorgonLibrary.Graphics.Sprite(model.Name,
                        GorgonLibrary.Graphics.Image.FromFile(filename));

                    model.GorgonSprite = sprite;

                    sprite.Color = Color.White;

                }
            }
        }

        public bool IsGorgonImageUsedInEntireTileMap(GorgonLibrary.Graphics.Image gorgonImage)
        {

            bool isUsed = this.IsGorgonImageUsedInTileModels(this.TileModelsTextureUsed,gorgonImage);
            if (isUsed == true)
                return true;

            isUsed = this.IsGorgonImageUsedInTileModels(this.TileModelsObjectsUsed, gorgonImage);
            if (isUsed == true)
                return true;

            List<TileModel> newTextureSequenceModelsUsed = this.getTextureSequenceModelsUsed();
            isUsed = this.IsGorgonImageUsedInTileModels(newTextureSequenceModelsUsed, gorgonImage);
            newTextureSequenceModelsUsed.Clear();
            newTextureSequenceModelsUsed = null;
            if (isUsed == true)
                return true;
            

            List<TileModel> newObjectSequenceModelsUsed = this.getObjectSequenceModelsUsed();
            isUsed = this.IsGorgonImageUsedInTileModels(newObjectSequenceModelsUsed, gorgonImage);
            newObjectSequenceModelsUsed.Clear();
            newObjectSequenceModelsUsed = null;
            if (isUsed == true)
                return true;
            

            return false;
        }

        public bool IsGorgonImageUsedInTileModels(List<TileModel> modelList, GorgonLibrary.Graphics.Image gorgonImage)
        {
            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    TileModel model = modelList[i];
                    if (model.GorgonSprite != null)
                    {
                        if (model.GorgonSprite.Image == gorgonImage)
                            return true;
                    }
                }
            }

            return false;
        }

        public bool IsTileModelExists(List<TileModel> modelList, string name)
        {
            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    TileModel model = modelList[i];
                    
                    if (model.Name.Equals(name))
                        return true;
                  
                }
            }

            return false;
        }

      

        public List<Tile> GetTileUsingTileModel(TileModel model)
        {
            if(this.TabTiles != null)
            {
                List<Tile> tilesUsingModels = new List<Tile>();
                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = this.TabTiles[i, j];
                        if (tile.TileModelImageObject != null && tile.TileModelImageObject == model)
                        {
                            tilesUsingModels.Add(tile);
                            continue;
                        }

                        if (tile.TileModelTexture != null && tile.TileModelTexture == model)
                        {
                            tilesUsingModels.Add(tile);
                            continue;
                        }

                        if (tile.TileTextureSequence != null && tile.TileTextureSequence.Frames.Contains(model))
                        {
                           
                            tilesUsingModels.Add(tile);
                            continue;
                        }

                        if (tile.TileObjectSequence != null && tile.TileObjectSequence.Frames.Contains(model))
                        {
                            tilesUsingModels.Add(tile);
                            continue;
                        }
                    }
                }

                if (tilesUsingModels.Count > 0)
                    return tilesUsingModels;
                else return null;
            }

            return null;
        }


        public void CleanAllTileModelsUsed(bool removeGorgon, bool removeAsset, bool checkIntegrity)
        {
            this.CleanTileModels(this.TileModelsTextureUsed, removeGorgon, removeAsset, checkIntegrity);
            this.CleanTileModels(this.TileModelsObjectsUsed, removeGorgon, removeAsset, checkIntegrity);

            for (int i = 0; i < this.TextureSequences.Count; i++)
            {
                TileSequence seq = this.TextureSequences[i];
                this.CleanTileModels(seq.Frames, removeGorgon, removeAsset, checkIntegrity);
            }

            for (int i = 0; i < this.ObjectSequences.Count; i++)
            {
                TileSequence seq = this.ObjectSequences[i];
                this.CleanTileModels(seq.Frames, removeGorgon, removeAsset, checkIntegrity);
            }

            if (removeAsset == true)
            {
                string assetDirectory = this.LayerParent.SceneParent.projectParent.ProjectPath+"\\Resources\\TileMaps\\"+this.TilesMapName;
                if(Directory.Exists(assetDirectory))
                    Directory.Delete(assetDirectory,true);
            }
        }


        public void CleanTileModels(List<TileModel> models, bool removeGorgon, bool removeAsset, bool checkIntegrity)
        {
            if (models != null)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    TileModel model = models[i];
                    this.CleanTileModel(model, removeGorgon, removeAsset, checkIntegrity);
                }
            }
        }

        public void CleanTileModel(TileModel model,bool removeGorgon,bool removeAsset, bool checkIntegrity)
        {
            if (model != null)
            {
                if (checkIntegrity == true)
                {
                    if (model.GorgonSprite != null)
                    {
                        if (model.GorgonSprite.Image != null)
                        {
                            bool isUsed = this.IsGorgonImageUsedInEntireTileMap(model.GorgonSprite.Image);
                            if (isUsed == false)
                            {


                                model.GorgonSprite.Image.Dispose();

                            }

                            model.GorgonSprite.Image = null;
                        }

                        model.GorgonSprite = null;
                    }
                }
                else
                {

                    if (model.GorgonSprite != null)
                    {
                        if (model.GorgonSprite.Image != null)
                        {
                            model.GorgonSprite.Image.Dispose();

                            model.GorgonSprite.Image = null;
                            
                        }
                        model.GorgonSprite = null;
                    }
                }


                if (removeAsset == true)
                {
                    CoronaGameProject project = this.LayerParent.SceneParent.projectParent;
                    string assetPath = Path.Combine(project.ProjectPath + "\\Resources\\TileMaps\\" + this.TilesMapName
                                            , model.Name + ".png");
                    if (File.Exists(assetPath))
                    {
                        File.Delete(assetPath);
                    }
                }

                model = null;
            }
        }

        public void ExportTileModelToProjectResources(TileModel tileModel)
        {
           

            CoronaGameProject project = this.LayerParent.SceneParent.projectParent;
            if (project != null)
            {
                if (!Directory.Exists(project.ProjectPath + "\\Resources\\TileMaps\\"+this.TilesMapName))
                    Directory.CreateDirectory(project.ProjectPath + "\\Resources\\TileMaps\\" + this.TilesMapName);

                if (tileModel.Image != null)
                {
                    
                    string filename = Path.Combine(project.ProjectPath + "\\Resources\\TileMaps\\" + this.TilesMapName
                                                    , tileModel.Name + ".png");

                    if (!File.Exists(filename))
                         tileModel.Image.Save(filename);

                    tileModel.Image.Dispose();
                    tileModel.Image = null;
                }
                else if (tileModel.GorgonSprite != null)
                {
                   

                    if (tileModel.GorgonSprite.Image != null)
                    {
                        string filename = Path.Combine(project.ProjectPath + "\\Resources\\TileMaps\\" + this.TilesMapName
                                                        , tileModel.Name + ".png");

                        Image temp = tileModel.GorgonSprite.Image.SaveBitmap();
                        if (!File.Exists(filename))
                            temp.Save(filename);

                        temp.Dispose();
                        temp = null;
                    }
                }

            }
        }

        private List<TileModel> getTextureListUsed()
        {
            if (this.TabTiles != null)
            {
                List<TileModel> listTextures = new List<TileModel>();

                this.textureContent = new int[this.NbLines * this.NbColumns];

                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = this.TabTiles[i,j];
                        if (tile != null)
                        {
                            if (tile.TileModelTexture != null)
                            {
                                if (!listTextures.Contains(tile.TileModelTexture))
                                    listTextures.Add(tile.TileModelTexture);

                                this.textureContent[i * this.NbColumns + j] = listTextures.IndexOf(tile.TileModelTexture);
                            }
                            else
                                this.textureContent[i * this.NbColumns + j] = -1;
                        }
                       
                    }
                }

                return listTextures;
            }

            return null;
        }

        public int getNbTexturesUsed()
        {
            List<TileModel> liste = getTextureListUsed();
            if (liste != null)
                return liste.Count;
            else return -1;
        }

        //--------------------- ObjectsSet Creation ------------------------------

        public void createObjectsSet(string folderDest, string sheetName,string suffix, float xRatio, float yRatio)
        {

            this.ObjectCountBySheet = new List<int>();
            
            if (this.TileModelsObjectsUsed == null || this.TileModelsObjectsUsed.Count <= 0) return;

            int tileHeight = Convert.ToInt32(this.TilesHeight * yRatio);
            int tileWidth = Convert.ToInt32(this.TilesWidth * xRatio);

            double nbLignesParSheet = Math.Ceiling((double)768 / (double)tileHeight);
            double nbColonnesParSheet = Math.Ceiling((double)1024 / (double)tileWidth);

            this.NbObjectSheets = (int)Math.Ceiling((double)this.TileModelsObjectsUsed.Count / (nbLignesParSheet * nbColonnesParSheet));

            int textureCount = this.TileModelsObjectsUsed.Count;
            for (int k = 0; k < this.NbObjectSheets; k++)
            {
                //Creer la TextureSet finale
                Bitmap objectSheet = null;

                //Creer la dataSheet 
                TilesMapDataSheetGenerator dataSheetGen = new TilesMapDataSheetGenerator(sheetName + k);

                if (k == this.NbObjectSheets-1)
                {
                    //Calculer la taile de la texture finale
                    int nbColonnes = (int)Math.Ceiling((double)1024 / (double)tileWidth);
                    int nbLignes = (int)Math.Ceiling(Convert.ToDouble(textureCount) / (double)nbColonnes);

                    objectSheet = new Bitmap(nbColonnes * tileWidth, nbLignes * tileHeight);

                    Graphics g = Graphics.FromImage(objectSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                             TileModel modelUsed = this.TileModelsObjectsUsed[TileModelsObjectsUsed.Count - textureCount];
                             if (modelUsed.GorgonSprite != null)
                             {
                                 Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                 g.DrawImage(new Bitmap(imageFrame, new Size(tileWidth, tileHeight)), positionFrame);
                                 dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                 textureCount--;


                                 imageFrame.Dispose();
                                 imageFrame = null;
                             }
                        }

                        if (textureCount == 0) break;

                    }
                    this.ObjectCountBySheet.Add(dataSheetGen.textureCount);

                    g.Dispose();
                    objectSheet.Save(folderDest + "\\" + sheetName + k +suffix+ ".png");
                    objectSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);

                }
                else
                {
                    objectSheet = new Bitmap(1024, 768);

                    int nbColonnes = (int)Math.Ceiling(nbColonnesParSheet);
                    int nbLignes = (int)Math.Ceiling(nbLignesParSheet);
                    Graphics g = Graphics.FromImage(objectSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                            TileModel modelUsed = this.TileModelsObjectsUsed[TileModelsObjectsUsed.Count - textureCount];
                            if (modelUsed.GorgonSprite != null)
                            {
                                Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                g.DrawImage(new Bitmap(imageFrame, new Size(tileWidth, tileHeight)), positionFrame);
                                dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                textureCount--;


                                imageFrame.Dispose();
                                imageFrame = null;
                            }
                        }

                        if (textureCount == 0) break;

                    }

                    this.ObjectCountBySheet.Add(dataSheetGen.textureCount);

                    g.Dispose();
                    objectSheet.Save(folderDest + "\\" + sheetName + k + suffix+".png");
                    objectSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);
                }


            }




            
        }


        private List<TileModel> getObjectsListUsed()
        {
            if (this.TabTiles != null)
            {
                List<TileModel> listObjects = new List<TileModel>();
                this.objectContent = new int[this.NbLines * this.NbColumns];

                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = this.TabTiles[i, j];
                        if (tile.TileModelImageObject != null)
                        {
                            if (!listObjects.Contains(tile.TileModelImageObject))
                                listObjects.Add(tile.TileModelImageObject);

                            this.objectContent[i * this.NbColumns + j] = listObjects.IndexOf(tile.TileModelImageObject);
                        }
                        else
                            this.objectContent[i * this.NbColumns + j] = -1;

                    }
                }

                return listObjects;
            }

            return null;
        }

        public int getNbObjectsImageUsed()
        {
            List<TileModel> liste = getObjectsListUsed();
            if (liste != null)
                return liste.Count;
            else return -1;
        }


        // --------------------------------------- SEQUENCE SET CREATION ----------------------------------------------------
        private List<TileModel> getTextureSequenceModelsUsed()
        {
            List<TileModel> modelsUsed = new List<TileModel>();
            if (this.TextureSequences != null)
            {
                for (int i = 0; i < this.TextureSequences.Count; i++)
                {
                    TileSequence seq = this.TextureSequences[i];
                    for (int j = 0; j < seq.Frames.Count; j++)
                    {
                        modelsUsed.Add(seq.Frames[j]);
                    }
                }


            }

            return modelsUsed;
        }
        
        private List<TileModel> getObjectSequenceModelsUsed()
        {
            List<TileModel> modelsUsed = new List<TileModel>();
            if (this.ObjectSequences != null)
            {
                for (int i = 0; i < this.ObjectSequences.Count; i++)
                {
                    TileSequence seq = this.ObjectSequences[i];
                    for (int j = 0; j < seq.Frames.Count; j++)
                    {
                        modelsUsed.Add(seq.Frames[j]);
                    }
                }


            }

            return modelsUsed;
        }

        public void creatTextureSequencesSet(string folderDest, string sheetName, string suffix,float xRatio, float yRatio)
        {
            //Creer une liste des textures a utiliser
            this.TextureSequenceCountBySheet = new List<int>();
            List<TileModel> modelsUsed = this.getTextureSequenceModelsUsed();
            if (modelsUsed == null || modelsUsed.Count <= 0) return;

            int tileHeight = Convert.ToInt32(this.TilesHeight * yRatio);
            int tileWidth = Convert.ToInt32(this.TilesWidth * xRatio);

            double nbLignesParSheet = Math.Ceiling((double)768 / (double)tileHeight);
            double nbColonnesParSheet = Math.Ceiling((double)1024 / (double)tileWidth);

            this.NbTextureSequenceSheets = (int)Math.Ceiling((double)modelsUsed.Count / (nbLignesParSheet * nbColonnesParSheet));

            int textureCount = modelsUsed.Count;
            for (int k = 0; k < this.NbTextureSequenceSheets; k++)
            {
                //Creer la TextureSet finale
                Bitmap objectSheet = null;

                //Creer la dataSheet 
                TilesMapDataSheetGenerator dataSheetGen = new TilesMapDataSheetGenerator(sheetName + k);

                if (k == this.NbTextureSequenceSheets - 1)
                {
                    //Calculer la taile de la texture finale
                    int nbColonnes = (int)Math.Ceiling((double)1024 / (double)tileWidth);
                    int nbLignes = (int)Math.Ceiling(Convert.ToDouble(textureCount) / (double)nbColonnes);

                    if (textureCount < nbColonnes)
                        nbColonnes = textureCount;

                    objectSheet = new Bitmap(nbColonnes * tileWidth, nbLignes * tileHeight);

                    Graphics g = Graphics.FromImage(objectSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                            TileModel modelUsed = modelsUsed[modelsUsed.Count - textureCount];
                            if (modelUsed.GorgonSprite != null)
                            {
                                Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                g.DrawImage(new Bitmap(imageFrame, new Size(tileWidth, tileHeight)), positionFrame);
                                dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                textureCount--;


                                imageFrame.Dispose();
                                imageFrame = null;
                            }
                        }

                        if (textureCount == 0) break;

                    }
                    this.TextureSequenceCountBySheet.Add(dataSheetGen.textureCount);

                    g.Dispose();
                    objectSheet.Save(folderDest + "\\" + sheetName + k + suffix+".png");
                    objectSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);

                }
                else
                {
                    objectSheet = new Bitmap(1024, 768);

                    int nbColonnes = (int)Math.Ceiling(nbColonnesParSheet);
                    int nbLignes = (int)Math.Ceiling(nbLignesParSheet);
                    Graphics g = Graphics.FromImage(objectSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                            TileModel modelUsed = modelsUsed[modelsUsed.Count - textureCount];
                            if (modelUsed.GorgonSprite != null)
                            {
                                Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                g.DrawImage(new Bitmap(imageFrame, new Size(tileWidth, tileHeight)), positionFrame);
                                dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                textureCount--;


                                imageFrame.Dispose();
                                imageFrame = null;
                            }
                        }

                        if (textureCount == 0) break;

                    }

                    this.TextureSequenceCountBySheet.Add(dataSheetGen.textureCount);

                    g.Dispose();
                    objectSheet.Save(folderDest + "\\" + sheetName + k + suffix+".png");
                    objectSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);
                }


            }

        }

        public void createObjectSequencesSet(string folderDest, string sheetName,string suffix, float xRatio, float yRatio)
        {
            //Creer une liste des textures a utiliser
            this.ObjectSequenceCountBySheet = new List<int>();
            List<TileModel> modelsUsed = this.TileModelsObjectsUsed;
            if (modelsUsed == null || modelsUsed.Count <= 0) return;

            int tileHeight = Convert.ToInt32(this.TilesHeight * yRatio);
            int tileWidth = Convert.ToInt32(this.TilesWidth * xRatio);

            double nbLignesParSheet = Math.Ceiling((double)768 / (double)tileHeight);
            double nbColonnesParSheet = Math.Ceiling((double)1024 / (double)tileWidth);

            this.NbObjectSequenceSheets = (int)Math.Ceiling((double)modelsUsed.Count / (nbLignesParSheet * nbColonnesParSheet));

            int textureCount = modelsUsed.Count;
            for (int k = 0; k < this.NbObjectSequenceSheets; k++)
            {
                //Creer la TextureSet finale
                Bitmap objectSheet = null;

                //Creer la dataSheet 
                TilesMapDataSheetGenerator dataSheetGen = new TilesMapDataSheetGenerator(sheetName + k);

                if (k == this.NbObjectSequenceSheets - 1)
                {
                    //Calculer la taile de la texture finale
                    int nbColonnes = (int)Math.Ceiling((double)1024 / (double)tileWidth);
                    int nbLignes = (int)Math.Ceiling(Convert.ToDouble(textureCount) / (double)nbColonnes);

                    objectSheet = new Bitmap(nbColonnes * tileWidth, nbLignes * tileHeight);

                    Graphics g = Graphics.FromImage(objectSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);

                              TileModel modelUsed = modelsUsed[modelsUsed.Count - textureCount];
                              if (modelUsed.GorgonSprite != null)
                              {
                                  Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                  g.DrawImage(new Bitmap(imageFrame, new Size(tileWidth, tileHeight)), positionFrame);
                                  dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                  textureCount--;


                                  imageFrame.Dispose();
                                  imageFrame = null;
                              }
                        }

                        if (textureCount == 0) break;

                    }
                    this.ObjectSequenceCountBySheet.Add(dataSheetGen.textureCount);

                    g.Dispose();
                    objectSheet.Save(folderDest + "\\" + sheetName + k + suffix+".png");
                    objectSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);

                }
                else
                {
                    objectSheet = new Bitmap(1024, 768);

                    int nbColonnes = (int)Math.Ceiling(nbColonnesParSheet);
                    int nbLignes = (int)Math.Ceiling(nbLignesParSheet);
                    Graphics g = Graphics.FromImage(objectSheet);
                    for (int i = 0; i < nbLignes; i++)
                    {
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            if (textureCount == 0) break;
                            Point positionFrame = new Point(j * tileWidth, i * tileHeight);
                             TileModel modelUsed = modelsUsed[modelsUsed.Count - textureCount];
                             if (modelUsed.GorgonSprite != null)
                             {
                                 Bitmap imageFrame = (Bitmap)modelUsed.GorgonSprite.Image.SaveBitmap();
                                 g.DrawImage(new Bitmap(imageFrame, new Size(tileWidth, tileHeight)), positionFrame);
                                 dataSheetGen.addFrame((i + 1) * j, positionFrame, new Size(tileWidth, tileHeight));
                                 textureCount--;


                                 imageFrame.Dispose();
                                 imageFrame = null;
                             }
                        }

                        if (textureCount == 0) break;

                    }

                    this.ObjectSequenceCountBySheet.Add(dataSheetGen.textureCount);

                    g.Dispose();
                    objectSheet.Save(folderDest + "\\" + sheetName + k + suffix+".png");
                    objectSheet.Dispose();
                    dataSheetGen.createDataSheetFileLua(folderDest,suffix, k);
                }


            }
            
        }


        public void refreshTilesMapContent ()
        {
            string mapContentPath = this.LayerParent.SceneParent.projectParent.ProjectPath + "\\Resources\\TileMaps\\" + this.TilesMapName;
            if (Directory.Exists(mapContentPath))
                Directory.Delete(mapContentPath, true);

            this.UpdateTileMapGraphicsContent();

            if (this.TabTiles != null)
            {
                int nbTotalOfTiles = this.NbLines * this.NbColumns;
                this.objectSequenceContent = new int[nbTotalOfTiles];
                this.textureSequenceContent = new int[nbTotalOfTiles];
                this.collisionContent = new int[nbTotalOfTiles];
                this.eventContent = new int[nbTotalOfTiles];

                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = this.TabTiles[i, j];
                        int tileIndexInTab = i * this.NbColumns + j;

                        if (tile.TileTextureSequence != null)
                        {
                            if (this.TextureSequences.Contains(tile.TileTextureSequence))
                                this.textureSequenceContent[tileIndexInTab] = this.TextureSequences.IndexOf(tile.TileTextureSequence);
                            else
                                this.textureSequenceContent[tileIndexInTab] = -1;

                        }
                        else
                            this.textureSequenceContent[tileIndexInTab] = -1;

                        if (tile.TileObjectSequence != null)
                        {
                            if (this.ObjectSequences.Contains(tile.TileObjectSequence))
                                this.objectSequenceContent[tileIndexInTab] = this.ObjectSequences.IndexOf(tile.TileObjectSequence);
                            else
                                this.objectSequenceContent[tileIndexInTab] = -1;

                        }
                        else
                            this.objectSequenceContent[tileIndexInTab] = -1;


                        
                        //Write collision state (0 = false, 1 = true)
                        if (tile.IsCrossable == true)
                        {
                            this.collisionContent[tileIndexInTab] = 0;
                        }
                        else
                            this.collisionContent[tileIndexInTab] = 1;

                        //Refresh events index
                        if (tile.TileEvent != null && this.TileEvents.Contains(tile.TileEvent))
                        {
                            int indexEventInTab = this.TileEvents.IndexOf(tile.TileEvent);
                            this.eventContent[tileIndexInTab] = indexEventInTab;
                        }
                        else
                        {
                            this.eventContent[tileIndexInTab] = -1;
                        }


                    }
                }

            }
        } 
        public string createJSONConfigFile(Scene sceneParent, string folderDest, float xRatio, float yRatio)
        {
            string fileNameDest = folderDest +"\\"+this.TilesMapName+"jsonconfig.json";

            JSONTileMap jsonMap = new JSONTileMap(this,sceneParent,xRatio,yRatio);
            jsonMap.serialize(folderDest,this);

            //Create the last modified file time for this tilesmap
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;

            int result = getSecondsFromDateSince2000(year, month, day, hour, minute, second);

            File.WriteAllText(folderDest + "\\" + this.TilesMapName.ToLower() + "lastmodifiedtime.txt", result.ToString());

            return fileNameDest;
           
        }

        

        public int getSecondsFromDate(int year, int month, int day,int hour, int minute, int second)
        {
            int YEAR = 31556926;
            int MONTH = 2629743;
            int DAY = 86400;
            int HOUR = 3600;
            int MIN = 60;

            return year * YEAR + month * MONTH + day * DAY + hour * HOUR + minute * MIN + second;
        }

        public int getSecondsFromDateSince2000(int year, int month, int day,int hour, int minute, int second)
        {
            int startSeconds = getSecondsFromDate(2000,1,1,0,0,0);
            int endsSeconds = getSecondsFromDate(year,month,day,hour,minute,second);

            return endsSeconds - startSeconds;
        }

        //----------------------- Collisionning Creation -----------------------
        public void createCollisionsContentFile(string folderDest)
        {
            //Generer le fichier de contenu des textures
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.NbLines; i++)
            {
                String tilesCollisionLineContent = "";
                for (int j = 0; j < this.NbColumns; j++)
                {
                    Tile tile = this.TabTiles[i, j];

                    //Write collision state (0 = false, 1 = true)
                    if (tile.IsCrossable == true)
                    {
                        tilesCollisionLineContent += "i0";
                    }
                    else
                        tilesCollisionLineContent += "i1";

                }
                tilesCollisionLineContent += "i";
                sb.AppendLine(tilesCollisionLineContent);


                

            }

            File.WriteAllText(folderDest + "\\" + this.TilesMapName.ToLower() + "collisionsinitfile.txt", sb.ToString());
        }


        public void UpdateTileMapGraphicsContent()
        {
            try
            {
                if (this.TileModelsTextureUsed != null)
                {
                    for (int i = 0; i < this.TileModelsTextureUsed.Count; i++)
                    {
                        TileModel tileModel = this.TileModelsTextureUsed[i];
                        if (tileModel.Name == null || tileModel.Name.Equals(""))
                        {
                            tileModel.Name = "TEXTURE_" + this.TilesMapName + "_" + i;
                            
                        }

                        this.UpdateGorgonTileModel(tileModel, true);
                    }
                }

                if (this.TileModelsObjectsUsed != null)
                {
                    for (int i = 0; i < this.TileModelsObjectsUsed.Count; i++)
                    {
                        TileModel tileModel = this.TileModelsObjectsUsed[i];
                        if (tileModel.Name == null || tileModel.Name.Equals(""))
                        {
                            tileModel.Name = "OBJECT_" + this.TilesMapName + "_" + i;

                        }
                        this.UpdateGorgonTileModel(tileModel, true);
                    }
                }


                if (this.TextureSequences != null)
                {
                    for (int i = 0; i < this.TextureSequences.Count; i++)
                    {
                        TileSequence seq = this.TextureSequences[i];
                        for (int j = 0; j < seq.Frames.Count; j++)
                        {
                            TileModel tileModel = seq.Frames[j];
                            if (tileModel.Name == null || tileModel.Name.Equals(""))
                            {
                                tileModel.Name = "TEXTURESEQUENCE_" + this.TilesMapName + "_" + i + "_" + j;

                            }
                            this.UpdateGorgonTileModel(tileModel, true);
                        }
                    }
                }

                if (this.ObjectSequences != null)
                {
                    for (int i = 0; i < this.ObjectSequences.Count; i++)
                    {
                        TileSequence seq = this.ObjectSequences[i];
                        for (int j = 0; j < seq.Frames.Count; j++)
                        {
                            TileModel tileModel = seq.Frames[j];
                            if (tileModel.Name == null || tileModel.Name.Equals(""))
                            {
                                tileModel.Name = "OBJECTSEQUENCE_" + this.TilesMapName + "_" + i+"_"+j;

                            }
                            this.UpdateGorgonTileModel(tileModel, true);
                        }
                    }
                }

                //Icons EVENTS
                if (this.TouchEventSprite == null)
                {
                    this.TouchEventSprite = new GorgonLibrary.Graphics.Sprite("TouchEventSprite",
                        GorgonLibrary.Graphics.Image.FromBitmap("TouchEventSprite", Properties.Resources.touchIcon));

                }

                if (this.CollisionEventSprite == null)
                {
                    this.CollisionEventSprite = new GorgonLibrary.Graphics.Sprite("CollisionEventSprite",
                        GorgonLibrary.Graphics.Image.FromBitmap("CollisionEventSprite", Properties.Resources.collisionIcon2));

                }

                if (this.PreCollisionEventSprite == null)
                {
                    this.PreCollisionEventSprite = new GorgonLibrary.Graphics.Sprite("PreCollisionEventSprite",
                        GorgonLibrary.Graphics.Image.FromBitmap("PreCollisionEventSprite", Properties.Resources.preCollisionIcon));

                }

                if (this.PostCollisionEventSprite == null)
                {
                    this.PostCollisionEventSprite = new GorgonLibrary.Graphics.Sprite("PostCollisionEventSprite",
                        GorgonLibrary.Graphics.Image.FromBitmap("PostCollisionEventSprite", Properties.Resources.postCollisionIcon));

                }
            }
            catch (Exception ex)
            {

            }

        }
        public bool reloadMapContent(string folder)
        {
            try
            {
                if (this.TextureSequences == null)
                    this.TextureSequences = new List<TileSequence>();

                if (this.ObjectSequences == null)
                    this.ObjectSequences = new List<TileSequence>();

                if (this.TileEvents == null)
                    this.TileEvents = new List<TileEvent>();


                this.UpdateTileMapGraphicsContent();

                //Creer le tableau a 2D des tiles
                this.TabTiles = new Tile[this.NbLines, this.NbColumns];

                JSONTileMap jsonMap = new JSONTileMap();
                jsonMap.deserialize(folder, this);
                jsonMap = null;

                for (int i = 0; i < this.NbLines; i++)
                {
                    for (int j = 0; j < this.NbColumns; j++)
                    {
                        Tile tile = new Tile(i, j, this.TilesWidth, this.TilesHeight, true, this) ;
                        int indexTileInTab = i * this.NbColumns + j;

                        if (this.textureContent != null)
                        {
                            int indexTexture = this.textureContent[indexTileInTab];
                            if (indexTexture > -1 && this.TileModelsTextureUsed.Count > indexTexture)
                            {
                                
                                tile.setTexture(this.TileModelsTextureUsed[indexTexture]);
                            }
                                
                        }

                        if (this.objectContent != null)
                        {
                            int indexObject = this.objectContent[indexTileInTab];
                            if (indexObject > -1 && this.TileModelsObjectsUsed.Count > indexObject)
                                tile.setObjectImage(this.TileModelsObjectsUsed[indexObject]);

                        }

                        if (this.textureSequenceContent != null)
                        {
                            int indexSequenceTexture = this.textureSequenceContent[indexTileInTab];
                            if (indexSequenceTexture > -1 && this.TextureSequences.Count > indexSequenceTexture)
                                tile.setTextureSequence(this.TextureSequences[indexSequenceTexture]);
                        }

                        if (this.objectSequenceContent != null)
                        {
                            int indexSequenceObject = this.objectSequenceContent[indexTileInTab];
                            if (indexSequenceObject > -1 && this.ObjectSequences.Count > indexSequenceObject)
                                tile.setObjectSequence(this.ObjectSequences[indexSequenceObject]);

                        }

                        if (this.collisionContent != null)
                        {
                            int collide = this.collisionContent[indexTileInTab];
                            if (collide == 0)
                                tile.IsCrossable = true;
                            else
                                tile.IsCrossable = false;

                        }

                        if (this.eventContent != null)
                        {
                            int indexEvent = this.eventContent[indexTileInTab];
                            if (indexEvent > -1 && this.TileEvents.Count > indexEvent)
                                tile.setEvent(this.TileEvents[indexEvent]);

                        }
                        this.TabTiles[i, j] = tile;

                    }
                }

                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }
           
        }

        public bool reloadFromFile(string folder)
        {
            try
            {
                //Creer un tableau de textureFrame
                int nbTotalTiles = this.NbColumns * this.NbLines;

                //Creer le tableau a 2D des tiles
                this.TabTiles = new Tile[this.NbLines, this.NbColumns];

                string[] tabFrameTexture = new string[nbTotalTiles];
                string[] tabFrameObj = new string[nbTotalTiles];

                //Lire le fichier des textures devant exister
                string finalpath = folder + "\\" + this.TilesMapName.ToLower() + "texturesinitfile.txt";

                //Lire le fichier des objets devant exister
                string finalpathObj = folder + "\\" + this.TilesMapName.ToLower() + "objectsinitfile.txt";

                if (File.Exists(finalpath) && File.Exists(finalpathObj))
                {
                    //Lire le fichier
                    string[] textContent = File.ReadAllLines(finalpath);

                    //Lire le fichier
                    string[] objContent = File.ReadAllLines(finalpathObj);

                    int indexDepText = 0;
                    int indexFinText = 0;
                    string indexFrameTexture = null;
                    int lastIndexOfText = 0;

                    int indexDepObj = 0;
                    int indexFinObj = 0;
                    string indexFrameObj = null;
                    int lastIndexOfObj = 0;

                    for (int i = 0; i < this.NbLines; i++)
                    {
                        indexDepText = 0;
                        indexFinText = 0;
                        indexFrameTexture = null;
                        lastIndexOfText = 0;

                        indexDepObj = 0;
                        indexFinObj = 0;
                        indexFrameObj = null;
                        lastIndexOfObj = 0;


                        for (int j = 0; j < this.NbColumns; j++)
                        {

                            //Creer une Tile
                            Tile tile = new Tile(i, j, this.TilesWidth, this.TilesHeight, true, this);

                            //Recuperer la texture
                            indexDepText = textContent[i].IndexOf("i", lastIndexOfText);
                            indexFinText = textContent[i].IndexOf("i", indexDepText + 1);

                            indexFrameTexture = textContent[i].Substring(indexDepText + 1, indexFinText - indexDepText - 1);
                            lastIndexOfText = indexFinText;

                            //Definir la texture du tile si elle existe
                            if (indexFrameTexture != null && !indexFrameTexture.Equals("N"))
                            {
                                int index = Convert.ToInt32(indexFrameTexture) - 1;
                                if (index < this.TileModelsTextureUsed.Count && index > -1)
                                    tile.setTexture(this.TileModelsTextureUsed[index]);
                            }

                            //Recuperer l'objet
                            indexDepObj = objContent[i].IndexOf("i", lastIndexOfObj);
                            indexFinObj = objContent[i].IndexOf("i", indexDepObj + 1);
                            indexFrameObj = objContent[i].Substring(indexDepObj + 1, indexFinObj - indexDepObj - 1);
                            lastIndexOfObj = indexFinObj;


                            //Definir l'image objet du tile si elle existe
                            if (indexFrameObj != null && !indexFrameObj.Equals("N"))
                            {
                                int index = Convert.ToInt32(indexFrameObj) - 1;
                                if (index < this.TileModelsObjectsUsed.Count && index > -1)
                                    tile.setObjectImage(this.TileModelsObjectsUsed[index]);

                            }

                            //L'inserer dans les tableau des tiles
                            this.TabTiles[i, j] = tile;
                        }
                    }



                }
                else
                {
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during tiles map reloading !\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

              
                return false;
            }
        }

        

    }
}
 