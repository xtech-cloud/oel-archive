
using System;
using XTC.oelMVCS;

namespace oel.archive
{
    public class FileController: Controller
    {
        public const string NAME = "oel.archive.FileController";

        private FileView view {get;set;}

        protected override void preSetup()
        {
            view = findView(FileView.NAME) as FileView;
        }

        protected override void setup()
        {
            getLogger().Trace("setup oel.archive.FileController");
        }
    }
}
