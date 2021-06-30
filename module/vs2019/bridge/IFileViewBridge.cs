
using XTC.oelMVCS;
namespace oel.archive
{
    public interface IFileViewBridge : View.Facade.Bridge
    {
        void OnWriteSubmit();
        void OnReadSubmit();

    }
}
