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
    public partial class PivotPropertiesPanel : UserControl
    {
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        CoronaJointure joint;
        Form1 mainForm;
        public CoronaObject objA;
        public CoronaObject objB;

        public Point anchorPoint;

        private String modePanel;

        public int CreationStep;

        //---------------------------------------------------
        //-------------------Constructeurs------------------------
        //---------------------------------------------------
        public PivotPropertiesPanel()
        {
            InitializeComponent();
            this.Name = "PIVOT";
            modePanel = "NEW";

        }


        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public void init(Form1 mainForm,CoronaJointure joint)
        {
            this.joint = joint;
            this.mainForm = mainForm;
            CreationStep = 0;

            if (this.joint.type.Equals("PIVOT"))
            {
                this.isMotorEnabledChkBx.Checked = joint.isMotorEnable;
                this.isLimitedChkBx.Checked = joint.isLimitEnabled;

                this.motorSpeedNumUpDw.Value = System.Convert.ToInt32(joint.motorSpeed);
                this.maxTorqueNumUpDw.Value = System.Convert.ToInt32(joint.maxMotorTorque);

                this.nameObj1TxtBx.Text = joint.coronaObjA.DisplayObject.Name;
                this.nameObj2TxtBx.Text = joint.coronaObjB.DisplayObject.Name;

                this.upperLimitNumUpDw.Value = Convert.ToDecimal(joint.upperLimit);
                this.lowerLimitNumUpDw.Value = Convert.ToDecimal(joint.lowerLimit);

                this.objA = joint.coronaObjA;
                this.objB = joint.coronaObjB;
                this.anchorPoint = joint.AnchorPointA;
                modePanel = "MODIFY";

            }
            else
            {
                this.isMotorEnabledChkBx.Checked = false;
                this.isLimitedChkBx.Checked = false;

                this.motorSpeedNumUpDw.Value = 0;
                this.maxTorqueNumUpDw.Value = 0;

                this.upperLimitNumUpDw.Value = 0;
                this.lowerLimitNumUpDw.Value = 0;
                this.nameObj1TxtBx.Text = "";
                this.nameObj2TxtBx.Text ="";
                modePanel = "NEW";
            }

        }

        public void setAnchorPoint(Point p)
        {
            this.anchorPoint = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
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
                        else
                            this.goCreationStep3(obj);
                    }
                }
            }
            else if (this.CreationStep == 3)
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
                this.mainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT";
                CreationStep = 3;
            }
            else
            {
                MessageBox.Show("Error during joint creation ! Object B not selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep2(this.objA);
            }
        }

        private void closeCreationJoint(Point anchorPoint)
        {
            if (anchorPoint != null)
            {

                this.setAnchorPoint(anchorPoint);
                this.mainForm.sceneEditorView1.SetDefaultCursor();
                this.mainForm.guideCreationJoint.Text = "CONFIGURE & VALID!";
                CreationStep = 4;
               /* //Verifier que le point d'ancrage est bien dans les deux objets
                if (this.objA.PhysicsBody.isPointIsInBody(anchorPoint) && this.objB.PhysicsBody.isPointIsInBody(anchorPoint))
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

        //---------------------------------------------------
        //-------------------Events------------------------
        //---------------------------------------------------

        private void saveBt_Click(object sender, EventArgs e)
        {
            if (this.joint != null)
            {
                if (this.objB != null && this.objA != null && !this.anchorPoint.IsEmpty)
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

                    //Si c'est un nouveau joint
                    if (this.modePanel.Equals("NEW"))
                    {
                        this.joint.InitPivotJointure(this.objA, this.objB, this.anchorPoint, this.isMotorEnabledChkBx.Checked, this.isLimitedChkBx.Checked,
                                            System.Convert.ToDouble(this.motorSpeedNumUpDw.Value), System.Convert.ToDouble(this.maxTorqueNumUpDw.Value),
                                                  System.Convert.ToDouble(this.upperLimitNumUpDw.Value), System.Convert.ToDouble(this.lowerLimitNumUpDw.Value));




                        this.mainForm.getElementTreeView().newJoint(this.joint, true, null);

                    }
                    else
                    {
                       this.joint.isMotorEnable= this.isMotorEnabledChkBx.Checked;
                       this.joint.isLimitEnabled= this.isLimitedChkBx.Checked;

                       this.joint.motorSpeed = System.Convert.ToDouble(this.motorSpeedNumUpDw.Value);
                       this.joint.upperLimit = System.Convert.ToDouble(this.upperLimitNumUpDw.Value);
                       this.joint.lowerLimit = System.Convert.ToDouble(this.lowerLimitNumUpDw.Value);

                       this.joint.maxMotorTorque = System.Convert.ToDouble(this.maxTorqueNumUpDw.Value);
                       
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
