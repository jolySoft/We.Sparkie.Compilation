using System;

namespace We.Sparkie.Compilation.Api.Entities
{
    public class Track : Entity
    {
        public Guid DigitalAssetId { get; set;  }
        public Artist Artist { get; set; }
        public string Name { get; set; }
    }
}