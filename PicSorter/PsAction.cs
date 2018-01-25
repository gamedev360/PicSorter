using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/**
 * PicSorter
 * (c)2018 William Wood Harter All Rights Reserved
 */

namespace PicSorter
{
    public class PsAction
    {

        public PsAction()
        {
        }

        public virtual void DoAction()
        {
        }

        public virtual void UndoAction()
        {

        }
    }

    public class PsActionFileMove : PsAction
    {
        private string stOrigFile;
        private string stNewFile;

        public PsActionFileMove(string stOrigFileSet, string stToDir)
        {
            stOrigFile = stOrigFileSet;

            FileInfo fi = new FileInfo(stOrigFileSet);
            string toloc = stToDir + "\\" + fi.Name;

            int cnt = 1;
            while (File.Exists(toloc))
            {
                string fiNamePart = fi.Name.Substring(0, fi.Name.IndexOf("."));
                toloc = stToDir + "\\" + fiNamePart + cnt + fi.Extension;
            }

            stNewFile = toloc;

        }
        public override void DoAction()
        {
            File.Move(stOrigFile, stNewFile);
        }

        public override void UndoAction()
        {
            File.Move(stNewFile,stOrigFile);
        }
    }
}
