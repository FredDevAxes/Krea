using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krea.GameEditor.CodeEditor.APIElements;
using Krea.CoronaClasses;
using Krea.Corona_Classes.Controls;
using Krea.Corona_Classes;
using System.Windows.Forms;

using System.Collections;
using ScintillaNet;
using System.Xml;
using System.IO;
namespace Krea.GameEditor.CodeEditor
{
    public class AutoCompletionEngine
    {
        //----------------------------------------------------------
        //--------------------- ATTRIBUTS --------------------------
        //----------------------------------------------------------
        public List<APICategory> StaticCategories;
        public List<APICategory> DynamicCategories;
        public APICategory CategorySelected;
        public List<APIItem> StaticItems;
        public List<APIItem> DynamicItems;
        private CGEeditor editorParent;
        public bool  showFunctions = false;
        public bool IsActive = false;
        string methodsImg;
        string classesImg;
        string propsImg ;
        string fieldsImg ;
        //----------------------------------------------------------
        //--------------------- CONSTRUCTEURS ----------------------
        //----------------------------------------------------------

        public AutoCompletionEngine(CGEeditor editorParent)
        {
            this.editorParent = editorParent;
            this.StaticCategories = new List<APICategory>();
            this.StaticItems = new List<APIItem>();
            this.DynamicCategories = new List<APICategory>();
            this.DynamicItems = new List<APIItem>();

            methodsImg = XpmConverter.ConvertToXPM(Properties.Resources.pubmethod);
            classesImg = XpmConverter.ConvertToXPM(Properties.Resources.pubclass);
            propsImg = XpmConverter.ConvertToXPM(Properties.Resources.pubproperty);
            fieldsImg = XpmConverter.ConvertToXPM(Properties.Resources.pubfield);


           
            this.RefreshDynamicList();

            
        }
        //----------------------------------------------------------
        //--------------------- METHODES ---------------------------
        //----------------------------------------------------------

        private List<APICategory> findCurrentCategoryHierarchy(List<APICategory> currentHierarchy,bool isFirst,int caretPos)
        {
            DocumentForm doc = this.editorParent.ActiveDocument;
            if (currentHierarchy == null)
                currentHierarchy = new List<APICategory>();

            if (doc != null)
            {

                string currentWord = doc.Scintilla.GetWordFromPosition(caretPos);
                int supposedIndexOfPoint = caretPos-currentWord.Length -1;
                char supposedStrOfPoint = doc.Scintilla.CharAt(supposedIndexOfPoint);
                bool isDot = supposedStrOfPoint.Equals('.');
                bool isDoubleDot = supposedStrOfPoint.Equals(':');
                if (isDot == true)
                {
                    string categoryName = doc.Scintilla.GetWordFromPosition(supposedIndexOfPoint - 1);
                    if (!categoryName.Equals("") && !categoryName.Equals(" ") && !categoryName.Equals("\n")
                        && !categoryName.Equals("\t") && !categoryName.Equals("\r"))
                    {
                        APICategory previousCategory = new APICategory(categoryName);
                        this.findCurrentCategoryHierarchy(currentHierarchy,false, supposedIndexOfPoint - 1 - categoryName.Length);
                        currentHierarchy.Add(previousCategory);
                    }


                }
                else if (isDoubleDot == true)
                {
                    string categoryName = doc.Scintilla.GetWordFromPosition(supposedIndexOfPoint - 1);
                    if (!categoryName.Equals("") && !categoryName.Equals(" ") && !categoryName.Equals("\n")
                        && !categoryName.Equals("\t") && !categoryName.Equals("\r"))
                    {
                        APICategory previousCategory = new APICategory(categoryName);
                        currentHierarchy.Add(previousCategory);
                    }

                }

                if (isFirst == false && !currentWord.Equals("") && !currentWord.Equals(" ") && !currentWord.Equals("\n")
                        && !currentWord.Equals("\t") && !currentWord.Equals("\r"))
                {
                    APICategory currentCategory = new APICategory(currentWord);
                    currentHierarchy.Add(currentCategory);
                }

            }

            return currentHierarchy;
        }

