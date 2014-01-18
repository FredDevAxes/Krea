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
    public partial class PulleyPropertiesPanel : UserControl
    {
        Form1 MainForm;
        CoronaJointure cJoint;

        public CoronaObject objA;
        public CoronaObject objB;
        public Point anchorPointA;
        public Point anchorPointB;

        public Point objAnchorPointA;
        public Point objAnchorPointB;

        String modePanel;

        public int CreationStep;

        public PulleyPropertiesPanel()
        {
            InitializeComponent();
            this.Name = "PULLEY"; 
        }

        public void Init(Form1 mainForm, CoronaJointure joint){
            this.MainForm = mainForm;
            this.cJoint = joint;
            this.modePanel = "NEW";
            if (this.cJoint.type.Equals("PULLEY"))
            {
                setObjectA(cJoint.coronaObjA);
                setObjectB(cJoint.coronaObjB);

                setAnchorPointA(cJoint.AnchorPointA);
                setAnchorPointB(cJoint.AnchorPointB);

                setObjAnchorPointA(cJoint.ObjectAnchorPointA);
                setObjAnchorPointB(cJoint.ObjectAnchorPointB);

                this.modePanel = "MODIFY";
            }
        }
        public void setAnchorPointA(Point p)
        {
            if (p == null) return;
            this.anchorPointA = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
            this.anchorPoint1Tb.Text = this.anchorPointA.ToString();
        }

        public void setObjectA(CoronaObject o)
        {
            if (o == null) return;
            this.objA = o;
            this.Obj1Tb.Text = this.objA.DisplayObject.Name;
        }
        public void setAnchorPointB(Point p)
        {
            if (p == null) return;
            this.anchorPointB = new Point(p.X - this.objB.DisplayObject.SurfaceRect.X, p.Y - this.objB.DisplayObject.SurfaceRect.Y);
            this.anchorPoint2Tb.Text = this.anchorPointB.ToString();
        }
        public void setObjectB(CoronaObject o)
        {
            if (o == null) return;
            this.objB = o;
            this.Obj2Tb.Text = this.objB.DisplayObject.Name;
        }
        public void setObjAnchorPointA(Point p)
        {
            if (p == null) return;
            this.objAnchorPointA = new Point(p.X - this.objA.DisplayObject.SurfaceRect.X, p.Y - this.objA.DisplayObject.SurfaceRect.Y);
            this.objAAnchorPointTxtBx.Text = this.objAnchorPointA.ToString();
        }
        public void setObjAnchorPointB(Point p)
        {
            if (p == null) return;
            this.objAnchorPointB = new Point(p.X - this.objB.DisplayObject.SurfaceRect.X, p.Y - this.objB.DisplayObject.SurfaceRect.Y);
            this.objBAnchorPointTxtBx.Text = this.objAnchorPointB.ToString();
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

            this.anchorPointA = Point.Empty;
            this.anchorPointB = Point.Empty;

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
                goCreationStep5(p);
            }
            else if (this.CreationStep == 5)
            {
                goCreationStep6(p);
            }
            else if (this.CreationStep == 6)
            {
                this.closeCreationJoint(p);
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
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT A";
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
                this.setAnchorPointA(anchorPoint);
                this.MainForm.sceneEditorView1.SetEditCursor();
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT B";
                CreationStep = 4;

             /*   //Verifier que le point d'ancrage est bien dans les deux objets
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

        private void goCreationStep5(Point anchorPoint)
        {
            if (!anchorPoint.IsEmpty)
            {
                this.setAnchorPointB(anchorPoint);
                this.MainForm.sceneEditorView1.SetEditCursor();
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT INSIDE THE OBJECT A BODY";
                CreationStep = 5;

                //Verifier que le point d'ancrage est bien dans les deux objets
               /* if (this.objA.PhysicsBody.isPointIsInBody(anchorPoint) && this.objB.PhysicsBody.isPointIsInBody(anchorPoint))
                {
                  
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the two Bodies !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep4(this.anchorPointA);
                }*/

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep4(this.anchorPointA);
            }
        }

        private void goCreationStep6(Point anchorPointObjA)
        {
            if (!anchorPointObjA.IsEmpty)
            {

                this.setObjAnchorPointA(anchorPointObjA);
                this.MainForm.sceneEditorView1.SetEditCursor();
                this.MainForm.guideCreationJoint.Text = "SELECT ANCHOR POINT INSIDE THE OBJECT B BODY";
                CreationStep = 6;
               
                /*//Verifier que le point d'ancrage est bien dans les deux objets
                if (this.objA.PhysicsBody.isPointIsInBody(anchorPointObjA))
                {
                
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the object A body !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep5(this.anchorPointB);
                }*/

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep5(this.anchorPointB);
            }

        }

        private void closeCreationJoint(Point anchorPointObjB)
        {
            if (!anchorPointObjB.IsEmpty)
            {
                this.setObjAnchorPointB(anchorPointObjB);
                this.MainForm.sceneEditorView1.SetDefaultCursor();
                this.MainForm.guideCreationJoint.Text = "READY FOR VALIDATION!";
                CreationStep = 7;

               /* //Verifier que le point d'ancrage est bien dans les deux objets
                if (this.objA.PhysicsBody.isPointIsInBody(anchorPointObjB))
                {
                    
                }
                else
                {
                    MessageBox.Show("Error during joint creation ! Anchor point is not contained inside the object B body !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goCreationStep6(this.objAnchorPointA);
                }*/

            }
            else
            {
                MessageBox.Show("Error during joint creation ! Anchor point Invalid !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                goCreationStep6(this.objAnchorPointA);
            }
        }


        //---------------------EVENT-------------------------------------------------------
        private void validateBt_Click(object sender, EventArgs e)
        {
            if (this.cJoint != null)
            {
                if (this.objA != null && this.objB != null && !this.anchorPointA.IsEmpty && !this.anchorPointB.IsEmpty
                            && !this.objAnchorPointA.IsEmpty && !this.objAnchorPointB.IsEmpty)
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

                    this.cJoint.InitPulleyJointure(objA,objB,this.objAnchorPointA,this.objAnchorPointB, anchorPointA, anchorPointB);

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
