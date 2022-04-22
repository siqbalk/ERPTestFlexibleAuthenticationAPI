using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class HomeDTO
    {
      
        public int UpcomingEventsCount { get; set; }
        public int NewAddedCasesCount { get; set; }
        public int CasesApproachingDeadlineCount { get; set; }
        public int CasesToBeInvoicedCount { get; set; }
    }
}
