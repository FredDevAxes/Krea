using System;
using Krea.CoronaClasses;
using System.Drawing;

namespace Krea.GameEditor.TilesMapMobileEditor
{
     [Serializable()]
    public class TilesMapEditorMobile : Scene
    {
        //---------------------------------------------------
        //-------------------Attributes-----------------------

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
         public TilesMapEditorMobile(CoronaGameProject projectParent)
             :base(new Size(320,480),projectParent.Orientation,projectParent)
         {
             this.projectParent = projectParent;

             this.Name = "mapmobileeditor";
         }
        //---------------------------------------------------
        //-------------------Methods-------------------------
        //---------------------------------------------------
    }
}
