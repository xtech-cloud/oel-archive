
using XTC.oelMVCS;

namespace wpf
{
}

namespace oel.archive
{
    public class ControlRoot
    {
        public ControlRoot()
        {
        }

        public void Inject(Framework _framework)
        {
            framework_ = _framework;
        }

        public void Register()
        {

            // 注册UI装饰
            FileFacade facadeFile = new FileFacade();
            framework_.getStaticPipe().RegisterFacade(FileFacade.NAME, facadeFile);
            FileControl controlFile = new FileControl();
            controlFile.facade = facadeFile;
            FileControl.FileUiBridge uiFileBridge = new FileControl.FileUiBridge();
            uiFileBridge.control = controlFile;
            facadeFile.setUiBridge(uiFileBridge);
        
        }

        public void Cancel()
        {

            // 注销UI装饰
            framework_.getStaticPipe().CancelFacade(FileFacade.NAME);
        
        }

        private Framework framework_ = null;
    }
}
