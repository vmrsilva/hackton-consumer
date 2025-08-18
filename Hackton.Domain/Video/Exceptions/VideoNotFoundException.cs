using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain.Video.Exceptions
{
    public class VideoNotFoundException : Exception
    {
        public VideoNotFoundException() : base(message: "Video não encontrado.")
        {

        }
    }
}
