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
namespace Krea.GameEditor.CodeEditor
{
    public class AutoCompletionEngine
    {
        //----------------------------------------------------------
        //--------------------- ATTRIBUTS --------------------------
        //----------------------------------------------------------
        public List<APICategory> Categories;
        public APICategory CategorySelected;
        public List<APIItem> Items;
        private CGEeditor editorParent;
        public bool isActive = false;
        public bool isInACategory = false;
        public bool  showFunctions = false;
        public string CurrentWord;

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
            this.Categories = new List<APICategory>();
            this.Items = new List<APIItem>();
            CurrentWord = "";

            methodsImg = XpmConverter.ConvertToXPM(Properties.Resources.pubmethod);
            classesImg = XpmConverter.ConvertToXPM(Properties.Resources.pubclass);
            propsImg = XpmConverter.ConvertToXPM(Properties.Resources.pubproperty);
            fieldsImg = XpmConverter.ConvertToXPM(Properties.Resources.pubfield);
            
        }
        //----------------------------------------------------------
        //--------------------- METHODES ---------------------------
        //----------------------------------------------------------
        public void resetCurrentWord()
        {
            this.CurrentWord = "";
            isActive = true;
            this.isInACategory = false;
            this.CategorySelected = null;
            showFunctions = false;
        }

