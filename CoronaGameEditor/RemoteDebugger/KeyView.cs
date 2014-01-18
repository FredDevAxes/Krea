using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Krea.RemoteDebugger
{
    public partial class KeyView : UserControl
    {
        AppRemoteController controllerParent;

        public KeyView()
        {
            InitializeComponent();
        }

        public void init(AppRemoteController controllerParent)
        {
            this.controllerParent = controllerParent;

        }

        private void backKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "back";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName,keyName,phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void backKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "back";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void searchKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "search";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void searchKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "search";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void menuKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "menu";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void menuKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "menu";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void volumeUpKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "volumeUp";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void volumeUpKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "volumeUp";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void volumeDownKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "volumeDown";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void volumeDownKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "volumeDown";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void upPadKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "up";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void upPadKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "up";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void leftPadKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "left";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void leftPadKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "left";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void rightPadKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "right";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void rightPadKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "right";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void centerPadKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "center";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void centerPadKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "center";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void downPadKeyBt_MouseDown(object sender, MouseEventArgs e)
        {
            string keyName = "down";
            string phase = "down";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void downPadKeyBt_MouseUp(object sender, MouseEventArgs e)
        {
            string keyName = "down";
            string phase = "up";
            string eventName = "key";

            RemoteEvent keyEvent = new RemoteEvent(eventName, keyName, phase);
            string eventJSON = keyEvent.serialize();
            if (eventJSON != null)
            {
                this.controllerParent.sendEvent(eventJSON);
            }
        }

        private void KeyView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                selectButton(this.downPadKeyBt);
                this.downPadKeyBt_MouseDown(null, null);
            }
            else if (e.KeyCode == Keys.Up)
            {
                selectButton(this.upPadKeyBt);
                this.upPadKeyBt_MouseDown(null, null);
            }
            else if (e.KeyCode == Keys.Left)
            {
                selectButton(this.leftPadKeyBt);
                this.leftPadKeyBt_MouseDown(null, null);
            }
            else if (e.KeyCode == Keys.Right)
            {
                selectButton(this.rightPadKeyBt);
                this.rightPadKeyBt_MouseDown(null, null);
            }
        }

        private void KeyView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                unselectButton(this.downPadKeyBt);
                this.downPadKeyBt_MouseUp(null, null);
            }
            else if (e.KeyCode == Keys.Up)
            {
                unselectButton(this.upPadKeyBt);
                this.upPadKeyBt_MouseUp(null, null);
            }
            else if (e.KeyCode == Keys.Left)
            {
                unselectButton(this.leftPadKeyBt);
                this.leftPadKeyBt_MouseUp(null, null);
            }
            else if (e.KeyCode == Keys.Right)
            {
                unselectButton(this.rightPadKeyBt);
                this.rightPadKeyBt_MouseUp(null, null);
            }
        }

        private void selectButton(Button bt)
        {
            bt.BackColor = Color.Turquoise;

        }

        private void unselectButton(Button bt)
        {
            bt.BackColor = Color.Transparent;
        }

        public void refreshButtonStateFromRemote(string name, string phase)
        {
            if (name.Equals("back"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.backKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.backKeyBt);
            }

            else if (name.Equals("menu"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.menuKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.menuKeyBt);
            }

            else if (name.Equals("search"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.searchKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.searchKeyBt);
            }

            else if (name.Equals("volumeDown"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.volumeDownKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.volumeDownKeyBt);
            }

            else if (name.Equals("volumeUp"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.volumeUpKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.volumeUpKeyBt);
            }

            else if (name.Equals("up"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.upPadKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.upPadKeyBt);
            }

            else if (name.Equals("down"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.downPadKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.downPadKeyBt);
            }

            else if (name.Equals("right"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.rightPadKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.rightPadKeyBt);
            }

            else if (name.Equals("left"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.leftPadKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.leftPadKeyBt);
            }

            else if (name.Equals("center"))
            {
                if (phase.Equals("down"))
                    this.selectButton(this.centerPadKeyBt);
                else if (phase.Equals("up"))
                    this.unselectButton(this.centerPadKeyBt);
            }
        }


        
    }
}
