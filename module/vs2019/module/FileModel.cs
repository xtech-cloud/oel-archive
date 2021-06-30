
using System;
using XTC.oelMVCS;

namespace oel.archive
{
    public class FileModel : Model
    {
        public const string NAME = "oel.archive.FileModel";

        public class FileStatus : Model.Status
        {
            public const string NAME = "oel.archive.FileStatus";
        }

        private FileController controller {get;set;}

        protected override void preSetup()
        {
            controller = findController(FileController.NAME) as FileController;
            Error err;
            status_ = spawnStatus<FileStatus>(FileStatus.NAME, out err);
            if(0 != err.getCode())
            {
                getLogger().Error(err.getMessage());
            }
        }

        protected override void setup()
        {
            getLogger().Trace("setup oel.archive.FileModel");
        }

        protected override void preDismantle()
        {
            Error err;
            killStatus(FileStatus.NAME, out err);
            if(0 != err.getCode())
            {
                getLogger().Error(err.getMessage());
            }
        }

        private FileStatus status
        {
            get
            {
                return status_ as FileStatus;
            }
        }
    }
}
