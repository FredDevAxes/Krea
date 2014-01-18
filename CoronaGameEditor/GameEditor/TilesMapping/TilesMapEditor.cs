using System;
using System.Drawing;
using System.Windows.Forms;
using Krea.GameEditor.PropertyGridConverters;
using Krea.Corona_Classes;
using System.Collections.Generic;

namespace Krea.GameEditor.TilesMapping
{
    public partial class TilesMapEditor : UserControl
    {
       

        //---------------------------------------------------
        //-------------------Attributes----------------------
        //---------------------------------------------------
        private TilesMap currentTilesMap;
        private Tile tileDepart;
        public Tile SelectedTile;
        private float currentScale;
        private Form1 mainForm;
        private bool isMousePressed = false;
        private String selectionMode;
        public String CreationMode;
        private TilesSelection ModelsSelection;

        private string showLayer = "ALL";
        private Cursor crayonCursor;
        private Cursor rectangleCursor;
        private Cursor potPeintureCursor;
        private bool showCollision = false;
        private Cursor gommeCursor;
        private TileModel ModelSelected;
        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public TilesMapEditor()
        {
            InitializeComponent();
        }

        //---------------------------------------------------
        //-------------------Methods------------------------
        //---------------------------------------------------
        public void init(Form1 mainForm)
        {
            this.mainForm = mainForm;

            this.crayonCursor = Form1.CreateCursor(new Bitmap(Properties.Resources.editIcon,
                new Size(Properties.Resources.editIcon.Width / 2, Properties.Resources.editIcon.Height / 2))
                                        , 0, Properties.Resources.editIcon.Height/2);

            this.rectangleCursor = this.crayonCursor;
            this.potPeintureCursor = this.crayonCursor;

            this.gommeCursor = Form1.CreateCursor(new Bitmap(Properties.Resources.gommeIcon,
                new Size(Properties.Resources.gommeIcon.Width / 2, Properties.Resources.gommeIcon.Height / 2)), 5, Properties.Resources.gommeIcon.Height/2);
           

            selectionMode = "NORMAL";

            CreationMode = "CREATING_MODELS";

            this.currentScale = 1;
            this.graduationBarY.setDefaultOffset(this.graduationBarX.Size.Height);

            
        }

        public TilesMap CurrentTilesMap
        {
            get { return this.currentTilesMap; }
        }

        public void setTilesMap(TilesMap tilesMap)
        {

           
            this.currentTilesMap = tilesMap;
          
            if (this.currentTilesMap != null)
            {
                this.tileEventManager.init(this.currentTilesMap, this.mainForm);

                this.ModelsSelection = new TilesSelection(tilesMap);
                this.setScrollValue();

                Point offsetPoint = this.getOffsetPoint();
                Rectangle rect = new Rectangle(new Point(-offsetPoint.X, -offsetPoint.Y), this.surfaceDessin.Size);
                    
                this.currentTilesMap.setSurfaceVisible(rect,1,1);

                this.refreshPaletteContent();
                GorgonLibrary.Gorgon.Go();
            }
            
        }

        public Point getOffsetPoint()
        {
            return new Point(-this.hScrollBar.Value, this.vScrollBar.Value);
        }

        public void setSelectedTile(Tile tileToSelect)
        {
            if (this.SelectedTile != null)
                this.SelectedTile.isSelected = false;

            this.SelectedTile = tileToSelect;
            this.SelectedTile.isSelected = true;

            

        }

       

        private void applyTextureOnSelectedRectangle()
        {
            if (this.showCollision == true && this.tileDepart != null)
            {
              

                int iMax = this.SelectedTile.LineIndex + 1;
                if (iMax > this.currentTilesMap.NbLines) iMax = this.currentTilesMap.NbLines;

                int jMax = this.SelectedTile.ColumnIndex + 1;
                if (jMax > this.currentTilesMap.NbColumns) jMax = this.currentTilesMap.NbColumns;

                for (int i = this.tileDepart.LineIndex; i < iMax; i++)
                {
                    for (int j = this.tileDepart.ColumnIndex; j < jMax; j++)
                    {

                         this.currentTilesMap.TabTiles[i, j].IsCrossable = this.tileDepart.IsCrossable;

                    }
                }

            }
            else if (this.ModelsSelection != null && this.tileDepart != null && ModelsSelection.ListModelsSelected.Count>0)
            {
                int indexModelToApply= -1;

                int iMax = this.SelectedTile.LineIndex + 1;
                if (iMax > this.currentTilesMap.NbLines) iMax = this.currentTilesMap.NbLines;

                int jMax = this.SelectedTile.ColumnIndex + 1;
                if (jMax > this.currentTilesMap.NbColumns) jMax = this.currentTilesMap.NbColumns;

                for (int i = this.tileDepart.LineIndex; i < iMax; i++)
                {
                    for (int j = this.tileDepart.ColumnIndex; j < jMax; j++)
                    {
                        //Recuperer le bon model de la liste
                        indexModelToApply++;

                        if (indexModelToApply == ModelsSelection.ListModelsSelected.Count)
                            indexModelToApply = 0;

                        
                        if (this.CreationMode.Equals("CREATING_MODELS"))
                        {
                            TileModel model = ModelsSelection.ListModelsSelected[indexModelToApply];

                            if (model.IsTexture == true)
                                this.currentTilesMap.TabTiles[i, j].setTexture(model);
                            else
                                this.currentTilesMap.TabTiles[i, j].setObjectImage(model);
                        }
                        else if (this.CreationMode.Equals("CREATING_TEXTURE_SEQUENCE"))
                        {
                            if (this.textureSequenceListView.SelectedItems.Count > 0)
                            {
                                this.currentTilesMap.TabTiles[i, j].setTextureSequence(this.textureSequenceListView.SelectedItems[0].Tag as TileSequence);
                            }
                        }
                        else if (this.CreationMode.Equals("CREATING_OBJECT_SEQUENCE"))
                        {
                            if (this.objectSequenceListView.SelectedItems.Count > 0)
                            {
                                this.currentTilesMap.TabTiles[i, j].setObjectSequence(this.objectSequenceListView.SelectedItems[0].Tag as TileSequence);
                            }
                        }
                        else if (this.CreationMode.Equals("CREATING_EVENT"))
                        {
                            if (this.tileEventManager.eventListView.SelectedItems.Count > 0)
                            {
                                this.currentTilesMap.TabTiles[i, j].setEvent(this.tileEventManager.eventListView.SelectedItems[0].Tag as TileEvent);
                            }
                        }
                        else if (this.CreationMode.Equals("REMOVING_TEXT"))
                        {
                            this.currentTilesMap.TabTiles[i, j].setTexture(null);
                            this.currentTilesMap.TabTiles[i, j].setTextureSequence(null);
                        }

                        else if (this.CreationMode.Equals("REMOVING_OBJ"))
                        {
                            this.currentTilesMap.TabTiles[i, j].setObjectImage(null);
                            this.currentTilesMap.TabTiles[i, j].setObjectSequence(null);
                        }
                        else if (this.CreationMode.Equals("REMOVING_EVENT"))
                        {
                            this.currentTilesMap.TabTiles[i, j].setEvent(null);
                        }
                       
                    }
                }
            }
        }


