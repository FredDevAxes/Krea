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
    public partial class PistonPropertiesPanel : UserControl
    {
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        CoronaJointure joint;
        Form1 mainForm;
        public CoronaObject objA;
        public CoronaObject objB;

        public Point anchorPoint;
        public Point axisDistance;

        private String modePanel;

        public int CreationStep;


        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public PistonPropertiesPanel()
        {
            InitializeComponent();

            this.Name = "PISTON";
        }



        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public void init(Form1 mainForm, CoronaJointure joint)
        {
            this.joint = joint;
            this.mainForm = mainForm;
            this.modePanel = "NEW";

            if (this.joint.type.Equals("PISTON"))
            {
                this.isMotorEnabledChkBx.Checked = joint.isMotorEnable;
                this.isLimitedChkBx.Checked = joint.isLimitEnabled;

                this.motorSpeedNumUpDw.Value = System.Convert.ToInt32(joint.motorSpeed);
                this.maxMotorForceNumUpDw.Value = System.Convert.ToInt32(joint.maxMotorForce);

                this.nameObj1TxtBx.Text = joint.coronaObjA.DisplayObject.Name;
                this.nameObj2TxtBx.Text = joint.coronaObjB.DisplayObject.Name;

                this.axisDistanceTxtBx.Text = joint.axisDistance.ToString();

                this.objA = joint.coronaObjA;
                this.objB = joint.coronaObjB;

                this.anchorPoint = joint.AnchorPointA;
                this.axisDistance = joint.axisDistance;
                this.modePanel = "MODIFY";

            }

           
        }

        public void setAnchorPoint(Point p)
        {
            this.anchorPoint = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
        }

        public void setAxisDistance(Point p)
        {
            this.axisDistance = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
            this.axisDistanceTxtBx.Text = this.axisDistance.ToString();
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

            CreationStep = 1;
        }

        public void nextCreationStep(Point p)
        {
            //Verifier l'etape de creation
            //--> Recuperer l'objet A 
            if (this.CreationStep == 1 || this.CreationStep == 2)
            {
                CoronaObject obj = this.mainForm.getElementTreeView().LayerSelected.getObjTouched(p);
                if (obj != null)
                {
                    if (obj.PhysicsBody != null)
                    {
                        if (this.CreationStep == 1)
                            this.goCreationStep2(obj);
                        else if (this.CreationStep == 2)
                            this.goCreationStep3(obj);
                    }
                }
            }
            else if (this.CreationStep == 3)
            {
                this.goCreationStep4(p);
            }
            else if (this.CreationStep == 4)
            {
                closeCreationJoint(p);
            }


        }

        private void goCreationStep2(CoronaObject objA)
        {
            if (objA != null)
            {
                this.setObjectA(objA);
                this.mainForm.sceneEditorView1.SetDefaultCursor();
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
                this.mainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT INSIDE OBJECT A";
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
            if (!anchorPoint.IsEmpty)
            {
                this.setAnchorPoint(anchorPoint);
                this.mainForm.sceneEditorView1.SetEditCursor();
                this.mainForm.guideCreationJoint.Text = "SELECT AXIS DISTANCE POINT";
                CreationStep = 4;
              
                /*//Verifier que le point d'ancrage est bien dans les deux objets
                if (this.objA.PhysicsBody.isPointIsInBody(anchorPoint))
                {
                   
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the OBJ A body!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep3(this.objB);
                }*/

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep3(this.objB);
            }
        }

        private void closeCreationJoint(Point axisDistancePoint)
        {
            if (!axisDistancePoint.IsEmpty)
            {
                this.setAxisDistance(axisDistancePoint);
                this.mainForm.sceneEditorView1.SetDefaultCursor();
                this.mainForm.guideCreationJoint.Text = "CONFIGURE & VALID!";
                this.CreationStep = 5;
            }
            else
            {
                MessageBox.Show("Error during joint creation ! Point invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep4(this.anchorPoint);
            }
        }

        //---------------------------------------------------
        //-------------------Events------------------------
        //---------------------------------------------------
   
         private void saveBt_Click(object sender, EventArgs e)
        {
            if (this.joint != null)
            {
                if (this.objB != null && this.objA != null && !this.anchorPoint.IsEmpty && !this.axisDistance.IsEmpty)
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

                        this.joint.InitPistonJointure(this.objA, this.objB, this.axisDistance,this.anchorPoint,
                            this.isMotorEnabledChkBx.Checked, this.isLimitedChkBx.Checked,
                                                System.Convert.ToDouble(this.motorSpeedNumUpDw.Value), System.Convert.ToDouble(this.maxMotorForceNumUpDw.Value),
                                                      System.Convert.ToDouble(this.upperLimitNumUpDw.Value), System.Convert.ToDouble(this.lowerLimitNumUpDw.Value));


                        this.mainForm.getElementTreeView().newJoint(this.joint, true, null);
                    }
                    else
                    {
                        this.joint.isMotorEnable = this.isMotorEnabledChkBx.Checked;
                        this.joint.isLimitEnabled = this.isLimitedChkBx.Checked;

                        this.joint.motorSpeed = Convert.ToDouble(this.motorSpeedNumUpDw.Value);
                        this.joint.maxMotorForce = Convert.ToDouble(this.maxMotorForceNumUpDw.Value);

                        this.joint.upperLimit = Convert.ToDouble(this.upperLimitNumUpDw.Value);
                        this.joint.lowerLimit = Convert.ToDouble(this.lowerLimitNumUpDw.Value);
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
