using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Krea.CoronaClasses;

namespace Krea.GameEditor
{
    public partial class ScenePreview : UserControl
    {
        private Form1 mainForm;
        public Graphics GraphicsToDraw;

        private float currentReverseXScale;
        private float currentReverseYScale;
        private bool isMousePressed = false;
        public float CurrentScale;
        public ScenePreview()
        {
            InitializeComponent();
        }

        public void init(Form1 mainForm)
        {
            this.mainForm = mainForm;

        }


        private Point getOffsetPoint()
        {
            return new Point(0,0);

        }
        private void previewPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (GorgonLibrary.Gorgon.IsInitialized == true)
                GorgonLibrary.Gorgon.Go();
            //try
            //{
     
            //    if (this.mainForm != null && this.mainForm.CurrentProject != null)
            //    {
            //         e.Graphics.Clear(Color.Black);
            //        Scene scene = this.mainForm.getElementTreeView().SceneSelected;
            //        if (scene != null)
            //        {
            //            float currentScale = 10;
            //            bool nextScale = true;

            //            float previewWidth = (float)this.previewPictBx.Width;
            //            float previewHeight = (float)this.previewPictBx.Height;

            //            while (nextScale == true)
            //            {
            //                float sceneWidthScaled = scene.Size.Width * currentScale;
            //                float sceneHeightScaled = scene.Size.Height * currentScale;
            //                if (sceneWidthScaled <= previewWidth && sceneHeightScaled <= previewHeight)
            //                {
            //                    nextScale = false;
            //                }
            //                else
            //                {
            //                    currentScale = currentScale /2;
            //                }
            //            }

            //            float xScale = currentScale;
            //            float yScale = currentScale;

            //            e.Graphics.FillRectangle(Brushes.AliceBlue, new Rectangle(new Point(0, 0), new Size((int)(scene.Size.Width * currentScale), (int)(scene.Size.Height * currentScale))));

            //            for (int i = 0; i < scene.Layers.Count; i++)
            //            {
            //                CoronaLayer layer = scene.Layers[i];
            //                Point pDest = new Point(0, 0);
            //                if (layer.TilesMap != null && this.showTilesMapChkBx.Checked == true)
            //                {
                               
            //                   Rectangle dispRect = new Rectangle(pDest, this.previewPictBx.Size);
            //                    layer.TilesMap.setSurfaceVisible(dispRect, xScale, yScale);
            //                    layer.TilesMap.DrawTilesInEditor(e.Graphics, xScale, yScale, pDest,"ALL",false);
                               
            //                    layer.TilesMap.resetSurfaceVisible();
                                
                                
            //                }
            //                for (int j = 0; j < layer.CoronaObjects.Count; j++)
            //                {
            //                    if (layer.CoronaObjects[j].isEntity == false)
            //                    {
            //                        layer.CoronaObjects[j].DisplayObject.dessineAt(e.Graphics, pDest,
            //                            true, null, xScale, yScale);
            //                    }
            //                    else
            //                    {
            //                        for (int k = 0; k < layer.CoronaObjects[j].Entity.CoronaObjects.Count; k++)
            //                        {
            //                            CoronaObject child = layer.CoronaObjects[j].Entity.CoronaObjects[k];
            //                            child.DisplayObject.dessineAt(e.Graphics, pDest, true, null, xScale, yScale);
            //                        }
            //                    }
            //                }

            //                for (int j = 0; j < layer.Widgets.Count; j++)
            //                {
            //                    layer.Widgets[j].Dessine(e.Graphics, new Point(0, 0), xScale, yScale);
            //                }
            //            }

            //            e.Graphics.ResetTransform();
            //            //Draw the view of the editor
            //            SolidBrush br = new SolidBrush(Color.FromArgb(150,Color.Black));

            //            GraphicsPath path = new GraphicsPath();
            //            path.AddRectangle(new Rectangle(new Point(0,0), this.previewPictBx.Size));

            //            Point offSet = this.mainForm.sceneEditorView1.getOffsetPoint();
            //            Size size = this.mainForm.sceneEditorView1.surfacePictBx.Size;
            //            Point pDestFocus = new Point((int)(-offSet.X * xScale), (int)(-offSet.Y * yScale));
            //            path.AddRectangle(new Rectangle(pDestFocus, new Size((int)(size.Width * xScale * (1 / this.mainForm.sceneEditorView1.CurrentScale)), (int)(size.Height * yScale * (1 / this.mainForm.sceneEditorView1.CurrentScale)))));
                        
            //            e.Graphics.FillPath(br, path);

            //        }

                    
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error during stage preview painting!\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);


