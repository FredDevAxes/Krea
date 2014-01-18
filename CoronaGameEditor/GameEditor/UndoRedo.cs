using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krea.CoronaClasses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Krea.GameEditor
{
    public class UndoRedo
    {
        //---------------------------------------------------
        //-------------------Attributs-------------------
        //---------------------------------------------------
        List<byte[]> ScenesStackUndo;
        List<byte[]> ScenesStackRedo;
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public UndoRedo()
        {
            ScenesStackUndo = new List<byte[]>();
            ScenesStackRedo = new List<byte[]>();
        }

        //---------------------------------------------------
        //-------------------Methodes-------------------
        //---------------------------------------------------
        public bool DO(Scene scene)
        {

            byte[] sceneSerialized = this.serializeScene(scene);
            if (sceneSerialized != null)
            {
                this.pushIntoStackUndo(sceneSerialized);
                this.ScenesStackRedo.Clear();
                return true;
            }
            else
                return false;
        }

        public Scene Undo()
        {
            byte[] seneSerialized = this.popFromStackUndo();
            if (seneSerialized != null)
            {
                Scene scene = this.DeSerializeScene(seneSerialized);

                return scene;
            }
            return null;
        }

        public Scene ReDo()
        {

            byte[] seneSerialized = this.popFromStackRedo();
            if (seneSerialized != null)
            {
                Scene scene = this.DeSerializeScene(seneSerialized);

                return scene;
            }
            return null;
        }

        public void clearBuffers()
        {
            this.ScenesStackRedo.Clear();
            this.ScenesStackUndo.Clear();
        }
        private bool pushIntoStackUndo(byte[] sceneSerialized)
        {
            if (this.ScenesStackUndo != null)
            {
                this.ScenesStackUndo.Add(sceneSerialized);
                if (this.ScenesStackUndo.Count > 10)
                    this.ScenesStackUndo.RemoveAt(0);
                return true;
            }
            else
                return false;
        }


        private bool pushIntoStackRedo(byte[] sceneSerialized)
        {
            if (this.ScenesStackRedo != null)
            {
                this.ScenesStackRedo.Add(sceneSerialized);
                if (this.ScenesStackRedo.Count > 10)
                    this.ScenesStackRedo.RemoveAt(0);
                return true;
            }
            else
                return false;
        }


        private byte[] popFromStackUndo()
        {
            if (this.ScenesStackUndo != null)
            {
                if (this.ScenesStackUndo.Count > 0)
                {
                    byte[] sceneSerialied = this.ScenesStackUndo[this.ScenesStackUndo.Count - 1];

                    //Save for REDO
                    this.pushIntoStackRedo(sceneSerialied);

                    this.ScenesStackUndo.RemoveAt(this.ScenesStackUndo.Count - 1);
                    return sceneSerialied;
                }
                else
                    return null;

            }
            return null;
        }

        private byte[] popFromStackRedo()
        {
            if (this.ScenesStackRedo != null)
            {
                if (this.ScenesStackRedo.Count > 0)
                {
                    byte[] sceneSerialied = this.ScenesStackRedo[this.ScenesStackRedo.Count - 1];

                    //Save into undo
                    this.pushIntoStackUndo(sceneSerialied);

                    this.ScenesStackRedo.RemoveAt(this.ScenesStackRedo.Count - 1);
                    return sceneSerialied;
                }
                else
                    return null;

            }
            return null;
        }

        private byte[] serializeScene(Scene scene)
        {
            try
            {
                MemoryStream memStream1 = new MemoryStream();

                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(memStream1, scene);

                byte[] bytes = memStream1.GetBuffer();

                memStream1.Close();

                return bytes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable do save Scene State !\n Undo Impossible ! \n" + ex.Message);
                return null;
            }
        }

        private Scene DeSerializeScene(byte[] sceneSerialized)
        {
            try
            {
                MemoryStream memStream2 = new MemoryStream(sceneSerialized);
                BinaryFormatter formatter = new BinaryFormatter();
                Scene scene = (Scene)formatter.Deserialize(memStream2);
                return scene;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable do load Scene State !\n Redo Impossible ! \n" + ex.Message);
                return null;
            }
        }
    }
}
