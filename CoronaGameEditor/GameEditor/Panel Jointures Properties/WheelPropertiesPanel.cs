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
    public partial class WheelPropertiesPanel : UserControl
    {
        Form1 MainForm;
        CoronaJointure cJoint;

        public CoronaObject objA;
        public CoronaObject objB;

        public Point axisDistance;
        public Point anchorPoint;
        
        String modePanel;

        public int CreationStep;

        public WheelPropertiesPanel()
        {
            InitializeComponent();
            this.Name = "WHEEL";
        }

        public void Init(Form1 mainForm, CoronaJointure joint) {
            this.MainForm = mainForm;
            this.cJoint = joint;
            modePanel = "NEW";

            if (this.cJoint.type.Equals("WHEEL")) {
                this.objA = cJoint.coronaObjA;
                obj1Tb.Text = objA.DisplayObject.Name;
                this.objB = cJoint.coronaObjB;
                obj2Tb.Text = objB.DisplayObject.Name;

                this.axisDistance = cJoint.axisDistance;
                axisDistanceTb.Text = axisDistance.ToString();

                this.setAnchorPoint(cJoint.AnchorPointA);

               

                motorSpeedNup.Value = Convert.ToInt32(cJoint.motorSpeed);

                motorForceNup.Value = Convert.ToInt32( cJoint.maxMotorTorque);

                modePanel = "MODIFY";
            }
        }
        public void setAnchorPoint(Point p)
        {
            this.anchorPoint = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
            this.anchorPointTxtBx.Text = this.anchorPoint.ToString();
        }

        public void setObjectA(CoronaObject obj)
        {
            this.objA = obj;
            this.obj1Tb.Text = this.objA.DisplayObject.Name;

        }

        public void setObjectB(CoronaObject obj)
        {
            this.objB = obj;
            this.obj2Tb.Text = this.objB.DisplayObject.Name;

        }

        public void setAxisDistance(Point p)
        {
            if (p == null) return;
            this.axisDistance = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
            this.axisDistanceTb.Text = this.axisDistance.ToString();
        }

        //---------------------------------------------------
        //-------Protocole de creation  jointures------------
        //---------------------------------------------------
        public void startCreationJoint()
        {
            this.MainForm.setModeJoint();
            this.MainForm.sceneEditorView1.SetDefaultCursor();
            this.MainForm.guideCreationJoint.Text = "SELECT OBJECT A";
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
                CoronaObject obj = this.MainForm.getElementTreeView().LayerSelected.getObjTouched(p);
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
                this.MainForm.sceneEditorView1.SetDefaultCursor();
                this.MainForm.guideCreationJoint.Text = "SELECT OBJECT B";
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
                this.MainForm.sceneEditorView1.SetEditCursor();
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT INSIDE OBJECT A";
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
                //Verifier que le point d'ancrage est bien dans les deux objets
                if (this.objA.PhysicsBody.isPointIsInBody(anchorPoint))
                {
                    this.setAnchorPoint(anchorPoint);
                    this.MainForm.sceneEditorView1.SetEditCursor();
                    this.MainForm.guideCreationJoint.Text = "SELECT AXIS DISTANCE POINT";
                    CreationStep = 4;
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the OBJ A body!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep3(this.objB);
                }

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
                this.MainForm.sceneEditorView1.SetDefaultCursor();
                this.MainForm.guideCreationJoint.Text = "CONFIGURE & VALID!";
                this.CreationStep = 5;
            }
            else
            {
                MessageBox.Show("Error during joint creation ! Point invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep4(this.anchorPoint);
            }
        }

        //-------------EVENTS--------------------------------
        private void validateBt_Click(object sender, EventArgs e)
        {
            if (this.cJoint != null)
            {
                if (this.objA != null && this.objB != null && !this.axisDistance.IsEmpty && !this.anchorPoint.IsEmpty)
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

                    this.cJoint.InitWheelJointure(objA, objB,this.anchorPoint,axisDistance,this.isMotorEnableCb.Checked
                                ,Convert.ToDouble(this.motorSpeedNup.Value),Convert.ToDouble(this.motorForceNup.Value));

                    if (this.modePanel.Equals("NEW"))
                    {
                        
                        this.MainForm.getElementTreeView().newJoint(this.cJoint, true,null);
                    }

                    this.Dispose();
                    this.MainForm.closeJointPage();
                }
                else
                {
                    MessageBox.Show("Joint not created!\n Please check the joint properties and try again !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Joint not created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