        //---------------------------------------------------
        //-------------------Events--------------------------
        //---------------------------------------------------

        public void DrawObjectModelsGorgon()
        {
            this.refreshObjectTileModelLocations();
            GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(Color.Black);
            GorgonLibrary.Gorgon.CurrentRenderTarget.BeginDrawing();

            if (this.currentTilesMap != null)
            {
                Point offsetPoint = new Point(0, this.objectModelsTrackBar.Value);
                List<TileModel> modelsSelected = this.currentTilesMap.TileModelsObjectsUsed;
                if (modelsSelected != null)
                {

                    for (int i = 0; i < modelsSelected.Count; i++)
                    {
                        modelsSelected[i].DrawGorgon(offsetPoint);

                    }
                }


                if (this.ModelsSelection != null)
                {
                    if (this.ModelsSelection.ListModelsSelected.Count > 0)
                    {
                        if (this.ModelsSelection.ListModelsSelected[0].IsTexture == false)
                        {
                            for (int i = 0; i < this.ModelsSelection.ListModelsSelected.Count; i++)
                            {
                                Rectangle finalRect = new Rectangle(offsetPoint.X + this.ModelsSelection.ListModelsSelected[i].surfaceRect.X,
                                    offsetPoint.Y + this.ModelsSelection.ListModelsSelected[i].surfaceRect.Y,
                                    this.ModelsSelection.ListModelsSelected[i].surfaceRect.Width,
                                    this.ModelsSelection.ListModelsSelected[i].surfaceRect.Height);

                                GorgonGraphicsHelper.Instance.DrawRectangle(finalRect, 2, Color.White, 1);

                            }
                        }
                    }
                  


                }
            }

            GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();
        }

