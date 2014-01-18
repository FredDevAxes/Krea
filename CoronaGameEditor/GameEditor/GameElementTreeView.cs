using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Krea.CoronaClasses;
using Krea.GameEditor.PropertyGridConverters;
using Krea.GameEditor.TilesMapping;
using Krea.GameEditor.PropertyGridConverters.JointsPropertyConverters;
using Krea.Corona_Classes.Controls;
using Krea.Corona_Classes.Widgets;
using Krea.Corona_Classes;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using Krea.GameEditor.FontManager;
using Krea.GameEditor;

namespace Krea
{
    public partial class GameElementTreeView : UserControl
    {

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam,
                                                 ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        private void HideCheckBox(TreeView tvw, TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

         //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public TreeNode ProjectRootNodeSelected;
        public CoronaGameProject ProjectSelected;
        public Scene SceneSelected;
        public CoronaLayer LayerSelected;
        private bool hasChecked = false;
        public CoronaJointure JointureSelected;
        public FontItem FontSelected;
        public CoronaObject CoronaObjectSelected;
        public CoronaControl ControlSelected;
        public AudioObject AudioObjectSelected;
        public CoronaWidget WidgetSelected;
        public Snippet SnippetSelected;

        public Form1 MainForm;
        private GameElement nodeTemp;
        private bool isRemovingNode = false;
        public List<TreeNode> SelectedNodes;
        protected TreeNode m_lastNode, m_firstNode;
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public GameElementTreeView()
        {
            InitializeComponent();

            SelectedNodes = new List<TreeNode>();
            
            
        }

        public void init(Form1 mainForm)
        {
            this.MainForm = mainForm;


            

        }

        public void clearTreeView()
        {
            this.LayerSelected = null;
            this.SceneSelected = null;
            CoronaObjectSelected = null;
            this.JointureSelected = null;
            this.nodeTemp = null;
            this.treeViewElements.Nodes.Clear();
        }

    
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        //----------------------------------------------------------
        //----------------Project-----------------------
        //----------------------------------------------------------

        public void loadProject(CoronaGameProject project)
        {
            if (project != null)
            {
                // FROM KREA 1.3.9 TO KREA 1.3.10
                initALLProjectEnabledState(project);

                for (int i = 0; i < project.Scenes.Count; i++)
                {
                    Scene scene = project.Scenes[i];
                    if (scene.Camera == null)
                    {
                        scene.Camera = new Corona_Classes.Camera(scene, scene.SurfaceFocus, scene.CameraFollowLimitRectangle);
                        if (scene.objectFocusedByCamera != null)
                            scene.Camera.setObjectFocusedByCamera(scene.objectFocusedByCamera);
                        scene.Camera.isSurfaceFocusVisible = scene.isSurfaceFocusVisible;

                    }

                    if (scene.CollisionFilterGroups == null)
                    {
                        scene.initCollisionGroupFilter();
                    }
                }

                this.treeViewElements.Nodes.Clear();
                this.treeViewElements.BeginUpdate();
                //Creer une node pour le project---------------------------------------------
                TreeNode nodeProject = new TreeNode(project.ProjectName);
              

                nodeProject.Tag = project;
                nodeProject.Name = "PROJECT";
                nodeProject.ImageIndex = 6;
                nodeProject.SelectedImageIndex = 6;
                this.treeViewElements.Nodes.Add(nodeProject);
                this.HideCheckBox(this.treeViewElements, nodeProject);
                this.ProjectRootNodeSelected = nodeProject;


                //Recreer les scenes
                for (int i = 0; i < project.Scenes.Count; i++)
                {
                    this.newScene(project.Scenes[i]);
                }

                //Creer une node pour les audios--------------------------------------------------
                TreeNode nodeAudios = new TreeNode("Audios");
               
                nodeAudios.Tag = project.AudioObjects;
                nodeAudios.Name = "AUDIOS";
                nodeAudios.ImageIndex = 0;
                nodeAudios.SelectedImageIndex = 0;
                nodeProject.Nodes.Add(nodeAudios);
                this.HideCheckBox(this.treeViewElements, nodeAudios);
                //Recreer les Sons
                for (int i = 0; i < project.AudioObjects.Count; i++)
                {
                    this.newAudioObject(project.AudioObjects[i]);
                }

                //Creer une node pour les Fonts--------------------------------------------------
                TreeNode nodeFonts = new TreeNode("Fonts");
              
                nodeFonts.Tag = project.AudioObjects;
                nodeFonts.Name = "FONTS";
                nodeFonts.ImageIndex = 13;
                nodeFonts.SelectedImageIndex = 13;
                nodeProject.Nodes.Add(nodeFonts);
                this.HideCheckBox(this.treeViewElements, nodeFonts);
                //Recreer les Sons
                for (int i = 0; i < project.AvailableFont.Count; i++)
                {
                    FontItem font =  project.AvailableFont[i];
                    string fileNameDestFont = project.SourceFolderPath +"\\"+font.NameForAndroid+".ttf";
                    if (File.Exists(fileNameDestFont))
                    {
                        bool res = font.InitFont(font.NameForAndroid, fileNameDestFont);
                        
                    }
                    else
                    {
                        MessageBox.Show("Could not found the True Type Font file \"" + font.NameForAndroid + "\" at location \""+fileNameDestFont+"\" !\n Please copy the correct font file to your project source folder: \"" + project.SourceFolderPath +"\" then install your font before restarting Krea!",
                            "Warning", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                     this.newFontItem(project.AvailableFont[i]);
                    
                }
               /* //Creer une node pour les langues-------------------------------------------------------
                TreeNode nodeLangues = new TreeNode("Languages");
                nodeLangues.Tag = project.Langues;
                nodeLangues.Name = "LANGUES";
                nodeLangues.ImageIndex = 1;
                nodeLangues.SelectedImageIndex = 1;
                nodeProject.Nodes.Add(nodeLangues);*/

                //Recreer les Sons
                /*for (int i = 0; i < project.Langues.Count; i++)
                {
                    //   this.newLangue(project.Langues[i]);
                }*/


                 //Creer une node pour les langues-------------------------------------------------------
                TreeNode nodeSnippets = new TreeNode("Snippets");
                nodeSnippets.Tag = project.Snippets;
                nodeSnippets.Name = "SNIPPETS";
                nodeSnippets.ImageIndex = 11;
                nodeSnippets.SelectedImageIndex = 11;
                nodeProject.Nodes.Add(nodeSnippets);
                this.HideCheckBox(this.treeViewElements, nodeSnippets);
                //Recreer les Sons
                for (int i = 0; i < project.Snippets.Count; i++)
                {
                    this.newSnippet(project.Snippets[i]);
                }


                //Selectionner la node project
                this.ProjectRootNodeSelected = nodeProject;
                this.ProjectSelected = project;

                //Ouvrir toutes les nodes
                this.ProjectRootNodeSelected.Expand();

                this.treeViewElements.EndUpdate();
            }
            else
            {
                MessageBox.Show("The project file seems to be corrupted ! \n Loading Failed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //----------------------------------------------------------
        //----------------SCENE-----------------------
        //----------------------------------------------------------
        public void newScene(Scene scene)
        {
            if (this.ProjectRootNodeSelected != null)
            {
                this.SceneSelected = scene;
                
                GameElement node = new GameElement("STAGE", scene.Name, scene);
                node.ExpandAll();
                this.ProjectRootNodeSelected.Nodes.Insert(scene.projectParent.Scenes.Count, node);

                if (scene.projectParent.isEnabled == false)
                {
                    scene.isEnabled = true;
                }

                node.Checked = scene.isEnabled;
              /*  //Ajouter une node pour les spritesheets  ---------------------------------------------
                TreeNode nodeSpriteSheets = new TreeNode("SpriteSheets");
                nodeSpriteSheets.Name = "SPRITESHEETS";
                nodeSpriteSheets.ImageIndex = 5;
                nodeSpriteSheets.SelectedImageIndex = 5;
                node.Nodes.Add(nodeSpriteSheets);

                for (int i = 0; i < scene.SpriteSheets.Count; i++)
                {
                    CoronaSpriteSheet sheet = scene.SpriteSheets[i];
                    GameElement nodeSheet = new GameElement("SPRITESHEET", sheet.Name, sheet);
                    nodeSpriteSheets.Nodes.Add(nodeSheet);
                }

                //Ajouter une node pour les spriteSets ---------------------------------------------
                TreeNode nodeSpriteSets = new TreeNode("SpriteSets");
                nodeSpriteSets.Name = "SPRITESETS";
                nodeSpriteSets.ImageIndex = 4;
                nodeSpriteSets.SelectedImageIndex = 4;
                node.Nodes.Add(nodeSpriteSets);

                for (int i = 0; i < scene.SpriteSets.Count; i++)
                {
                    CoronaSpriteSet set = scene.SpriteSets[i];
                    GameElement nodeSet = new GameElement("SPRITESET", set.Name, set);
                    nodeSpriteSets.Nodes.Add(nodeSet);
                }*/

                //Recreer les layer
                for (int i = 0; i < scene.Layers.Count; i++)
                {
                    if(scene.Layers[i].Entities == null)
                        scene.Layers[i].Entities = new List<CoronaEntity>();

                    this.newLayer(scene, scene.Layers[i]);
                }

                this.treeViewElements_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, 0, 0));

            }

        }

        public void removeScene(GameElement elem)
        {
            //Recuperer la scene
            Scene scene = (Scene)elem.InstanceObjet;
            if (scene != null)
            {
                this.MainForm.CurrentProject.Scenes.Remove(scene);

                if (this.SelectedNodes.Contains(elem))
                    this.SelectedNodes.Remove(elem);

                isRemovingNode = true;
                elem.Remove();
                isRemovingNode = false;
            }

            this.SceneSelected = null;
            GorgonLibrary.Gorgon.Go();
        }

        public Scene getSceneFromNode(GameElement node)
        {
            if (node.InstanceObjet != null)
                return (Scene)node.InstanceObjet;
            else return null;
        }

        public TreeNode getRootNode(TreeNode node)
        {
            if (node.Parent != null)
            {
                return getRootNode(node.Parent);
            }
            else
            {
                return node;
            }

        }

        //----------------------------------------------------------
         //----------------LAYER-----------------------
        //----------------------------------------------------------
        public void newLayer(Scene scene,CoronaLayer layer)
        {
            this.LayerSelected = layer;
            this.CoronaObjectSelected = null;
            this.LayerSelected.deselectAllObjects();
            GameElement node = new GameElement("LAYER",layer.Name, layer);
            if (scene.projectParent.isEnabled == false)
            {
                layer.isEnabled = true;
            }

            node.Checked = layer.isEnabled;

            GameElement sceneNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, scene);
            if (sceneNode != null)
            {
                
                sceneNode.Nodes.Add(node);

                //Ajouter les jointures
                TreeNode nodeJoints = new TreeNode("Joints");
            
                nodeJoints.Name = "JOINTS";
                nodeJoints.ImageIndex = 8;
                nodeJoints.SelectedImageIndex = 8;
                node.Nodes.Add(nodeJoints);
                this.HideCheckBox(this.treeViewElements, nodeJoints);
                for (int i = 0; i < layer.Jointures.Count; i++)
                {
                    this.newJoint(layer.Jointures[i], false, null);
                }

                //Ajouter une tiles map si existante
                if (layer.TilesMap != null)
                {
                    layer.TilesMap.LayerParent = layer;
                    this.newTilesMap(layer.TilesMap);

                }

                
                //Ajouter tous les objet du layer
                for (int i = 0; i < layer.CoronaObjects.Count; i++)
                {
                    this.newCoronaObject(layer.CoronaObjects[i]);
                    this.CoronaObjectSelected = null;
                }

                //Ajouter tous les Controls du layer
              
                for (int i = 0; i < layer.Controls.Count; i++)
                {
                    this.newCoronaControl(layer.Controls[i]);
                }

                //Ajouter tous les Widgets du layer

                for (int i = 0; i < layer.Widgets.Count; i++)
                {
                    this.newCoronaWidget(layer.Widgets[i]);
                }

                node.Expand();
            }

            sceneNode.Expand();
        }

        public void removeLayer(GameElement layerNode)
        {
            if (this.SceneSelected != null)
            {
                CoronaLayer layer = this.getLayerFromNode(layerNode);
                if (layer != null)
                {

                    //this.MainForm.UndoRedo.DO(this.SceneSelected);

                    this.SceneSelected.Layers.Remove(layer);

                    this.MainForm.sceneEditorView1.GraphicsContentManager.CleanLayerGraphics(layer, true, true);

                    //Remove all 
                    isRemovingNode = true;
                    layerNode.Remove();
                    isRemovingNode = false;
                }
                this.LayerSelected = null;

            
                this.MainForm.propertyGrid1.SelectedObject = null;

                if (this.MainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();

                
            }
        }

        public void initALLProjectEnabledState(CoronaGameProject project)
        {
            if (project.isEnabled == false)
            {
                project.isEnabled = true;
                for (int i = 0; i < project.Scenes.Count; i++)
                {
                    Scene scene = project.Scenes[i];
                    scene.isEnabled = true;

                    for (int j = 0; j < scene.Layers.Count; j++)
                    {
                        CoronaLayer layer = scene.Layers[j];
                        layer.isEnabled = true;

                        if (layer.TilesMap != null)
                        {
                            layer.TilesMap.isEnabled = true;
                        }

                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {
                            CoronaObject obj = layer.CoronaObjects[k];
                            obj.isEnabled = true;

                            if (obj.isEntity == true)
                            {
                                CoronaEntity entity = obj.Entity;
                                if (entity != null)
                                {
                                    entity.isEnabled = true;

                                    for (int l = 0; l < entity.CoronaObjects.Count; l++)
                                    {
                                        CoronaObject child = entity.CoronaObjects[l];
                                        child.isEnabled = true;
                                    }

                                    for (int l = 0; l < entity.Jointures.Count; l++)
                                    {
                                        CoronaJointure jointChild = entity.Jointures[l];
                                        jointChild.isEnabled = true;
                                    }
                                }
                            }
                        }

                        for (int k = 0; k < layer.Jointures.Count; k++)
                        {
                            CoronaJointure joint = layer.Jointures[k];
                            joint.isEnabled = true;
                        }

                        for (int k = 0; k < layer.Controls.Count; k++)
                        {
                            CoronaControl control = layer.Controls[k];
                            control.isEnabled = true;
                        }
                    }
                }

                for (int i = 0; i < project.AudioObjects.Count; i++)
                {
                    project.AudioObjects[i].isEnabled = true;
                }
            }
        }

        public CoronaLayer getLayerFromNode(GameElement node)
        {
            return (CoronaLayer)node.InstanceObjet;
        }

        //----------------------------------------------------------
        //----------------Grille Map-----------------------
        //----------------------------------------------------------
        public void newTilesMap(TilesMap map)
        {
            if (this.LayerSelected != null)
            {
                GameElement node = new GameElement("TILESMAP", map.TilesMapName, map);

                node.Checked = map.isEnabled;
                GameElement layerNode = getNodeFromObjectInstance(this.treeViewElements.Nodes, LayerSelected);
                if (layerNode != null)
                {
                    Console.WriteLine("Map ADDED");
                    layerNode.Nodes.Add(node);
                }

            }
            
        }

        public void removeTilesMap()
        {            
            if (this.LayerSelected != null && this.LayerSelected.TilesMap != null)
            {
                //Recuperer la node du layer
                GameElement layerNode = getNodeFromObjectInstance(this.treeViewElements.Nodes, LayerSelected);
                if (layerNode != null)
                {
                    //Recuperer la node de la tilesmap
                    GameElement tilesMapNode = getNodeFromObjectInstance(layerNode.Nodes, this.LayerSelected.TilesMap);
                    if (tilesMapNode != null)
                    {
                        Console.WriteLine("Map Removed");

                        if (this.SelectedNodes.Contains(tilesMapNode))
                            this.SelectedNodes.Remove(tilesMapNode);

                        isRemovingNode = true;
                        layerNode.Nodes.Remove(tilesMapNode);
                        isRemovingNode = false;

                        this.LayerSelected.TilesMap = null;

                        if (this.MainForm.isFormLocked == false)
                            GorgonLibrary.Gorgon.Go();
                    }
                }

                this.MainForm.propertyGrid1.SelectedObject = null;
            }
            
        }
        public TilesMap getGrilleMapFromNode(GameElement node)
        {
            return (TilesMap)node.InstanceObjet;
        }

        //----------------------------------------------------------
        //----------------Corona Object-----------------------
        //----------------------------------------------------------

        public void newCoronaObject(CoronaObject obj)
        {
            if (this.LayerSelected != null || 
                (this.CoronaObjectSelected !=null && (this.CoronaObjectSelected.isEntity == true || this.CoronaObjectSelected.EntityParent != null)))
            {


                GameElement node = null;
                if(obj.isEntity == false)
                    node = new GameElement("OBJECT", obj.DisplayObject.Name, obj);
                else
                    node = new GameElement("ENTITY", obj.Entity.Name, obj);

               
                node.Checked = obj.isEnabled;

                if (this.CoronaObjectSelected != null && obj.isEntity == false)
                {
                    if(this.CoronaObjectSelected.EntityParent != null)
                    {
                         GameElement entityNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.CoronaObjectSelected.EntityParent.objectParent);
                        if (entityNode != null)
                        {
                            entityNode.Nodes.Add(node);

                        }

                        this.CoronaObjectSelected = obj;
                    }
                    else if (this.CoronaObjectSelected.isEntity == true)
                    {
                        GameElement entityNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.CoronaObjectSelected);
                        if (entityNode != null)
                        {
                            entityNode.Nodes.Add(node);

                        }

                        this.CoronaObjectSelected = obj;
                    }
                    else
                    {
                        GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                        if (layerNode != null)
                        {
                            layerNode.Nodes.Add(node);

                        }

                        this.CoronaObjectSelected = obj;
                    }
                   
                }

                else
                {
                    GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                    if (layerNode != null)
                    {
                        layerNode.Nodes.Add(node);

                    }

                    this.CoronaObjectSelected = obj;

                    if (obj.isEntity == true)
                    {
                        //Ajouter les jointures
                        TreeNode nodeJoints = new TreeNode("Joints");
                        
                        nodeJoints.Name = "JOINTS";
                        nodeJoints.ImageIndex = 8;
                        nodeJoints.SelectedImageIndex = 8;
                        node.Nodes.Add(nodeJoints);
                        this.HideCheckBox(this.treeViewElements, nodeJoints);
                        for (int i = 0; i < obj.Entity.CoronaObjects.Count; i++)
                        {
                            this.newCoronaObject(obj.Entity.CoronaObjects[i]);
                        }

                        for (int i = 0; i < obj.Entity.Jointures.Count; i++)
                        {
                            this.newJoint(obj.Entity.Jointures[i], false, null);
                        }
                    }
                }

            }
        }


        public void removeAllJointsUsedByObject(CoronaObject obj)
        {
            //Get layer parent
            CoronaLayer layerParent = null;
            if (obj.EntityParent !=null)
                layerParent = obj.EntityParent.objectParent.LayerParent;
            else
                layerParent = obj.LayerParent;

            //--Search in layer's joints
            CoronaJointure[] joints = new CoronaJointure[layerParent.Jointures.Count];
            layerParent.Jointures.CopyTo(joints);
            for (int i = 0; i < joints.Length; i++)
            {
                CoronaJointure joint = joints[i];
                if (joint.coronaObjA != null)
                {
                    if (joint.coronaObjA == obj)
                    {
                        this.removeCoronaJointure(joint, layerParent);
                        continue;
                    }

                    if (joint.coronaObjB == obj)
                    {
                        this.removeCoronaJointure(joint, layerParent);
                        continue;
                    }

                }
            }

            //--Search in ALL entities joints
            for (int j = 0; j < layerParent.CoronaObjects.Count; j++)
            {
                CoronaObject currObj = layerParent.CoronaObjects[j];
                if (currObj.isEntity == true)
                {
                    CoronaJointure[] joints2 = new CoronaJointure[currObj.Entity.Jointures.Count];
                    currObj.Entity.Jointures.CopyTo(joints2);
                    for (int i = 0; i < joints2.Length; i++)
                    {
                        CoronaJointure joint = joints2[i];
                        if (joint.coronaObjA != null)
                        {
                            if (joint.coronaObjA == obj)
                            {
                                this.removeCoronaJointure(joint, currObj);
                                continue;
                            }

                            if (joint.coronaObjB == obj)
                            {
                                this.removeCoronaJointure(joint, currObj);
                                continue;
                            }

                        }
                    }
                }
            }
            
        }
        public void removeCoronaObject(CoronaObject obj)
        {
            if (obj != null && ProjectRootNodeSelected != null)
            {
                //Remove all joints including this object
                if (obj.isEntity == true)
                {
                    CoronaObject[] listToTreat = new CoronaObject[obj.Entity.CoronaObjects.Count];
                    obj.Entity.CoronaObjects.CopyTo(listToTreat);
                    for (int i = 0; i < listToTreat.Length; i++)
                    {
                        CoronaObject child = listToTreat[i];
                        this.removeCoronaObject(child);

                    }

                    GameElement gElementObj = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, obj);
                    if (gElementObj != null)
                    {
                        if (this.SelectedNodes.Contains(gElementObj))
                            this.SelectedNodes.Remove(gElementObj);

                        isRemovingNode = true;
                        gElementObj.Remove();
                        isRemovingNode = false;
                    }

                    this.CoronaObjectSelected = obj;
                    this.removeAttributeToolStripMenuItem_Click(null, null);
                    this.CoronaObjectSelected = null;

                    obj.LayerParent.removeCoronaObject(obj);
                    this.MainForm.propertyGrid1.SelectedObject = null;

                    obj = null;

                    if (this.MainForm.isFormLocked == false)
                        GorgonLibrary.Gorgon.Go();

                    

                }
                else
                {
                    if (obj.EntityParent != null)
                    {
                        CoronaEntity entityParent = obj.EntityParent;                        
                        this.removeAllJointsUsedByObject(obj);
                        
                        GameElement gElementObj = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, obj);
                        if (gElementObj != null)
                        {
                            if (this.SelectedNodes.Contains(gElementObj))
                                this.SelectedNodes.Remove(gElementObj);

                            isRemovingNode = true;
                            gElementObj.Remove();
                            isRemovingNode = false;
                        }

                        this.CoronaObjectSelected = obj;
                        this.removeAttributeToolStripMenuItem_Click(null, null);
                        this.CoronaObjectSelected = null;

                        if (obj.LayerParent.SceneParent.Camera.objectFocusedByCamera == obj)
                            obj.LayerParent.SceneParent.Camera.objectFocusedByCamera = null;

                        entityParent.CoronaObjects.Remove(obj);
                        this.MainForm.sceneEditorView1.GraphicsContentManager.CleanSprite(obj, true, true);
                        this.MainForm.propertyGrid1.SelectedObject = null;

                        if (this.MainForm.isFormLocked == false)
                            GorgonLibrary.Gorgon.Go();
                    }
                    else
                    {
                        CoronaLayer layerParent = obj.LayerParent;
                        if (layerParent != null)
                        {
                            CoronaJointure[] joints = new CoronaJointure[layerParent.Jointures.Count];
                            layerParent.Jointures.CopyTo(joints);
                            for (int i = 0; i < joints.Length; i++)
                            {
                                CoronaJointure joint = joints[i];
                                if (joint.coronaObjA != null)
                                {
                                    if (joint.coronaObjA == obj)
                                    {
                                        this.removeCoronaJointure(joint,layerParent);
                                        continue;
                                    }

                                    if (joint.coronaObjB == obj)
                                    {
                                        this.removeCoronaJointure(joint,layerParent);
                                        continue;
                                    }

                                }
                            }
                        }

                        GameElement gElementObj = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, obj);
                        if (gElementObj != null)
                        {
                            if (this.SelectedNodes.Contains(gElementObj))
                                this.SelectedNodes.Remove(gElementObj);

                            isRemovingNode = true;
                            gElementObj.Remove();
                            isRemovingNode = false;


                        }

                        this.CoronaObjectSelected = obj;
                        this.removeAttributeToolStripMenuItem_Click(null, null);
                        this.CoronaObjectSelected = null;

                        layerParent.removeCoronaObject(obj);

                        this.MainForm.sceneEditorView1.GraphicsContentManager.CleanSprite(obj, true, true);
                        this.MainForm.propertyGrid1.SelectedObject = null;

                        if (this.MainForm.isFormLocked == false)
                            GorgonLibrary.Gorgon.Go();
                    }
                    
                }
                
            }
        }

