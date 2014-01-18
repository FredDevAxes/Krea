using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
namespace Krea.GameEditor.Panel_Jointures_Properties
{
    public partial class TouchPropertiesPanel : UserControl
    {
        Form1 MainForm;
        CoronaJointure cJoint;

        CoronaObject obj;
        Point anchorPoint;

        String modePanel;

        public int CreationStep;


        public TouchPropertiesPanel()
        {
            InitializeComponent();
            this.Name = "TOUCH";
        }

        public void Init(Form1 mainForm, CoronaJointure joint ){
            this.MainForm = mainForm;
            this.cJoint = joint;
            this.modePanel="NEW";
            if(this.cJoint.type.Equals("TOUCH")){
                this.objTb.Text = cJoint.coronaObjA.DisplayObject.Name;
                this.obj = cJoint.coronaObjA;
                this.anchorPointTb.Text = cJoint.AnchorPointA.ToString();
                this.anchorPoint = cJoint.AnchorPointA;
                this.modePanel="MODIFY";
            }
        }

        public void setAnchorPoint(Point p){
            if(p==null) return;
            this.anchorPoint = new Point(p.X - this.obj.DisplayObject.SurfaceRect.X, p.Y - this.obj.DisplayObject.SurfaceRect.Y);
            this.anchorPointTb.Text = this.anchorPoint.ToString();
        }
        public void setObj(CoronaObject o){
            if(o==null) return;
            this.obj = o;
            this.objTb.Text = this.obj.DisplayObject.Name;
        }

        //---------------------------------------------------
        //-------Protocole de creation  jointures------------
        //---------------------------------------------------
        public void startCreationJoint()
        {
            this.MainForm.setModeJoint();
            this.MainForm.guideCreationJoint.Text = "SELECT OBJECT";
            this.obj = null;

            CreationStep = 1;
        }

        public void nextCreationStep(Point p)
        {
            //Verifier l'etape de creation
            //--> Recuperer l'objet A 
            if (this.CreationStep == 1)
            {
                CoronaObject obj = this.MainForm.getElementTreeView().LayerSelected.getObjTouched(p);
                if (obj != null)
                {
                    if (obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                    {
                         this.goCreationStep2(obj);
                    }
                }
            }
            else if (this.CreationStep == 2)
            {
                closeCreationJoint(p);
            }


        }

        private void goCreationStep2(CoronaObject objA)
        {
            if (objA != null)
            {
                this.setObj(objA);
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT";
                CreationStep = 2;
            }
            else
            {
                MessageBox.Show("Error during joint creation ! Object A not selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.startCreationJoint();
            }
        }

        private void closeCreationJoint(Point anchorPoint)
        {
            if (anchorPoint != null)
            {

                this.setAnchorPoint(anchorPoint);
                this.MainForm.guideCreationJoint.Text = "CONFIGURE & VALID!";
                CreationStep = 3;
               

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep2(this.obj);
            }
        }


        private void validateBt_Click(object sender, EventArgs e)
        {
            if(this.cJoint != null){
                if(this.obj != null && this.anchorPoint != null){

                    if (this.obj != null)
                    {
                        if (this.obj.PhysicsBody.Mode == PhysicsBody.PhysicBodyMode.Inactive)
                        {
                            this.obj.PhysicsBody.Mode = PhysicsBody.PhysicBodyMode.Dynamic;
                        }

                    }

                  
                    this.cJoint.InitTouchJointure(obj,anchorPoint);

                    if (this.modePanel.Equals("NEW"))
                    {
                        this.MainForm.getElementTreeView().newJoint(this.cJoint, true, null);
                    }


                   
                    this.Dispose();
                    this.MainForm.closeJointPage();
                }
                else{
                    MessageBox.Show("Joint not created!\n Please check the joint properties and try again !", "Error", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
            else{
                    MessageBox.Show("Joint not created!", "Error", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
    }
}
