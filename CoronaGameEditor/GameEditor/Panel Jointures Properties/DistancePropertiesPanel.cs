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
    public partial class DistancePropertiesPanel : UserControl
    {
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        CoronaJointure joint;
        Form1 mainForm;
        public CoronaObject objA;
        public CoronaObject objB;

        public Point anchorPointA;
        public Point anchorPointB;

        private String modePanel;

        public int CreationStep;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public DistancePropertiesPanel()
        {
            InitializeComponent();
            this.Name = "DISTANCE";
        }

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public void init(Form1 mainForm, CoronaJointure joint)
        {
            this.joint = joint;
            this.mainForm = mainForm;


            if (this.joint.type.Equals("DISTANCE"))
            {
                this.frequencyNumUpDw.Value = System.Convert.ToInt32(joint.frequency);
                this.dampingRatioNumUpDw.Value = System.Convert.ToInt32(joint.dampingRatio);

                this.nameObj1TxtBx.Text = joint.coronaObjA.DisplayObject.Name;
                this.nameObj2TxtBx.Text = joint.coronaObjB.DisplayObject.Name;

                this.objA = joint.coronaObjA;
                this.objB = joint.coronaObjB;

                this.modePanel = "MODIFY";
            }
            else
            {
                this.modePanel = "NEW";
                this.frequencyNumUpDw.Value = 0;
                this.dampingRatioNumUpDw.Value = 0;

                this.nameObj1TxtBx.Text = "";
                this.nameObj2TxtBx.Text = "";

            }
        }


        public void setAnchorPointA(Point p)
        {
            this.anchorPointA = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X,p.Y - this.objA.DisplayObject.SurfaceRect.Y);
        }

        public void setAnchorPointB(Point p)
        {
            this.anchorPointB = new Point(p.X - this.objB.DisplayObject.SurfaceRect.X, p.Y - this.objB.DisplayObject.SurfaceRect.Y);
        }


        public void setObjectA(CoronaObject obj)
        {
            this.objA = obj;
            this.nameObj1TxtBx.Text = this.objA.DisplayObject.Name;

        }

        public void setObjectB(CoronaObject obj)
        {
            this.objB = obj;
            this.nameObj2TxtBx.Text = this.objB.DisplayObject.Name;

        }


        //---------------------------------------------------
        //-------Protocole de creation  jointures------------
        //---------------------------------------------------
        public void startCreationJoint()
        {
            this.mainForm.setModeJoint();
            this.mainForm.sceneEditorView1.SetDefaultCursor();
            this.mainForm.guideCreationJoint.Text = "SELECT OBJECT A";
            this.objA = null;
            this.objB = null;

            this.anchorPointA = Point.Empty;
            this.anchorPointB = Point.Empty;

            CreationStep = 1;
        }

        public void nextCreationStep(Point p)
        {
            //Verifier l'etape de creation
            //--> Recuperer l'objet A 
            if (this.CreationStep == 1 || this.CreationStep == 2 )
            {
                CoronaObject obj = this.mainForm.getElementTreeView().LayerSelected.getObjTouched(p);
                if (obj != null)
                {
                    if (obj.PhysicsBody != null)
                    {
                        if (this.CreationStep == 1)
                            this.goCreationStep2(obj);
                        else
                            this.goCreationStep3(obj);
                    }
                }
            }
            else if (this.CreationStep == 3)
            {
                goCreationStep4(p);
            }
            else if (this.CreationStep == 4)
            {
                this.closeCreationJoint(p);
            }


        }

        private void goCreationStep2(CoronaObject objA)
        {
            if (objA != null)
            {
                this.setObjectA(objA);
                this.mainForm.guideCreationJoint.Text = "SELECT OBJECT B";
                CreationStep = 2;
            }
            else
            {
                MessageBox.Show("Error during joint creation ! Object A not selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.startCreationJoint();
            }
        }


        private void goCreationStep3(CoronaObject objB)
        {
            if (objB != null)
            {
                this.setObjectB(objB);
                this.mainForm.sceneEditorView1.SetEditCursor();
                this.mainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT A";
                CreationStep = 3;
            }
            else
            {
                MessageBox.Show("Error during joint creation ! Object B not selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep2(this.objA);
            }
        }

        private void goCreationStep4(Point anchorPoint)
        {
            if (anchorPoint != null)
            {
                this.setAnchorPointA(anchorPoint);
                this.mainForm.sceneEditorView1.SetEditCursor();
                this.mainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT B";
                CreationStep = 4;

                //Verifier que le point d'ancrage est bien dans les deux objets
              /*  if (this.objA.PhysicsBody.isPointIsInBody(anchorPoint) && this.objB.PhysicsBody.isPointIsInBody(anchorPoint))
                {
                   
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the two Bodies !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep3(this.objB);
                }*/

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep3(this.objB);
            }
        }

        private void closeCreationJoint(Point anchorPoint)
        {
            if (anchorPoint != null)
            {
                this.setAnchorPointB(anchorPoint);
                this.mainForm.sceneEditorView1.SetDefaultCursor();
                this.mainForm.guideCreationJoint.Text = "CONFIGURE AND VALID";
                CreationStep = 5;
               
                /*//Verifier que le point d'ancrage est bien dans les deux objets
                if (this.objA.PhysicsBody.isPointIsInBody(anchorPoint) && this.objB.PhysicsBody.isPointIsInBody(anchorPoint))
                {
                  
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the two Bodies !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep4(this.anchorPointA);
                }**/

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep4(this.anchorPointA);
            }
        }


        //---------------------------------------------------
        //-------------------Events------------------------
        //---------------------------------------------------

        private void saveBt_Click(object sender, EventArgs e)
        {
            if (this.joint != null)
            {
                if (this.objB != null && this.objA != null && !this.anchorPointA.IsEmpty && !this.anchorPointB.IsEmpty)
                {

                    if (this.objA != null)
                    {
                        if (this.objA.PhysicsBody.Mode == PhysicsBody.PhysicBodyMode.Inactive)
                        {
                            this.objA.PhysicsBody.Mode = PhysicsBody.PhysicBodyMode.Dynamic;
                        }

                    }

                    if (this.objB != null)
                    {
                        if (this.objB.PhysicsBody.Mode == PhysicsBody.PhysicBodyMode.Inactive)
                        {
                            this.objB.PhysicsBody.Mode = PhysicsBody.PhysicBodyMode.Dynamic;
                        }

                    }

                    if (this.modePanel.Equals("NEW"))
                    {
                        this.joint.InitDistanceJointure(this.objA, this.objB, this.anchorPointA, this.anchorPointB, 
                            System.Convert.ToDouble(this.dampingRatioNumUpDw.Value)
                            , System.Convert.ToDouble(this.frequencyNumUpDw.Value));


                        this.mainForm.getElementTreeView().newJoint(this.joint, true, null);

                    }
                    else
                    {
                        this.joint.dampingRatio = Convert.ToDouble(this.dampingRatioNumUpDw.Value);
                        this.joint.frequency =  Convert.ToDouble(this.frequencyNumUpDw.Value);


                    }
                    
                   
                    this.Dispose();
                    this.mainForm.closeJointPage();
                }
                else
                {
                    MessageBox.Show("Joint not created !\n Please check the joint properties and try again !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            else
            {
                MessageBox.Show("Joint not created !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
