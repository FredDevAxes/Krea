using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.Corona_Classes.Widgets;

namespace Krea.GameEditor.Panel_Widgets
{
    public partial class PanelTabBar : UserControl
    {
        private WidgetTabBar tabBar;
        private WidgetTabBar.TabBarButton buttonSelected;
        public PanelTabBar(WidgetTabBar tabBar)
        {
            InitializeComponent();
            this.tabBar = tabBar;


            if (this.tabBar != null)
            {
                this.init();
            }

        }

        private void init()
        {
            this.buttonSelected = null;
             this.widgetNameTxtBx.Text = tabBar.Name;

             this.buttonsListBx.Items.Clear();
             this.buttonNameTxtBx.Text = "";
             this.iconPressedPictBx.Image = null;
             this.iconReleasePictBx.Image = null;

             this.labelButtonTxtBx.Text = "";
             this.fontNameTxtBx.Text = "";
             this.fontSizeNumUpDw.Value = 10;
             this.colorButton.BackColor = Color.Black;
             this.conerRadiusUpDw.Value = 4;
             this.isSelectedChkBx.Checked = false;

             for (int i = 0; i < this.tabBar.Buttons.Count; i++)
             {
                 this.buttonsListBx.Items.Add(this.tabBar.Buttons[i]);
             }

             
        }

        private void setButtonSelected(WidgetTabBar.TabBarButton buttonSelected)
        {
            if (buttonSelected != null)
            {
                this.buttonSelected = buttonSelected;

                this.buttonNameTxtBx.Text = buttonSelected.Id;
                this.iconPressedPictBx.Image = buttonSelected.ImagePressedState;
                this.iconReleasePictBx.Image = buttonSelected.ImageUnPressedState;

                this.labelButtonTxtBx.Text = buttonSelected.Label;
                this.fontNameTxtBx.Text = buttonSelected.FontName;
                this.fontSizeNumUpDw.Value = buttonSelected.FontSize;
                this.colorButton.BackColor = buttonSelected.LabelColor;
                this.conerRadiusUpDw.Value = buttonSelected.CornerRadius;
                this.isSelectedChkBx.Checked = buttonSelected.Selected;

                if (buttonSelected.ImagePressedState != null)
                {
                    this.iconPressedWidthUpDw.Value = buttonSelected.ImagePressedState.Size.Width;
                    this.iconPressedHeightUpDw.Value = buttonSelected.ImagePressedState.Size.Height;
                }

                if (buttonSelected.ImageUnPressedState != null)
                {
                    this.iconReleasedWidthUpDw.Value = buttonSelected.ImageUnPressedState.Size.Width;
                    this.iconReleaseHeightUpDw.Value = buttonSelected.ImageUnPressedState.Size.Height;
                }

                  
            }
          
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            if (dial.ShowDialog() == DialogResult.OK)
            {
                this.colorButton.BackColor = dial.Color;
            }
        }

        private void buttonsListBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.buttonsListBx.SelectedItem != null)
            {
                WidgetTabBar.TabBarButton button = (WidgetTabBar.TabBarButton)this.buttonsListBx.SelectedItem;
                this.setButtonSelected(button);
            }
        }

        private void saveCurrentBt_Click(object sender, EventArgs e)
        {
            if (this.buttonSelected != null)
            {
                this.saveButtonProperties(this.buttonSelected);
                this.init();
            }
        }

        private void saveButtonProperties(WidgetTabBar.TabBarButton button)
        {
            if(button != null)
            {
                button.Id = this.buttonNameTxtBx.Text;
                button.ImagePressedState = this.iconPressedPictBx.Image;
                button.ImageUnPressedState = this.iconReleasePictBx.Image;
                button.Label = this.labelButtonTxtBx.Text;
                button.FontName = this.fontNameTxtBx.Text;
                button.FontSize = (int)this.fontSizeNumUpDw.Value;
                button.LabelColor = this.colorButton.BackColor;
                button.CornerRadius = (int)this.conerRadiusUpDw.Value;
                button.Selected = this.isSelectedChkBx.Checked;

                if (button.ImagePressedState != null)
                    button.ImagePressedState = new Bitmap(button.ImagePressedState, new Size((int)this.iconPressedWidthUpDw.Value, (int)this.iconPressedHeightUpDw.Value));

                if (button.ImageUnPressedState != null)
                    button.ImageUnPressedState = new Bitmap(button.ImageUnPressedState, new Size((int)this.iconReleasedWidthUpDw.Value, (int)this.iconReleaseHeightUpDw.Value));
            }
        }

        private void AddBt_Click(object sender, EventArgs e)
        {
            WidgetTabBar.TabBarButton newButton = new WidgetTabBar.TabBarButton("button" + this.tabBar.Buttons.Count, "button" + this.tabBar.Buttons.Count);
            this.saveButtonProperties(newButton);
            this.tabBar.Buttons.Add(newButton);

            this.init();
        }

        private void removeBt_Click(object sender, EventArgs e)
        {
            if (this.buttonsListBx.SelectedItem != null)
            {
                WidgetTabBar.TabBarButton button = (WidgetTabBar.TabBarButton)this.buttonsListBx.SelectedItem;
                this.tabBar.Buttons.Remove(button);

                this.init();
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (this.tabBar.Buttons.Count > 2)
            {
                if (this.buttonsListBx.SelectedIndex > 0)
                {
                    int selectedIndex = this.buttonsListBx.SelectedIndex;
                    this.tabBar.Buttons.Reverse(selectedIndex - 1, 2);

                    this.init();
                }
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (this.tabBar.Buttons.Count > 2)
            {
                if (this.buttonsListBx.SelectedIndex < this.tabBar.Buttons.Count -1)
                {
                    int selectedIndex = this.buttonsListBx.SelectedIndex;
                    this.tabBar.Buttons.Reverse(selectedIndex, 2);

                    this.init();
                }
            }
           
        }

        private void validerBt_Click(object sender, EventArgs e)
        {
            this.Parent.Dispose();
        }

        private void iconPressedPictBx_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileD = new OpenFileDialog();
            openFileD.Multiselect = false;
            openFileD.DefaultExt = ".png";
            openFileD.AddExtension = false;

            if (openFileD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(openFileD.FileName);

                    this.iconPressedPictBx.Image = img;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during image loading ! \n\n " + ex.Message);
                }
            }
        }

        private void iconReleasePictBx_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileD = new OpenFileDialog();
            openFileD.Multiselect = false;
            openFileD.DefaultExt = ".png";
            openFileD.AddExtension = false;

            if (openFileD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(openFileD.FileName);

                    this.iconReleasePictBx.Image = img;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during image loading ! \n\n " + ex.Message);
                }
            }
        }
        
    }
}
