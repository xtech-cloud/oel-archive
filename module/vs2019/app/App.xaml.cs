
using System.Windows;
using XTC.oelMVCS;
using oel.archive;

namespace app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Framework framework_ { get; set; }
        private ConsoleLogger logger_ { get; set; }
        private Config config_ { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 静态管线注册组件
            registerMVCS();

            ModuleRoot moduleRoot = new ModuleRoot();
            moduleRoot.Inject(framework_);
            moduleRoot.Register();
            ControlRoot controlRoot = new ControlRoot();
            controlRoot.Inject(framework_);
            controlRoot.Register();
            framework_.Setup();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            logger_ = new ConsoleLogger();
            config_ = new AppConfig();

            MainWindow mainWindow = new MainWindow();
            this.MainWindow = mainWindow;
            logger_.rtbLog = mainWindow.rtbLog;
            mainWindow.Show();

            framework_ = new Framework();
            framework_.setLogger(logger_);
            framework_.setConfig(config_);
            framework_.Initialize();

            base.OnStartup(e);

            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            framework_.Release();
            framework_ = null;
        }

        private void registerMVCS()
        {
            BlankModel blankModel = new BlankModel();
            framework_.getStaticPipe().RegisterModel(BlankModel.NAME, blankModel);

            AppView appView = new AppView();
            framework_.getStaticPipe().RegisterView(AppView.NAME, appView);
        }
    }
}

