using System.Collections.Generic;

namespace LightPlayer.Core.Models
{
    public record PlayList
    {
        public string Title { get; set; }
        
        public IReadOnlyCollection<Video> Videos { get; set; }
    }
}