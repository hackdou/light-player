namespace LightPlayer.Core.Models
{
    public record Video
    {
        public string ProviderId { get; set; }
        
        public string ExternalId { get; set; }
        
        public string Title { get; set; }
    }
}