        public void DrawTextureModelsGorgon()
        {
            try
            {
                this.refreshTextureTileModelLocations();
                GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(Color.Black);
                GorgonLibrary.Gorgon.CurrentRenderTarget.BeginDrawing();

                if (this.currentTilesMap != null)
                {
                    Point offsetPoint = new Point(0, this.textureModelsTrackBar.Value);
                    List<TileModel> modelsSelected = this.currentTilesMap.TileModelsTextureUsed;
                    if (modelsSelected != null)
                    {

                        for (int i = 0; i < modelsSelected.Count; i++)
                        {
                            modelsSelected[i].DrawGorgon(offsetPoint);

                        }
                    }


                    if (this.ModelsSelection != null)
                    {
                        if (this.ModelsSelection.ListModelsSelected.Count > 0)
                        {
                            if (this.ModelsSelection.ListModelsSelected[0].IsTexture == true)
                            {
                                for (int i = 0; i < this.ModelsSelection.ListModelsSelected.Count; i++)
                                {
                                    Rectangle finalRect = new Rectangle(offsetPoint.X + this.ModelsSelection.ListModelsSelected[i].surfaceRect.X,
                                        offsetPoint.Y + this.ModelsSelection.ListModelsSelected[i].surfaceRect.Y,
                                        this.ModelsSelection.ListModelsSelected[i].surfaceRect.Width,
                                        this.ModelsSelection.ListModelsSelected[i].surfaceRect.Height);

                                    GorgonGraphicsHelper.Instance.DrawRectangle(finalRect, 2, Color.White, 1);

                                }
                            }
                        }


                    }
                }

                GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();
            }
            catch (Exception ex)
            {
                GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();
            }
        }

       
        public void DrawGorgon()
        {
            try
            {
                GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(Color.Black);
                GorgonLibrary.Gorgon.CurrentRenderTarget.BeginDrawing();
                if (this.currentTilesMap != null)
                {

                    Point scroolBarOffset = this.getOffsetPoint();
                    Point offsetPoint = new Point(scroolBarOffset.X - this.currentTilesMap.Location.X, scroolBarOffset.Y - this.currentTilesMap.Location.Y);
                    Rectangle rect = new Rectangle(new Point(-scroolBarOffset.X, -scroolBarOffset.Y), this.surfaceDessin.Size);
                    this.currentTilesMap.setSurfaceVisible(rect, currentScale, currentScale);

                    this.currentTilesMap.DrawGorgon(this.currentScale, this.currentScale, offsetPoint, this.showLayer, this.showCollision);



                    if (this.SelectedTile != null)
                    {
                        Rectangle rectDestSelection = new Rectangle(
                            new Point(scroolBarOffset.X + this.SelectedTile.Location.X, scroolBarOffset.Y + this.SelectedTile.Location.Y)
                                , new Size(this.SelectedTile.Width, this.SelectedTile.Height));

                        GorgonGraphicsHelper.Instance.DrawRectangle(rectDestSelection,2,Color.White,this.currentScale);
                    }
                      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during tiles drawing !\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();

        }

        private void surfaceDessin_Paint(object sender, PaintEventArgs e)
        {
            if (GorgonLibrary.Gorgon.IsInitialized == true)
                GorgonLibrary.Gorgon.Go();


            //try
            //{

            //    if (this.currentTilesMap != null)
            //    {
                    
            //        Point scroolBarOffset =  this.getOffsetPoint();
            //        Point offsetPoint = new Point(scroolBarOffset.X - this.currentTilesMap.Location.X, scroolBarOffset.Y - this.currentTilesMap.Location.Y);
            //        Rectangle rect = new Rectangle(new Point(-scroolBarOffset.X, -scroolBarOffset.Y), this.surfaceDessin.Size);
            //        this.currentTilesMap.setSurfaceVisible(rect, currentScale, currentScale);

            //        this.currentTilesMap.DrawTilesInEditor(e.Graphics, this.currentScale,this.currentScale, offsetPoint,this.showLayer,this.showCollision);


            //        if (this.SelectedTile != null)
            //            e.Graphics.DrawRectangle(new Pen(Brushes.White, 2), new Rectangle(
            //                new Point(scroolBarOffset.X + this.SelectedTile.Location.X, scroolBarOffset.Y + this.SelectedTile.Location.Y)
            //                    , new Size(this.SelectedTile.Width, this.SelectedTile.Height)));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error during tiles drawing !\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //    //----------------------------------------------------------------------------------------------------
            //    //-------------------------------------- EVENT REPORTING ---------------------------------------------
            //    //----------------------------------------------------------------------------------------------------
            //    Krea.KreaEventReports.NativeKreaEvent kreaEvent = new Krea.KreaEventReports.NativeKreaEvent(ex);
            //    if (Settings1.Default.KreaEventReportAutomaticSend == true)
            //    {
            //        KreaEventReports.KreaEventReportSender.ReportEvent(kreaEvent);
            //    }
            //    else
            //    {
            //        KreaEventReports.KreaEventReporterForm reportForm = new KreaEventReports.KreaEventReporterForm();
            //        reportForm.init(kreaEvent);
            //        reportForm.Show();
            //    }
            //    //----------------------------------------------------------------------------------------------------
            //    //----------------------------------------------------------------------------------------------------
            //}
           
        }

        private void surfaceDessin_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.mainForm != null)
            {
                if (this.currentTilesMap != null)
                {
                    TilesMapPropertyConverter tilesMapConverter = new TilesMapPropertyConverter(this.currentTilesMap, this.mainForm);
                    this.mainForm.propertyGrid1.SelectedObject = tilesMapConverter;
                }
            }
           
        }

