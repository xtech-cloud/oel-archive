
using XTC.oelMVCS;
namespace oel.archive
{
    public class FileViewBridge : IFileViewBridge
    {
        public FileView view{ get; set; }
        public FileService service{ get; set; }


        public void OnWriteSubmit()
        {
            Proto.BlankRequest req = new Proto.BlankRequest();

            service.PostWrite(req);
        }
        

        public void OnReadSubmit()
        {
            Proto.BlankRequest req = new Proto.BlankRequest();

            service.PostRead(req);
        }
        


    }
}