        public void checkAutoCompletion(char lastCharEntered)
        {
            if (lastCharEntered.Equals('\r') || lastCharEntered.Equals('\n'))
            {
                isActive = true;
                CurrentWord = "";
                isInACategory = false;
                showFunctions = false;
                return;
            }

            if (!this.CurrentWord.Equals(""))
            {
                //Verifier s'il contient des points
                if (this.CurrentWord.Contains("."))
                {
                    //Decouper le mot 
                    string[] tabMot = this.CurrentWord.Split('.');

                }
            }

        }
        public void checkForAutoCompletion(char lastCharEntered)
        {

           

            if (lastCharEntered.Equals(' ') || lastCharEntered.Equals('\t')
                || lastCharEntered.Equals('\r') || lastCharEntered.Equals('\n'))
            {
                isActive = true;
                CurrentWord = "";
                isInACategory = false;
                showFunctions = false;


                return;
            }

           /* if (this.autoCompleteForm.Visible == false)
            {
                this.autoCompleteForm.Show(this.editorParent);
                this.editorParent.Focus();
            }
            this.autoCompleteForm.Location = Cursor.Position;
            this.autoCompleteForm.refreshValues(this.Items);*/

           
            if (isActive == true)
            {
                if (lastCharEntered.Equals('.') || lastCharEntered.Equals(':'))
                {
                    if (CategorySelected != null && this.isInACategory == true)
                    {
                        bool containsCategory = false;
                        
                        for (int i = 0; i < CategorySelected.SubCategories.Count; i++)
                        {
                            APICategory cat = CategorySelected.SubCategories[i];
                            

                            if (cat.name.Equals(CurrentWord, StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                CategorySelected = cat;
                                CurrentWord = "";
                                containsCategory = true;
                                break;
                            }
                            
                        }


                    }
                    else
                    {
                        bool containsCategory = false;
                        for (int i = 0; i < this.Categories.Count; i++)
                        {
                            APICategory cat = Categories[i];
                            if (cat.name.Equals(CurrentWord, StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                CategorySelected = cat;
                                containsCategory = true;
                                isInACategory = true;
                                CurrentWord = "";
                                break;
                            }
                        }
                        if (containsCategory == false)
                        {
                            this.resetCurrentWord();
                            this.isActive = true;
                            return;
                        } 
                    }
                }
                else
                {
                    if(!lastCharEntered.Equals('!'))
                        CurrentWord += lastCharEntered;
                }
                    

                //Chercher dans les categories 
                string autoCompleteStr = "";
               

                if (lastCharEntered.Equals(':'))
                    showFunctions = true;

                if (CategorySelected != null && this.isInACategory == true )
                {
                    for (int i = 0; i < CategorySelected.SubCategories.Count; i++)
                    {
                        APICategory cat = CategorySelected.SubCategories[i];
                        if (cat.name.StartsWith(CurrentWord, StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            autoCompleteStr += cat.name + "?1 ";
                        }

                    }
                    for (int i = 0; i < CategorySelected.Items.Count; i++)
                    {
                        APIItem item = CategorySelected.Items[i];
                        if (item.name.StartsWith(CurrentWord, StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            if (showFunctions == true)
                            {
                                if (item.isFunction == true)
                                {
                                    autoCompleteStr += item.name + "?0 ";
                                }
                            }
                            else
                            {
                                if (item.isFunction == false)
                                {
                                    autoCompleteStr += item.name + "?2 ";
                                }
                            }

                        }
                    }
                }
                else
                {

                    for (int i = 0; i < this.Categories.Count; i++)
                    {
                        APICategory cat = this.Categories[i];
                        if (cat.name.StartsWith(CurrentWord, StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            autoCompleteStr += cat.name + "?1 ";
                        }

                    }

                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        APIItem item = this.Items[i];
                        if (item.name.StartsWith(CurrentWord, StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            if (showFunctions == true)
                            {
                                if (item.isFunction == true)
                                {
                                    autoCompleteStr += item.name + "?0 ";
                                }
                            }
                            else
                            {
                                if (item.isFunction == false)
                                {
                                    autoCompleteStr += item.name + "?2 ";
                                }
                            }
                        }
                    }
                    
                }

                if (!autoCompleteStr.Equals(""))
                {
                    if (this.editorParent.ActiveDocument != null)
                    {
                        this.editorParent.ActiveDocument.Scintilla.AutoComplete.ListString = autoCompleteStr;
                        

                        if (this.editorParent.ActiveDocument.Scintilla.AutoComplete.IsActive == false)
                        {
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.SingleLineAccept = true;
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.DropRestOfWord = true;
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.ClearRegisteredImages();

                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.RegisterImage(0, methodsImg);
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.RegisterImage(1, classesImg);
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.RegisterImage(2, propsImg);
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.RegisterImage(3, fieldsImg);

                        }



                        if (this.editorParent.ActiveDocument.Scintilla.AutoComplete.IsActive == false)
                            this.editorParent.ActiveDocument.Scintilla.AutoComplete.Show();
                      // this.editorParent.ActiveDocument.Scintilla.AutoComplete.ShowUserList(0,autoCompleteStr);

                       

                    }
                    
                }
                
            }
        }

        public void refreshAPI(string sourcePage)
        {
            //Clear all lists
            this.Categories.Clear();
            this.Items.Clear();


            //Chercher toutes les infex de href="#
            int indexStart = 0;
            int indexFirstEndBalise = 0;
            int indexSecondEndBalise = 0;
            string strStart  = "href=\"#";
            string strEndFirstBalise =">";
            string strEndSecondBalise = "</a>";
            while (indexStart < sourcePage.Length)
            {
                indexStart = sourcePage.IndexOf(strStart, indexSecondEndBalise);
                if (indexStart == -1) break;

                indexFirstEndBalise = sourcePage.IndexOf(strEndFirstBalise, indexStart);
                if (indexFirstEndBalise == -1) break;

                indexSecondEndBalise = sourcePage.IndexOf(strEndSecondBalise, indexFirstEndBalise);
                if (indexSecondEndBalise == -1) break;

                string content = sourcePage.Substring(indexFirstEndBalise + 1, indexSecondEndBalise - indexFirstEndBalise -1);

                //Treat the content
                content = content.Replace("\n", "");
                content = content.Replace(" ", "");

                //Treat if is a category
                if (content.Contains("."))
                {
                    string catName = content.Substring(0, content.IndexOf("."));
                    int len =content.Length - content.IndexOf(".");
                    string itemName = content.Substring(content.IndexOf(".") + 1, len-1);
                    this.addItem(catName, itemName, false);

                }
                else if (content.Contains(":"))
                {
                    string catName = content.Substring(0, content.IndexOf(":"));
                    int len = content.Length - content.IndexOf(":");
                    string itemName = content.Substring(content.IndexOf(":") + 1, len - 1);
                    this.addItem(catName, itemName, true);

                }
                else
                {
                     this.addItem(null, content, false);
                }

            }

            APICategory objectCategory = null;
            for(int i =0;i<this.Categories.Count;i++)
            {
                if(this.Categories[i].name.ToLower().Equals("object"))
                {
                    objectCategory = this.Categories[i];
                    break;
                }
            }

            if(this.editorParent.sceneSelected!=null)
            {
                CoronaGameProject currentProject = this.editorParent.sceneSelected.projectParent;
                if(currentProject != null)
                {
                    APICategory catStoryboard = new APICategory("storyboard");
                    this.Categories.Add(catStoryboard);

                    if (currentProject.Snippets.Count > 0)
                    {
                        APICategory catSnippets = new APICategory("snippets");
                        this.Categories.Add(catSnippets);
                        for (int i = 0; i < currentProject.Snippets.Count; i++)
                        {
                            Snippet snippet = currentProject.Snippets[i];
                            APIItem itemSnippet = new APIItem(snippet.Name.Replace(" ",""), true);
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

                        for (int j = 0; j < scene.Layers.Count; j++)
                        {
                            CoronaLayer layer = scene.Layers[j];
                            APICategory catLayer = new APICategory(layer.Name);
                            catScene.SubCategories.Add(catLayer);

                            for (int k = 0; k < layer.CoronaObjects.Count; k++)
                            {
                                CoronaObject obj = layer.CoronaObjects[k];
                                APICategory catObj = new APICategory(obj.DisplayObject.Name);
                                catLayer.SubCategories.Add(catObj);

                                if (objectCategory != null)
                                {
                                    catObj.SubCategories.Add(objectCategory);
                                }
                            }

                            for (int k = 0; k < layer.Jointures.Count; k++)
                            {
                                CoronaJointure joint = layer.Jointures[k];
                                APIItem itemJoint = new APIItem(joint.name,false);
                                catLayer.Items.Add(itemJoint);

                            }

                            for (int k = 0; k < layer.Controls.Count; k++)
                            {
                                CoronaControl control = layer.Controls[k];
                                APIItem itemControl = new APIItem(control.ControlName, false);
                                catLayer.Items.Add(itemControl);
                            }
                        }
                       
                    }
                }
            }

            
                
        }

        private void addItem(string catName, string itemName, bool isFunction)
        {
            if (catName != null)
            {
                //Check if the category already exists
                bool doesCategoryExist = false;
                int index = -1;

                for (int i = 0; i < this.Categories.Count; i++)
                {
                    if (this.Categories[i].name.Equals(catName))
                    {
                        doesCategoryExist = true;
                        index = i;
                        break;
                    }
                }

                if (doesCategoryExist == true)
                    this.Categories[index].Items.Add(new APIItem(itemName, isFunction));
                else
                {
                    APICategory cat = new APICategory(catName);
                    cat.Items.Add(new APIItem(itemName, isFunction));
                    this.Categories.Add(cat);
                }
            }
            else
                this.Items.Add(new APIItem(itemName, isFunction));
        }

    }
}
