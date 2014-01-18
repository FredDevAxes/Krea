using System.Collections.Generic;
using System.Drawing;

namespace Krea.GameEditor.TilesMapping
{
    public class TilesSelection
    {
        //---------------------------------------------------
        //-------------------Attributes----------------------
        //---------------------------------------------------
        public Rectangle SelectionRect;
        public TileModel[,] TilesSelected;
        public Point LocationToApply;
        public TilesMap TilesMapParent;
        public List<TileModel> ListModelsSelected;
        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public TilesSelection(TilesMap tilesMapParent)
        {
            this.TilesMapParent = tilesMapParent;
            ListModelsSelected = new List<TileModel>();
        }

        //---------------------------------------------------
        //-------------------Methods-------------------------
        //---------------------------------------------------
        public void drawSelection(Graphics g)
        {
            if (this.SelectionRect != null && this.TilesSelected != null && this.LocationToApply != null)
            {
                int nbTilesByLine = getNbTilesByLine();
                int nbTilesByColumn = getNbTilesByColumn();

                for (int i = 0; i < nbTilesByLine; i++)
                {
                    for (int j = 0; j < nbTilesByColumn; j++)
                    {
                        TileModel model = this.TilesSelected[i,j];
                        Bitmap bmp = model.Image;
                        int xDest = this.LocationToApply.X + i * 32;
                        int yDest  = this.LocationToApply.Y + j * 32;
                        g.DrawImage(bmp, new Rectangle(new Point(xDest, yDest), model.surfaceRect.Size));
                    }
                }
            }
        }

        public void applySelectionToMap()
        {
            if (this.SelectionRect != null && this.TilesSelected != null && this.LocationToApply != null && this.TilesMapParent != null)
            {
                int nbTilesByLine = getNbTilesByLine();
                int nbTilesByColumn = getNbTilesByColumn();

                for (int i = 0; i < nbTilesByLine; i++)
                {
                    for (int j = 0; j < nbTilesByColumn; j++)
                    {
                        TileModel model = this.TilesSelected[i,j];
                        //definir le point visé
                        int xDest = this.LocationToApply.X + i * 32;
                        int yDest  = this.LocationToApply.Y + j * 32;

                        this.TilesMapParent.applyTextureOnTilePointed(new Point(xDest, yDest), model);
                    }
                }
            }
        }

        public void applyListModelToMap()
        {

        }

        public int getNbTilesByLine()
        {
            if (this.SelectionRect != null && TilesMapParent != null)
            {
                return this.SelectionRect.Width / this.TilesMapParent.TilesWidth;
            }
            return -1;
        }

        public int getNbTilesByColumn()
        {
            if (this.SelectionRect != null && TilesMapParent != null)
            {
                return this.SelectionRect.Height / this.TilesMapParent.TilesHeight;
            }
            return -1;
        }

    }
}
