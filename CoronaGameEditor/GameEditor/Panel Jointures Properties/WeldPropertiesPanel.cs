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
    public partial class WeldPropertiesPanel : UserControl
    {
        Form1 MainForm;
        CoronaJointure cJoint;

        String modePanel;

        public int CreationStep;

        public CoronaObject objA;
        public CoronaObject objB;
        public Point anchorPoint;

        public WeldPropertiesPanel()
        {
            InitializeComponent();
            this.Name = "WELD";
        }

        public void Init(Form1 mainForm, CoronaJointure joint)
        {
            this.MainForm = mainForm;
            this.cJoint = joint;
            this.modePanel = "NEW";
            if (this.cJoint.type.Equals("WELD"))
            {
                this.obj1Tb.Text = cJoint.coronaObjA.DisplayObject.Name;
                objA = cJoint.coronaObjA;
                this.obj2Tb.Text = cJoint.coronaObjB.DisplayObject.Name;
                objB = cJoint.coronaObjB;
                this.anchorPointTb.Text = cJoint.AnchorPointA.ToString();
                anchorPoint = cJoint.AnchorPointA;
                this.modePanel = "MODIFY";
            }
        }
        public void setAnchorPoint(Point p)
        {
            if (p == null) return;
            this.anchorPoint = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
            this.anchorPointTb.Text = this.anchorPoint.ToString();
        }
        public void setObj1(CoronaObject o)
        {
            if (o == null) return;
            this.objA = o;
            this.obj1Tb.Text = this.objA.DisplayObject.Name;
        }
        public void setObj2(CoronaObject o)
        {
            if (o == null) return;
            this.objB = o;
            this.obj2Tb.Text = this.objB.DisplayObject.Name;
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
            if (this.MainForm.getElementTreeView().LayerSelected != null)
            {
                if (this.CreationStep == 1 || this.CreationStep == 2)
                {
                    CoronaObject obj = this.MainForm.getElementTreeView().LayerSelected.getObjTouched(p);
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
           


        }

        private void goCreationStep2(CoronaObject objA)
        {
            if (objA != null)
            {
                this.setObj1(objA);
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
                this.setObj2(objB);
                this.MainForm.sceneEditorView1.SetEditCursor();
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT";
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
                this.MainForm.sceneEditorView1.SetDefaultCursor();
                this.MainForm.guideCreationJoint.Text = "CONFIGURE & VALID!";
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
        private void validateBt_Click(object sender, EventArgs e)
        {
            if (this.cJoint != null)
            {
                if (this.objA != null && this.objB != null && !this.anchorPoint.IsEmpty)
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

                    this.cJoint.InitWeldJointure(objA, objB, anchorPoint);

                    if (this.modePanel.Equals("NEW"))
                    {
                        this.MainForm.getElementTreeView().newJoint(this.cJoint, true, null);
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