        private APICategory getStaticCategoryFromName(APICategory catParent,string name)
        {
            if (catParent != null)
            {
                for (int i = 0; i < catParent.SubCategories.Count; i++)
                {
                    if (catParent.SubCategories[i].name.Equals(name))
                        return catParent.SubCategories[i];
                    else
                    {
                        APICategory catFound = this.getStaticCategoryFromName(catParent.SubCategories[i], name);
                        if (catFound != null)
                            return catFound;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.StaticCategories.Count; i++)
                {
                    if (this.StaticCategories[i].name.Equals(name))
                        return this.StaticCategories[i];
                    else
                    {
                        APICategory catFound = this.getStaticCategoryFromName(this.StaticCategories[i], name);
                        if (catFound != null)
                            return catFound;
                    }
                }

                
            }
            return null;
        }

        private APICategory getDynamicCategoryFromName(APICategory catParent, string name)
        {
            if (catParent != null)
            {
                for (int i = 0; i < catParent.SubCategories.Count; i++)
                {
                    if (catParent.SubCategories[i].name.Equals(name))
                        return catParent.SubCategories[i];
                    else
                    {
                        APICategory catFound = this.getDynamicCategoryFromName(catParent.SubCategories[i], name);
                        if (catFound != null)
                            return catFound;
                    }
                }
            }
            else
            {
                //Search in first level
                for (int i = 0; i < this.DynamicCategories.Count; i++)
                {
                    if (this.DynamicCategories[i].name.Equals(name))
                        return this.DynamicCategories[i];
                   
                }

                //Search in next levels
                 for (int i = 0; i < this.DynamicCategories.Count; i++)
                {
                    APICategory catFound = this.getDynamicCategoryFromName(this.DynamicCategories[i], name);
                    if (catFound != null)
                        return catFound;
                   
                }
                 
            }
            return null;
        }

        public void checkForAutoCompletion(char lastCharEntered)
        {
            if (this.IsActive == false)
            {
                //this.IsActive = true;
                return;
            } 

            DocumentForm doc = this.editorParent.ActiveDocument;
            if (doc != null)
            {
                string autoCompleteStr = "";
                if (lastCharEntered.Equals('.'))
                    this.showFunctions = false;
               else if (lastCharEntered.Equals(':'))
                    this.showFunctions = true;

                string CurrentWord = doc.Scintilla.GetWordFromPosition(doc.Scintilla.CurrentPos);
                if (lastCharEntered.Equals(' ') || lastCharEntered.Equals('\n')
                        || lastCharEntered.Equals('\t') || lastCharEntered.Equals('\r'))
                    return;
                else if (CurrentWord.Equals("") && !lastCharEntered.Equals('.') && !lastCharEntered.Equals(':'))
                    return;

                List<APICategory> currentHierarchy = this.findCurrentCategoryHierarchy(null,true, doc.Scintilla.CurrentPos);

                //----------------- Search in STATIC ---------------------------------
                this.CategorySelected = null;
                if (currentHierarchy.Count > 0)
                    this.CategorySelected = this.getStaticCategoryFromName(null, currentHierarchy[currentHierarchy.Count - 1].name);

                if (CategorySelected != null)
                {
                    if (showFunctions == false)
                    {
                        for (int i = 0; i < CategorySelected.SubCategories.Count; i++)
                        {
                            APICategory cat = CategorySelected.SubCategories[i];
                            if (cat.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                if (!autoCompleteStr.Contains(cat.name + "?1 "))
                                    autoCompleteStr += cat.name + "?1 ";
                            }

                        }
                    }
                    
                    for (int i = 0; i < CategorySelected.Items.Count; i++)
                    {
                        APIItem item = CategorySelected.Items[i];
                        if (item.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            if (showFunctions == true)
                            {
                                if (item.isClassFunction == true)
                                {
                                    if (!autoCompleteStr.Contains(item.syntax.Replace(" ","") + "?0 "))
                                        autoCompleteStr += item.syntax.Replace(" ", "") + "?0 ";
                                }
                            }
                            else
                            {
                                if (item.isClassFunction == false)
                                {
                                    if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?2 "))
                                        autoCompleteStr += item.syntax.Replace(" ","") + "?2 ";
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (currentHierarchy.Count == 0)
                    {
                        if (showFunctions == false)
                        {
                            for (int i = 0; i < this.StaticCategories.Count; i++)
                            {
                                APICategory cat = this.StaticCategories[i];
                                if (cat.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                                {
                                    if (!autoCompleteStr.Contains(cat.name + "?1 "))
                                        autoCompleteStr += cat.name + "?1 ";
                                }

                            }
                        }

                        for (int i = 0; i < this.StaticItems.Count; i++)
                        {
                            APIItem item = this.StaticItems[i];
                            if (item.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                            {

                                if (showFunctions == true)
                                {
                                    if (item.isClassFunction == true)
                                    {
                                        if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?0 "))
                                            autoCompleteStr += item.syntax.Replace(" ", "") + "?0 ";
                                    }
                                }
                                else
                                {
                                    if ( item.isClassFunction == false)
                                    {
                                        if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?2 "))
                                            autoCompleteStr += item.syntax.Replace(" ", "") + "?2 ";
                                    }
                                }
                            }
                        }
                    }

                    

                }

                //----------------- Search in DYNAMIC ---------------------------------
                CategorySelected = null;
                if (this.CategorySelected == null && currentHierarchy.Count > 0)
                    this.CategorySelected = this.getDynamicCategoryFromName(null, currentHierarchy[currentHierarchy.Count - 1].name);

                if (CategorySelected != null)
                {
                    if (showFunctions == false)
                    {
                        for (int i = 0; i < CategorySelected.SubCategories.Count; i++)
                        {
                            APICategory cat = CategorySelected.SubCategories[i];
                            if (cat.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                if (!autoCompleteStr.Contains(cat.name + "?1 "))
                                    autoCompleteStr += cat.name + "?1 ";
                            }

                        }
                    }

                    for (int i = 0; i < CategorySelected.Items.Count; i++)
                    {
                        APIItem item = CategorySelected.Items[i];
                        if (item.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            if (showFunctions == true)
                            {
                                if (item.isClassFunction == true)
                                {
                                    if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?0 "))
                                        autoCompleteStr += item.syntax.Replace(" ", "") + "?0 ";
                                }
                            }
                            else
                            {
                                if ( item.isClassFunction == false)
                                {
                                    if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?2 "))
                                        autoCompleteStr += item.syntax.Replace(" ", "") + "?2 ";
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (currentHierarchy.Count == 0)
                    {
                        for (int i = 0; i < this.DynamicCategories.Count; i++)
                        {
                            APICategory cat = this.DynamicCategories[i];
                            if (cat.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                if (!autoCompleteStr.Contains(cat.name + "?1 "))
                                    autoCompleteStr += cat.name + "?1 ";
                            }

                        }

                        for (int i = 0; i < this.DynamicItems.Count; i++)
                        {
                            APIItem item = this.DynamicItems[i];
                            if (item.name.ToLower().StartsWith(CurrentWord.ToLower(), StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                if (showFunctions == true)
                                {
                                    if (item.isClassFunction == true)
                                    {
                                        if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?0 "))
                                            autoCompleteStr += item.syntax.Replace(" ", "") + "?0 ";
                                    }
                                }
                                else
                                {
                                    if (item.isClassFunction == false)
                                    {
                                        if (!autoCompleteStr.Contains(item.syntax.Replace(" ", "") + "?2 "))
                                            autoCompleteStr += item.syntax.Replace(" ", "") + "?2 ";
                                    }
                                }
                            }
                        }
                    }
                }

                if (!autoCompleteStr.Equals(""))
                {
                    
                    doc.Scintilla.AutoComplete.ListString = autoCompleteStr;


                    if (doc.Scintilla.AutoComplete.IsActive == false)
                    {
                        doc.Scintilla.AutoComplete.FillUpCharacters = "([";
                        doc.Scintilla.AutoComplete.SingleLineAccept = false;
                        doc.Scintilla.AutoComplete.DropRestOfWord = true;
                        doc.Scintilla.AutoComplete.ClearRegisteredImages();
                        doc.Scintilla.AutoComplete.IsCaseSensitive = true;
                        doc.Scintilla.AutoComplete.RegisterImage(0, methodsImg);
                        doc.Scintilla.AutoComplete.RegisterImage(1, classesImg);
                        doc.Scintilla.AutoComplete.RegisterImage(2, propsImg);
                        doc.Scintilla.AutoComplete.RegisterImage(3, fieldsImg);

                        doc.Scintilla.AutoComplete.Show(CurrentWord.Length, autoCompleteStr);
                    }


                }
            }
            

        }

        private string clearStringDefaults(string text)
        {
            bool hasFound = false;

            if (text.StartsWith("\t") || text.StartsWith("\r") || text.StartsWith("\n") || text.StartsWith(" ") || text.StartsWith(" "))
            {
                text = text.Substring(1);
                hasFound = true;
            }


            if (text.EndsWith("\t") || text.EndsWith("\r") || text.EndsWith("\n") || text.EndsWith(" ") || text.EndsWith(" "))
            {
                text = text.Substring(0, text.Length - 1);
                hasFound = true;
            }

            if (hasFound == false)
                return text;
            else
                return clearStringDefaults(text);
        }

        public void refreshStaticAPI()
        {
            string xmlFilePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\staticAPI.xml";
            if(File.Exists(xmlFilePath))
            {
                //Clear all lists
                this.StaticCategories.Clear();
                this.StaticItems.Clear();

                XmlTextReader reader = new XmlTextReader(xmlFilePath);

                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    XmlNode node = doc.ChildNodes[i];
                    for (int j = 0; j < node.ChildNodes.Count; j++)
                    {
                        XmlNode node2 = node.ChildNodes[j];
                        for (int k = 0; k < node2.ChildNodes.Count; k++)
                        {
                            XmlNode node3 = node2.ChildNodes[k];

                            //Get the category of this node
                            string finalName = node3.Name;
                            if(finalName.Contains(".."))
                            {
                                finalName = finalName.Replace("..",":");
                            }

                            string name ="";
                            bool isFunction = false;
                            bool isClassFunction= false;
                            string syntax = "";
                            string returns = "";
                            string remarks = "";
                            string description = "";
                            string parameters = "";
                            string example = "";

                            string categoryName ="";
                            if(finalName.Contains(":"))
                            {
                                categoryName = finalName.Split(':')[0];
                                name = finalName.Split(':')[1];
                            }
                            else if(finalName.Contains("."))
                            {
                                categoryName = finalName.Split('.')[0];
                                name = finalName.Split('.')[1];
                            }
                            APICategory catParent = this.getStaticCategoryFromName(null, categoryName);
                            if (catParent == null)
                            {
                                catParent = new APICategory(categoryName);
                                this.StaticCategories.Add(catParent);
                            }

                           
                            for (int l = 0; l < node3.ChildNodes.Count; l++)
                            {
                                XmlNode node4 = node3.ChildNodes[l];

                                switch (node4.Name)
                                {
                                    case "IsFunction":
                                        isFunction = Convert.ToBoolean(node4.InnerText);
                                        break;

                                    case "IsClassFunction":
                                        isClassFunction = Convert.ToBoolean(node4.InnerText);
                                        break;

                                    case "Description":
                                        description = this.clearStringDefaults(node4.InnerText);
                                        break;

                                    case "Syntax":
                                        syntax = this.clearStringDefaults(node4.InnerText);

                                        //Should remove the "local truc = "
                                        int indexCatNameInSyntax = syntax.IndexOf(categoryName);
                                        if (indexCatNameInSyntax > -1)
                                        {
                                            int index = syntax.IndexOf("=", 0, indexCatNameInSyntax);
                                            if (index > -1)
                                                syntax = syntax.Remove(0, index + 1);
                                        }
                                        

                                        if (syntax.Contains(":"))
                                            syntax = syntax.Replace(categoryName + ":", "");
                                        else if (syntax.Contains("."))
                                            syntax = syntax.Replace(categoryName + ".", "");

                                        if (syntax.Equals(""))
                                            syntax = name;
                                        break;

                                    case "Example":
                                        example = this.clearStringDefaults(node4.InnerText);
                                        break;

                                    case "Parameters":
                                        parameters = this.clearStringDefaults(node4.InnerText);
                                        break;

                                    case "Returns":
                                        returns = this.clearStringDefaults(node4.InnerText);
                                        break;

                                    case "Remarks":
                                        remarks = this.clearStringDefaults(node4.InnerText);
                                        break;
                                }
                            }

                            //Creer l'item
                            APIItem item = new APIItem(name, isFunction, isClassFunction, syntax.Replace(" ",""), returns, remarks, description, parameters);
                            catParent.Items.Add(item);
                            
                        }
                    }

                }

            }
           
        }

        

        public void RefreshDynamicList()
        {
            this.DynamicCategories.Clear();
            this.DynamicItems.Clear();


            APICategory objectCategory = null;
            for (int i = 0; i < this.StaticCategories.Count; i++)
            {
                if (this.StaticCategories[i].name.ToLower().Equals("object"))
                {
                    objectCategory = this.StaticCategories[i];
                    break;
                }
            }

            if (this.editorParent.sceneSelected != null)
            {
                CoronaGameProject currentProject = this.editorParent.sceneSelected.projectParent;
                if (currentProject != null)
                {
                    APICategory catStoryboard = new APICategory("storyboard");
                    this.DynamicCategories.Add(catStoryboard);

                    if (currentProject.Snippets.Count > 0)
                    {
                        APICategory catSnippets = new APICategory("snippets");
                        this.DynamicCategories.Add(catSnippets);
                        for (int i = 0; i < currentProject.Snippets.Count; i++)
                        {
                            Krea.Corona_Classes.Snippet snippet = currentProject.Snippets[i];
                            if (snippet.Syntax == null) snippet.Syntax = snippet.Name + "()";
                            APIItem itemSnippet = new APIItem(snippet.Name.Replace(" ", ""), true, false, snippet.Syntax, "", "", "", "");
                            catSnippets.Items.Add(itemSnippet);
                        }
                    }


                    APICategory catResources = new APICategory("resources");
                    catStoryboard.SubCategories.Add(catResources);


                    //AJouter les scenes
                    for (int i = 0; i < currentProject.Scenes.Count; i++)
                    {
                        Scene scene = currentProject.Scenes[i];
                        APICategory catScene = new APICategory(scene.Name);
                        catResources.SubCategories.Add(catScene);

                        if (this.editorParent.sceneSelected == scene)
                            this.DynamicCategories.Add(catScene);

                        for (int j = 0; j < scene.Layers.Count; j++)
                        {
                            CoronaLayer layer = scene.Layers[j];
                            APICategory catLayer = new APICategory(layer.Name);
                            catScene.SubCategories.Add(catLayer);
                            if (this.editorParent.sceneSelected == scene)
                                this.DynamicCategories.Add(catLayer);

                            for (int k = 0; k < layer.CoronaObjects.Count; k++)
                            {
                                CoronaObject obj = layer.CoronaObjects[k];
                                if (obj.isEntity == false)
                                {
                                    APICategory catObj = new APICategory(obj.DisplayObject.Name);
                                    catLayer.SubCategories.Add(catObj);

                                    if (objectCategory != null)
                                    {
                                        catObj.SubCategories.Add(objectCategory);
                                    }
                                }
                                else
                                {
                                    APICategory objectEntityCategory = new APICategory(obj.Entity.Name);
                                    catLayer.SubCategories.Add(objectEntityCategory);

                                    APIItem itemAddObjectFunction = new APIItem("addObject", false, true, "addObject(obj)", "", "", "", "object obj");
                                    objectEntityCategory.Items.Add(itemAddObjectFunction);

                                    APIItem itemRemoveObjectFunction = new APIItem("removeObject", false, true, "removeObject(obj,doClean)", "", "", "", "object obj, bool doClean");
                                    objectEntityCategory.Items.Add(itemRemoveObjectFunction);

                                    APIItem itemGetObjectJointsObjectFunction = new APIItem("getAllObjectJoints", false, true, "getAllObjectJoints(obj)", "returns a table of joint instances", "", "", "object obj");
                                    objectEntityCategory.Items.Add(itemGetObjectJointsObjectFunction);

                                    APIItem itemRemoveJointFunction = new APIItem("removeJoint", false, true, "removeJoint(joint)", "", "", "", "jointInstance joint");
                                    objectEntityCategory.Items.Add(itemRemoveJointFunction);

                                    APIItem itemCreateJointFunction = new APIItem("createJoint", false, true, "createJoint(jointParams)", "", "", "", "table paramsJoint");
                                    objectEntityCategory.Items.Add(itemCreateJointFunction);

                                    APIItem itemCloneEntityFunction = new APIItem("clone", false, true, "clone(insertCloneAtEndOfGroup)", "return an entity instance", "", "", "");
                                    objectEntityCategory.Items.Add(itemCloneEntityFunction);

                                    APIItem itemCleanEntityFunction = new APIItem("clean", false, true, "clean()", "", "", "", "");
                                    objectEntityCategory.Items.Add(itemCloneEntityFunction);

                                    for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                                    {
                                        CoronaObject child = obj.Entity.CoronaObjects[l];
                                        APICategory catObj = new APICategory(child.DisplayObject.Name);
                                        catLayer.SubCategories.Add(catObj);

                                        if (objectCategory != null)
                                        {
                                            catObj.SubCategories.Add(objectCategory);

                                        }

                                        APIItem itemCloneObjectFunction = new APIItem("clone", false, true, "clone(insertCloneAtEndOfGroup)", "return an object instance", "", "", "");
                                        catObj.Items.Add(itemCloneObjectFunction);

                                        APIItem itemCleanObjectFunction = new APIItem("clean", false, true, "clean()", "", "", "", "");
                                        catObj.Items.Add(itemCleanObjectFunction);

                                        APIItem itemStartInteractionsObjectFunction = new APIItem("startInteractions", false, true, "startInteractions()", "", "", "", "");
                                        catObj.Items.Add(itemStartInteractionsObjectFunction);

                                        APIItem itemPauseInteractionsObjectFunction = new APIItem("pauseInteractions", false, true, "pauseInteractions()", "", "", "", "");
                                        catObj.Items.Add(itemPauseInteractionsObjectFunction);
                                    }
                                }
                            }

                            for (int k = 0; k < layer.Jointures.Count; k++)
                            {
                                CoronaJointure joint = layer.Jointures[k];
                                APICategory catJoint = new APICategory(joint.name);
                                catLayer.SubCategories.Add(catJoint);

                                APIItem isActiveItem = new APIItem("isActive", false, false, "isActive", "", "", "", "");
                                catJoint.Items.Add(isActiveItem);

                                APIItem isCollideConnectedItem = new APIItem("isCollideConnected", false, false, "isCollideConnected", "", "", "", "");
                                catJoint.Items.Add(isCollideConnectedItem);

                                APIItem reactionTorqueItem = new APIItem("reactionTorque", false, false, "reactionTorque", "", "", "", "");
                                catJoint.Items.Add(reactionTorqueItem);

                                APIItem getAnchorAItem = new APIItem("getAnchorA", false, true, "getAnchorA()", "", "", "", "");
                                catJoint.Items.Add(getAnchorAItem);

                                APIItem getAnchorBItem = new APIItem("getAnchorB", false, true, "getAnchorB()", "", "", "", "");
                                catJoint.Items.Add(getAnchorBItem);

                                APIItem getReactionForceItem = new APIItem("getReactionForce", false, true, "getReactionForce()", "", "", "", "");
                                catJoint.Items.Add(getReactionForceItem);

                                APIItem removeSelfItem = new APIItem("removeSelf", false, true, "removeSelf()", "", "", "", "");
                                catJoint.Items.Add(removeSelfItem);

                                if (joint.type.Equals("PIVOT"))
                                {
                                    APIItem maxMotorTorqueItem = new APIItem("maxMotorTorque", false, false, "maxMotorTorque", "", "", "", "");
                                    catJoint.Items.Add(maxMotorTorqueItem);

                                    APIItem referenceAngleItem = new APIItem("referenceAngle", false, false, "referenceAngle", "", "", "", "");
                                    catJoint.Items.Add(referenceAngleItem);

                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);

                                    APIItem isLimitEnabledItem = new APIItem("isLimitEnabled", false, false, "isLimitEnabled", "", "", "", "");
                                    catJoint.Items.Add(isLimitEnabledItem);

                                    APIItem isMotorEnabledItem = new APIItem("isMotorEnabled", false, false, "isMotorEnabled", "", "", "", "");
                                    catJoint.Items.Add(isMotorEnabledItem);

                                    APIItem jointAngleItem = new APIItem("jointAngle", false, false, "jointAngle", "", "", "", "");
                                    catJoint.Items.Add(jointAngleItem);

                                    APIItem jointSpeedItem = new APIItem("jointSpeed", false, false, "jointSpeed", "", "", "", "");
                                    catJoint.Items.Add(jointSpeedItem);

                                    APIItem motorSpeedItem = new APIItem("motorSpeed", false, false, "motorSpeed", "", "", "", "");
                                    catJoint.Items.Add(motorSpeedItem);

                                    APIItem motorTorqueItem = new APIItem("motorTorque", false, false, "motorTorque", "", "", "", "");
                                    catJoint.Items.Add(motorTorqueItem);

                                    APIItem getRotationLimitsItem = new APIItem("getRotationLimits", false, true, "getRotationLimits()", "", "", "", "");
                                    catJoint.Items.Add(getRotationLimitsItem);

                                    APIItem setRotationLimitsItem = new APIItem("setRotationLimits", false, true, "setRotationLimits()", "", "", "", "");
                                    catJoint.Items.Add(setRotationLimitsItem);

                                }
                                else if (joint.type.Equals("PISTON"))
                                {
                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);

                                    APIItem getLocalAxisAItem = new APIItem("getLocalAxisA", false, true, "getLocalAxisA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAxisAItem);

                                    APIItem referenceAngleItem = new APIItem("referenceAngle", false, false, "referenceAngle", "", "", "", "");
                                    catJoint.Items.Add(referenceAngleItem);

                                    APIItem isLimitEnabledItem = new APIItem("isLimitEnabled", false, false, "isLimitEnabled", "", "", "", "");
                                    catJoint.Items.Add(isLimitEnabledItem);

                                    APIItem isMotorEnabledItem = new APIItem("isMotorEnabled", false, false, "isMotorEnabled", "", "", "", "");
                                    catJoint.Items.Add(isMotorEnabledItem);

                                    APIItem jointSpeedItem = new APIItem("jointSpeed", false, false, "jointSpeed", "", "", "", "");
                                    catJoint.Items.Add(jointSpeedItem);

                                    APIItem jointTranslationItem = new APIItem("jointTranslation", false, false, "jointTranslation", "", "", "", "");
                                    catJoint.Items.Add(jointTranslationItem);

                                    APIItem maxMotorForceItem = new APIItem("maxMotorForce", false, false, "maxMotorForce", "", "", "", "");
                                    catJoint.Items.Add(maxMotorForceItem);

                                    APIItem motorSpeedItem = new APIItem("motorSpeed", false, false, "motorSpeed", "", "", "", "");
                                    catJoint.Items.Add(motorSpeedItem);

                                    APIItem motorForceItem = new APIItem("motorForce", false, false, "motorForce", "", "", "", "");
                                    catJoint.Items.Add(motorForceItem);

                                   
                                }
                                else if (joint.type.Equals("WHEEL"))
                                {
                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);

                                    APIItem getLocalAxisAItem = new APIItem("getLocalAxisA", false, true, "getLocalAxisA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAxisAItem);

                                    APIItem motorTorqueItem = new APIItem("motorTorque", false, false, "motorTorque", "", "", "", "");
                                    catJoint.Items.Add(motorTorqueItem);

                                    APIItem maxMotorTorqueItem = new APIItem("maxMotorTorque", false, false, "maxMotorTorque", "", "", "", "");
                                    catJoint.Items.Add(maxMotorTorqueItem);

                                    APIItem springFrequencyItem = new APIItem("springFrequency", false, false, "springFrequency", "", "", "", "");
                                    catJoint.Items.Add(springFrequencyItem);

                                    APIItem springDampingRatioItem = new APIItem("springDampingRatio", false, false, "springDampingRatio", "", "", "", "");
                                    catJoint.Items.Add(springDampingRatioItem);

                                    APIItem isMotorEnabledItem = new APIItem("isMotorEnabled", false, false, "isMotorEnabled", "", "", "", "");
                                    catJoint.Items.Add(isMotorEnabledItem);

                                    APIItem jointSpeedItem = new APIItem("jointSpeed", false, false, "jointSpeed", "", "", "", "");
                                    catJoint.Items.Add(jointSpeedItem);

                                    APIItem jointTranslationItem = new APIItem("jointTranslation", false, false, "jointTranslation", "", "", "", "");
                                    catJoint.Items.Add(jointTranslationItem);

                                    APIItem maxMotorForceItem = new APIItem("maxMotorForce", false, false, "maxMotorForce", "", "", "", "");
                                    catJoint.Items.Add(maxMotorForceItem);

                                    APIItem motorSpeedItem = new APIItem("motorSpeed", false, false, "motorSpeed", "", "", "", "");
                                    catJoint.Items.Add(motorSpeedItem);

                                    
                                }
                                else if (joint.type.Equals("PULLEY"))
                                {
                                    APIItem groundAnchorAItem = new APIItem("groundAnchorA", false, false, "groundAnchorA", "", "", "", "");
                                    catJoint.Items.Add(groundAnchorAItem);

                                    APIItem groundAnchorBItem = new APIItem("groundAnchorB", false, false, "groundAnchorB", "", "", "", "");
                                    catJoint.Items.Add(groundAnchorBItem);

                                    APIItem length1Item = new APIItem("length1", false, false, "length1", "", "", "", "");
                                    catJoint.Items.Add(length1Item);

                                    APIItem length2Item = new APIItem("length2", false, false, "length2", "", "", "", "");
                                    catJoint.Items.Add(length2Item);

                                    APIItem ratioItem = new APIItem("ratio", false, false, "ratio", "", "", "", "");
                                    catJoint.Items.Add(ratioItem);

                                    APIItem getGroundAnchorAItem = new APIItem("getGroundAnchorA", false, true, "getGroundAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getGroundAnchorAItem);

                                    APIItem getGroundAnchorBItem = new APIItem("getGroundAnchorB", false, true, "getGroundAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getGroundAnchorBItem);
                                }
                                else if (joint.type.Equals("WELD"))
                                {
                                    APIItem dampingRatioItem = new APIItem("dampingRatio", false, false, "dampingRatio", "", "", "", "");
                                    catJoint.Items.Add(dampingRatioItem);

                                    APIItem frequencyItem = new APIItem("frequency", false, false, "frequency", "", "", "", "");
                                    catJoint.Items.Add(frequencyItem);

                                    APIItem referenceAngleItem = new APIItem("referenceAngle", false, false, "referenceAngle", "", "", "", "");
                                    catJoint.Items.Add(referenceAngleItem);

                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);
                                }

                                else if (joint.type.Equals("DISTANCE"))
                                {
                                    APIItem dampingRatioItem = new APIItem("dampingRatio", false, false, "dampingRatio", "", "", "", "");
                                    catJoint.Items.Add(dampingRatioItem);

                                    APIItem frequencyItem = new APIItem("frequency", false, false, "frequency", "", "", "", "");
                                    catJoint.Items.Add(frequencyItem);

                                    APIItem lengthItem = new APIItem("length", false, false, "length", "", "", "", "");
                                    catJoint.Items.Add(lengthItem);

                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);
                                }

                                else if (joint.type.Equals("FRICTION"))
                                {
                                    APIItem maxTorqueItem = new APIItem("maxTorque", false, false, "maxTorque", "", "", "", "");
                                    catJoint.Items.Add(maxTorqueItem);

                                    APIItem maxForceItem = new APIItem("maxForce", false, false, "maxForce", "", "", "", "");
                                    catJoint.Items.Add(maxForceItem);

                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);
                                }
                                else if (joint.type.Equals("GEAR"))
                                {
                                    APIItem joint1Item = new APIItem("joint1", false, false, "joint1", "", "", "", "");
                                    catJoint.Items.Add(joint1Item);

                                    APIItem joint2Item = new APIItem("joint2", false, false, "joint2", "", "", "", "");
                                    catJoint.Items.Add(joint2Item);

                                    APIItem ratioItem = new APIItem("ratio", false, false, "ratio", "", "", "", "");
                                    catJoint.Items.Add(ratioItem);
                                }
                                else if (joint.type.Equals("ROPE"))
                                {
                                    APIItem limitStateItem = new APIItem("limitState", false, false, "limitState", "", "", "", "");
                                    catJoint.Items.Add(limitStateItem);

                                    APIItem maxLenghtItem = new APIItem("maxLenght", false, false, "maxLenght", "", "", "", "");
                                    catJoint.Items.Add(maxLenghtItem);

                                    APIItem getLocalAnchorAItem = new APIItem("getLocalAnchorA", false, true, "getLocalAnchorA()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorAItem);

                                    APIItem getLocalAnchorBItem = new APIItem("getLocalAnchorB", false, true, "getLocalAnchorB()", "", "", "", "");
                                    catJoint.Items.Add(getLocalAnchorBItem);
                                   
                                }


                            }

                            for (int k = 0; k < layer.Controls.Count; k++)
                            {
                                CoronaControl control = layer.Controls[k];
                                APIItem itemControl = new APIItem(control.ControlName, false, false, control.ControlName, "", "", "", "");
                                catLayer.Items.Add(itemControl);
                            }
                        }

                    }
                }
            }
        }

        private void addItem(string catName, string itemName, bool isFunction,bool isClassFunction, string syntax, string returns, string remarks, string description, string parameters)
        {
            if (catName != null)
            {
                //Check if the category already exists
                bool doesCategoryExist = false;
                int index = -1;

                for (int i = 0; i < this.StaticCategories.Count; i++)
                {
                    if (this.StaticCategories[i].name.Equals(catName))
                    {
                        doesCategoryExist = true;
                        index = i;
                        break;
                    }
                }

                if (doesCategoryExist == true)
                    this.StaticCategories[index].Items.Add(new APIItem(itemName, isFunction, isClassFunction,syntax, returns, remarks, description, parameters));
                else
                {
                    APICategory cat = new APICategory(catName);
                    cat.Items.Add(new APIItem(itemName, isFunction, isClassFunction, syntax, returns, remarks, description, parameters));
                    this.StaticCategories.Add(cat);
                }
            }
            else
                this.StaticItems.Add(new APIItem(itemName, isFunction, isClassFunction, syntax, returns, remarks, description, parameters));
        }

    }
}
