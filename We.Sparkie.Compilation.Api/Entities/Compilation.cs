﻿using System.Collections.Generic;

namespace We.Sparkie.Compilation.Api.Entities
{
    public class Compilation : Entity
    {
        public CompilationType CompilationType { get; set; }
        public string CreatedBy { get; set; }
        public List<Track> Tracks { get; set; }
    }
}