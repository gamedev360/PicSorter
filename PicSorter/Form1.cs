using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

/**
 * PicSorter
 * (c)2018 William Wood Harter All Rights Reserved
 */

namespace PicSorter
{
    public partial class Form1 : Form
    {
        //private static string ST_IMAGE_FILE_FILTER = "*.jpg";
        string stDirBrowsing;
        string[] filesInDir;
        int iCurPic = 0;
        int iCurPic2 = 0;
        string pic1File;
        string pic2File;
        Bitmap bmPic1;
        Bitmap bmPic2;
        bool bUndoDown=false;
        
        Stack<PsAction> stackUndo;
        Stack<PsAction> stackRedo;

        public Form1()
        {
            InitializeComponent();
            LayoutControls();

            stackUndo = new Stack<PsAction>();
            stackRedo = new Stack<PsAction>();

            string sSettingsFile = Application.StartupPath+"\\PicSorter.ini";
            if (File.Exists(sSettingsFile))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(sSettingsFile))
                    {
                        String line = sr.ReadLine();
                        Console.WriteLine(line);
                        textBoxDir.Text = line;
                        stDirBrowsing = line;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
        }

        private string[] GetImageFiles(string stdir)
        {
            //string[] tmpres = Directory.EnumerateFiles(stdir).ToArray();
            return Directory.EnumerateFiles(stdir, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".jpg") || s.EndsWith(".JPG") || s.EndsWith(".bmp") || s.EndsWith("*.BMP") || s.EndsWith(".png") || s.EndsWith("*.PNG")).ToArray();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            // get all the files in the directory
            stDirBrowsing = textBoxDir.Text;
            if (!Directory.Exists(stDirBrowsing))
            {
                stDirBrowsing = "c:\\";
            }

            filesInDir = GetImageFiles(stDirBrowsing);
            //filesInDir = Directory.GetFiles(stDirBrowsing, ST_IMAGE_FILE_FILTER);

            /*
            int i;
            for (i = 0; i < filesInDir.Length; i++)
            {
                Console.Write("filesInDir[" + i + "] = " + filesInDir[i]+"\n");
            }
            */
            iCurPic = 0;
            iCurPic2 = 1;
            SetupPictureBoxes(true);

        }

        private void buttonChooseDir_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.
            folderBrowserDialog1.SelectedPath = textBoxDir.Text;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxDir.Text = folderBrowserDialog1.SelectedPath;
                stDirBrowsing = folderBrowserDialog1.SelectedPath;

