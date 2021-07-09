
using System;
using System.Collections.Generic;
using XTC.oelMVCS;

namespace oel.archive
{
    public class FileView : View
    {
        public const string NAME = "oel.archive.FileView";

        private Facade facade = null;
        private FileModel model = null;
        private IFileUiBridge bridge = null;

        protected override void preSetup()
        {
            model = findModel(FileModel.NAME) as FileModel;
            var service = findService(FileService.NAME) as FileService;
            facade = findFacade("oel.archive.FileFacade");
            FileViewBridge vb = new FileViewBridge();
            vb.view = this;
            vb.service = service;
            facade.setViewBridge(vb);
        }

        protected override void setup()
        {
            getLogger().Trace("setup oel.archive.FileView");

            addRouter("/oel/archive/File/Write", this.handleFileWrite);

            addRouter("/oel/archive/File/Read", this.handleFileRead);

        }

        protected override void postSetup()
        {
            bridge = facade.getUiBridge() as IFileUiBridge;
            object rootPanel = bridge.getRootPanel();
            // 通知主程序挂载界面
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["oel.archive.File"] = rootPanel;
            model.Broadcast("/module/view/attach", data);
        }

        public void Alert(string _message)
        {
            bridge.Alert(_message);
        }

        private void handleFileWrite(Model.Status _status, object _data)
        {
            var rsp = (Proto.BlankResponse)_data;
            if (rsp._status._code.AsInt() == 0)
                bridge.Alert("Success");
            else
                bridge.Alert(string.Format("Failure：\n\nCode: {0}\nMessage:\n{1}", rsp._status._code.AsInt(), rsp._status._message.AsString()));
        }

        private void handleFileRead(Model.Status _status, object _data)
        {
            var rsp = (Proto.BlankResponse)_data;
            if (rsp._status._code.AsInt() == 0)
                bridge.Alert("Success");
            else
                bridge.Alert(string.Format("Failure：\n\nCode: {0}\nMessage:\n{1}", rsp._status._code.AsInt(), rsp._status._message.AsString()));
        }

    }
}
