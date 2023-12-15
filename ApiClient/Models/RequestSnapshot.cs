namespace ApiClient.Models
{
    public class DefaultSaveRequest : ISaveRequest
    {
        public void Save(RequestSnapshot requestSnapshot) { }
    }

    public interface ISaveRequest
    {
        public void Save(RequestSnapshot requestSnapshot);
    }

    public class RequestSnapshot
    {
        public long RequestID { get; set; }

        public string Route { get; set; } = null!;

        public string RouteParameter { get; set; } = null!;

        public string Parameters { get; set; } = null!;

        public string Response { get; set; } = null!;

        public DateTime DateTime { get; set; }
    }
}
