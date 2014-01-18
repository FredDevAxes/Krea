using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SdlDotNet.Audio;
using Krea.CoronaClasses;
using System.IO;
namespace Krea.Asset_Manager
{
    public partial class AudioManagerPanel : UserControl
    {

        // Sound Engine C#
        //
        Sound SoundPlayer;
        Channel channelSound;
        Music MusicFile;
        //Corona Audio Object
        //
        AudioObject AudioFile;
        private AssetManagerForm MainForm;
        ///////////////////////////////////
        //Default Constructor
        ///////////////////////////////////
        public AudioManagerPanel(AssetManagerForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
        }

        ///////////////////////////////////
        //Custom Constructor
        ///////////////////////////////////
        // allow Reloading of an AudioObject
        public AudioManagerPanel(AudioObject AudioInformation, AssetManagerForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;

            AudioFile = AudioInformation;

            nameTb.Enabled = false;
            this.nameTb.Text=AudioFile.name;
            this.CompteurVolumeTb.Text =  AudioFile.volume.ToString();
            this.repeatNUD.Value =  AudioFile.loops;
            this.preloadCB.Checked = AudioFile.isPreloaded;

            if (this.AudioFile.type.Equals("STREAM"))
            {
                this.typeSoundRb.Checked = false;
                this.typeStreamRb.Checked = true;
            }
            else
            {
                this.typeSoundRb.Checked = true;
                this.typeStreamRb.Checked = false;
            }


            if (File.Exists(AudioFile.path))
            {
                if (!AudioFile.path.Equals(""))
                {
                    string[] ext = nameTb.Text.Split('.');

                    try
                    {
                        if (ext[1].ToString().ToLower() == "wav")
                        {
                            SoundPlayer = new Sound(AudioFile.path);

                        }
                        else if (ext[1].ToString().ToLower() == "ogg")
                        {
                            MusicFile = new Music(AudioFile.path);

                        }
                        else
                        {
                            SoundPlayer = null;
                            MusicFile = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Audio file loading failed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
          
           
            
        }

        private void displayObjectProperties()
        {

              this.MainForm.propertyGrid1.SelectedObject = null;
        }

        ///////////////////////////////////
        //Event
        ///////////////////////////////////

        //Close the current editor and audio file
        //
        private void closeBt_Click(object sender, EventArgs e)
        {
            if (SoundPlayer != null) SoundPlayer.Dispose();
            if (MusicPlayer.IsPlaying)
            {
                MusicPlayer.Stop();
                if (MusicPlayer.CurrentMusic != null)
                {
                    MusicPlayer.CurrentMusic.Close();
                    MusicPlayer.CurrentMusic.Dispose();
                }


            }
            if (MusicFile != null)
            {
                MusicFile.Close();
                MusicFile.Dispose();

            }

            if (channelSound != null)
            {

                channelSound.Stop();
                channelSound.Close();
                channelSound.Dispose();

            }

            this.Parent.Dispose();
        }

        // Save the instance of Corona AudioObject File
        //
        private void saveBt_Click(object sender, EventArgs e)
        {

            this.Clean();
          
        }

        public void Clean()
        {
            if (SoundPlayer != null) SoundPlayer.Dispose();
            if (MusicPlayer.IsPlaying)
            {
                MusicPlayer.Stop();
                if (MusicPlayer.CurrentMusic != null)
                {
                    MusicPlayer.CurrentMusic.Close();
                    MusicPlayer.CurrentMusic.Dispose();
                }


            }
            if (MusicFile != null)
            {
                MusicFile.Close();
                MusicFile.Dispose();

            }

            if (channelSound != null)
            {

                channelSound.Stop();
                channelSound.Close();
                channelSound.Dispose();

            }

            AudioFile.volume = Convert.ToDouble(this.CompteurVolumeTb.Text.Replace('.', ','));
            AudioFile.loops = Convert.ToInt32(this.repeatNUD.Value);
            AudioFile.isPreloaded = this.preloadCB.Checked;
            if (this.typeSoundRb.Checked)
            {
                AudioFile.type = "SOUND";
            }
            else
            {
                AudioFile.type = "STREAM";
            }


            this.MainForm.RemoveControlFromObjectsPanel(this);

            this.MainForm.RefreshAssetListView();
            this.Dispose(true);
        }
        // Import an sound file
        //
        private void importBt_Click(object sender, EventArgs e)
        {
            // We clean previous Audio file
            //
            if (channelSound != null)
            {
                channelSound.Dispose();
                channelSound = null;
            }
            if (SoundPlayer != null) SoundPlayer.Dispose();
            if (MusicPlayer.IsPlaying) MusicPlayer.Stop();
            if (MusicFile != null) MusicFile.Dispose();
            
            //Open dialogue to get the file
            //
            OpenFileDialog openFileD = new OpenFileDialog();
            openFileD.Multiselect = false;
            openFileD.DefaultExt = ".wav";
            openFileD.AddExtension = false;

            //Configure allowed extensions
            //
            openFileD.Filter = "Audio files (*.wav)|*.wav|MP3 (*.mp3)|*.mp3|OGG (*.ogg)|*.ogg|M4a (*.m4a)|*.m4a";

            if (openFileD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(openFileD.FileName))
                    {
                        string directoryDest = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software\\Asset Manager";
                        if (!Directory.Exists(directoryDest))
                            Directory.CreateDirectory(directoryDest);

                        string fileNameDest = directoryDest+"\\"+this.MainForm.CurrentAssetProject.ProjectName+"\\"+openFileD.SafeFileName;
                        File.Copy(openFileD.FileName, fileNameDest,true);
                        //Get File Name and Path
                        AudioFile.name = openFileD.SafeFileName;
                        AudioFile.path = fileNameDest;
                        this.nameTb.Text = AudioFile.name;

                        //Load a sound or a music regarding the format
                        // (only wav and ogg are supported by SDL_mixer library :(
                        // TODO : Found an other Audio Library
                        //
                        string[] ext = nameTb.Text.Split('.');

                        if (ext[1].ToString().ToLower() == "wav")
                        {
                            SoundPlayer = new Sound(fileNameDest);

                        }
                        else if (ext[1].ToString().ToLower() == "ogg")
                        {
                            MusicFile = new Music(fileNameDest);

                        }
                        else
                        {
                            //If the format is not supported, we make sure that no Audio file is loaded
                            //
                            SoundPlayer = null;
                            MusicFile = null;
                        }

                        this.nameTb.Text = AudioFile.name;
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during image loading ! \n\n " + ex.Message);
                }
            }
        }

        // Play a sound or Music with a specific Volume information
        //
        private void PlayBt_Click(object sender, EventArgs e)
        {
            if (nameTb.Text != "")
            {
                if (nameTb.Text.Contains("."))
                {
                    string[] ext = nameTb.Text.Split('.');
                    if (ext[1].ToString().ToLower() == "wav")
                    {
                        if (SoundPlayer != null)
                        {
                            SoundPlayer.Volume = VolumeTb.Value;
                            if (channelSound != null)
                            {
                                channelSound.Stop();
                                channelSound.Dispose();
                            }
                            int volumeTemp = Convert.ToInt32(Convert.ToDouble(CompteurVolumeTb.Text.Replace('.', ',')) * 128);
                            SoundPlayer.Volume = volumeTemp;

                            this.channelSound = SoundPlayer.Play(Convert.ToInt32(repeatNUD.Value));

                        }
                        else
                        {
                            MessageBox.Show("Sorry, this format is currently not supported by Krea");
                        }
                    }
                    else
                    {
                        if (MusicFile != null)
                        {
                            MusicPlayer.Volume = VolumeTb.Value;
                            if (MusicPlayer.IsPlaying) MusicPlayer.Stop();
                            int volumeTemp = Convert.ToInt32(Convert.ToDouble(CompteurVolumeTb.Text.Replace(".", ",")) * 128);
                            MusicPlayer.Volume = volumeTemp;
                            MusicPlayer.Load(MusicFile);
                            MusicPlayer.Play(1);
                        }
                        else
                        {
                            MessageBox.Show("Sorry, this format is currently not supported by this software");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please load an Audio file (.wav, .mp3, .ogg, .m4a) before!");
                }
            }
            else {
                MessageBox.Show("Please load an Audio file (.wav, .mp3, .ogg, .m4a) before!");
            }
        }

        //Stop Audio file
        //
        private void stopBt_Click(object sender, EventArgs e)
        {
            //Check if a sound is played on a channel
            //
            if (channelSound != null)
            {
                if (channelSound.IsPlaying())
                {
                    //Dispose it
                    //
                    channelSound.Stop();
                    channelSound.Dispose();
                }
            }
            //Check if a music is played and dispose it
            //
            if (MusicPlayer.IsPlaying) MusicPlayer.Stop();
        }

        // Volume Event : refresh in real time the music or sound with the correct Volume Level 
        //
        private void VolumeTb_Scroll(object sender, EventArgs e)
        {
            double result = Convert.ToDouble(VolumeTb.Value) / 100;
            CompteurVolumeTb.Text = Convert.ToString(result);

            if (MusicPlayer.IsPlaying) {
                int volumeTemp = Convert.ToInt32(Convert.ToDouble(CompteurVolumeTb.Text) * 128);
                MusicPlayer.Volume = volumeTemp;
            }
            if (channelSound != null)
            {
                if (channelSound.IsPlaying())
                {
                    int volumeTemp = Convert.ToInt32(Convert.ToDouble(CompteurVolumeTb.Text) * 128);
                    channelSound.Volume = volumeTemp;
                }
            }
        }

        // Repeat Event : reload the music or sound with the correct repeat settings
        //
        private void repeatNUD_ValueChanged(object sender, EventArgs e)
        {
            if (channelSound != null)
            {
                if (channelSound.IsPlaying())
                {
                    channelSound.Stop();
                    channelSound.Dispose();
                    SoundPlayer.Play(Convert.ToInt32(repeatNUD.Value));
                }
                else {
                    SoundPlayer.Play(Convert.ToInt32(repeatNUD.Value));
                }
            }
            if (MusicPlayer.IsPlaying)
            {
                MusicPlayer.Stop();
                MusicPlayer.Play(Convert.ToInt32(repeatNUD.Value));
            }
        }


    }
}
