using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Shared.Dto.Video
{
    public record VideoMessageDto
    {
        public Guid VideoId { get; set; }
        public string FileName { get; set; }
    }
}
