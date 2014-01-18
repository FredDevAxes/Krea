using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;

namespace Krea.Asset_Manager
{
    public partial class SpriteSheetSplitter : UserControl
    {

        public List<Bitmap> Frames;


        private Bitmap currentSpriteSheet;
        private List<Rectangle> alphaCutSelectors;
        private List<Rectangle> gridSelectors;
        private Rectangle manualSelector;
        private List<Rectangle> selectorsSelected;
        private bool isMousePressed = false;

        private Color colorToClean;
        private Point tileSizeOffset = Point.Empty;
        private Size tileSize = new Size(50, 50);

        private string mode="";

        private Point lastTouchedPoint;
        private int indexRectSelected = -1;

        public SpriteSheetSplitter()
        {
            InitializeComponent();

            this.graduationBarY.setDefaultOffset(this.graduationBarX.Size.Height);

            this.Frames = new List<Bitmap>();
            alphaCutSelectors = new List<Rectangle>();
            this.gridSelectors = new List<Rectangle>();
            this.manualSelector = new Rectangle(0, 0, 20, 20);
            selectorsSelected = new List<Rectangle>();

            this.setMode("MANUAL");
        }

        private void removeSelectedFramesBt_Click(object sender, EventArgs e)
        {
            if (this.framesListView.SelectedItems.Count > 0)
            {
                ListViewItem[] items = new ListViewItem[this.framesListView.SelectedItems.Count];
                this.framesListView.SelectedItems.CopyTo(items, 0);

                for (int i = 0; i < items.Length; i++)
                {
                    ListViewItem itemSelected = items[i];
                    Bitmap frame = itemSelected.Tag as Bitmap;
                    if (this.Frames.Contains(frame))
                        this.Frames.Remove(frame);

                    frame.Dispose();
                    frame = null;
                    itemSelected.Remove();
                }

                items = null;
            }
        }


        public void cleanAll()
        {
            this.framesImageList.Dispose();

            if (currentSpriteSheet != null)
            {
                currentSpriteSheet.Dispose();
                currentSpriteSheet = null;
            }
        }

        private void runBt_Click(object sender, EventArgs e)
        {
            this.setMode("MANUAL");
            this.sheetPictBx.Refresh();
        }

        private void alphaCutBt_Click(object sender, EventArgs e)
        {
            this.setMode("ALPHA_CUT") ;
            if (this.currentSpriteSheet != null)
            {
                this.alphaCutSelectors.Clear();


                this.alphaCutSelectors = SpriteSheetAndTextureFuncs.CutByAlpha(this.currentSpriteSheet);


                this.sheetPictBx.Refresh();
            }
        }

        private void importBt_Click(object sender, EventArgs e)
        {
            Form form = this.FindForm();
            if (form != null)
            {

                this.Frames.Clear();

                for (int i = 0; i < this.framesListView.Items.Count; i++)
                {
                    ListViewItem item = this.framesListView.Items[i];
                    Bitmap bmp = item.Tag as Bitmap;
                    if (bmp != null)
                    {
                        this.Frames.Add(bmp);
                    }

                    
                }

                this.cleanAll();

                form.DialogResult = DialogResult.OK;
                form.Close();
            }
        }

        private void sheetPictBx_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point offset = new Point(-hScrollBar1.Value, -vScrollBar1.Value);


            g.Clear(Color.LightGray);

            if (this.currentSpriteSheet != null)
            {
                Rectangle rectFinal = new Rectangle(offset, this.currentSpriteSheet.Size);
                g.FillRectangle(Brushes.White, rectFinal);
                g.DrawImage(this.currentSpriteSheet, rectFinal);

            }

