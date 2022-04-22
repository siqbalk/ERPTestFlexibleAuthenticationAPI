using Microsoft.AspNetCore.Http;
using System;

namespace CommonLayer.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int PlanId { get; set; }
        public string SubscriptionPlanName { get; set; }
        public string ImageURL { get; set; }
        public int ProfileCompletion { get; set; }
        public DateTime? RecurringDate { get; set; }

        public bool IsFreePlan { get; set; }
        public bool IsPaidPlan { get; set; }
        public int DaysLeft { get; set; }

        public string SubscriptionMessage { get; set; }
        public bool IsValidPlan { get; set; }
        public int NumberOfUsersPurchased { get; set; }
        public string CurrentPlanStatus { get; set; }
    }
}