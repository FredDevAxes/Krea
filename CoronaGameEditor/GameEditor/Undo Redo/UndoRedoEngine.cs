using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.Undo_Redo
{
    public class UndoRedoEngine
    {
        private Form1 mainForm;
        private List<ActionCommand> UndoCommandList;
        private List<ActionCommand> RedoCommandList;

        public UndoRedoEngine(Form1 mainForm)
        {
            this.mainForm = mainForm;
            this.UndoCommandList = new List<ActionCommand>();
            this.RedoCommandList = new List<ActionCommand>();
        }

        public void Do(ActionCommand command)
        {
            
        }

        public bool Undo()
        {
          
            return false;
        }

        public bool ReDo()
        {
            return false;
        }

        public void clearBuffers()
        {
            this.UndoCommandList.Clear();
            this.RedoCommandList.Clear();
        }
        private bool pushIntoStackUndo(ActionCommand command)
        {

                return false;
        }


        private bool pushIntoStackRedo(ActionCommand command)
        {
         
              return false;
        }


        private ActionCommand popFromStackUndo()
        {
            return null;
        }

        private ActionCommand popFromStackRedo()
        {
            return null;
        }

        private ActionCommand invertCommand(ActionCommand command)
        {
            
            if (command.ActionCommandType == ActionCommand.ActionType.EditObject)
            {
             //   ActionCommand invertedCommand = new ActionCommand();
            }
            return null;
        }
    }
}
