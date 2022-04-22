using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class ClaimValueDTO
    {
        public bool View { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
    }
}
