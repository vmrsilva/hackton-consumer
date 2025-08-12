using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Shared.FileServices
{
    public interface IFileService
    {
        Task<Stream> DownloadVideoAsync(string blobName);
    }
}
