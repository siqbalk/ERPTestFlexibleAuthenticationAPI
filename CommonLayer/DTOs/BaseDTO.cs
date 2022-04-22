using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class BaseDTO
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }

    }
}