                // save the current dir for next startup
                string sSettingsFile = Application.StartupPath + "\\PicSorter.ini";
                try
                {
                    using (StreamWriter sw = new StreamWriter(sSettingsFile))
                    {
                        sw.WriteLine(stDirBrowsing);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The settings file could not be written:");
                    Console.WriteLine(ex.Message);
                }


                filesInDir = GetImageFiles(stDirBrowsing);
                //filesInDir = Directory.GetFiles(stDirBrowsing,ST_IMAGE_FILE_FILTER);

                iCurPic = 0;
                iCurPic2 = 1;
                SetupPictureBoxes(true);

            }
        }

        private void SetupPictureBoxes(bool bReloadImages)
        {
            if (bReloadImages)
            {
                pic1File = null;
                if (iCurPic < filesInDir.Length)
                {
                    pic1File = filesInDir[iCurPic];
                }

                pic2File = null;
                if (iCurPic2 < filesInDir.Length)
                {
                    pic2File = filesInDir[iCurPic2];
                }

                // Sets up an image object to be displayed. 
                if (bmPic1 != null)
                {
                    bmPic1.Dispose();
                }
                if (bmPic2 != null)
                {
                    bmPic2.Dispose();
                }
                bmPic1 = null;
                bmPic2 = null;

                // Stretches the image to fit the pictureBox.
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            if (pic1File != null)
            {
                bmPic1 = new Bitmap(pic1File);
                SetPictureBoxSize(splitContainer1.Panel1, pictureBox1, bmPic1);
                pictureBox1.Image = (Image)bmPic1;
            }


            if (pic2File != null)
            {
                bmPic2 = new Bitmap(pic2File);
                SetPictureBoxSize(splitContainer1.Panel2, pictureBox2, bmPic2);
                pictureBox2.Image = (Image)bmPic2;
            }
        }

        private void SetPictureBoxSize(SplitterPanel pan, PictureBox pbSet, Bitmap bmSize)
        {
            int pbx = pan.Size.Width;
            int pby = pan.Size.Height;

            if (bmSize.Width > bmSize.Height)
            {
                // what is the % diff in size if we resize the horiz
                float sizeratio = (float)bmSize.Width / (float)pan.Size.Width;
                pby = (int)((float)bmSize.Height / sizeratio);
            }
            else
            {
                float sizeratio = (float)bmSize.Height / (float)pan.Size.Height;
                pbx = (int)((float)bmSize.Width / sizeratio);
            }

            /*
            float sizeratio = (float) bmSize.Width / (float) bmSize.Height;

            int pbx = pan.Size.Width;
            int pby = pan.Size.Height;

            if (sizeratio > 1)
            {
                pby = (int) ((float)pan.Size.Height / sizeratio);
            }
            else
            {
                pbx = (int) ((float) pan.Size.Width * sizeratio);
            }
            */

            pbSet.ClientSize = new Size(pbx, pby);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (filesInDir != null)
            {
                if (keyData == Keys.Left)
                {
                    if (iCurPic > 0)
                    {
                        iCurPic--;
                        iCurPic2 = iCurPic + 1;
                    }
                    else if (filesInDir.Length>1)
                    {
                        iCurPic = filesInDir.Length - 1;
                        iCurPic2 = 0;
                    }

                    SetupPictureBoxes(true);
                }
                else if (keyData == Keys.Right)
                {
                    if (iCurPic2 < filesInDir.Length - 1)
                    {
                        iCurPic2++;
                        iCurPic = iCurPic2 -1;
                    }
                    else if (filesInDir.Length > 1)
                    {
                        iCurPic = filesInDir.Length - 1;
                        iCurPic2 = 0;
                    }
                    SetupPictureBoxes(true);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // checking for ctrl key
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Z:
                    if (bUndoDown)
                        break;

                    if (e.Modifiers == (Keys.Control | Keys.Shift))
                    {
                        bUndoDown = true;
                        Console.WriteLine("redo pressed");

                        // redo pressed
                        if (stackRedo.Count > 0)
                        {
                            // undo
                            PsAction psaTmp = stackRedo.Pop();
                            psaTmp.UndoAction();

                            // put it onto the redo stack
                            stackUndo.Push(psaTmp);
                        }


                    }
                    else if (Control.ModifierKeys == Keys.Control)
                    {
                        bUndoDown = true;
                       // Console.WriteLine("undo pressed ");

                        // undo pressed
                        if (stackUndo.Count > 0)
                        {
                            // undo
                            PsAction psaTmp = stackUndo.Pop();
                            psaTmp.UndoAction();

                            // put it onto the redo stack
                            stackRedo.Push(psaTmp);

                            filesInDir = GetImageFiles(stDirBrowsing);
                            if (iCurPic > 0)
                            {
                                iCurPic--;
                            }
                            iCurPic2 = iCurPic + 1;
                            
                            SetupPictureBoxes(true);
                        }
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (bUndoDown)
            {
                if (e.KeyCode==Keys.Z)
                {
                    bUndoDown = false;
                }
            }
        }
        // Detect all numeric characters at the form level
        // Note that Form.KeyPreview must be set to true for this event handler to be called. 
        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char x = e.KeyChar;

            Console.WriteLine("Key Press, " + e.KeyChar);

            switch (e.KeyChar)
            {
                case 'D':  
                case 'd': 
                    MoveFileTo("Del");
                    break;
                case 'B':
                case 'b':
                    MoveFileTo("bside");
                    break;
                    
                case 'R':
                case 'r':
                    RotateCurrentImage();
                    break;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
           // Console.WriteLine(" Main window activated");
            
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            MoveFileTo("Del");
        }

        private void MoveFileTo(string subdirto)
        {
            // move the current image to a delete folder

            // get all the files in the directory
            string stDirDelDir = stDirBrowsing + "\\"+subdirto;
            if (!Directory.Exists(stDirDelDir))
            {
                Directory.CreateDirectory(stDirDelDir);
            }

            // Release the two images, one is about to be deleted. 
            if (bmPic1 != null)
            {
                bmPic1.Dispose();
            }
            if (bmPic2 != null)
            {
                bmPic2.Dispose();
            }
            bmPic1 = null;
            bmPic2 = null;

            PsActionFileMove psam = new PsActionFileMove(filesInDir[iCurPic], stDirDelDir);
            psam.DoAction();
            stackUndo.Push(psam);
            // need to clear the tail of the redo chain if we moved back and now forward
            stackRedo.Clear();

            filesInDir = GetImageFiles(stDirBrowsing);
            //filesInDir = Directory.GetFiles(stDirBrowsing, ST_IMAGE_FILE_FILTER);
            if (iCurPic > filesInDir.Length - 1)
            {
                iCurPic = filesInDir.Length - 1;
                if (iCurPic == 0)
                {
                    iCurPic2 = 1; // set this to 1 since there is only 1 file left
                }
            }

            if (iCurPic2 > filesInDir.Length - 1)
            {
                iCurPic2 = 0;
                if (iCurPic2 == iCurPic)
                {
                    iCurPic2 = 1;
                }

            }

            SetupPictureBoxes(true);
            /*
            int i;
            for (i = 0; i < filesInDir.Length; i++)
            {
                Console.Write("filesInDir[" + i + "] = " + filesInDir[i] + "\n");
            }
             */

        }

        private void buttonBside_Click(object sender, EventArgs e)
        {
            MoveFileTo("bside");
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("split container resize " + this.Size);
            /*
            // Sets up an image object to be displayed. 
            if (bmPic1 != null)
            {
                bmPic1.Dispose();
            }
            if (bmPic2 != null)
            {
                bmPic2.Dispose();
            }
            bmPic1 = null;
            bmPic2 = null;
            */
        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("split container size changed " + this.Size);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Console.WriteLine("form1 setup to auto size");
        }

        private void LayoutControls()
        {
            splitContainer1.SetBounds(0, textBoxDir.Bottom+15, this.ClientSize.Width, this.ClientSize.Height - 150);
            Rectangle bound = buttonDelete.Bounds;
            buttonDelete.SetBounds(bound.X, this.ClientSize.Height - (bound.Height + 15), bound.Width, bound.Height);
            bound = buttonBside.Bounds;
            buttonBside.SetBounds(bound.X, this.ClientSize.Height - (bound.Height + 15), bound.Width, bound.Height);
            bound = buttonRotate.Bounds;
            buttonRotate.SetBounds(bound.X, this.ClientSize.Height - (bound.Height + 15), bound.Width, bound.Height);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            LayoutControls();
            SetupPictureBoxes(false);
            Console.WriteLine("form1 size changed");

        }
        
        private void buttonRotate_Click(object sender, EventArgs e)
        {

            RotateCurrentImage();
        }

        private void RotateCurrentImage()
        {
            // Sets up an image object to be displayed. 
            if (bmPic1 != null)
            {
                bmPic1.Dispose();
            }
            if (bmPic2 != null)
            {
                bmPic2.Dispose();
            }
            bmPic1 = null;
            bmPic2 = null;

            //create an object that we can use to examine an image file
            using (Image img = Image.FromFile(filesInDir[iCurPic]))
            {
                //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                FileInfo fi = new FileInfo(filesInDir[iCurPic]);
                img.Save(fi.Directory + "\\" + fi.Name, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            SetupPictureBoxes(true);
        }


    }
}