        private void surfaceDessin_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.mainForm != null)
            {

                isMousePressed = true;
                Point offSetPoint = this.getOffsetPoint();
                Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.currentScale)),
                    Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.currentScale)));

                if (this.showCollision == true)
                {
                    
                    this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                    this.tileDepart.IsCrossable = !this.tileDepart.IsCrossable;

                    GorgonLibrary.Gorgon.Go();
                }
                else
                {
                    if (this.CreationMode.Equals("CREATING_MODELS"))
                    {
                        
                        if (ModelsSelection != null && ModelsSelection.ListModelsSelected.Count > 0)
                        {
                            if (this.selectionMode.Equals("ALL"))
                            {
                                this.CurrentTilesMap.applyTextureOnAllTiles(ModelsSelection.ListModelsSelected[0]);
                            }
                            else
                            {
                                this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                                this.currentTilesMap.applyTextureOnTilePointed(pTouched, ModelsSelection.ListModelsSelected[0]);

                            }
                            GorgonLibrary.Gorgon.Go();
                        }
                    }
                    else if (this.CreationMode.Equals("CREATING_TEXTURE_SEQUENCE"))
                    {
                       
                        if (this.textureSequenceListView.SelectedItems.Count > 0)
                        {
                            if (this.selectionMode.Equals("ALL"))
                            {
                                this.CurrentTilesMap.applyTextureSequenceOnAllTiles(this.textureSequenceListView.SelectedItems[0].Tag as TileSequence);
                            }
                            else
                            {
                                this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                                this.currentTilesMap.applyTextureSequenceOnTilePointed(pTouched, this.textureSequenceListView.SelectedItems[0].Tag as TileSequence);

                            }
                            GorgonLibrary.Gorgon.Go();
                        }
                    }
                    else if (this.CreationMode.Equals("CREATING_OBJECT_SEQUENCE"))
                    {
                        
                        if (this.objectSequenceListView.SelectedItems.Count > 0)
                        {
                            if (this.selectionMode.Equals("ALL"))
                            {
                                this.CurrentTilesMap.applyObjectSequenceOnAllTiles(this.objectSequenceListView.SelectedItems[0].Tag as TileSequence);
                            }
                            else
                            {
                                this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                                this.currentTilesMap.applyObjectSequenceOnTilePointed(pTouched, this.objectSequenceListView.SelectedItems[0].Tag as TileSequence);

                            }
                            GorgonLibrary.Gorgon.Go();
                        }
                    }

                    else if (this.CreationMode.Equals("CREATING_EVENT"))
                    {
                        TileEventManager eventManager = this.tileEventManager;
                        if (eventManager.eventListView.SelectedItems.Count > 0)
                        {
                            if (this.selectionMode.Equals("ALL"))
                            {
                                this.CurrentTilesMap.applyEventOnAllTiles(eventManager.eventListView.SelectedItems[0].Tag as TileEvent);
                            }
                            else
                            {
                                this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                                this.currentTilesMap.applyEventOnTilePointed(pTouched, eventManager.eventListView.SelectedItems[0].Tag as TileEvent);

                            }
                            GorgonLibrary.Gorgon.Go();
                        }
                    }
                    else if (this.CreationMode.Equals("REMOVING_TEXT"))
                    {
                        if (this.selectionMode.Equals("ALL"))
                        {
                            for (int i = 0; i < this.currentTilesMap.NbLines; i++)
                            {
                                for (int j = 0; j < this.currentTilesMap.NbColumns; j++)
                                {
                                    this.currentTilesMap.TabTiles[i, j].setTexture(null);
                                    this.currentTilesMap.TabTiles[i, j].setTextureSequence(null);
                                }
                            }
                        }
                        else
                        {
                            this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                            this.currentTilesMap.removeTextureOnTilePointed(pTouched);
                        }

                        GorgonLibrary.Gorgon.Go();
                    }

                    else if (this.CreationMode.Equals("REMOVING_OBJ"))
                    {
                        if (this.selectionMode.Equals("ALL"))
                        {
                            for (int i = 0; i < this.currentTilesMap.NbLines; i++)
                            {
                                for (int j = 0; j < this.currentTilesMap.NbColumns; j++)
                                {
                                    this.currentTilesMap.TabTiles[i, j].setObjectImage(null);
                                    this.currentTilesMap.TabTiles[i, j].setObjectSequence(null);
                                }
                            }
                        }
                        else
                        {
                            this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                            this.currentTilesMap.removeObjectOnTilePointed(pTouched);
                        }

                        GorgonLibrary.Gorgon.Go();
                    }
                    else if (this.CreationMode.Equals("REMOVING_EVENT"))
                    {
                        if (this.selectionMode.Equals("ALL"))
                        {
                            for (int i = 0; i < this.currentTilesMap.NbLines; i++)
                            {
                                for (int j = 0; j < this.currentTilesMap.NbColumns; j++)
                                {
                                    this.currentTilesMap.TabTiles[i, j].setEvent(null);
                                }
                            }
                        }
                        else
                        {
                            this.tileDepart = this.currentTilesMap.getTileAtLocation(pTouched);
                            this.currentTilesMap.removeEventOnTilePointed(pTouched);
                        }

                        GorgonLibrary.Gorgon.Go();
                    }
                }
                    
              
            }
            
        }

        private void surfaceDessin_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.currentTilesMap != null)
            {
                if(this.showCollision == true)
                    Cursor = this.crayonCursor;
                else if (this.CreationMode.Equals("REMOVING_TEXT"))
                {
                    Cursor = this.gommeCursor;
                }
                else if (this.CreationMode.Equals("REMOVING_OBJ"))
                {
                    Cursor = this.gommeCursor;
                }
                else if (this.CreationMode.Equals("PIPETTE"))
                {
                    Cursor = this.gommeCursor;
                }
                else if (this.selectionMode.Equals("NORMAL"))
                {
                    Cursor = this.crayonCursor;
                }
                else if (this.selectionMode.Equals("RECTANGLE"))
                {
                    Cursor = this.rectangleCursor;
                }
                else if (this.selectionMode.Equals("ALL"))
                {
                    Cursor = this.potPeintureCursor;
                }

                Point offSetPoint = this.getOffsetPoint();
                Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.currentScale)),
                    Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.currentScale)));

                //Selectioner le tile pointé
                Tile tile = this.currentTilesMap.getTileAtLocation(pTouched);
                if (tile != null)
                    setSelectedTile(tile);
                else
                    this.SelectedTile = null;

                if (this.isMousePressed == true)
                {
                    TileModel modelToApply = this.ModelSelected;
                    
                    if (this.selectionMode.Equals("NORMAL"))
                    {

                        if (this.showCollision == false)
                        {
                            if (this.CreationMode.Equals("CREATING_MODELS") && modelToApply != null)
                                this.currentTilesMap.applyTextureOnTilePointed(pTouched, modelToApply);
                            else if (this.CreationMode.Equals("CREATING_TEXTURE_SEQUENCE")
                                && this.textureSequenceListView.SelectedItems.Count > 0)
                            {
                                TileSequence seq = this.textureSequenceListView.SelectedItems[0].Tag as TileSequence;
                                this.currentTilesMap.applyTextureSequenceOnTilePointed(pTouched, seq);
                            }
                            else if (this.CreationMode.Equals("CREATING_OBJECT_SEQUENCE")
                                && this.objectSequenceListView.SelectedItems.Count > 0)
                            {
                                TileSequence seq = this.objectSequenceListView.SelectedItems[0].Tag as TileSequence;
                                this.currentTilesMap.applyObjectSequenceOnTilePointed(pTouched, seq);
                            }
                            else if (this.CreationMode.Equals("CREATING_EVENT")
                            && this.tileEventManager.eventListView.SelectedItems.Count > 0)
                            {
                                TileEvent ev = this.tileEventManager.eventListView.SelectedItems[0].Tag as TileEvent;
                                this.currentTilesMap.applyEventOnTilePointed(pTouched, ev);
                            }
                            else if (this.CreationMode.Equals("REMOVING_TEXT"))
                                this.currentTilesMap.removeTextureOnTilePointed(pTouched);
                            else if (this.CreationMode.Equals("REMOVING_OBJ"))
                                this.currentTilesMap.removeObjectOnTilePointed(pTouched);
                            else if (this.CreationMode.Equals("REMOVING_EVENT"))
                                this.currentTilesMap.removeEventOnTilePointed(pTouched);

                        }
                        
                    }
                    else if (this.selectionMode.Equals("RECTANGLE"))
                    {
                       
                            applyTextureOnSelectedRectangle();
                    }

                    

                }

                GorgonLibrary.Gorgon.Go();

                this.graduationBarX.reportMouseLocation(pTouched.X);
                this.graduationBarY.reportMouseLocation(pTouched.Y);

                this.Parent.Select();

            }

        }



        private void surfaceDessin_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMousePressed = false;
        }

        private void rectangleSelectionBt_Click(object sender, EventArgs e)
        {
            this.selectionMode = "RECTANGLE";
            this.rectangleSelectionBt.Checked = true;

            this.normalSelectionBt.Checked = false;
            this.applyToAllBt.Checked = false;
            this.removeObjectBt.Checked = false;
            this.removeTextureBt.Checked = false;
        }

       

        private void normalSelectionBt_Click(object sender, EventArgs e)
        {
            this.selectionMode = "NORMAL";
            this.normalSelectionBt.Checked = true;

            this.rectangleSelectionBt.Checked = false;
            this.applyToAllBt.Checked = false;
            this.removeObjectBt.Checked = false;
            this.removeTextureBt.Checked = false;

          
        }

        private void applyToAllBt_Click(object sender, EventArgs e)
        {
            this.selectionMode = "ALL";
            this.applyToAllBt.Checked = true;

            this.normalSelectionBt.Checked = false;
            this.rectangleSelectionBt.Checked = false;
            this.removeObjectBt.Checked = false;
            this.removeTextureBt.Checked = false;
        }

       

        private void removeTextureBt_Click(object sender, EventArgs e)
        {
            CreationMode = "REMOVING_TEXT";
            this.removeTextureBt.Checked = true;

            this.normalSelectionBt.Checked = false;
            this.applyToAllBt.Checked = false;
            this.removeObjectBt.Checked = false;
            this.rectangleSelectionBt.Checked = false;
        }

        private void removeObjectBt_Click(object sender, EventArgs e)
        {
            CreationMode = "REMOVING_OBJ";
            this.removeObjectBt.Checked = true;

            this.normalSelectionBt.Checked = false;
            this.applyToAllBt.Checked = false;
            this.rectangleSelectionBt.Checked = false;
            this.removeTextureBt.Checked = false;
        }

       

        private void zoomBackBt_Click(object sender, EventArgs e)
        {
            if (this.currentTilesMap!= null)
            {
                this.zoomArriere();
            }
        }

        private void zoomInBt_Click(object sender, EventArgs e)
        {
            this.zoomAvant();
        }

        private void zoomArriere()
        {
            this.currentScale = this.currentScale / 2;
            if (this.currentScale < 0.25f) this.currentScale = 0.25f;

            this.setScrollValue();

            GorgonLibrary.Gorgon.Go();

            this.graduationBarX.setScale(this.currentScale);
            this.graduationBarY.setScale(this.currentScale);
        }

        private void zoomAvant()
        {
            this.currentScale = this.currentScale * 2;
            if (this.currentScale > 4) this.currentScale = 4;

            this.setScrollValue();

            GorgonLibrary.Gorgon.Go();
            this.graduationBarX.setScale(this.currentScale);
            this.graduationBarY.setScale(this.currentScale);
        }

        private void setScrollValue()
        {
            if (this.currentTilesMap != null)
            {

                this.vScrollBar.Maximum = 0;
                this.hScrollBar.Maximum =  Convert.ToInt32(currentTilesMap.TilesMapSize.Width * this.currentScale - this.surfaceDessin.Width);

                this.vScrollBar.Minimum = -Convert.ToInt32(currentTilesMap.TilesMapSize.Height * this.currentScale - this.surfaceDessin.Height);
                this.hScrollBar.Minimum = 0;
            }
          
        }

        private void TilesMapEditor_SizeChanged(object sender, EventArgs e)
        {
            this.setScrollValue();
        }

        private void closePhysicBodyManagerBt_Click(object sender, EventArgs e)
        {
            this.mainForm.closeTilesMapEditor();
        }

        private void showTextureLayerBt_Click(object sender, EventArgs e)
        {
            this.showLayer = "TEXTURE";
            this.showTextureLayerBt.Checked = true;

            this.showAllLayersBt.Checked = false;
            this.showEventLayerBt.Checked = false;
            this.showObjectLayerBt.Checked = false;

            GorgonLibrary.Gorgon.Go();
        }

        private void showObjectLayerBt_Click(object sender, EventArgs e)
        {
            this.showLayer = "OBJECT";
            this.showObjectLayerBt.Checked = true;

            this.showAllLayersBt.Checked = false;
            this.showEventLayerBt.Checked = false;
            this.showTextureLayerBt.Checked = false;

            GorgonLibrary.Gorgon.Go();
        }

        private void showEventLayerBt_Click(object sender, EventArgs e)
        {
            this.showLayer = "EVENT";
            this.showEventLayerBt.Checked = true;

            this.showAllLayersBt.Checked = false;
            this.showObjectLayerBt.Checked = false;
            this.showTextureLayerBt.Checked = false;

            GorgonLibrary.Gorgon.Go();
        }

        private void showAllLayersBt_Click(object sender, EventArgs e)
        {
            this.showLayer = "ALL";
            this.showAllLayersBt.Checked = true;

            this.showEventLayerBt.Checked = false;
            this.showObjectLayerBt.Checked = false;
            this.showTextureLayerBt.Checked = false;

            GorgonLibrary.Gorgon.Go();
        }

        private void collisionLayerBt_Click(object sender, EventArgs e)
        {
            this.showCollision = !this.showCollision;
            this.collisionLayerBt.Checked = this.showCollision;

            GorgonLibrary.Gorgon.Go();
        }

        private void surfaceDessin_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void textureModelsPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (GorgonLibrary.Gorgon.IsInitialized == true)
                GorgonLibrary.Gorgon.Go();
        }

        private void objectModelsPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (GorgonLibrary.Gorgon.IsInitialized == true)
                GorgonLibrary.Gorgon.Go();
        }

        private void textureModelsTrackBar_Scroll(object sender, EventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private void objectModelsTrackBar_Scroll(object sender, EventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private TileModel getTileModelTouched(Point p,List<TileModel> list)
        {
            List<TileModel> modelsSelected = list;
            if (modelsSelected != null)
            {
                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    if (modelsSelected[i].isTouched(p) == true)
                        return modelsSelected[i];
                }
            }
            return null;
        }

        private void textureModelsPictBx_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.currentTilesMap != null)
            {
                this.mainForm.tilesMapEditor1.CreationMode = "CREATING_MODELS";
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point pTouched = new Point(e.Location.X, e.Location.Y - this.textureModelsTrackBar.Value);
                    this.ModelSelected = getTileModelTouched(pTouched, this.currentTilesMap.TileModelsTextureUsed);
                    if (this.ModelSelected != null)
                    {
                        if (this.ModelsSelection != null)
                        {
                            //Verifier si le control est actif : si oui ajouter a la liste 
                            if (Control.ModifierKeys == Keys.Control)
                                this.ModelsSelection.ListModelsSelected.Add(this.ModelSelected);
                            else if (Control.ModifierKeys == Keys.Shift && this.ModelsSelection.ListModelsSelected.Count > 0)
                            {
                                List<TileModel> models = this.currentTilesMap.TileModelsTextureUsed;
                                if (models != null)
                                {
                                    int indexLastModelSelected = models.IndexOf(this.ModelsSelection.ListModelsSelected[this.ModelsSelection.ListModelsSelected.Count - 1]);
                                    int indexTileModelTouched = models.IndexOf(this.ModelSelected);

                                    if (indexLastModelSelected >= 0 && indexTileModelTouched >= 0)
                                    {
                                        if (indexTileModelTouched > indexLastModelSelected)
                                        {
                                            this.ModelsSelection.ListModelsSelected.Clear();
                                            for (int i = indexLastModelSelected; i <= indexTileModelTouched; i++)
                                            {
                                                TileModel model = models[i];
                                                this.ModelsSelection.ListModelsSelected.Add(model);
                                            }
                                        }

                                    }

                                }

                            }
                            else
                            {
                                //SInn effacer la liste et ajouter le model
                                this.ModelsSelection.ListModelsSelected.Clear();
                                this.ModelsSelection.ListModelsSelected.Add(this.ModelSelected);

                            }
                        }
                    }

                    GorgonLibrary.Gorgon.Go();
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    this.textureModelsPictBx.ContextMenuStrip = this.modelsListMenuStrip;
                    this.modelsListMenuStrip.Show();
                }

            }
        }

        private void objectModelsPictBx_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.currentTilesMap != null)
            {
                this.mainForm.tilesMapEditor1.CreationMode = "CREATING_MODELS";
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point pTouched = new Point(e.Location.X, e.Location.Y - this.objectModelsTrackBar.Value);
                    this.ModelSelected = getTileModelTouched(pTouched, this.currentTilesMap.TileModelsObjectsUsed);
                    if (this.ModelSelected != null)
                    {
                        if (this.ModelsSelection != null)
                        {
                            //Verifier si le control est actif : si oui ajouter a la liste 
                            if (Control.ModifierKeys == Keys.Control)
                                this.ModelsSelection.ListModelsSelected.Add(this.ModelSelected);
                            else if (Control.ModifierKeys == Keys.Shift && this.ModelsSelection.ListModelsSelected.Count > 0)
                            {
                                List<TileModel> models = this.currentTilesMap.TileModelsObjectsUsed;
                                if (models != null)
                                {
                                    int indexLastModelSelected = models.IndexOf(this.ModelsSelection.ListModelsSelected[this.ModelsSelection.ListModelsSelected.Count - 1]);
                                    int indexTileModelTouched = models.IndexOf(this.ModelSelected);

                                    if (indexLastModelSelected >= 0 && indexTileModelTouched >= 0)
                                    {
                                        if (indexTileModelTouched > indexLastModelSelected)
                                        {
                                            this.ModelsSelection.ListModelsSelected.Clear();
                                            for (int i = indexLastModelSelected; i <= indexTileModelTouched; i++)
                                            {
                                                TileModel model = models[i];
                                                this.ModelsSelection.ListModelsSelected.Add(model);
                                            }
                                        }

                                    }

                                }

                            }
                            else
                            {
                                //SInn effacer la liste et ajouter le model
                                this.ModelsSelection.ListModelsSelected.Clear();
                                this.ModelsSelection.ListModelsSelected.Add(this.ModelSelected);

                            }
                        }
                    }

                    GorgonLibrary.Gorgon.Go();
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    this.objectModelsPictBx.ContextMenuStrip = this.modelsListMenuStrip;
                    this.modelsListMenuStrip.Show();
                }


               
            }
        }

        private void vScrollBar_Scroll(object sender, EventArgs e)
        {
            GorgonLibrary.Gorgon.Go();

            Point offsetInversed = getOffsetPoint();
            Point offSetFinal = new Point(-offsetInversed.X, -offsetInversed.Y);
            this.graduationBarX.reportOffSetScrolling(offSetFinal);
            this.graduationBarY.reportOffSetScrolling(offSetFinal);
        }


        public void refreshTextureTileModelLocations()
        {
            if (this.currentTilesMap != null)
            {
                List<TileModel> modelsSelected = this.currentTilesMap.TileModelsTextureUsed;
                if (modelsSelected != null)
                {
                    int xDest = 0;
                    int yDest = 0;
                    Size size = new Size(32, 32);

                    for (int i = 0; i < modelsSelected.Count; i++)
                    {
                        TileModel model = modelsSelected[i];
                        if (i == 0)
                            model.surfaceRect.Location = new Point(0, 0);
                        else
                        {
                            xDest = modelsSelected[i - 1].surfaceRect.Location.X + 32;
                            yDest = modelsSelected[i - 1].surfaceRect.Location.Y;

                            if (xDest + 32 >= this.textureModelsPictBx.Size.Width)
                            {
                                xDest = 0;
                                yDest += 32;
                                size = new Size(32, yDest + 32);
                            }

                            model.surfaceRect.Location = new Point(xDest, yDest);
                        }
                    }

                    if (size.Height > this.textureModelsPictBx.Height)
                    {
                        this.textureModelsTrackBar.Visible = true;
                        int maxScrollValue = size.Height - this.textureModelsPictBx.Height;
                        this.refreshTextureModelsScrollBarValues(maxScrollValue, -this.textureModelsTrackBar.Value);
                    }
                    else
                    {
                        this.refreshTextureModelsScrollBarValues(0, 0);
                        this.textureModelsTrackBar.Visible = false;

                    }

                    

                }
            }
        }

        public void refreshObjectTileModelLocations()
        {
            if (this.currentTilesMap != null)
            {
                List<TileModel> modelsSelected = this.currentTilesMap.TileModelsObjectsUsed;
                if (modelsSelected != null)
                {
                    int xDest = 0;
                    int yDest = 0;
                    Size size = new Size(32, 32);
                    for (int i = 0; i < modelsSelected.Count; i++)
                    {
                        TileModel model = modelsSelected[i];
                        if (i == 0)
                            model.surfaceRect.Location = new Point(0, 0);
                        else
                        {
                            xDest = modelsSelected[i - 1].surfaceRect.Location.X + 32;
                            yDest = modelsSelected[i - 1].surfaceRect.Location.Y;

                            if (xDest + 32 >= this.objectModelsPictBx.Size.Width)
                            {
                                xDest = 0;
                                yDest += 32;
                                size = new Size(32, yDest + 32);
                            }

                            model.surfaceRect.Location = new Point(xDest, yDest);
                        }
                    }



                    if (size.Height > this.objectModelsPictBx.Height)
                    {
                        this.objectModelsTrackBar.Visible = true;
                        int maxScrollValue = size.Height - this.objectModelsPictBx.Height;
                        this.refreshObjectModelsScrollBarValues(maxScrollValue, -this.objectModelsTrackBar.Value);
                    }
                    else
                    {
                        this.refreshObjectModelsScrollBarValues(0, 0);
                        this.objectModelsTrackBar.Visible = false;

                    }

                }
            }

        }

        private void refreshTextureModelsScrollBarValues(int maxVal, int currentValue)
        {
            this.textureModelsTrackBar.Minimum = -maxVal;
            this.textureModelsTrackBar.Maximum = 0;

            if (-currentValue > 0)
                currentValue = 0;

            if (-currentValue < this.textureModelsTrackBar.Minimum)
                currentValue = this.textureModelsTrackBar.Minimum;

            this.textureModelsTrackBar.Value = -currentValue;

            GorgonLibrary.Gorgon.Go();
        }

        private void refreshObjectModelsScrollBarValues(int maxVal, int currentValue)
        {
            this.objectModelsTrackBar.Minimum = -maxVal;
            this.objectModelsTrackBar.Maximum = 0;

            if (-currentValue > 0)
                currentValue = 0;

            if (-currentValue < this.objectModelsTrackBar.Minimum)
                currentValue = this.objectModelsTrackBar.Minimum;


            this.objectModelsTrackBar.Value = -currentValue;

            GorgonLibrary.Gorgon.Go();
        }

        public void AddTextureSequence(TileSequence seq)
        {
            if (this.currentTilesMap != null && seq.Frames.Count > 0)
            {
                this.currentTilesMap.TextureSequences.Add(seq);
                int indexImage = this.textureSequenceImageList.Images.Count;
                this.textureSequenceImageList.Images.Add(seq.Frames[0].GorgonSprite.Image.SaveBitmap());


                ListViewItem seqItem = new ListViewItem();
                seqItem.Text = seq.Name;
                seqItem.ImageIndex = indexImage;
                seqItem.Tag = seq;
                this.textureSequenceListView.Items.Add(seqItem);


            }
        }

        public void AddObjectSequence(TileSequence seq)
        {
            if (this.currentTilesMap != null && seq.Frames.Count > 0)
            {
                this.currentTilesMap.ObjectSequences.Add(seq);
                int indexImage = this.objectSequenceImageList.Images.Count;
              
                this.objectSequenceImageList.Images.Add(seq.Frames[0].GorgonSprite.Image.SaveBitmap());

                ListViewItem seqItem = new ListViewItem();
                seqItem.Text = seq.Name;
                seqItem.ImageIndex = indexImage;
                seqItem.Tag = seq;
                this.objectSequenceListView.Items.Add(seqItem);


            }
        }
        private void refreshPaletteContent()
        {
            for (int i = 0; i < this.textureSequenceImageList.Images.Count; i++)
            {
                this.textureSequenceImageList.Images[i].Dispose();
            }

            this.textureSequenceImageList.Images.Clear();
            this.textureSequenceListView.Items.Clear();
            this.selectedSequencePropGrid.SelectedObject = null;

            for (int i = 0; i < this.objectSequenceImageList.Images.Count; i++)
            {
                this.objectSequenceImageList.Images[i].Dispose();
            }

            this.objectSequenceImageList.Images.Clear();
            this.objectSequenceListView.Items.Clear();
            this.selectedSequencePropGrid.SelectedObject = null;


            if (this.currentTilesMap != null)
            {

                if (currentTilesMap.TextureSequences == null)
                    currentTilesMap.TextureSequences = new List<TileSequence>();

                if (currentTilesMap.ObjectSequences == null)
                    currentTilesMap.ObjectSequences = new List<TileSequence>();

                for (int i = 0; i < currentTilesMap.TextureSequences.Count; i++)
                {
                    TileSequence seq = currentTilesMap.TextureSequences[i];
                    int indexImage = this.textureSequenceImageList.Images.Count;
                    this.textureSequenceImageList.Images.Add(seq.Frames[0].GorgonSprite.Image.SaveBitmap());


                    ListViewItem seqItem = new ListViewItem();
                    seqItem.Text = seq.Name;
                    seqItem.ImageIndex = indexImage;
                    seqItem.Tag = seq;
                    this.textureSequenceListView.Items.Add(seqItem);

                }

                for (int i = 0; i < currentTilesMap.ObjectSequences.Count; i++)
                {
                    TileSequence seq = currentTilesMap.ObjectSequences[i];
                    int indexImage = this.objectSequenceImageList.Images.Count;
                    this.objectSequenceImageList.Images.Add(seq.Frames[0].GorgonSprite.Image.SaveBitmap());


                    ListViewItem seqItem = new ListViewItem();
                    seqItem.Text = seq.Name;
                    seqItem.ImageIndex = indexImage;
                    seqItem.Tag = seq;
                    this.objectSequenceListView.Items.Add(seqItem);

                }
            }

        }

        private void removeSelectedModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ModelsSelection != null)
            {
                TilesMap currentMap = this.ModelsSelection.TilesMapParent;
                for (int i = 0; i < this.ModelsSelection.ListModelsSelected.Count; i++)
                {
                    TileModel model = this.ModelsSelection.ListModelsSelected[i];
                    this.ModelsSelection.TilesMapParent.CleanTileModel(model, true, true, false);

                    if (currentMap.TileModelsTextureUsed.Contains(model))
                        currentMap.TileModelsTextureUsed.Remove(model);
                    else if (currentMap.TileModelsObjectsUsed.Contains(model))
                        currentMap.TileModelsObjectsUsed.Remove(model);

                    model = null;
                }

                this.ModelsSelection.ListModelsSelected.Clear();
                GorgonLibrary.Gorgon.Go();
            }
        }

        private void textureSequenceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.textureSequenceListView.SelectedItems.Count > 0)
            {
                this.CreationMode = "CREATING_TEXTURE_SEQUENCE";
                TileSequence seqSelected = (TileSequence)this.textureSequenceListView.SelectedItems[0].Tag;
                this.selectedSequencePropGrid.SelectedObject = seqSelected;
            }
        }

        private void textureSequenceListView_ItemActivate(object sender, EventArgs e)
        {
            this.CreationMode = "CREATING_TEXTURE_SEQUENCE";
        }

        private void objectSequenceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.objectSequenceListView.SelectedItems.Count > 0)
            {
                this.CreationMode = "CREATING_OBJECT_SEQUENCE";
                TileSequence seqSelected = (TileSequence)this.objectSequenceListView.SelectedItems[0].Tag;
                this.selectedSequencePropGrid.SelectedObject = seqSelected;
            }
        }

        private void objectSequenceListView_ItemActivate(object sender, EventArgs e)
        {
            this.CreationMode = "CREATING_OBJECT_SEQUENCE";
        }

        private void textureSequenceListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (this.textureSequenceListView.SelectedItems.Count > 0)
                {
                    this.textureSequenceListView.ContextMenuStrip = this.textureSequenceContextMenUStrip;
                    this.textureSequenceContextMenUStrip.Show();
                }
            }
        }
        private void objectSequenceListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (this.objectSequenceListView.SelectedItems.Count > 0)
                {
                    this.objectSequenceListView.ContextMenuStrip = this.objectSequenceMenuStrip;
                    this.objectSequenceMenuStrip.Show();
                }
            }
        }
        private void textureSequenceMenuStrip_Click(object sender, EventArgs e)
        {
            if (this.textureSequenceListView.SelectedItems.Count > 0 && this.currentTilesMap != null)
            {
                for (int i = 0; i < this.textureSequenceListView.SelectedIndices.Count; i++)
                {
                    int index = this.textureSequenceListView.SelectedIndices[i];
                    
                    TileSequence seq = this.currentTilesMap.TextureSequences[index];
                    this.currentTilesMap.ObjectSequences.Remove(seq);

                    this.currentTilesMap.TextureSequences.RemoveAt(index);
                    this.textureSequenceListView.Items.RemoveAt(index);
                    this.textureSequenceImageList.Images.RemoveAt(index);

                    for (int j = 0; j < seq.Frames.Count; j++)
                    {
                        TileModel model = seq.Frames[j];
                        this.CurrentTilesMap.CleanTileModel(model, true, true, true);
                    }
                }
            }
        }

        private void removeSelectedObjectSequenceBt_Click(object sender, EventArgs e)
        {
            if (this.objectSequenceListView.SelectedItems.Count > 0 && this.currentTilesMap != null)
            {
                for (int i = 0; i < this.objectSequenceListView.SelectedIndices.Count; i++)
                {
                    int index = this.objectSequenceListView.SelectedIndices[i];

                    TileSequence seq = this.currentTilesMap.ObjectSequences[index];
                    this.currentTilesMap.ObjectSequences.Remove(seq);
                    this.objectSequenceListView.Items.RemoveAt(index);
                    this.objectSequenceImageList.Images.RemoveAt(index);

                    for (int j = 0; j < seq.Frames.Count; j++)
                    {
                        TileModel model = seq.Frames[j];
                        this.CurrentTilesMap.CleanTileModel(model, true, true, true);
                    }

                    
                  
                }
            }
        }

       
    }
}