        public CoronaObject getObjectFromNode(GameElement node)
        {
            return (CoronaObject)node.InstanceObjet;
        }

        //----------------------------------------------------------
        //----------------Corona COntrol-----------------------
        //----------------------------------------------------------
        public void newCoronaControl(CoronaControl control)
        {
            if (this.LayerSelected != null)
            {
                 GameElement node = new GameElement("CONTROL", control.ControlName, control);
                 node.Checked = control.isEnabled;
                GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                if (layerNode != null)
                {
                    layerNode.Nodes.Add(node);
                    
                }

            }
        }

        public void removeCoronaJointure(CoronaJointure joint,object objectParent)
        {
            if (objectParent != null)
            {
                if (objectParent is CoronaLayer)
                {
                    CoronaLayer layerParent = objectParent as CoronaLayer;
                    GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, layerParent);
                    if (layerNode != null)
                    {

                        GameElement gElementControl = this.getNodeFromObjectInstance(layerNode.Nodes, joint);
                        if (gElementControl != null)
                        {
                            layerParent.Jointures.Remove(joint);
                            layerNode.Nodes["JOINTS"].Nodes.Remove(gElementControl);
                        }

                        joint = null;
                    }
                }
                else if (objectParent is CoronaObject)
                {
                    CoronaObject objParent = objectParent as CoronaObject;
                    GameElement objNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, objParent);
                    if (objNode != null)
                    {

                        GameElement gElementControl = this.getNodeFromObjectInstance(objNode.Nodes, joint);
                        if (gElementControl != null)
                        {
                            objParent.Entity.Jointures.Remove(joint);
                            objNode.Nodes["JOINTS"].Nodes.Remove(gElementControl);
                        }

                        joint = null;
                    }
                }
                
            }
        }

        public void removeCoronaControl(CoronaControl control)
        {
            if (this.LayerSelected != null)
            {
                GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                 if (layerNode != null)
                 {

                    
                    GameElement gElementControl = this.getNodeFromObjectInstance(layerNode.Nodes,control);
                    if (gElementControl != null)
                    {
                        LayerSelected.Controls.Remove(control);
                        layerNode.Nodes.Remove(gElementControl);
                    }

                    this.MainForm.sceneEditorView1.GraphicsContentManager.CleanSprite(control, true);
                    control = null;
                 }
            }
            else if (this.SceneSelected != null)
            {


                //Get the layer parent
                CoronaLayer layerParent = null;
                for (int i = 0; i < this.SceneSelected.Layers.Count; i++)
                {
                    bool hasFound = false;
                    CoronaLayer currentLayer = this.SceneSelected.Layers[i];
                    for (int j = 0; j < currentLayer.Controls.Count; j++)
                    {
                        CoronaControl currentControl = currentLayer.Controls[j];
                        if (control == currentControl)
                        {
                            hasFound = true;
                            break;
                        }

                    }

                    if (hasFound == true)
                    {
                        layerParent = currentLayer;
                        break;
                    }
                }

                if (layerParent != null)
                {
                    GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                    if (layerNode != null)
                    {
                        GameElement gElementControl = this.getNodeFromObjectInstance(layerNode.Nodes, control);
                        if (gElementControl != null)
                        {
                            layerParent.Controls.Remove(control);
                            layerNode.Nodes.Remove(gElementControl);
                        }

                        control = null;
                    }
                }


            }

            this.MainForm.propertyGrid1.SelectedObject = null;

            if (this.MainForm.isFormLocked == false)
                this.MainForm.sceneEditorView1.Refresh();

        }

        //----------------------------------------------------------
        //----------------Corona Widget-----------------------
        //----------------------------------------------------------

        public void newCoronaWidget(CoronaWidget widget)
        {
            if (this.LayerSelected != null)
            {

                GameElement node = new GameElement("WIDGET", widget.Name, widget);
                node.Checked = false;

                GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                if (layerNode != null)
                {
                    layerNode.Nodes.Add(node);

                }

            }
        }

        

        public void removeCoronaWidget(CoronaWidget widget)
        {
            if (this.LayerSelected != null)
            {
                GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, LayerSelected);
                if (layerNode != null)
                {


                    GameElement gElementWidget= this.getNodeFromObjectInstance(layerNode.Nodes, widget);
                    if (gElementWidget != null)
                    {
                        if (this.SelectedNodes.Contains(gElementWidget))
                            this.SelectedNodes.Remove(gElementWidget);

                        LayerSelected.Widgets.Remove(widget);
                        layerNode.Nodes.Remove(gElementWidget);
                    }

                    widget = null;
                }
            }

            this.MainForm.propertyGrid1.SelectedObject = null;

            if (this.MainForm.isFormLocked == false)
                GorgonLibrary.Gorgon.Go();

        }
        //----------------------------------------------------------
        //----------------CORONA JOINTURE-----------------------
        //----------------------------------------------------------
        public void newJoint(CoronaJointure joint,bool addToList,TreeNode nodeExisting)
        {

            if (joint != null)
            {
                CoronaObject coronaObjA = joint.coronaObjA;
                CoronaObject coronaObjB = joint.coronaObjB;
                if (coronaObjA != null && coronaObjB != null)
                {
                    if (coronaObjA.EntityParent != null && coronaObjB.EntityParent != null)
                    {
                        if (coronaObjA.EntityParent == coronaObjB.EntityParent)
                        {
                            GameElement entityNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, coronaObjA.EntityParent.objectParent);
                            if (entityNode != null)
                            {

                                if (addToList == true)
                                    coronaObjA.EntityParent.Jointures.Add(joint);

                                TreeNode nodeJoints = entityNode.Nodes["JOINTS"];

                                if (nodeExisting == null)
                                {
                                    GameElement nodeNewJoint = new GameElement("JOINT", joint.name, joint);
                                    nodeJoints.Nodes.Add(nodeNewJoint);

                                    nodeNewJoint.Checked = joint.isEnabled;
                                }
                                else
                                {
                                    nodeJoints.Nodes.Add(nodeExisting);
                                }

                                return;
                            }


                        }
                    }
                    else if (coronaObjA != null && coronaObjB == null)
                    {
                        if (coronaObjA.EntityParent != null)
                        {
                            GameElement entityNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, coronaObjA.EntityParent.objectParent);
                            if (entityNode != null)
                            {
                                if (addToList == true)
                                    coronaObjA.EntityParent.Jointures.Add(joint);

                                TreeNode nodeJoints = entityNode.Nodes["JOINTS"];

                                if (nodeExisting == null)
                                {
                                    GameElement nodeNewJoint = new GameElement("JOINT", joint.name, joint);
                                    nodeJoints.Nodes.Add(nodeNewJoint);
                                    nodeNewJoint.Checked = joint.isEnabled;
                                }
                                else
                                {
                                    nodeJoints.Nodes.Add(nodeExisting);
                                }

                                return;
                            }
                        }
                    }

                }

                
                GameElement layerNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, joint.layerParent);
                if (layerNode != null)
                {

                    if (addToList == true)
                        joint.layerParent.Jointures.Add(joint);

                    TreeNode nodeJoints = layerNode.Nodes["JOINTS"];
                  
                    if (nodeExisting == null)
                    {
                        GameElement nodeNewJoint = new GameElement("JOINT", joint.name, joint);
                        nodeNewJoint.Checked = joint.isEnabled;
                        nodeJoints.Nodes.Add(nodeNewJoint);
                    }
                    else
                    {
                        nodeJoints.Nodes.Add(nodeExisting);
                    }
                }

            }

        }


        public void removeJointSelected()
        {
            if (this.SelectedNodes.Count > 0)
            {
                TreeNode nodeSelected = this.SelectedNodes[0];
                if (nodeSelected != null)
                {
                    if (nodeSelected.Name.Equals("GAME_ELEMENT"))
                    {
                        //Recuperer le layer parent
                        GameElement nodeElem = (GameElement)nodeSelected;
                        if (nodeElem.NodeType.Equals("JOINT"))
                        {

                            CoronaJointure joint = (CoronaJointure)nodeElem.InstanceObjet;
                            CoronaLayer layerParent = joint.layerParent;

                            if (nodeElem.Parent!= null)
                            {
                                if (nodeElem.Parent.Parent is GameElement)
                                {
                                    GameElement elemParent = nodeElem.Parent.Parent as GameElement;
                                    if (elemParent.NodeType.Equals("LAYER"))
                                    {
                                        CoronaLayer layerSelected = elemParent.InstanceObjet as CoronaLayer;
                                        layerSelected.Jointures.Remove(joint);
                                    }
                                    else if(elemParent.NodeType.Equals("ENTITY"))
                                    {
                                        CoronaObject entitySelected = elemParent.InstanceObjet as CoronaObject;
                                        entitySelected.Entity.Jointures.Remove(joint);
                                    }
                                }
                            }

                        

                            if (this.SelectedNodes.Contains(nodeElem))
                                this.SelectedNodes.Remove(nodeElem);

                            nodeElem.Remove();
                            nodeElem = null;

                            layerParent.JointureSelected = null;
                            this.JointureSelected = null;
                            this.MainForm.propertyGrid1.SelectedObject = null;

                            if (this.MainForm.isFormLocked == false)
                                GorgonLibrary.Gorgon.Go();


                        }
                    }
                }
            }
        }

        //----------------------------------------------------------
        //----------------Corona AUDIO OBJECT-----------------------
        //----------------------------------------------------------
        public void newAudioObject(AudioObject audio)
        {
            if(this.ProjectRootNodeSelected != null)
            {
                GameElement node = new GameElement("AUDIO", audio.name, audio);
                node.Checked = false;

                this.ProjectRootNodeSelected.Nodes["AUDIOS"].Nodes.Add(node);
                this.HideCheckBox(this.treeViewElements, node);

            }
        }

        //----------------------------------------------------------
        //----------------Corona FONT OBJECT-----------------------
        //----------------------------------------------------------
        public void newFontItem(FontItem font)
        {
            if (this.ProjectRootNodeSelected != null)
            {
                GameElement node = new GameElement("FONT", font.NameForIphone, font);
                node.Checked = false;
                this.ProjectRootNodeSelected.Nodes["FONTS"].Nodes.Add(node);
                this.HideCheckBox(this.treeViewElements, node);

            }
        }

        //----------------------------------------------------------
        //----------------Corona Snippet - ----------------------
        //----------------------------------------------------------
        public void newSnippet(Snippet snippet)
        {
            if (this.ProjectRootNodeSelected != null)
            {
                GameElement node = new GameElement("SNIPPET", snippet.Name, snippet);
                node.Checked = false;

                this.ProjectRootNodeSelected.Nodes["SNIPPETS"].Nodes.Add(node);
                this.HideCheckBox(this.treeViewElements, node);
                

            }
        }

        //----------------------------------------------------------
        //---------------Sprite Sets-----------------------
        //----------------------------------------------------------
        public void newSpriteSet(CoronaSpriteSet set)
        {
            if (this.ProjectRootNodeSelected != null && this.SceneSelected != null)
            {
                if (isSpriteSetExistsInScene(this.SceneSelected, set) == false)
                {
                    GameElement sceneNode = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.SceneSelected);
                    GameElement node = new GameElement("SPRITESET", set.Name, set);
                    sceneNode.Nodes["SPRITESETS"].Nodes.Add(node);

                /*    if (!this.SceneSelected.SpriteSets.Contains(set))
                        this.SceneSelected.SpriteSets.Add(set);*/

                    //Ajouter toutes les sheets necessaires à la sprite set
                    for (int i = 0; i < set.Frames.Count; i++)
                    {
                        if (isSheetExistsInScene(this.SceneSelected, set.Frames[i].SpriteSheetParent) == false)
                        {
                            newSpriteSheet(set.Frames[i].SpriteSheetParent);
                        }
                    }
                }

            }
        }

        public bool isSpriteSetExistsInScene(Scene scene, CoronaSpriteSet set)
        {
            //Recueprer la node de la scene
            GameElement sceneNode = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.SceneSelected);

            for (int i = 0; i < sceneNode.Nodes["SPRITESETS"].Nodes.Count; i++)
            {
                CoronaSpriteSet setInProject = (CoronaSpriteSet)((GameElement)sceneNode.Nodes["SPRITESETS"].Nodes[i]).InstanceObjet;
                if (set.Name.Equals(setInProject.Name))
                    return true;
            }

            return false;
        }
        //----------------------------------------------------------
        //---------------Sprite SHEETS-----------------------
        //----------------------------------------------------------
        public void newSpriteSheet(CoronaSpriteSheet sheet)
        {
            if (this.ProjectRootNodeSelected != null && this.SceneSelected != null)
            {
                if (isSheetExistsInScene(this.SceneSelected, sheet) == false)
                {
                    GameElement sceneNode = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.SceneSelected);
                    GameElement node = new GameElement("SPRITESHEET", sheet.Name, sheet);
                    sceneNode.Nodes["SPRITESHEETS"].Nodes.Add(node);

                    if(!this.SceneSelected.SpriteSheets.Contains(sheet))
                        this.SceneSelected.SpriteSheets.Add(sheet);
                }
                
            }
        }


        public bool isSheetExistsInScene(Scene scene,CoronaSpriteSheet sheet)
        {
            //Recueprer la node de la scene
            GameElement sceneNode = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.SceneSelected);

            for (int i = 0; i < sceneNode.Nodes["SPRITESHEETS"].Nodes.Count; i++)
            {
                CoronaSpriteSheet sheetInProject = (CoronaSpriteSheet)((GameElement)sceneNode.Nodes["SPRITESHEETS"].Nodes[i]).InstanceObjet;
                if (sheet.Name.Equals(sheetInProject.Name))
                    return true;
            }

            return false;
        }
        //----------------------------------------------------------
        // ------------ UTILS ----------------
        //----------------------------------------------------------

        public GameElement getNodeFromObjectInstance(TreeNodeCollection nodeCollection, Object obj)
        {
            if (obj != null)
            {
                for (int i = 0; i < nodeCollection.Count; i++)
                {
                    TreeNode node = nodeCollection[i];
                    if (node.Name.Equals("GAME_ELEMENT"))
                    {
                        GameElement nodeElement = (GameElement)node;

                        if (nodeElement.InstanceObjet == obj)
                            return nodeElement;
                        else if (nodeElement.Nodes.Count > 0)
                        {
                            GameElement retour = getNodeFromObjectInstance(nodeElement.Nodes, obj);
                            if (retour != null)
                                return retour;
                        }
                    }
                    else
                    {
                        GameElement elemret = getNodeFromObjectInstance(node.Nodes, obj);
                        if (elemret != null)
                            return elemret;

                    }
                   
                }
            }

            return null;
        }



        public TreeView getTreeView()
        {
            return this.treeViewElements;
        }


        // On Click New Layer Menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.SceneSelected != null)
                {
                    CoronaLayer layer = this.SceneSelected.newLayer();
                    this.newLayer(this.SceneSelected, layer);

                    if (this.MainForm.isFormLocked == false)
                        GorgonLibrary.Gorgon.Go();
                }
                
            }
            catch(Exception ex)
            {

                Application.Exit();
            }

        }

        private void treeViewElements_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (hasChecked == false)
            {
                try
                {
                    bool isCustomBuild = this.MainForm.IsCustomBuild;
                    float XRatio = 1;
                    float YRatio = 1;
                    if (isCustomBuild == true)
                    {
                        XRatio = (float)this.MainForm.currentTargetResolution.Resolution.Width / (float)this.MainForm.CurrentProject.width;
                        YRatio = (float)this.MainForm.currentTargetResolution.Resolution.Height / (float)this.MainForm.CurrentProject.height;
                    }

                    //Recuperer la node Project root
                    ProjectRootNodeSelected = getRootNode(e.Node);
                    ProjectSelected = (CoronaGameProject)ProjectRootNodeSelected.Tag;


                    //Si il y a deja un panel Physic d'ouvert : Le fermer

                    if (this.MainForm.CurrentObjectPhysicEditorPanel != null)
                        if (this.MainForm.getMapEditorPage().Controls.Contains(this.MainForm.CurrentObjectPhysicEditorPanel))
                        {
                            this.MainForm.getMapEditorPage().Controls.Remove(this.MainForm.CurrentObjectPhysicEditorPanel);
                        }


                    //SI la node selectionnée est un game element

                    if (e.Node == this.ProjectRootNodeSelected)
                    {
                        ProjectPropertyConverter converter = new ProjectPropertyConverter(this.ProjectSelected, this.MainForm);

                        this.MainForm.propertyGrid1.SelectedObject = converter;
                    }
                    if (e.Node.Name.Equals("GAME_ELEMENT"))
                    {
                        //Faire le traitement de selection
                        GameElement nodeSelected = (GameElement)e.Node;

                        if (nodeSelected.NodeType.Equals("STAGE"))
                        {

                            this.SceneSelected = ((Scene)nodeSelected.InstanceObjet);
                            this.MainForm.sceneEditorView1.objectsSelected.Clear();
                            this.MainForm.sceneEditorView1.setModeSceneEditor(this.SceneSelected);

                            //Deselect others
                            if (this.LayerSelected != null)
                            {
                                this.LayerSelected.deselectAllObjects();
                                this.LayerSelected.deselectAllControls();
                            }


                            this.LayerSelected = null;
                            this.CoronaObjectSelected = null;

                            //Afficher les proprietes du layer dans le property grid
                            ScenePropertyConverter sceneConverter = new ScenePropertyConverter(this.SceneSelected, this.MainForm);
                            this.MainForm.propertyGrid1.SelectedObject = sceneConverter;



                            //Mettre a jour le fichier lua
                            this.MainForm.cgEeditor1.RefreshSceneLuaCode(this.SceneSelected, isCustomBuild, XRatio, YRatio);
                        }
                        else if (nodeSelected.NodeType.Equals("LAYER"))
                        {

                            //Deselect others
                            if (this.LayerSelected != null)
                            {
                                this.LayerSelected.deselectAllObjects();
                                this.LayerSelected.deselectAllControls();
                            }

                            this.LayerSelected = ((CoronaLayer)nodeSelected.InstanceObjet);
                            this.LayerSelected.deselectAllObjects();
                            this.LayerSelected.deselectAllControls();
                            this.MainForm.sceneEditorView1.objectsSelected.Clear();

                            this.SceneSelected = this.LayerSelected.SceneParent;
                            this.CoronaObjectSelected = null;

                            this.MainForm.sceneEditorView1.setModeLayerEditor(this.LayerSelected);

                            //Afficher les proprietes du layer dans le property grid
                            LayerPropertyConverter layerConverter = new LayerPropertyConverter(this.LayerSelected, this.MainForm);
                            this.MainForm.propertyGrid1.SelectedObject = layerConverter;

                            //Mettre a jour le fichier lua
                            //this.MainForm.cgEeditor1.RefreshSceneLuaCode(this.SceneSelected, isCustomBuild, XRatio, YRatio);
                        }
                        else if (nodeSelected.NodeType.Equals("OBJECT") || nodeSelected.NodeType.Equals("ENTITY"))
                        {

                            CoronaObject obj = ((CoronaObject)nodeSelected.InstanceObjet);

                            //Selectionner le layer parent
                            this.LayerSelected = obj.LayerParent;

                            if (nodeSelected.NodeType.Equals("ENTITY"))
                            {
                                this.MainForm.SetModeEntity();
                                //this.LayerSelected.deselectAllObjects();
                            }
                            else
                                this.MainForm.SetModeObject();


                            //Fermer le layer et la scene
                            this.SceneSelected = this.LayerSelected.SceneParent;

                            this.CoronaObjectSelected = obj;


                            //Mettre a jour le fichier lua
                            //this.MainForm.cgEeditor1.RefreshSceneLuaCode(this.SceneSelected, isCustomBuild, XRatio, YRatio);

                            /*if (this.MainForm.isFormLocked == false)
                                this.MainForm.sceneEditorView1.surfacePictBx.Refresh();*/
                        }
                        else if (nodeSelected.NodeType.Equals("TILESMAP"))
                        {
                            this.MainForm.SetModeObject();
                            TilesMap map = ((TilesMap)nodeSelected.InstanceObjet);

                            //Selectionner le layer parent
                            this.CoronaObjectSelected = null;

                            this.LayerSelected = (CoronaLayer)((GameElement)nodeSelected.Parent).InstanceObjet;
                            this.SceneSelected = this.LayerSelected.SceneParent;

                            //Deselect all objects
                            this.LayerSelected.deselectAllObjects();

                            //Afficher les proprietes de l'objet dans le property grid
                            TilesMapPropertyConverter mapConverter = new TilesMapPropertyConverter(map, this.MainForm);
                            this.MainForm.propertyGrid1.SelectedObject = mapConverter;

                            if (this.MainForm.isFormLocked == false)
                                GorgonLibrary.Gorgon.Go();

                        }
                        else if (nodeSelected.NodeType.Equals("CONTROL"))
                        {
                            this.MainForm.SetModeControl();
                            CoronaControl control = ((CoronaControl)nodeSelected.InstanceObjet);
                            this.ControlSelected = control;
                            if (control.type == CoronaControl.ControlType.joystick)
                            {

                                JoystickControl joy = (JoystickControl)control;
                                JoystickPropertyConverter joyConverter = new JoystickPropertyConverter(joy, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = joyConverter;


                            }
                        }
                        else if (nodeSelected.NodeType.Equals("WIDGET"))
                        {
                            CoronaWidget widget = ((CoronaWidget)nodeSelected.InstanceObjet);
                            this.WidgetSelected = widget;
                            widget.Type = CoronaWidget.WidgetType.tabBar;
                            if (widget.Type == CoronaWidget.WidgetType.tabBar)
                            {

                                TabBarPropertyConverter converter = new TabBarPropertyConverter(widget, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;


                            }
                            else if (widget.Type == CoronaWidget.WidgetType.pickerWheel)
                            {

                                WidgetPickerWheel pickerW = (WidgetPickerWheel)widget;
                                this.MainForm.propertyGrid1.SelectedObject = pickerW;


                            }
                        }
                        else if (nodeSelected.NodeType.Equals("JOINT"))
                        {


                            CoronaJointure joint = nodeSelected.InstanceObjet as CoronaJointure;


                            this.MainForm.setModeJoint();

                            //Selectionner le layer parent
                            this.CoronaObjectSelected = null;
                            this.JointureSelected = joint;
                            this.LayerSelected = joint.layerParent;
                            this.LayerSelected.JointureSelected = joint;
                            this.SelectedNodes.Add(nodeSelected);


                            this.MainForm.sceneEditorView1.setModeLayerEditor(joint.layerParent);
                            //Ouvrir le property converter correspondant au joint
                            if (this.JointureSelected.type.Equals("DISTANCE"))
                            {
                                DistancePropertyConverter converter = new DistancePropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("FRICTION"))
                            {
                                FrictionPropertyConverter converter = new FrictionPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("PISTON"))
                            {
                                PistonPropertyConverter converter = new PistonPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("PIVOT"))
                            {
                                PivotPropertyConverter converter = new PivotPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("WELD"))
                            {
                                WeldPropertyConverter converter = new WeldPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("WHEEL"))
                            {
                                WheelPropertyConverter converter = new WheelPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("PULLEY"))
                            {
                                PulleyPropertyConverter converter = new PulleyPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }
                            else if (this.JointureSelected.type.Equals("TOUCH"))
                            {
                                TouchPropertyConverter converter = new TouchPropertyConverter(this.JointureSelected, this.MainForm);
                                this.MainForm.propertyGrid1.SelectedObject = converter;
                            }

                            //Mettre a jour le fichier lua
                            this.MainForm.cgEeditor1.RefreshSceneLuaCode(this.SceneSelected, isCustomBuild, XRatio, YRatio);

                        }
                        else if (nodeSelected.NodeType.Equals("AUDIO"))
                        {

                            this.AudioObjectSelected = (AudioObject)nodeSelected.InstanceObjet;
                        }
                        else if (nodeSelected.NodeType.Equals("SNIPPET"))
                        {
                            this.SnippetSelected = (Snippet)nodeSelected.InstanceObjet;
                            this.MainForm.cgEeditor1.RefreshSnippetLuaCode(this.ProjectSelected);
                        }
                        else if (nodeSelected.NodeType.Equals("FONT"))
                        {
                            this.FontSelected = (FontItem)nodeSelected.InstanceObjet;
                        }

                        //------------Verifier si le clic est un clic droit 
                        if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {

                            if (nodeSelected.NodeType.Equals("STAGE"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.menuScene;
                                this.treeViewElements.ContextMenuStrip.Show();
                                this.LayerSelected = null;
                            }
                            else if (nodeSelected.NodeType.Equals("LAYER"))
                            {

                                this.treeViewElements.ContextMenuStrip = this.menuLayer;
                                this.treeViewElements.ContextMenuStrip.Show();

                            }
                            else if (nodeSelected.NodeType.Equals("OBJECT") || nodeSelected.NodeType.Equals("ENTITY"))
                            {

                                CoronaObject obj = ((CoronaObject)nodeSelected.InstanceObjet);

                                activerBoutonsNecessairesMenuObject(obj);

                                this.treeViewElements.ContextMenuStrip = this.menuObject;
                                this.treeViewElements.ContextMenuStrip.Show();

                            }
                            else if (nodeSelected.NodeType.Equals("CONTROL"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.menuControl;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else if (nodeSelected.NodeType.Equals("WIDGET"))
                            {
                                CoronaWidget widget = (CoronaWidget)nodeSelected.InstanceObjet;
                                if (widget.Type == CoronaWidget.WidgetType.tabBar)
                                {
                                    this.treeViewElements.ContextMenuStrip = this.menuWidgetTabBar;
                                    this.treeViewElements.ContextMenuStrip.Show();
                                }
                                else if (widget.Type == CoronaWidget.WidgetType.pickerWheel)
                                {
                                    this.treeViewElements.ContextMenuStrip = this.menuWidgetPickerWheel;
                                    this.treeViewElements.ContextMenuStrip.Show();
                                }


                            }
                            else if (nodeSelected.NodeType.Equals("SPRITESHEET") || nodeSelected.NodeType.Equals("SPRITESET"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.menuSpriteSetSheet;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else if (nodeSelected.NodeType.Equals("JOINT"))
                            {

                                this.treeViewElements.ContextMenuStrip = this.menuJointures;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else if (nodeSelected.NodeType.Equals("AUDIO"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.menuAudio;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else if (nodeSelected.NodeType.Equals("TILESMAP"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.menuTilesmap;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else if (nodeSelected.NodeType.Equals("SNIPPET"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.menuSnippets;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else if (nodeSelected.NodeType.Equals("FONT"))
                            {
                                this.treeViewElements.ContextMenuStrip = this.fontMenu;
                                this.treeViewElements.ContextMenuStrip.Show();
                            }
                            else
                                this.treeViewElements.ContextMenuStrip = null;
                        }

                        else
                            this.treeViewElements.ContextMenuStrip = null;
                    }
                    else if (e.Node.Name.Equals("PROJECT"))
                    {
                        //Verifier si le clic est un clic droit 
                        if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            this.treeViewElements.ContextMenuStrip = this.menuProject;
                            this.treeViewElements.ContextMenuStrip.Show();
                        }
                    }
                    else
                        this.treeViewElements.ContextMenuStrip = null;

                }
                catch (Exception ex)
                {
                    Application.Exit();
                }
            }
        }

        public void activerBoutonsNecessairesMenuObject(CoronaObject obj)
        {
            
            if (obj.isEntity == false)
            {
                this.addToolStripMenuItem.Visible = true;
                this.toolStripSeparator2.Visible = true;
                this.setFocusCameraToolStripMenuItem.Visible = true;
                this.toolStripSeparator1.Visible = true;
                this.toolStripSeparator14.Visible = true;
                this.pathFollowToolStripMenuItem.Visible = true;
                this.toolStripSeparator19.Visible = true;
                this.setGeneratorAttachOnObjectToolStripMenuItem.Visible = true;
                this.updateAssetToolStripMenuItem.Visible = true;
                this.physicBodyManagerToolStripMenuItem.Visible = true;
                this.updateAssetToolStripMenuItem.Visible = true;
                this.mobileEditorToolStripMenuItem.Visible = true;
                this.exportEntityToolStripMenuItem.Visible = false;
                this.exportObjectToolStripMenuItem.Visible = true;

                if (this.SceneSelected != null)
                {
                    if (this.SceneSelected.Camera.objectFocusedByCamera != null)
                    {
                        if (this.SceneSelected.Camera.objectFocusedByCamera == obj)
                            this.setFocusCameraToolStripMenuItem.Text = "Cancel follow by camera";
                        else
                            this.setFocusCameraToolStripMenuItem.Text = "Follow by camera";
                    }
                    else
                        this.setFocusCameraToolStripMenuItem.Text = "Follow by camera";

                }

                if (obj.isGenerator == true)
                {
                    if (obj.objectAttachedToGenerator != null)
                        this.setGeneratorAttachOnObjectToolStripMenuItem.Text = "Remove generator fastener";
                    else
                        this.setGeneratorAttachOnObjectToolStripMenuItem.Text = "Set generator fastener";

                    this.setGeneratorAttachOnObjectToolStripMenuItem.Visible = true;
                }
                else
                    this.setGeneratorAttachOnObjectToolStripMenuItem.Visible = false;

                //Griser les composants inutiles
                if (obj.DisplayObject.Type.Equals("FIGURE"))
                {
                    this.addToolStripMenuItem.Visible = false;
                    this.physicBodyManagerToolStripMenuItem.Visible = false;
                    this.animationManagerToolStripMenuItem.Visible = false;
                    this.updateAssetToolStripMenuItem.Visible = false;
                }
                else
                {
                    this.addToolStripMenuItem.Visible = true;
                    this.physicBodyManagerToolStripMenuItem.Visible = true;
                    this.updateAssetToolStripMenuItem.Visible = true;

                    if(obj.DisplayObject.Type.Equals("SPRITE"))
                        this.animationManagerToolStripMenuItem.Visible = true;
                    else
                        this.animationManagerToolStripMenuItem.Visible = false;
                }

                if (obj.LayerParent.SceneParent.Name.Equals("mapeditormobile"))
                {
                    this.mobileEditorToolStripMenuItem.Visible = true;

                }
                else
                {
                    this.mobileEditorToolStripMenuItem.Visible = false;
                }
            }
            else
            {
                this.addToolStripMenuItem.Visible = false;
                this.toolStripSeparator2.Visible = false;
                this.setFocusCameraToolStripMenuItem.Visible = false;
                this.toolStripSeparator1.Visible = false;
                this.toolStripSeparator14.Visible = false;
                this.pathFollowToolStripMenuItem.Visible = false;
                this.toolStripSeparator19.Visible = false;
                this.setGeneratorAttachOnObjectToolStripMenuItem.Visible = false;
                this.updateAssetToolStripMenuItem.Visible = false;
                this.physicBodyManagerToolStripMenuItem.Visible = false;
                this.updateAssetToolStripMenuItem.Visible = false;
                this.mobileEditorToolStripMenuItem.Visible = false;
                this.exportEntityToolStripMenuItem.Visible = true;
                this.exportObjectToolStripMenuItem.Visible = false;
                this.animationManagerToolStripMenuItem.Visible = false;
            }
            
        }

        //DRAG AND DROP EVENT   
        private void treeViewElements_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {

                for (int i = 0; i < this.SelectedNodes.Count; i++)
                {
                    if (this.SelectedNodes[i] is GameElement)
                    {
                        GameElement elem = (GameElement)this.SelectedNodes[i];
                        if (elem.NodeType.Equals("OBJECT") || elem.NodeType.Equals("ENTITY") || elem.NodeType.Equals("LAYER") || elem.NodeType.Equals("STAGE"))
                            DoDragDrop(elem, DragDropEffects.Move);
                        else
                            this.nodeTemp = null;
                    }
                  
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Drag&Drop not allowed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        

        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam,int lParam);

        private void treeViewElements_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                // Set a constant to define the autoscroll region
                const Single scrollRegion = 20;
                // See where the cursor is
                Point pt = this.treeViewElements.PointToClient(Cursor.Position);

                // See if we need to scroll up or down

                if ((pt.Y + scrollRegion) > treeViewElements.Height)
                {

                    // Call the API to scroll down
                    SendMessage(treeViewElements.Handle, (int)277, (int)1, 0);

                }

                else if (pt.Y < (treeViewElements.Top + scrollRegion))
                {

                    // Call thje API to scroll up

                    SendMessage(treeViewElements.Handle, (int)277, (int)0, 0);

                }
            }
            catch (Exception ex)
            {
                    
            }
        }

        private void checkJointIntegrity(CoronaJointure joint)
        {

            //Get joint node
            GameElement jointNode = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, joint);
            if (jointNode != null)
            {
                
                bool hasFoundJoint = false;

                for (int i = 0; i < joint.layerParent.Jointures.Count; i++)
                {
                    if(joint.layerParent.Jointures[i] == joint)
                    {
                        joint.layerParent.Jointures.Remove(joint);
                        hasFoundJoint = true;
                        break;
                    }
                        
                }

                if (hasFoundJoint == false)
                {
                    for (int i = 0; i < joint.layerParent.CoronaObjects.Count; i++)
                    {
                        CoronaObject obj = joint.layerParent.CoronaObjects[i];
                        if (obj.isEntity == true)
                        {
                            for (int j = 0; j < obj.Entity.Jointures.Count; j++)
                            {
                                if (obj.Entity.Jointures[j] == joint)
                                {
                                    obj.Entity.Jointures.Remove(joint);
                                    hasFoundJoint = true;
                                    break;
                                }

                            }
                        }

                        if (hasFoundJoint == true)
                            break;
                    }
                }

                if (hasFoundJoint == true)
                {
                    jointNode.Parent.Nodes.Remove(jointNode);

                    if (joint.coronaObjA.LayerParent != joint.layerParent)
                    {
                        MessageBox.Show("The object \""+joint.coronaObjA.DisplayObject.Name+"\" used in the joint named \""+joint.name+"\" does not exists anymore in the layer \""+joint.layerParent.Name+"\"!\n So the joint has been automatically removed to preserve the project integrity!",
                        "Joint Integrity Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        joint = null;
                        jointNode = null;
                        return;
                    }

                    if(joint.coronaObjB != null && (joint.coronaObjB.LayerParent != joint.layerParent))
                    {
                        MessageBox.Show("The object \"" + joint.coronaObjB.DisplayObject.Name + "\" used in the joint named \"" + joint.name + "\" does not exists anymore in the layer \"" + joint.layerParent.Name + "\"!\n So the joint has been automatically removed to preserve the project integrity!",
                        "Joint Integrity Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        joint = null;
                        jointNode = null;
                        return;

                    }

                    this.newJoint(joint, true, jointNode);
                }

            }
        }

        private List<CoronaJointure> getAllJointsUsedByObject(CoronaObject obj)
        {
            if (obj.LayerParent != null)
            {
                List<CoronaJointure> jointsUsed = new List<CoronaJointure>();
                for (int i = 0; i < obj.LayerParent.Jointures.Count; i++)
                {
                    CoronaJointure joint = obj.LayerParent.Jointures[i];
                    if (joint.coronaObjA == obj || joint.coronaObjB == obj)
                        jointsUsed.Add(joint);
                }

                for (int i = 0; i < obj.LayerParent.CoronaObjects.Count; i++)
                {
                    CoronaObject obj2 = obj.LayerParent.CoronaObjects[i];
                    if (obj2.isEntity == true)
                    {
                        for (int j = 0; j < obj2.Entity.Jointures.Count; j++)
                        {
                            CoronaJointure joint = obj2.Entity.Jointures[j];
                            if (joint.coronaObjA == obj || joint.coronaObjB == obj)
                                jointsUsed.Add(joint);
                        }

                    }
                }

                return jointsUsed;
            }

            return null;
        }

        private void treeViewElements_DragDrop(object sender, DragEventArgs e)
        {

            try
            {
                Point loc = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

                TreeNode destNode = ((TreeView)sender).GetNodeAt(loc);
                GameElement nodeDragged = (GameElement)e.Data.GetData(typeof(GameElement));

                if (destNode != null && nodeDragged != null)
                {
                    
                    if (!destNode.Name.Equals("GAME_ELEMENT"))
                        return;

                    this.treeViewElements.BeginUpdate();
                    //Verifier que le fichier de destination n'est pas le fichier root
                    if (destNode.Parent != null)
                    {
                        //Recuperer le gameElement

                        GameElement elemDest = (GameElement)destNode;
                        if ((elemDest.NodeType.Equals("OBJECT") && nodeDragged.NodeType.Equals("ENTITY"))
                            || (elemDest.NodeType.Equals("ENTITY") && nodeDragged.NodeType.Equals("OBJECT"))
                             || (elemDest.NodeType.Equals("ENTITY") && nodeDragged.NodeType.Equals("ENTITY"))
                             || (elemDest.NodeType.Equals("OBJECT") && nodeDragged.NodeType.Equals("OBJECT")))
                        {

                            if (elemDest != nodeDragged)
                            {

                                //Recuperer les objets concernés
                                CoronaObject objDrag = (CoronaObject)nodeDragged.InstanceObjet;
                                CoronaObject objDest = (CoronaObject)elemDest.InstanceObjet;

                                //Recuperer le layer parent de la dest
                                CoronaLayer layerDep = ((CoronaObject)nodeDragged.InstanceObjet).LayerParent;
                                CoronaLayer layerDest = ((CoronaObject)elemDest.InstanceObjet).LayerParent;

                                bool isOBJDraggedInEntity = (objDrag.EntityParent != null);
                                bool isOBJDraggedEntity = objDrag.isEntity;

                                bool isOBJDestInEntity = (objDest.EntityParent != null);
                                bool isOBJDestEntity = objDest.isEntity;

                                //Cancel CASES
                                // ENTITY TO OBJECT_IN_ENTITY
                                if (isOBJDraggedInEntity == false && isOBJDraggedEntity == true && isOBJDestInEntity == true && isOBJDestEntity == false)
                                {
                                    this.treeViewElements.EndUpdate();
                                    return;
                                }

                                //Deplacer la node
                                nodeDragged.Parent.Nodes.Remove(nodeDragged);

                                // OBJECT TO OBJECT_IN_ENTITY
                                if (isOBJDraggedInEntity == false && isOBJDraggedEntity == false && isOBJDestInEntity == true)
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);
                                    
                                    layerDep.CoronaObjects.Remove(objDrag);
                                    objDest.EntityParent.CoronaObjects.Add(objDrag);
                                    elemDest.Parent.Nodes.Add(nodeDragged);
                                    objDrag.EntityParent = objDest.EntityParent;
                                    objDrag.LayerParent = objDest.LayerParent;


                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }
                                }

                                // OBJECT TO OBJECT || ENTITY TO ENTITY
                                else if ((isOBJDraggedInEntity == false && isOBJDraggedEntity == false && isOBJDestInEntity == false && isOBJDestEntity == false)
                                    ||(isOBJDraggedInEntity == false && isOBJDraggedEntity == true && isOBJDestInEntity == false && isOBJDestEntity == true))
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    layerDep.CoronaObjects.Remove(objDrag);
                                    int indexOfDestInListObjects = layerDest.CoronaObjects.IndexOf(objDest);
                                    if (indexOfDestInListObjects < 0)
                                        indexOfDestInListObjects = 0;

                                    layerDest.CoronaObjects.Insert(indexOfDestInListObjects, objDrag);
                                    elemDest.Parent.Nodes.Insert(destNode.Index, nodeDragged);
                                    objDrag.LayerParent = objDest.LayerParent;


                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }
                                }

                                // OBJECT TO ENTITY
                                else if (isOBJDraggedInEntity == false && isOBJDraggedEntity == false && isOBJDestInEntity == false && isOBJDestEntity == true) 
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    layerDep.CoronaObjects.Remove(objDrag);
                                    objDest.Entity.CoronaObjects.Add(objDrag);
                                    elemDest.Nodes.Add(nodeDragged);
                                    objDrag.EntityParent = objDest.Entity;
                                    objDrag.LayerParent = objDest.LayerParent;

                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }
                                }

                                // ENTITY TO OBJECT
                                else if (isOBJDraggedInEntity == false && isOBJDraggedEntity == true && isOBJDestInEntity == false && isOBJDestEntity == false) 
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    layerDep.CoronaObjects.Remove(objDrag);
                                    int indexOfDestInListObjects = layerDest.CoronaObjects.IndexOf(objDest);
                                    if (indexOfDestInListObjects < 0)
                                        indexOfDestInListObjects = 0;

                                    layerDest.CoronaObjects.Insert(indexOfDestInListObjects, objDrag);
                                    elemDest.Parent.Nodes.Insert(destNode.Index, nodeDragged);
                                    objDrag.LayerParent = objDest.LayerParent;
                                    for (int i = 0; i < objDrag.Entity.CoronaObjects.Count; i++)
                                    {
                                        objDrag.Entity.CoronaObjects[i].LayerParent = objDest.LayerParent;
                                    }

                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }

                                }
                                // OBJECT_IN_ENTITY TO OBJECT_IN_ENTITY
                                else if (isOBJDraggedInEntity == true && isOBJDraggedEntity == false && isOBJDestInEntity == true && isOBJDestEntity == false)
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    objDrag.EntityParent.CoronaObjects.Remove(objDrag);
                                    objDrag.EntityParent = null;

                                    objDest.EntityParent.CoronaObjects.Add(objDrag);
                                    elemDest.Parent.Nodes.Add(nodeDragged);
                                    objDrag.EntityParent = objDest.EntityParent;
                                    objDrag.LayerParent = objDest.LayerParent;

                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }

                                }
                                // OBJECT_IN_ENTITY TO OBJECT
                                else if (isOBJDraggedInEntity == true && isOBJDraggedEntity == false && isOBJDestInEntity == false && isOBJDestEntity == false)
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    objDrag.EntityParent.CoronaObjects.Remove(objDrag);
                                    objDrag.EntityParent = null;

                                    int indexOfDestInListObjects = layerDest.CoronaObjects.IndexOf(objDest);
                                    if (indexOfDestInListObjects < 0)
                                        indexOfDestInListObjects = 0;

                                    layerDest.CoronaObjects.Insert(indexOfDestInListObjects, objDrag);
                                    elemDest.Parent.Nodes.Insert(destNode.Index, nodeDragged);
                                    objDrag.LayerParent = objDest.LayerParent;

                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }
                                }
                                // OBJECT_IN_ENTITY TO ENTITY
                                else if (isOBJDraggedInEntity == true && isOBJDraggedEntity == false && isOBJDestInEntity == false && isOBJDestEntity == true)
                                {
                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    objDrag.EntityParent.CoronaObjects.Remove(objDrag);
                                    objDrag.EntityParent = null;

                                    objDest.Entity.CoronaObjects.Add(objDrag);
                                    elemDest.Nodes.Add(nodeDragged);
                                    objDrag.EntityParent = objDest.Entity;
                                    objDrag.LayerParent = objDest.LayerParent;

                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }
                                }

                                
                            }
                        }

                        else if (elemDest.NodeType.Equals("LAYER") && nodeDragged.NodeType.Equals("LAYER"))
                        {
                            if (elemDest != nodeDragged)
                            {
                                //Deplacer la node
                                nodeDragged.Parent.Nodes.Remove(nodeDragged);

                                //Recuperer les objets concernés
                                CoronaLayer objDrag = (CoronaLayer)nodeDragged.InstanceObjet;
                                CoronaLayer objDest = (CoronaLayer)elemDest.InstanceObjet;

                                Scene sceneSourceParent = objDrag.SceneParent;
                                Scene sceneDestParent = objDest.SceneParent;

                                sceneSourceParent.Layers.Remove(objDrag);

                                int index = destNode.Index;
                                sceneDestParent.Layers.Insert(index, objDrag);
                                elemDest.Parent.Nodes.Insert(index, nodeDragged);

                                objDrag.SceneParent = sceneDestParent;
                            }
                        }
                        else if (elemDest.NodeType.Equals("STAGE") && nodeDragged.NodeType.Equals("STAGE"))
                        {
                            if (elemDest != nodeDragged)
                            {
                                //Deplacer la node
                                nodeDragged.Parent.Nodes.Remove(nodeDragged);

                                //Recuperer les objets concernés
                                Scene objDrag = (Scene)nodeDragged.InstanceObjet;
                                Scene objDest = (Scene)elemDest.InstanceObjet;

                                CoronaGameProject projectParent = objDrag.projectParent;

                                projectParent.Scenes.Remove(objDrag);

                                projectParent.Scenes.Insert(destNode.Index + 1, objDrag);
                                elemDest.Parent.Nodes.Insert(destNode.Index + 1, nodeDragged);

                            }
                        }
                        else if (elemDest.NodeType.Equals("STAGE") && nodeDragged.NodeType.Equals("LAYER"))
                        {


                            //Recuperer les objets concernés
                            CoronaLayer objDrag = (CoronaLayer)nodeDragged.InstanceObjet;
                            Scene objDest = (Scene)elemDest.InstanceObjet;

                            if (objDrag.SceneParent != objDest)
                            {
                                //Deplacer la node
                                nodeDragged.Parent.Nodes.Remove(nodeDragged);

                                Scene sceneSourceParent = objDrag.SceneParent;
                                sceneSourceParent.Layers.Remove(objDrag);


                                objDest.Layers.Insert(objDest.Layers.Count, objDrag);
                                elemDest.Nodes.Insert(elemDest.Nodes.Count, nodeDragged);
                                objDrag.SceneParent = objDest;

                            }

                        }
                        else if (elemDest.NodeType.Equals("LAYER") && (nodeDragged.NodeType.Equals("OBJECT") || nodeDragged.NodeType.Equals("ENTITY")))
                        {


                            //Recuperer les objets concernés
                            CoronaObject objDrag = (CoronaObject)nodeDragged.InstanceObjet;
                            CoronaLayer objDest = (CoronaLayer)elemDest.InstanceObjet;

                            if (objDrag.EntityParent == null)
                            {
                                if (objDrag.LayerParent != objDest)
                                {
                                    //Deplacer la node
                                    nodeDragged.Parent.Nodes.Remove(nodeDragged);

                                    List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                    CoronaLayer layerSourceParent = objDrag.LayerParent;
                                    layerSourceParent.CoronaObjects.Remove(objDrag);


                                    objDest.CoronaObjects.Insert(objDest.CoronaObjects.Count, objDrag);
                                    elemDest.Nodes.Insert(elemDest.Nodes.Count, nodeDragged);
                                    objDrag.LayerParent = objDest;

                                    if (objDrag.isEntity == true)
                                    {
                                        for (int i = 0; i < objDrag.Entity.CoronaObjects.Count; i++)
                                        {
                                            joints.AddRange(getAllJointsUsedByObject(objDrag.Entity.CoronaObjects[i]));
                                            objDrag.Entity.CoronaObjects[i].LayerParent = objDest;
                                        }

                                        for (int i = 0; i < objDrag.Entity.Jointures.Count; i++)
                                        {
                                            objDrag.Entity.Jointures[i].layerParent = objDest;
                                        }
                                    }

                                    for (int i = 0; i < joints.Count; i++)
                                    {
                                        this.checkJointIntegrity(joints[i]);
                                    }

                                }
                            }
                            else
                            {
                                List<CoronaJointure> joints = getAllJointsUsedByObject(objDrag);

                                //Deplacer la node
                                nodeDragged.Parent.Nodes.Remove(nodeDragged);

                                objDrag.EntityParent.CoronaObjects.Remove(objDrag);
                                objDrag.EntityParent = null;

                                objDest.CoronaObjects.Insert(objDest.CoronaObjects.Count, objDrag);
                                elemDest.Nodes.Insert(elemDest.Nodes.Count, nodeDragged);
                                objDrag.LayerParent = objDest;


                                for (int i = 0; i < joints.Count; i++)
                                {
                                    this.checkJointIntegrity(joints[i]);
                                }
                            }

                        }

                    }
                    this.treeViewElements.EndUpdate();

                    if (this.MainForm.isFormLocked == false)
                        GorgonLibrary.Gorgon.Go();

                }

            }
            catch (Exception ex)
            {
                this.treeViewElements.EndUpdate();
            }
           
        }

        private void treeViewElements_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }


        //CONTEXT MENU STRIP OBJECTS EVENTS
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.MainForm.removeObjectsSelected_Click(null, null);
 
        }

        private void physicBodyManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CoronaObject> listObj = this.MainForm.sceneEditorView1.objectsSelected;
            if (listObj != null)
            {
                if(listObj.Count > 0)
                    this.MainForm.initPhysicBodyManager(listObj[0]);
            }
           
        }

      

 

        private void collisionMaskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProjectSelected != null)
            {
                if (this.SceneSelected != null)
                {
                    if (!this.MainForm.getGameEditorTabControl().Controls.Contains(this.MainForm.getCollisionMaskTabPage()))
                    {
                        this.MainForm.getGameEditorTabControl().Controls.Add(this.MainForm.getCollisionMaskTabPage());
                    }

                    this.MainForm.getCollisionMask().Init(this.SceneSelected);
                }
                else
                    MessageBox.Show("No stage selected !\n Please select a stage first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("No project loaded !\n Please create/load a project first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SceneSelected != null)
            {
               DialogResult result = MessageBox.Show("Are you sure to remove this stage from the project ?", 
                                "Removing stage " + this.SceneSelected.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);


               if (result == DialogResult.Yes)
               {
                   GameElement elem = this.getNodeFromObjectInstance(this.treeViewElements.Nodes, this.SceneSelected);

                   this.MainForm.cgEeditor1.closeFile(this.ProjectSelected.SourceFolderPath + "\\" + this.SceneSelected.Name + ".lua");
                   File.Delete(this.ProjectSelected.SourceFolderPath + "\\" + this.SceneSelected.Name + ".lua");
                   this.MainForm.CurrentProject.Scenes.Remove(this.SceneSelected);

                   this.MainForm.sceneEditorView1.GraphicsContentManager.CleanSceneGraphics(this.SceneSelected, true, true);

                   this.SceneSelected = null;
                   elem.Remove();
                   elem = null;

                   this.MainForm.sceneEditorView1.setModeNormal();

                   //Try to get last scene
                   if (this.ProjectSelected.Scenes.Count > 0)
                   {
                       GameElement nodeScene = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.ProjectSelected.Scenes[0]);
                       if (nodeScene != null)
                       {
                           this.treeViewElements_NodeMouseClick(null,new TreeNodeMouseClickEventArgs(nodeScene,MouseButtons.Left,1,0,0));
                       }
                   }
                   
               }
            }
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.SceneSelected != null)
            {
                GameElement sceneNode = getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, SceneSelected);
                if (sceneNode != null)
                {

                    //Recuperer la node selectionnée
                    if (this.treeViewElements.SelectedNode.Name.Equals("GAME_ELEMENT"))
                    {
                        GameElement elem = (GameElement)this.treeViewElements.SelectedNode;
                        if (elem.NodeType.Equals("SPRITESHEET"))
                        {
                            CoronaSpriteSheet sheet = (CoronaSpriteSheet)elem.InstanceObjet;
                            this.SceneSelected.SpriteSheets.Remove(sheet);
                            elem.Remove();
                        }
                        else if (elem.NodeType.Equals("SPRITESET"))
                        {
                            CoronaSpriteSet set = (CoronaSpriteSet)elem.InstanceObjet;
                            this.SceneSelected.SpriteSets.Remove(set);
                            elem.Remove();
                        }
                    }

                    if (this.MainForm.isFormLocked == false)
                        GorgonLibrary.Gorgon.Go();
                }
            }
        }

        private void sceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MainForm.addSceneToProject(null);
        }


        private void removeJointBt_Click(object sender, EventArgs e)
        {
            this.removeJointSelected();
        }

        
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MainForm.openNewProjectPanel();
        }


        

        private void removeAudioBt_Click(object sender, EventArgs e)
        {
            if (AudioObjectSelected != null)
            {

                //Recuperer la node de l'instance
                GameElement elem = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes["AUDIOS"].Nodes,this.AudioObjectSelected);
                if (elem != null)
                {
                    if (this.SelectedNodes.Contains(elem))
                        this.SelectedNodes.Remove(elem);

                    elem.Remove();
                }
                   

                this.ProjectSelected.AudioObjects.Remove(AudioObjectSelected);
                AudioObjectSelected = null;


            }
        }

        private void removeLayerBt_Click(object sender, EventArgs e)
        {
            if (this.LayerSelected != null)
            {
                //Recuperer la node de l'instance
                GameElement elem = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.LayerSelected);
                this.removeLayer(elem);
            }
        }

        private void setFocusCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MainForm.sceneEditorView1.objectsSelected.Count >0)
            {
                if (this.SceneSelected.Camera.objectFocusedByCamera != null)
                {
                    if (this.SceneSelected.Camera.objectFocusedByCamera == this.MainForm.sceneEditorView1.objectsSelected[0])
                    {
                        this.SceneSelected.Camera.objectFocusedByCamera = null;
                    }
                    else
                        this.SceneSelected.Camera.setObjectFocusedByCamera(this.MainForm.sceneEditorView1.objectsSelected[0]);
                }
                else
                    this.SceneSelected.Camera.setObjectFocusedByCamera(this.MainForm.sceneEditorView1.objectsSelected[0]);


                bool isCustomBuild = this.MainForm.IsCustomBuild;

                float XRatio = (float)this.MainForm.currentTargetResolution.Resolution.Width / (float)this.MainForm.CurrentProject.width;
                float YRatio = (float)this.MainForm.currentTargetResolution.Resolution.Height / (float)this.MainForm.CurrentProject.height;

                //Mettre a jour le fichier lua
                this.MainForm.cgEeditor1.RefreshSceneLuaCode(this.SceneSelected,isCustomBuild,XRatio,YRatio);

            }
        }

        //----------------------------------------------------------------
        //------------------TILES MAP -----------------------------------
        //----------------------------------------------------------------

        private void newToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.LayerSelected.newTilesMap();
            this.newTilesMap(this.LayerSelected.TilesMap);
        }

        private void removeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.removeTilesMap();
        }

        private void openTilesMapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LayerSelected != null)
            {
                if (this.LayerSelected.TilesMap == null)
                {

                    this.LayerSelected.newTilesMap();
                    this.newTilesMap(this.LayerSelected.TilesMap);
                }

                this.MainForm.openTilesMapEditor(this.LayerSelected.TilesMap);
            }
        }

      
        //--------------------UNDO REDO -----------------
       /* public void undo()
        {
            if (this.ProjectSelected != null)
            {
                Scene scene = this.MainForm.UndoRedo.Undo();
                if (scene != null)
                {
                    for (int i = 0; i < this.ProjectSelected.Scenes.Count; i++)
                    {

                        if (this.ProjectSelected.Scenes[i].Name.Equals(scene.Name))
                        {
                            this.ProjectSelected.Scenes[i] = scene;
                            this.loadProject(this.ProjectSelected);

                            for (int j = 0; j < scene.Layers.Count; j++)
                            {
                                CoronaLayer layer = scene.Layers[j];
                                if (layer.TilesMap != null)
                                {
                                    layer.TilesMap.reloadFromFile(scene.projectParent.SourceFolderPath);
                                }
                            }

                            selectCorrectMode();

                            if (this.MainForm.isFormLocked == false)
                                 this.MainForm.sceneEditorView1.Refresh();

                            return;
                        }

                    }
                }
            }
        }

        public void Redo()
        {
            if (this.ProjectSelected != null)
            {
                Scene scene = this.MainForm.UndoRedo.ReDo();
                if (scene != null)
                {
                    for (int i = 0; i < this.ProjectSelected.Scenes.Count; i++)
                    {

                        if (this.ProjectSelected.Scenes[i].Name.Equals(scene.Name))
                        {
                            this.ProjectSelected.Scenes[i] = scene;
                            this.loadProject(this.ProjectSelected);
                            
                            for (int j = 0; j < scene.Layers.Count; j++)
                            {
                                CoronaLayer layer = scene.Layers[j];
                                if (layer.TilesMap != null)
                                {
                                    layer.TilesMap.reloadFromFile(scene.projectParent.SourceFolderPath);
                                }
                            }

                            selectCorrectMode();

                            if (this.MainForm.isFormLocked == false)
                                this.MainForm.sceneEditorView1.Refresh();
                            return;
                        }

                    }
                }
            }
        }*/


        public void selectCorrectMode()
        {
            if (this.MainForm.sceneEditorView1.CurentCalque.Equals("STAGE"))
            {
                int index = this.MainForm.sceneEditorView1.indexSceneSelected;
                if (index < this.ProjectSelected.Scenes.Count && index != -1)
                    this.MainForm.sceneEditorView1.setModeSceneEditor(this.ProjectSelected.Scenes[index]);

            }
            else if (this.MainForm.sceneEditorView1.CurentCalque.Equals("LAYER"))
            {
                int indexScene = this.MainForm.sceneEditorView1.indexSceneSelected;
                int indexLayer = this.MainForm.sceneEditorView1.indexLayerSelected;
                if (indexScene < this.ProjectSelected.Scenes.Count && indexScene != -1)
                {
                    Scene sceneSelected = this.ProjectSelected.Scenes[indexScene];
                    if (indexLayer < sceneSelected.Layers.Count && indexLayer != -1)
                    {
                        this.MainForm.sceneEditorView1.setModeLayerEditor(sceneSelected.Layers[indexLayer]);
                    }

                }


            }
        }

        private void joystickToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.LayerSelected != null)
            {
                if(this.LayerSelected.Controls == null)
                    this.LayerSelected.Controls = new List<CoronaControl>();

                
                JoystickControl joy = new JoystickControl(this.LayerSelected);
                this.LayerSelected.Controls.Add(joy);

                joy.joystickLocation = this.LayerSelected.SceneParent.Camera.SurfaceFocus.Location;
                this.newCoronaControl(joy);
                GorgonLibrary.Gorgon.Go();
            }
        }

        private void deleteControlBt_Click(object sender, EventArgs e)
        {
            if (this.ControlSelected != null)
                this.removeCoronaControl(ControlSelected);
        }


        public void DupplicateSelectedObjects()
        {
            CoronaObject[] listObjSelected = new CoronaObject[this.MainForm.sceneEditorView1.objectsSelected.Count];
            this.MainForm.sceneEditorView1.objectsSelected.CopyTo(listObjSelected);
            this.MainForm.sceneEditorView1.selectObject(null, false);
            if (listObjSelected.Length > 0)
            {
                for (int j = 0; j < listObjSelected.Length; j++)
                {
                    CoronaObject obj = listObjSelected[j];

                    CoronaObject newObject = obj.cloneObject(obj.LayerParent, true, false);
                    if (newObject != null)
                    {
                        if (obj.EntityParent != null)
                        {
                            obj.EntityParent.addObject(newObject);
                        }

                        this.MainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteStates(newObject,
                            this.MainForm.sceneEditorView1.CurrentScale, this.MainForm.sceneEditorView1.getOffsetPoint());

                        this.newCoronaObject(newObject);

                       
                        this.MainForm.sceneEditorView1.selectObject(newObject, true);
                        

                    }

                }

                this.MainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                if (this.MainForm.imageObjectsPanel1.ShouldBeRefreshed == true)
                {
                    this.MainForm.imageObjectsPanel1.RefreshCurrentAssetProject();
                    this.MainForm.imageObjectsPanel1.ShouldBeRefreshed = false;
                }

                this.refreshNodesSelectedSceneEditor();

                if (this.MainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
            }

            listObjSelected = null;
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DupplicateSelectedObjects();
        }

        private void oKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int nbCopies = -1;
            if (int.TryParse(this.nbOfCopyTxtBx.Text, out nbCopies) == true)
            {
                List<CoronaObject> listObjSelected = this.MainForm.sceneEditorView1.objectsSelected;
                if (listObjSelected.Count> 0)
                {
                    for (int j = 0; j < listObjSelected.Count; j++)
                    {
                        CoronaObject obj = listObjSelected[j];
                        for (int i = 0; i < nbCopies; i++)
                        {

                            CoronaObject newObject = obj.cloneObject(obj.LayerParent,true,false);
                            obj = newObject;
                            if (newObject != null)
                            {
                                if (newObject.DisplayObject != null)
                                {
                                    
                                    this.newCoronaObject(newObject);
                                  
                                    this.CoronaObjectSelected = newObject;
                                }
                            }
                        }
                    }

                    this.MainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                    if (this.MainForm.imageObjectsPanel1.ShouldBeRefreshed == true)
                    {
                        this.MainForm.imageObjectsPanel1.RefreshCurrentAssetProject();
                        this.MainForm.imageObjectsPanel1.ShouldBeRefreshed = false;
                    }

                    if (this.MainForm.isFormLocked == false)
                        GorgonLibrary.Gorgon.Go();
                }
            }
            else
            {
                MessageBox.Show("Only numbers are allowed !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void languageManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MainForm.initLangageManager();
        }

        private void removeTilesMapBt_Click(object sender, EventArgs e)
        {
            if (this.LayerSelected != null && this.LayerSelected.TilesMap != null)
            {
                TreeNode node = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes,this.LayerSelected.TilesMap);
                node.Remove();

                this.MainForm.sceneEditorView1.GraphicsContentManager.CleanTileMap(this.LayerSelected.TilesMap, true, true);
                this.LayerSelected.TilesMap = null;
                this.MainForm.closeTilesMapEditor();

               
                this.MainForm.Refresh();
            }
        }

      
        //--------------------------------------------------------------------------------------------
        //-------------------- WIDGET CONTROL SECTION -----------------------------------------
        //--------------------------------------------------------------------------------------------
        //--- For the TABBAR WIDGET ----

        private void setTabBarButtonsToolStrip_Click(object sender, EventArgs e)
        {
            WidgetTabBar tabBar = (WidgetTabBar)this.WidgetSelected;
            this.MainForm.openTabBarWidgetPanel(tabBar);

        }

        //--- For the PICKERWHEEL WIDGET ----
        private void setColumnsBt_Click(object sender, EventArgs e)
        {
            WidgetPickerWheel pickerW = (WidgetPickerWheel)this.WidgetSelected;
            this.MainForm.openPickerWheelWidgetPanel(pickerW);
        }

        private void removeToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (this.WidgetSelected != null)
            {
                this.removeCoronaWidget(this.WidgetSelected);
            }
        }

        private void removeToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (this.WidgetSelected != null)
            {
                this.removeCoronaWidget(this.WidgetSelected);
            }
        }

        private void importStageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //Recuperer la scene
            if (this.ProjectSelected != null)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = ".krs";
                open.AddExtension = false;

                //Configure allowed extensions
                //
                open.Filter = "Krea Scene Files (*.krs)|*.krs";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    
                    if (this.MainForm.mainBackWorker.IsBusy == false)
                    {
                        this.MainForm.filenameSelected = open.FileName;
                        this.MainForm.currentWorkerAction = "ACTION_IMPORTSTAGE";
                        this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_IMPORTSTAGE");
                    }
                    else
                    {
                        MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }
            }
            else
            {
                MessageBox.Show("No project opened !\nPlease load or create a project first !", "No project loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void exportStageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SceneSelected != null)
            {
                FolderBrowserDialog folderB = new FolderBrowserDialog();

                if (folderB.ShowDialog() == DialogResult.OK)
                {
                    string directoryName = folderB.SelectedPath;

                    if (this.MainForm.mainBackWorker.IsBusy == false)
                    {
                        this.MainForm.directorySelectedDest = directoryName;
                        this.MainForm.currentWorkerAction = "ACTION_EXPORTSTAGE";
                        this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_EXPORTSTAGE");
                    }
                    else
                    {
                        MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //--------------------- MOBILE EDITOR ---------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void loadButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.removeAttributeToolStripMenuItem_Click(null, null);
            this.CoronaObjectSelected.otherAttribute = "LOAD";
            this.loadButtonToolStripMenuItem.Visible = false;
        }

        private void saveButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "SAVE";
                this.saveButtonToolStripMenuItem.Visible = false;
            }
           
        }

        private void texturesModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_TEXTURE";
                this.texturesModeButtonToolStripMenuItem.Visible = false;
            }
        }

        private void objectsModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_OBJECT";
                this.objectsModeButtonToolStripMenuItem.Visible = false;
            }
        }

        private void rectangleSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_RECTANGLE";
                this.rectangleSelectionToolStripMenuItem.Visible = false;
            }
        }

        private void by1SelectionButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_ONEBYONE";
                this.by1SelectionButtonToolStripMenuItem.Visible = false;
            }
        }

        private void allSelectionButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_ALL";
                this.allSelectionButtonToolStripMenuItem.Visible = false;
            }
        }

        private void applyModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_APPLY";
                this.applyModeButtonToolStripMenuItem.Visible = false;
            }
        }

        private void removeModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_REMOVE";
                this.removeModeButtonToolStripMenuItem.Visible = false;
            }
        }

        private void scrollingModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_SCROLLING";
                this.scrollingModeButtonToolStripMenuItem.Visible = false;
            }
        }

        private void editionModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_EDITION";
                this.editionModeButtonToolStripMenuItem.Visible = false;
            }
        }

        private void collisionsModeButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.removeAttributeToolStripMenuItem_Click(null, null);
                this.CoronaObjectSelected.otherAttribute = "MODE_COLLISIONS";
                this.collisionsModeButtonToolStripMenuItem.Visible = false;
            }
        }



        private void removeAttributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                if (this.CoronaObjectSelected.otherAttribute.Equals("LOAD"))
                {
                    this.loadButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("SAVE"))
                {
                    this.saveButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_TEXTURE"))
                {
                    this.texturesModeButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_OBJECT"))
                {
                    this.objectsModeButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_RECTANGLE"))
                {
                    this.rectangleSelectionToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_ONEBYONE"))
                {
                    this.by1SelectionButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_ALL"))
                {
                    this.allSelectionButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_APPLY"))
                {
                    this.applyModeButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_REMOVE"))
                {
                    this.removeModeButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_SCROLLING"))
                {
                    this.scrollingModeButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_EDITION"))
                {
                    this.editionModeButtonToolStripMenuItem.Visible = true;
                }
                else if (this.CoronaObjectSelected.otherAttribute.Equals("MODE_COLLISIONS"))
                {
                    this.collisionsModeButtonToolStripMenuItem.Visible = true;
                }

                this.CoronaObjectSelected.otherAttribute = "";
            }
           
        }

        private void tilesmapEditorSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool res = false;
            bool hasTileMap = false;

            for (int i = 0; i < this.ProjectSelected.Scenes.Count; i++)
            {
                if (this.ProjectSelected.Scenes[i].Name.Equals("mapeditormobile"))
                {
                    res = true;
                    break;
                }

                for (int j = 0; j < this.ProjectSelected.Scenes[i].Layers.Count; j++)
                {
                    if (this.ProjectSelected.Scenes[i].Layers[j].TilesMap != null)
                    {
                        hasTileMap = true;
                        break;
                    }
                }
            }

            if (res == false)
            {
                if (hasTileMap == true)
                {
                    this.ProjectSelected.IncludeTilesMapEditorMobile = true;
                    //Ajouter une scene au projet
                    this.MainForm.addSceneToProject("mapeditormobile");
                }
                else
                {
                    MessageBox.Show("Cannot add a map editor scene if no Tile Map is inserted into the current project!\nPlease create a Tile Map first!", "Cannot create scene", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
            else
            {
                MessageBox.Show("Cannot insert several map editor scenes into the same project!","Cannot create scene", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        //----------------------------------------------------------------------------------------------------------
        //-------------------------------Snippets Section ----------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(this.ProjectSelected != null)
            {
                if (this.SnippetSelected != null)
                {
                    
                    GameElement elem = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes["SNIPPETS"].Nodes, this.SnippetSelected);
                    if (elem != null)
                    {
                        if (this.SelectedNodes.Contains(elem))
                            this.SelectedNodes.Remove(elem);

                        elem.Remove();
                    }
                        

                    this.ProjectSelected.Snippets.Remove(this.SnippetSelected);
                    this.SnippetSelected = null;

                    this.MainForm.cgEeditor1.RefreshSnippetLuaCode(this.ProjectSelected);
                }
            }
           
        }


        //----------------------------------------------------------------------------------------------------------
        //-------------------------------Path follow Section ----------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        private void startPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.CoronaObjectSelected.PathFollow.Path.Clear();
                //Add the position of the object into the path
                Rectangle rect = this.CoronaObjectSelected.DisplayObject.SurfaceRect;
                Point point = new Point(rect.Location.X + rect.Width /2,rect.Location.Y + rect.Height /2);
                this.CoronaObjectSelected.PathFollow.Path.Add(point);

                this.CoronaObjectSelected.PathFollow.isEnabled = true;
            }

            this.MainForm.SetModeObject();
            this.MainForm.sceneEditorView1.Mode = "PATH_FOLLOW";
        }

        private void validToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MainForm.SetModeObject();
        }

        //----------------------------------------------------------------------------------------------------------
        //-------------------------------Path follow Section ----------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        private void setGeneratorAttachOnObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                this.MainForm.SetModeObject();
                if (this.CoronaObjectSelected.objectAttachedToGenerator == null)
                    this.MainForm.sceneEditorView1.Mode = "GENERATOR_ATTACH";
                else
                {
                    this.CoronaObjectSelected.objectAttachedToGenerator = null;
                    this.MainForm.sceneEditorView1.Mode = "NONE";
                }
                    
            }
            
        }

        private void openInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProjectSelected != null)
            {
                Process.Start(this.ProjectSelected.ProjectPath);
            }
        }

        private void addExternalFilesToProjectSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProjectSelected != null)
            {
                try
                {
                    OpenFileDialog openF = new OpenFileDialog();
                    openF.Multiselect = true;
                    openF.Filter = "All files type (*.*)|*.*";
                    if (openF.ShowDialog() == DialogResult.OK)
                    {
                        int count = 0;
                        for (int i = 0; i < openF.FileNames.Length; i++)
                        {
                            string filename = openF.FileNames[i];
                            if (File.Exists(filename))
                            {
                                File.Copy(filename, this.ProjectSelected.SourceFolderPath + "\\" + openF.SafeFileNames[i], true);
                                count++;
                            }
                            else
                            {
                                MessageBox.Show("The file "+filename+" does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        
                        if(count >1)
                             MessageBox.Show(count+ " files have been successfully imported!", "Importation succeed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show(count + " file has been successfully imported!", "Importation succeed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during file importation!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void addMaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProjectSelected != null)
            {
                try
                {
                    OpenFileDialog openF = new OpenFileDialog();
                    openF.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Lua Repository";
                    openF.Multiselect = true;
                    openF.Filter = "Lua files (*.lua)|*.lua";
                    if (openF.ShowDialog() == DialogResult.OK)
                    {
                        int count = 0;
                        for (int i = 0; i < openF.FileNames.Length; i++)
                        {
                            string filename = openF.FileNames[i];
                            if (File.Exists(filename))
                            {
                                File.Copy(filename, this.ProjectSelected.SourceFolderPath + "\\" + openF.SafeFileNames[i], true);
                                count++;
                            }
                            else
                            {
                                MessageBox.Show("The file " + filename + " does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        if (count > 1)
                            MessageBox.Show(count + " files have been successfully imported!", "Importation succeed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show(count + " file has been successfully imported!", "Importation succeed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during file importation!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void duplicateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.ProjectSelected != null)
            {
                if (this.SceneSelected != null)
                {
                    if (this.LayerSelected != null)
                    {
                        CoronaLayer newLayer = this.LayerSelected.Clone();
                        if (newLayer != null)
                        {
                            this.SceneSelected.Layers.Add(newLayer);
                            this.loadProject(this.ProjectSelected);
                        }

                        try
                        {
                            this.treeViewElements.BeginUpdate();
                            TreeNode nodeScene = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, this.SceneSelected);
                            if (nodeScene != null)
                            {
                                nodeScene.ExpandAll();
                            }
                            this.treeViewElements.EndUpdate();
                        }
                        catch (Exception ex)
                        {
                            this.treeViewElements.EndUpdate();
                        }
                    }
                }
            }
            
        }

        private void GameElementTreeView_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }


        protected bool isParent(TreeNode parentNode, TreeNode childNode)
        {
            if (parentNode == childNode)
                return true;

            TreeNode n = childNode;
            bool bFound = false;
            while (!bFound && n != null)
            {
                n = n.Parent;
                bFound = (n == parentNode);
            }
            return bFound;
        }


        protected void paintSelectedNodes()
        {
            foreach (TreeNode n in this.SelectedNodes)
            {
                n.BackColor = SystemColors.Highlight;
                n.ForeColor = SystemColors.HighlightText;

                
            }
        }

        protected void removePaintFromNodes()
        {
            if (SelectedNodes.Count == 0) return;

          
            Color back = this.treeViewElements.BackColor;
            Color fore = this.treeViewElements.ForeColor;
            
            foreach (TreeNode n in SelectedNodes)
            {
                if (n is GameElement)
                {
                    GameElement elem = n as GameElement;
                    fore = elem.getFontColor();
                }
                n.BackColor = back;
                n.ForeColor = fore;
            }
        }

        private void treeViewElements_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(hasChecked == false)
            {
            // e.Node is the current node exposed by the base TreeView control
            try
            {
                this.treeViewElements.BeginUpdate();

                bool bControl = (ModifierKeys == Keys.Control);
                bool bShift = (ModifierKeys == Keys.Shift);

                removePaintFromNodes();

                if (this.SceneSelected != null)
                {
                    for (int i = 0; i < this.SceneSelected.Layers.Count; i++)
                    {
                        this.SceneSelected.Layers[i].deselectAllObjects();
                    }

                }

                this.MainForm.sceneEditorView1.objectsSelected.Clear();

                if (e.Node is GameElement)
                {
                    GameElement elem = e.Node as GameElement;
                    bool isFirstCoronaObject = true;
                    for (int i = 0; i < this.SelectedNodes.Count; i++)
                    {
                        if (this.SelectedNodes[i] is GameElement)
                        {
                            GameElement elem2 = this.SelectedNodes[i] as GameElement;
                            if (elem2.InstanceObjet is CoronaObject)
                            {
                                CoronaObject obj = elem2.InstanceObjet as CoronaObject;
                                if (obj.isEntity == true)
                                {
                                    isFirstCoronaObject = false;
                                    break;
                                }
                                //else if (obj.EntityParent != null)
                                //{
                                //    isFirstCoronaObject = false;
                                //    break;
                                //}

                            }
                            else
                            {
                                isFirstCoronaObject = false;
                                break;
                            }
                        }
                        else
                        {
                            isFirstCoronaObject = false;
                            break;
                        }
                    }

                    if (elem.NodeType.Equals("ENTITY"))
                        isFirstCoronaObject = false;

                    if (elem.InstanceObjet is CoronaObject && isFirstCoronaObject == true)
                    {
                        if (bControl)
                        {
                            if (!this.SelectedNodes.Contains(e.Node)) // new node ?
                            {
                                this.SelectedNodes.Add(e.Node);
                            }

                        }
                        else
                        {
                            if (bShift)
                            {
                                Queue myQueue = new Queue();

                                TreeNode uppernode = m_firstNode;
                                TreeNode bottomnode = e.Node;

                                // case 1 : begin and end nodes are parent
                                bool bParent = isParent(m_firstNode, e.Node);
                                if (!bParent)
                                {
                                    bParent = isParent(bottomnode, uppernode);
                                    if (bParent) // swap nodes
                                    {
                                        TreeNode t = uppernode;
                                        uppernode = bottomnode;
                                        bottomnode = t;
                                    }
                                }
                                if (bParent)
                                {
                                    TreeNode n = bottomnode;
                                    while (n != uppernode.Parent)
                                    {
                                        if (!this.SelectedNodes.Contains(n)) // new node ?
                                            myQueue.Enqueue(n);

                                        n = n.Parent;
                                    }
                                }
                                // case 2 : nor the begin nor the
                                // end node are descendant one another
                                else
                                {
                                    // are they siblings ?                 

                                    if ((uppernode.Parent == null && bottomnode.Parent == null)
                                          || (uppernode.Parent != null &&
                                          uppernode.Parent.Nodes.Contains(bottomnode)))
                                    {
                                        int nIndexUpper = uppernode.Index;
                                        int nIndexBottom = bottomnode.Index;
                                        if (nIndexBottom < nIndexUpper) // reversed?
                                        {
                                            TreeNode t = uppernode;
                                            uppernode = bottomnode;
                                            bottomnode = t;
                                            nIndexUpper = uppernode.Index;
                                            nIndexBottom = bottomnode.Index;
                                        }

                                        TreeNode n = uppernode;
                                        while (nIndexUpper <= nIndexBottom)
                                        {
                                            if (!this.SelectedNodes.Contains(n)) // new node ?
                                                myQueue.Enqueue(n);

                                            n = n.NextNode;

                                            nIndexUpper++;
                                        } // end while

                                    }
                                    else
                                    {
                                        if (!this.SelectedNodes.Contains(uppernode))
                                            myQueue.Enqueue(uppernode);
                                        if (!this.SelectedNodes.Contains(bottomnode))
                                            myQueue.Enqueue(bottomnode);
                                    }

                                }

                                foreach (object objQueue in myQueue)
                                {
                                    TreeNode node = objQueue as TreeNode;
                                    this.SelectedNodes.Add(node);
                                }



                                // let us chain several SHIFTs if we like it
                                m_firstNode = e.Node;

                            } // end if m_bShift
                            else
                            {
                                // in the case of a simple click, just add this item
                                if (this.SelectedNodes != null && this.SelectedNodes.Count > 0)
                                {
                                    removePaintFromNodes();
                                    this.SelectedNodes.Clear();
                                }
                                this.SelectedNodes.Add(e.Node);
                            }
                        }

                        foreach (TreeNode node in this.SelectedNodes)
                        {
                            if (node is GameElement)
                            {
                                GameElement elemNode = node as GameElement;
                                object tag = elemNode.InstanceObjet;
                                if (tag != null)
                                {
                                    if (tag is CoronaObject)
                                    {
                                        CoronaObject objSelected = tag as CoronaObject;
                                        this.MainForm.sceneEditorView1.selectObject(objSelected, true);
                                    }
                                }
                            }

                        }

                    }
                    else
                    {
                        this.SelectedNodes.Clear();
                        this.SelectedNodes.Add(e.Node);

                        if (e.Node is GameElement)
                        {
                            GameElement elemNode = e.Node as GameElement;
                            object tag = elemNode.InstanceObjet;
                            if (tag != null)
                            {
                                if (tag is CoronaObject)
                                {
                                    CoronaObject objSelected = tag as CoronaObject;
                                    this.MainForm.sceneEditorView1.selectObject(objSelected, false);
                                }
                            }
                        }
                    }

                }
                else
                {
                    this.SelectedNodes.Clear();
                    this.SelectedNodes.Add(e.Node);
                }



                paintSelectedNodes();

                this.treeViewElements.EndUpdate();
                GorgonLibrary.Gorgon.Go();
            }
            catch (Exception ex)
            {
                this.treeViewElements.EndUpdate();
                GorgonLibrary.Gorgon.Go();
            }
            }
        }

        private void treeViewElements_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(hasChecked == false)
            {
                if (this.isRemovingNode == false)
                {
                
                    bool bControl = (ModifierKeys == Keys.Control);
                    bool bShift = (ModifierKeys == Keys.Shift);



                    m_lastNode = e.Node;
                    if (!bShift) m_firstNode = e.Node; // store begin of shift sequence
                }
                else
                {
                    e.Cancel = true;
                }
            }
            
        }


        public void refreshNodesSelectedSceneEditor()
        {
            this.treeViewElements.BeginUpdate();
            this.treeViewElements.SelectedNode = null;
            this.removePaintFromNodes();

            this.SelectedNodes.Clear();
            List<CoronaObject> selectedObjects = this.MainForm.sceneEditorView1.objectsSelected;

            for (int i = 0; i < selectedObjects.Count; i++)
            {
                CoronaObject obj = selectedObjects[i];
                TreeNode node = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes, obj);
                if (node != null)
                    this.SelectedNodes.Add(node);
            }

            this.paintSelectedNodes();
            this.treeViewElements.EndUpdate();
        }

        private void removeFont_Click(object sender, EventArgs e)
        {
            if (this.FontSelected != null)
            {
                //Recuperer la node de l'instance
                GameElement elem = this.getNodeFromObjectInstance(this.ProjectRootNodeSelected.Nodes["FONTS"].Nodes, this.FontSelected);
                if (elem != null)
                {
                    if (this.SelectedNodes.Contains(elem))
                        this.SelectedNodes.Remove(elem);

                    elem.Remove();
                }


                this.ProjectSelected.AvailableFont.Remove(FontSelected);
                AudioObjectSelected = null;
            }
        }

        private void updateAssetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CoronaObject> objectsSelected = this.MainForm.sceneEditorView1.objectsSelected;
            if (objectsSelected.Count > 0)
            {
                List<object> objectsToUpdate = new List<object>();

                for (int i = 0; i < objectsSelected.Count; i++)
                {
                    DisplayObject dispObject = objectsSelected[i].DisplayObject;
                    if (!objectsToUpdate.Contains(dispObject))
                    {
                        if (dispObject.Type.Equals("IMAGE"))
                        {
                            objectsToUpdate.Add(dispObject);
                        }
                        else if (dispObject.Type.Equals("SPRITE"))
                        {
                            //Rechercher dans TOUT le projet tous les objets de type sprite utilisant la meme sprite set et mettre a jour ces memes assets
                            CoronaGameProject project = this.ProjectSelected;
                            for (int a = 0; a < project.Scenes.Count; a++)
                            {
                                Scene scene = project.Scenes[a];
                                for (int j = 0; j < scene.Layers.Count; j++)
                                {
                                    CoronaLayer layer = scene.Layers[j];

                                    for (int k = 0; k < layer.CoronaObjects.Count; k++)
                                    {
                                        CoronaObject objectToBuild = layer.CoronaObjects[k];

                                        if (objectToBuild.isEntity == false)
                                        {
                                            if (objectToBuild.DisplayObject.Type.Equals("SPRITE"))
                                            {
                                                if (objectToBuild.DisplayObject.SpriteSet.Name.Equals(dispObject.SpriteSet.Name))
                                                {
                                                    if (!objectsToUpdate.Contains(objectToBuild.DisplayObject))
                                                        objectsToUpdate.Add(objectToBuild.DisplayObject);
                                                }
                                            }
                                        }
                                        else
                                        {

                                            for (int l = 0; l < objectToBuild.Entity.CoronaObjects.Count; l++)
                                            {
                                                CoronaObject child = objectToBuild.Entity.CoronaObjects[l];
                                                if (child.DisplayObject.Type.Equals("SPRITE"))
                                                {
                                                    if (child.DisplayObject.SpriteSet.Name.Equals(dispObject.SpriteSet.Name))
                                                    {
                                                        if (!objectsToUpdate.Contains(child.DisplayObject))
                                                            objectsToUpdate.Add(child.DisplayObject);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (objectsToUpdate.Count > 0)
                {
                    this.MainForm.imageObjectsPanel1.updateAssets(objectsToUpdate);
                    for (int i = 0; i < objectsToUpdate.Count; i++)
                    {
                        if (objectsToUpdate[i] is DisplayObject)
                        {
                            DisplayObject dispObj = objectsToUpdate[i] as DisplayObject;
                            this.MainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(dispObj.CoronaObjectParent,
                                this.MainForm.sceneEditorView1.CurrentScale, this.MainForm.sceneEditorView1.getOffsetPoint());
                        }
                        
                    }
                  
                    GorgonLibrary.Gorgon.Go();
                }
                    
            }
        }

        private void treeViewElements_AfterCheck(object sender, TreeViewEventArgs e)
        {
            hasChecked = true;
            if (e.Node is GameElement)
            {
                GameElement elem = e.Node as GameElement;

                if (elem.NodeType.Equals("OBJECT"))
                {
                    CoronaObject obj = elem.InstanceObjet as CoronaObject;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("STAGE"))
                {
                    Scene obj = elem.InstanceObjet as Scene;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("ENTITY"))
                {
                    CoronaObject obj = elem.InstanceObjet as CoronaObject;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("TILESMAP"))
                {
                    TilesMap obj = elem.InstanceObjet as TilesMap;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("CONTROL"))
                {
                    CoronaControl obj = elem.InstanceObjet as CoronaControl;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("LAYER"))
                {
                    CoronaLayer obj = elem.InstanceObjet as CoronaLayer;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("AUDIO"))
                {
                    AudioObject obj = elem.InstanceObjet as AudioObject;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (elem.NodeType.Equals("JOINT"))
                {
                    CoronaJointure obj = elem.InstanceObjet as CoronaJointure;
                    if (obj != null)
                    {
                        obj.isEnabled = elem.Checked;
                        GorgonLibrary.Gorgon.Go();
                        
                    }
                }

            }
        }

        private void treeViewElements_MouseUp(object sender, MouseEventArgs e)
        {
            hasChecked = false;
        }

        private void treeViewElements_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            hasChecked = true;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LayerSelected != null)
            {
                FolderBrowserDialog folderB = new FolderBrowserDialog();

                if (folderB.ShowDialog() == DialogResult.OK)
                {
                    string directoryName = folderB.SelectedPath;

                    if (this.MainForm.mainBackWorker.IsBusy == false)
                    {
                        this.MainForm.directorySelectedDest = directoryName;
                        this.MainForm.currentWorkerAction = "ACTION_EXPORTLAYER";
                        this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_EXPORTLAYER");
                    }
                    else
                    {
                        MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            
            
        }

        private void importLayerBt_Click(object sender, EventArgs e)
        {
            //Recuperer la scene
            if (this.SceneSelected != null)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = ".krl";
                open.AddExtension = false;

                //Configure allowed extensions
                //
                open.Filter = "Krea Layer Files (*.krl)|*.krl";
                if (open.ShowDialog() == DialogResult.OK)
                {

                    if (this.MainForm.mainBackWorker.IsBusy == false)
                    {
                        this.MainForm.filenameSelected = open.FileName;
                        this.MainForm.currentWorkerAction = "ACTION_IMPORTLAYER";
                        this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_IMPORTLAYER");
                    }
                    else
                    {
                        MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            else
            {
                MessageBox.Show("No scene selected !\nPlease select a scene first !", "No scene selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void importEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Recuperer la scene
            if (this.LayerSelected != null)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = ".kre";
                open.AddExtension = false;

                //Configure allowed extensions
                //
                open.Filter = "Krea Entity Files (*.kre)|*.kre";
                if (open.ShowDialog() == DialogResult.OK)
                {

                    if (this.MainForm.mainBackWorker.IsBusy == false)
                    {
                        this.MainForm.filenameSelected = open.FileName;
                        this.MainForm.currentWorkerAction = "ACTION_IMPORTENTITY";
                        this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_IMPORTENTITY");
                    }
                    else
                    {
                        MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            else
            {
                MessageBox.Show("No layer selected !\nPlease select a layer first !", "No layer selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exportObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                if (this.CoronaObjectSelected.isEntity == false)
                {
                    FolderBrowserDialog folderB = new FolderBrowserDialog();

                    if (folderB.ShowDialog() == DialogResult.OK)
                    {
                        string directoryName = folderB.SelectedPath;

                        if (this.MainForm.mainBackWorker.IsBusy == false)
                        {
                            this.MainForm.directorySelectedDest = directoryName;
                            this.MainForm.currentWorkerAction = "ACTION_EXPORTOBJECT";
                            this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_EXPORTOBJECT");
                        }
                        else
                        {
                            MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Cannot export a entity object to an simple object file", "Exportation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void exportEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null)
            {
                if (this.CoronaObjectSelected.isEntity == true)
                {
                    FolderBrowserDialog folderB = new FolderBrowserDialog();

                    if (folderB.ShowDialog() == DialogResult.OK)
                    {
                        string directoryName = folderB.SelectedPath;

                        if (this.MainForm.mainBackWorker.IsBusy == false)
                        {
                            this.MainForm.directorySelectedDest = directoryName;
                            this.MainForm.currentWorkerAction = "ACTION_EXPORTENTITY";
                            this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_EXPORTENTITY");
                        }
                        else
                        {
                            MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Cannot export a non entity object","Exportation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void importObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Recuperer la scene
            if (this.LayerSelected != null)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = ".kro";
                open.AddExtension = false;

                //Configure allowed extensions
                //
                open.Filter = "Krea Object Files (*.kro)|*.kro";
                if (open.ShowDialog() == DialogResult.OK)
                {

                    if (this.MainForm.mainBackWorker.IsBusy == false)
                    {
                        this.MainForm.filenameSelected = open.FileName;
                        this.MainForm.currentWorkerAction = "ACTION_IMPORTOBJECT";
                        this.MainForm.mainBackWorker.RunWorkerAsync("ACTION_IMPORTOBJECT");
                    }
                    else
                    {
                        MessageBox.Show("A background task need to be completed before executing this action !\n Please retry in a few moment!", "Sorry...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            else
            {
                MessageBox.Show("No layer selected !\nPlease select a layer first !", "No layer selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void animationManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CoronaObjectSelected != null && this.CoronaObjectSelected.DisplayObject != null )
            {
                if(this.CoronaObjectSelected.DisplayObject.SpriteSet != null)
                {
                    AnimationManager animManager = new AnimationManager();
                    animManager.init(this.MainForm, this.CoronaObjectSelected.DisplayObject.SpriteSet);

                    animManager.ShowDialog(this.MainForm);

                    animManager.Dispose();
                    animManager = null;

                    this.MainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                    if (this.MainForm.imageObjectsPanel1.ShouldBeRefreshed == true)
                    {
                        this.MainForm.imageObjectsPanel1.RefreshCurrentAssetProject();
                        this.MainForm.imageObjectsPanel1.ShouldBeRefreshed = false;
                    }

                }
            }
        }
     
        

    }
}
