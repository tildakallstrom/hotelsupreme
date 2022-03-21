namespace projekt.Models
{
    public class Rooms
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        internal Task<Rooms> AddOrUpdate(Rooms oRoom)
        {
            throw new NotImplementedException();
        }
    }
}
