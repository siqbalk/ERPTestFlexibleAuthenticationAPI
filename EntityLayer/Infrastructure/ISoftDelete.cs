using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Infrastructure
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
