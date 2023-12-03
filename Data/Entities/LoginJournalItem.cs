using System;

namespace ASP_SPD111.Data.Entities
{
    public class LoginJournalItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Moment {  get; set; }

    }
}
