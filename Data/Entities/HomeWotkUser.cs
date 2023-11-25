namespace ASP_SPD111.Data.Entities
{
    public class HomeWotkUser
    {
        public Guid Id { get; set; }
        public String FirstName { get; set; } = null;
        public String LastName { get; set; } = null;
        public String Email { get; set; } = null;
        public String Phone { get; set; } = null;
        public DateTime Moment {  get; set; }
    }
}