            //}
        }

        private void previewPictBx_MouseDown(object sender, MouseEventArgs e)
        {
            this.isMousePressed = true;

            Scene scene = this.mainForm.getElementTreeView().SceneSelected;
            if (scene != null)
            {
                float xScale = (float)(Convert.ToDouble(this.previewPictBx.Width) /
                                     Convert.ToDouble(scene.Size.Width * (1 / this.mainForm.sceneEditorView1.CurrentScale)));
                float yScale = (float)(Convert.ToDouble(this.previewPictBx.Height) /
                    Convert.ToDouble(scene.Size.Height * (1 / this.mainForm.sceneEditorView1.CurrentScale)));

                Size size = this.mainForm.sceneEditorView1.surfacePictBx.Size;
                SizeF sizeSceneScaled = new SizeF(scene.Size.Width * xScale, scene.Size.Height * yScale);

                Size focusSize = new Size((int)(size.Width * this.CurrentScale), (int)(size.Height * this.CurrentScale));


                double reverseX = ((e.Location.X) * (1 / this.CurrentScale)) - (focusSize.Width * (1 / this.mainForm.sceneEditorView1.CurrentScale));
                double reverseY = ((e.Location.Y) * (1 / this.CurrentScale)) - (focusSize.Height * (1 / this.mainForm.sceneEditorView1.CurrentScale));

                int X = Convert.ToInt32(reverseX);
                int Y = Convert.ToInt32(reverseY);
                this.mainForm.sceneEditorView1.scrollView(X, Y);

              
            }
           

        }

        private void previewPictBx_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMousePressed == true)
            {
                 Scene scene = this.mainForm.getElementTreeView().SceneSelected;
                 if (scene != null)
                 {

                     float xScale = (float)(Convert.ToDouble(this.previewPictBx.Width) /
                                     Convert.ToDouble(scene.Size.Width * (1/this.mainForm.sceneEditorView1.CurrentScale)));
                     float yScale = (float)(Convert.ToDouble(this.previewPictBx.Height) /
                         Convert.ToDouble(scene.Size.Height * (1 / this.mainForm.sceneEditorView1.CurrentScale)));

                     Size size = this.mainForm.sceneEditorView1.surfacePictBx.Size;
                     SizeF sizeSceneScaled = new SizeF(scene.Size.Width * xScale, scene.Size.Height * yScale);

                     Size focusSize = new Size((int)(size.Width * this.CurrentScale), (int)(size.Height * this.CurrentScale));


                     double reverseX = ((e.Location.X) * (1 / this.CurrentScale)) - (focusSize.Width  * (1 / this.mainForm.sceneEditorView1.CurrentScale));
                     double reverseY = ((e.Location.Y) * (1 / this.CurrentScale)) - (focusSize.Height  * (1 / this.mainForm.sceneEditorView1.CurrentScale));
                     
                     int X = Convert.ToInt32(reverseX);
                     int Y = Convert.ToInt32(reverseY);
                     this.mainForm.sceneEditorView1.scrollView(X, Y);

                     //float xScale = (float)(Convert.ToDouble(this.previewPictBx.Width) /
                     //                Convert.ToDouble(scene.Size.Width));
                     //float yScale = (float)(Convert.ToDouble(this.previewPictBx.Height) /
                     //    Convert.ToDouble(scene.Size.Height));

                     //Size size = this.mainForm.sceneEditorView1.surfacePictBx.Size;
                     //SizeF sizeSceneScaled = new SizeF(scene.Size.Width * xScale, scene.Size.Height * yScale);

                     //Size focusSize = new Size((int)(size.Width * xScale * (1 / this.mainForm.sceneEditorView1.CurrentScale)), (int)(size.Height * yScale * (1 / this.mainForm.sceneEditorView1.CurrentScale)));

                     //double reverseX = Convert.ToDouble((((e.Location.X*xScale) - (this.previewPictBx.Width - sizeSceneScaled.Width)) - (focusSize.Width / 2)) * currentReverseXScale);
                     //double reverseY = Convert.ToDouble((((e.Location.Y/yScale) - (this.previewPictBx.Height - sizeSceneScaled.Height)) - (focusSize.Height / 2)) * currentReverseYScale);
                     //int X = Convert.ToInt32(reverseX);
                     //int Y = Convert.ToInt32(reverseY);
                     //if (X < 0) X = 0;
                     //if (Y < 0) Y = 0;
                     //if (X > this.mainForm.sceneEditorView1.hScrollBar1.Maximum) X = this.mainForm.sceneEditorView1.hScrollBar1.Maximum;
                     //if (Y > this.mainForm.sceneEditorView1.vScrollBar1.Maximum) Y = this.mainForm.sceneEditorView1.vScrollBar1.Maximum;

                     //this.mainForm.sceneEditorView1.hScrollBar1.Value = X;
                     //this.mainForm.sceneEditorView1.vScrollBar1.Value = Y;

                     //this.mainForm.sceneEditorView1.RefreshScrollView();


                 }
            }
        }

        private void previewPictBx_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private void showTilesMapChkBx_CheckedChanged(object sender, EventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private void ScenePreview_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }



    }
}
