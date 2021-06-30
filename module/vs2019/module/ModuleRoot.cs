
using System.Collections.Generic;
using XTC.oelMVCS;

namespace oel.archive
{
    public class ModuleRoot
    {
        public ModuleRoot()
        {
        }

        public void Inject(Framework _framework)
        {
            framework_ = _framework;
        }

        public void Register()
        {

            // 注册数据层
            framework_.getStaticPipe().RegisterModel(FileModel.NAME, new FileModel());
            // 注册视图层
            framework_.getStaticPipe().RegisterView(FileView.NAME, new FileView());
            // 注册控制层
            framework_.getStaticPipe().RegisterController(FileController.NAME, new FileController());
            // 注册服务层
            framework_.getStaticPipe().RegisterService(FileService.NAME, new FileService());
    
        }

        public void Cancel()
        {

            // 注销服务层
            framework_.getStaticPipe().CancelService(FileService.NAME);
            // 注销控制层
            framework_.getStaticPipe().CancelController(FileController.NAME);
            // 注销视图层
            framework_.getStaticPipe().CancelView(FileView.NAME);
            // 注销数据层
            framework_.getStaticPipe().CancelModel(FileModel.NAME);
    
        }

        private Framework framework_ = null;
    }
}
