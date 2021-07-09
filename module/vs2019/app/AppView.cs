
using System.Collections.Generic;
using XTC.oelMVCS;

namespace app
{
    class AppView : View
    {
        public const string NAME = "AppView";

        protected override void preSetup()
        {
        }

        protected override void setup()
        {
            addRouter("/module/view/attach", this.handleAttachView);
        }

        private void handleAttachView(Model.Status _status, object _data)
        {
            MainWindow mainWindow = App.Current.MainWindow as MainWindow;
            getLogger().Trace("attach view");
            Dictionary<string, object> data = _data as Dictionary<string, object>;
            foreach(string key in data.Keys)
            {
                mainWindow.AddPage(key, data[key]);
            }
        }
    }//class
}//namespace
