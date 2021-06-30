
using XTC.oelMVCS;
namespace oel.archive
{
    public interface IFileUiBridge : View.Facade.Bridge
    {
        object getRootPanel();
        void Alert(string _message);
    }
}
