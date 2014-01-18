using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krea.CoronaClasses;

namespace Krea.GameEditor.Undo_Redo
{
    public class ActionCommand
    {

        public enum ActionType
        {
            NewObject = 1,
            RemoveObject = 2,
            EditObject = 3
            
        }

        public ActionType ActionCommandType;
        private CoronaObject obj;
        public bool IsUndo;
        private object propToModify;
        private object valueAffected;

        public ActionCommand(ActionType actionCommandType, CoronaObject obj, bool isUndo)
        {
            this.ActionCommandType = actionCommandType;
            this.obj = obj;
            this.IsUndo = isUndo;

        }

        public ActionCommand(ActionType actionCommandType, CoronaObject obj,object propToModify,object valueAffected, bool isUndo)
        {
            this.ActionCommandType = actionCommandType;
            this.obj = obj;
            this.IsUndo = isUndo;
            this.valueAffected = valueAffected;
            this.propToModify = propToModify;

        }
    }
}