            Pen p = Pens.Blue;
            if (this.mode.Equals("ALPHA_CUT"))
            {
               
                for (int i = 0; i < this.alphaCutSelectors.Count; i++)
                {
                    Rectangle manualRect = new Rectangle(new Point(this.alphaCutSelectors[i].X + offset.X,
                        this.alphaCutSelectors[i].Y + offset.Y), this.alphaCutSelectors[i].Size);

                    Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                                 20, 20);

                    g.DrawRectangle(p, manualRect);
                    g.FillRectangle(Brushes.YellowGreen, resizeRect);

                   
                   
                }
               
            }
            else if (this.mode.Equals("MANUAL"))
            {
                if (this.manualSelector.IsEmpty == false)
                {
                    Rectangle manualRect = new Rectangle(new Point(manualSelector.X + offset.X,
                        manualSelector.Y + offset.Y), manualSelector.Size);

                    Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                                 20, 20);

                    g.DrawRectangle(p, manualRect);
                    g.FillRectangle(Brushes.YellowGreen, resizeRect);

                }
            }
            else if (this.mode.Equals("TILE_SIZE"))
            {
                for (int i = 0; i < this.gridSelectors.Count; i++)
                {

                    Rectangle manualRect = new Rectangle(new Point(this.gridSelectors[i].X + offset.X,
                         this.gridSelectors[i].Y + offset.Y), this.gridSelectors[i].Size);

                    g.DrawRectangle(p, manualRect);

                    if (i == 0)
                    {
                        Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                                     20, 20);


                        g.FillRectangle(Brushes.YellowGreen, resizeRect);
                    }

                }
            }

            Pen selectedPen = new Pen(Brushes.Red, 4);
            for (int i = 0; i < this.selectorsSelected.Count; i++)
            {

                g.DrawRectangle(selectedPen, new Rectangle(new Point(this.selectorsSelected[i].X + offset.X,
                    this.selectorsSelected[i].Y + offset.Y), this.selectorsSelected[i].Size));

            }
            selectedPen.Dispose();
        }

        private void loadSheetBt_Click(object sender, EventArgs e)
        {
            this.alphaCutSelectors.Clear();
            this.gridSelectors.Clear();
            
            if (this.currentSpriteSheet != null)
            {
                this.currentSpriteSheet.Dispose();
                this.currentSpriteSheet = null;
            }

            OpenFileDialog openFileD = new OpenFileDialog();
            openFileD.Multiselect = false;
            openFileD.DefaultExt = ".png";
            openFileD.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileD.AddExtension = false;

            if (openFileD.ShowDialog() == DialogResult.OK)
            {
                string filenames = openFileD.FileName;

                
                this.currentSpriteSheet = (Bitmap) Bitmap.FromFile(filenames);

                this.hScrollBar1.Minimum = 0;
                int max = this.currentSpriteSheet.Width - this.sheetPictBx.Width;
                if(max<0) max =0;
                this.hScrollBar1.Maximum = max;

                this.vScrollBar1.Minimum = 0;
                max = this.currentSpriteSheet.Height - this.sheetPictBx.Height;
                if (max < 0) max = 0;
                this.vScrollBar1.Maximum = max;
            }

            openFileD.Dispose();

            this.sheetPictBx.Refresh();
        }

        private void sheetPictBx_SizeChanged(object sender, EventArgs e)
        {
            if (this.currentSpriteSheet != null)
            {
                this.hScrollBar1.Minimum = 0;
                int max = this.currentSpriteSheet.Width - this.sheetPictBx.Width;
                if (max < 0) max = 0;
                this.hScrollBar1.Maximum = max;

                this.vScrollBar1.Minimum = 0;
                max = this.currentSpriteSheet.Height - this.sheetPictBx.Height;
                if (max < 0) max = 0;
                this.vScrollBar1.Maximum = max;
            }

        }

        private Point getOffsetPoint()
        {
            Point p = new Point(-this.hScrollBar1.Value, -this.vScrollBar1.Value);
            return p;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Point offsetInversed = this.getOffsetPoint();
            Point offSetFinal = new Point(-offsetInversed.X, -offsetInversed.Y);

            this.graduationBarX.reportOffSetScrolling(offSetFinal);
            this.graduationBarY.reportOffSetScrolling(offSetFinal);

            this.sheetPictBx.Refresh();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Point offsetInversed = this.getOffsetPoint();
            Point offSetFinal = new Point(-offsetInversed.X, -offsetInversed.Y);

            this.graduationBarX.reportOffSetScrolling(offSetFinal);
            this.graduationBarY.reportOffSetScrolling(offSetFinal);

            this.sheetPictBx.Refresh();
        }

        private void sheetPictBx_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePressed = true;
            Point offset = this.getOffsetPoint();
            Point pTouched = new Point(e.Location.X - offset.X, e.Location.Y  - offset.Y);
            lastTouchedPoint = pTouched;

            if (mode.Equals("ALPHA_CUT") && Control.ModifierKeys == Keys.Control)
            {
                for (int i = 0; i < this.alphaCutSelectors.Count; i++)
                {
                    Rectangle rect = new Rectangle(this.alphaCutSelectors[i].X + offset.X, this.alphaCutSelectors[i].Y + offset.Y,
                        this.alphaCutSelectors[i].Width, this.alphaCutSelectors[i].Height);

                    if (rect.Contains(pTouched))
                    {
                        if (this.selectorsSelected.Contains(this.alphaCutSelectors[i]))
                            this.selectorsSelected.Remove(this.alphaCutSelectors[i]);
                        else
                            this.selectorsSelected.Add(this.alphaCutSelectors[i]);
                    }
                }
            }

            this.sheetPictBx.Refresh();
        }

        private void sheetPictBx_MouseMove(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control) return;
            if (mode.Equals("MANUAL"))
            {
                Point offset = this.getOffsetPoint();
                Point pTouched = new Point(e.Location.X - offset.X, e.Location.Y - offset.Y);

                Rectangle manualRect = this.manualSelector;

                Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                    20, 20);

                if (resizeRect.Contains(pTouched))
                {
                    this.Cursor = Cursors.SizeNWSE;

                    if (isMousePressed == true)
                    {
                        int xMove = pTouched.X - lastTouchedPoint.X;
                        int yMove = pTouched.Y - lastTouchedPoint.Y;

                        int width = this.manualSelector.Width + xMove;
                        if(width <5) width = 5;

                        int height = this.manualSelector.Height + yMove;
                        if(height <5) height = 5;


                        this.manualSelector.Size = new Size(width, height);
                        lastTouchedPoint = pTouched;
                        this.sheetPictBx.Refresh();
                    }
                }
                else if (manualRect.Contains(pTouched))
                {
                    this.Cursor = Cursors.SizeAll;

                    if (isMousePressed == true)
                    {
                        int xMove = pTouched.X - lastTouchedPoint.X;
                        int yMove = pTouched.Y - lastTouchedPoint.Y;

                        this.manualSelector.Location = new Point(this.manualSelector.Location.X + xMove,
                            this.manualSelector.Location.Y + yMove);

                        lastTouchedPoint = pTouched;
                        this.sheetPictBx.Refresh();
                    }
                }
                else
                    this.Cursor = Cursors.Arrow;
            }

            else if (mode.Equals("ALPHA_CUT"))
            {
                Point offset = this.getOffsetPoint();
                Point pTouched = new Point(e.Location.X - offset.X, e.Location.Y - offset.Y);

                if (indexRectSelected > -1 && isMousePressed == true && indexRectSelected < this.alphaCutSelectors.Count)
                {
                    Rectangle manualRect = this.alphaCutSelectors[indexRectSelected];

                    Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                        20, 20);

                    if (resizeRect.Contains(pTouched))
                    {
                        this.selectorsSelected.Clear();

                        int xMove = pTouched.X - lastTouchedPoint.X;
                        int yMove = pTouched.Y - lastTouchedPoint.Y;

                        int width = this.alphaCutSelectors[indexRectSelected].Width + xMove;
                        if (width < 5) width = 5;

                        int height = this.alphaCutSelectors[indexRectSelected].Height + yMove;
                        if (height < 5) height = 5;


                        this.alphaCutSelectors[indexRectSelected] = new Rectangle(this.alphaCutSelectors[indexRectSelected].Location, new Size(width, height));
                        lastTouchedPoint = pTouched;
                        this.sheetPictBx.Refresh();

                        return;
                    }
                    else if (manualRect.Contains(pTouched))
                    {
                        this.selectorsSelected.Clear();

                        int xMove = pTouched.X - lastTouchedPoint.X;
                        int yMove = pTouched.Y - lastTouchedPoint.Y;

                        this.alphaCutSelectors[indexRectSelected] = new Rectangle(new Point(this.alphaCutSelectors[indexRectSelected].Location.X + xMove,
                            this.alphaCutSelectors[indexRectSelected].Location.Y + yMove), this.alphaCutSelectors[indexRectSelected].Size);

                        lastTouchedPoint = pTouched;
                        this.sheetPictBx.Refresh();

                        return;
                    }

                }
                else
                {
                    for (int i = 0; i < this.alphaCutSelectors.Count; i++)
                    {
                        Rectangle manualRect = this.alphaCutSelectors[i];

                        Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                            20, 20);

                        if (resizeRect.Contains(pTouched))
                        {
                            this.Cursor = Cursors.SizeNWSE;
                            indexRectSelected = i;

                            return;
                        }
                        else if (manualRect.Contains(pTouched))
                        {
                            this.Cursor = Cursors.SizeAll;
                            indexRectSelected = i;
                            return;
                        }
                        else
                        {
                            this.Cursor = Cursors.Arrow;
                            indexRectSelected = -1;
                        }
                    }
                }
            }

            else if (mode.Equals("TILE_SIZE"))
            {
                Point offset = this.getOffsetPoint();
                Point pTouched = new Point(e.Location.X - offset.X, e.Location.Y - offset.Y);

                Point moveOffset = Point.Empty;
                Size newSize = Size.Empty;

                if (this.gridSelectors.Count > 0)
                {

                    Rectangle manualRect = this.gridSelectors[0];

                    Rectangle resizeRect = new Rectangle(manualRect.X + manualRect.Width - 10, manualRect.Y + manualRect.Height - 10,
                        20, 20);

                    if (resizeRect.Contains(pTouched))
                    {
                        this.Cursor = Cursors.SizeNWSE;

                        if (isMousePressed == true)
                        {
                            int xMove = pTouched.X - lastTouchedPoint.X;
                            int yMove = pTouched.Y - lastTouchedPoint.Y;

                            int width = this.gridSelectors[0].Width + xMove;
                            if (width < 5) width = 5;

                            int height = this.gridSelectors[0].Height + yMove;
                            if (height < 5) height = 5;


                            newSize = new Size(width, height);
                            lastTouchedPoint = pTouched;



                        }

                    }
                    else if (manualRect.Contains(pTouched))
                    {
                        this.Cursor = Cursors.SizeAll;

                        if (isMousePressed == true)
                        {
                            int xMove = pTouched.X - lastTouchedPoint.X;
                            int yMove = pTouched.Y - lastTouchedPoint.Y;

                            moveOffset = new Point(xMove, yMove);

                            lastTouchedPoint = pTouched;



                        }

                    }
                    else
                        this.Cursor = Cursors.Arrow;
                }

                if (isMousePressed == true && (moveOffset != Point.Empty || newSize != Size.Empty))
                {
                    this.tileSizeOffset = new Point(tileSizeOffset.X + moveOffset.X, tileSizeOffset.Y + moveOffset.Y);
                    int xOff = this.tileSizeOffset.X;
                    if (xOff < 0) this.tileSizeOffset = new Point(0, this.tileSizeOffset.Y);

                    int yOff = this.tileSizeOffset.Y;
                    if (yOff < 0) this.tileSizeOffset = new Point(this.tileSizeOffset.X, 0);

                    if (newSize != Size.Empty)
                        this.tileSize = newSize;

                    this.xOffsetTxtBx.Text = this.tileSizeOffset.X.ToString();
                    this.yOffsetTxtBx.Text = this.tileSizeOffset.Y.ToString();
                    this.tileWidthTxtBx.Text = this.tileSize.Width.ToString();
                    this.tileHeightTxtBx.Text = this.tileSize.Height.ToString();

                    this.gridProcessBt_Click(null, null);
                }

            }
            
        }

        private void sheetPictBx_MouseUp(object sender, MouseEventArgs e)
        {
            
            isMousePressed = false;
            indexRectSelected = -1;
            if (this.currentSpriteSheet != null)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    //Get pixel selected
                    Point offset = this.getOffsetPoint();
                    Point pTouched = new Point(e.Location.X - offset.X, e.Location.Y - offset.Y);

                    colorToClean = this.currentSpriteSheet.GetPixel(pTouched.X, pTouched.Y);
                    if (colorToClean.A >0)
                    {
                        this.colorSelectedMenuItem.BackColor = Color.FromArgb(255,colorToClean);

                        this.contextMenuStrip1.Show(this.sheetPictBx, e.Location);
                    }
                }
            }
          
        }

        private Rectangle getSelectorTouched(Point p)
        {
            for (int i = 0; i < this.alphaCutSelectors.Count; i++)
            {
                if (this.alphaCutSelectors[i].Contains(p))
                    return this.alphaCutSelectors[i];
            }

            return Rectangle.Empty;
        }

        private void importAllFramesBt_Click(object sender, EventArgs e)
        {

            if (this.mode.Equals("MANUAL"))
            {
                Bitmap bmp = this.createFrameFromRectangle(this.manualSelector);
                if (bmp == null)
                {
                    MessageBox.Show("The frame cannot be created for some reason.\nPlease check that the selector rectangle is inside the sheet and that your system has enough memory available to create the image.",
                        "A frame cannot be created", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                this.framesImageList.Images.Add(bmp);
                ListViewItem item = new ListViewItem();
                item.Tag = bmp;

                this.framesListView.Items.Add(item);
                item.ImageIndex = this.framesImageList.Images.Count - 1;


            }
            else if (this.mode.Equals("ALPHA_CUT"))
            {
                for (int i = 0; i < this.alphaCutSelectors.Count; i++)
                {
                    Bitmap bmp = this.createFrameFromRectangle(this.alphaCutSelectors[i]);
                    if (bmp == null)
                    {
                        if (MessageBox.Show("The frame " + i + " cannot be created for some reason.\nPlease check that the selector rectangle is inside the sheet and that your system has enough memory available to create the image.\n\nDo you want to continue the importation for the other frames?",
                            "A frame cannot be created, continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            continue;
                        }
                        else
                            return;
                    }

                    this.framesImageList.Images.Add(bmp);
                    ListViewItem item = new ListViewItem();
                    item.Tag = bmp;

                    this.framesListView.Items.Add(item);
                    item.ImageIndex = this.framesImageList.Images.Count - 1;
                }
            }

            else if (this.mode.Equals("TILE_SIZE"))
            {
                for (int i = 0; i < this.gridSelectors.Count; i++)
                {
                    Bitmap bmp = this.createFrameFromRectangle(this.gridSelectors[i]);
                    if (bmp == null)
                    {
                        if (MessageBox.Show("The frame " + i + " cannot be created for some reason.\nPlease check that the selector rectangle is inside the sheet and that your system has enough memory available to create the image.\n\nDo you want to continue the importation for the other frames?",
                            "A frame cannot be created, continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            continue;
                        }
                        else
                            return;
                    }

                    this.framesImageList.Images.Add(bmp);
                    ListViewItem item = new ListViewItem();
                    item.Tag = bmp;

                    this.framesListView.Items.Add(item);
                    item.ImageIndex = this.framesImageList.Images.Count - 1;
                }
            }

        }

        private void importFrameBt_Click(object sender, EventArgs e)
        {
            if (this.mode.Equals("MANUAL"))
            {
                Bitmap bmp = this.createFrameFromRectangle(this.manualSelector);
                if (bmp == null) return;

                this.framesImageList.Images.Add(bmp);
                ListViewItem item = new ListViewItem();
                item.Tag = bmp;

                this.framesListView.Items.Add(item);
                item.ImageIndex = this.framesImageList.Images.Count - 1;


            }
            else
            {
                for (int i = 0; i < this.selectorsSelected.Count; i++)
                {
                    Bitmap bmp = this.createFrameFromRectangle(this.selectorsSelected[i]);
                    if (bmp == null)
                    {
                        if (MessageBox.Show("The frame " + i + " cannot be created for some reason.\nPlease check that the selector rectangle is inside the sheet and that your system has enough memory available to create the image.\n\nDo you want to continue the importation for the other frames?",
                            "A frame cannot be created, continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            continue ;
                        }
                        else
                            return;
                    }

                    this.framesImageList.Images.Add(bmp);
                    ListViewItem item = new ListViewItem();
                    item.Tag = bmp;

                    this.framesListView.Items.Add(item);
                    item.ImageIndex = this.framesImageList.Images.Count - 1;
                }
            }

        }

        private Bitmap createFrameFromRectangle(Rectangle rect)
        {
            if (this.currentSpriteSheet != null)
            {
                if (rect.IsEmpty == false)
                {
                    try
                    {
                        Bitmap bmp = this.currentSpriteSheet.Clone(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        return bmp;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            return null;
        }
        private void gridModeBt_Click(object sender, EventArgs e)
        {
            this.setMode("TILE_SIZE");
        }

        private void setMode(string mode)
        {
            this.alphaCutSelectors.Clear();
            this.gridSelectors.Clear();
            this.selectorsSelected.Clear();
            this.mode = mode;

            if (mode.Equals("TILE_SIZE"))
            {
                 this.gridModeToolStrip.Visible = true;
                this.manualModeBt.Checked = false;
                this.gridModeBt.Checked = true;
                this.alphaCutBt.Checked = false;
            }
            else if (mode.Equals("MANUAL"))
            {
                this.gridModeToolStrip.Visible = false;
                this.manualModeBt.Checked = true;
                this.gridModeBt.Checked = false;
                this.alphaCutBt.Checked = false;

            }
            else if (mode.Equals("ALPHA_CUT"))
            {
                this.gridModeToolStrip.Visible = false;
                this.manualModeBt.Checked = false;
                this.gridModeBt.Checked = false;
                this.alphaCutBt.Checked = true;

            }

            this.sheetPictBx.Refresh();
        }

        private void gridProcessBt_Click(object sender, EventArgs e)
        {

            this.gridSelectors.Clear();

            if(this.currentSpriteSheet == null) return;

            int columnCount = -1;
            if (!this.columnsCountTxtBx.Text.Equals(""))
            {
                bool res = int.TryParse(this.columnsCountTxtBx.Text, out columnCount);
                if (res == false)
                {
                    MessageBox.Show("Cannot cut images since all the sheet details have not been set!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMousePressed = false;
                    
                    return;
                }
            }

            int lineCount = -1;
            if (!this.linesCountTxtBx.Text.Equals(""))
            {
                bool res = int.TryParse(this.linesCountTxtBx.Text, out lineCount);
                if (res == false)
                {
                    MessageBox.Show("Cannot cut images since all the sheet details have not been set!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMousePressed = false;
                    
                    return;
                }
            }


            int xOffset = -1;
            if (!this.xOffsetTxtBx.Text.Equals(""))
            {
                bool res = int.TryParse(this.xOffsetTxtBx.Text, out xOffset);
                if (res == false)
                {
                    MessageBox.Show("Cannot cut images since all the sheet details have not been set!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMousePressed = false;
                    
                    return;
                }
            }


            int yOffset = -1;
            if (!this.yOffsetTxtBx.Text.Equals(""))
            {
                bool res = int.TryParse(this.yOffsetTxtBx.Text, out yOffset);
                if (res == false)
                {
                    MessageBox.Show("Cannot cut images since all the sheet details have not been set!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMousePressed = false;
                    
                    return;
                }
            }

            int tileWidth = -1;
            if (!this.tileWidthTxtBx.Text.Equals(""))
            {
                bool res = int.TryParse(this.tileWidthTxtBx.Text, out tileWidth);
                if (res == false)
                {
                    MessageBox.Show("Cannot cut images since all the sheet details have not been set!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMousePressed = false;
                    return;
                }
            }

            int tileHeight = -1;
            if (!this.tileHeightTxtBx.Text.Equals(""))
            {
                bool res = int.TryParse(this.tileHeightTxtBx.Text, out tileHeight);
                if (res == false)
                {
                    MessageBox.Show("Cannot cut images since all the sheet details have not been set!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMousePressed = false;
                    
                    return;
                }
            }

            int totalWidth = columnCount * tileWidth + xOffset;
            bool hasSetDefaultConfig = false;
            if (totalWidth > this.currentSpriteSheet.Width)
            {
                MessageBox.Show("The width of the image sheet is too small for this configuration!\n The max config will be set!", "Configuration refused",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                isMousePressed = false;
                hasSetDefaultConfig = true;
                this.tileSizeOffset = Point.Empty;
                xOffset = 0;
                this.xOffsetTxtBx.Text = "0";

                yOffset = 0;
                this.yOffsetTxtBx.Text = "0";

                int maxTileWidth = this.currentSpriteSheet.Width / columnCount;
                int maxTileHeight = this.currentSpriteSheet.Height / lineCount;
                this.tileSize = new Size(maxTileWidth, maxTileHeight);
                tileWidth = maxTileWidth;
                tileHeight = maxTileHeight;

            }

            int totalHeight = lineCount * tileHeight + yOffset;
            if (totalHeight > this.currentSpriteSheet.Height && hasSetDefaultConfig == false)
            {
                MessageBox.Show("The height of the image sheet is too small for this configuration!\n The max config will be set!", "Configuration refused",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                isMousePressed = false;
                this.tileSizeOffset = Point.Empty;
                xOffset = 0;
                this.xOffsetTxtBx.Text = "0";

                yOffset = 0;
                this.yOffsetTxtBx.Text = "0";

                int maxTileWidth = this.currentSpriteSheet.Width / columnCount;
                int maxTileHeight = this.currentSpriteSheet.Height / lineCount;
                this.tileSize = new Size(maxTileWidth, maxTileHeight);
                tileWidth = maxTileWidth;
                tileHeight = maxTileHeight;
            }

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < lineCount; j++)
                {
                    Rectangle rect = new Rectangle(i * tileWidth + xOffset, j * tileHeight + yOffset, tileWidth, tileHeight);
                    this.gridSelectors.Add(rect);
                }
            }

            this.sheetPictBx.Refresh();
        }

        private void sheetPictBx_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void clearSelectedColorBt_Click(object sender, EventArgs e)
        {
            if (!colorToClean.IsEmpty)
                this.ClearColorInSheet(colorToClean);
        }

        private void ClearColorInSheet(Color color)
        {
            if (this.currentSpriteSheet != null)
            {

                int c = color.ToArgb();

                this.currentSpriteSheet.MakeTransparent(color);

                //for (int x = 0; x < this.currentSpriteSheet.Width; x++)
                //{
                //    for (int y = 0; y < this.currentSpriteSheet.Height; y++)
                //    {
                //        if (c.Equals(this.currentSpriteSheet.GetPixel(x, y).ToArgb()))
                //        {
                //            this.currentSpriteSheet.SetPixel(x, y, Color.Transparent);

                //            Color color2 = this.currentSpriteSheet.GetPixel(x, y);
                //        }
                //    }
                //}

                this.sheetPictBx.Refresh();
            }
        }
    }
}
