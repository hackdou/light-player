namespace LightPlayer.Core.Models
{
    public record Episode
    {
        public string ProviderId { get; set; }
        
        public string ExternalId { get; set; }
        
        public string Title { get; set; }
        
        public string ImageUrl { get; set; }
    }
}