using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.IGDB
{
    public class Platform
    {
        public int id { get; set; }

        public string abbreviation { get; set; }

        public string alternative_name { get; set; }

        public PlatformCategory? category { get; set; }

        public string created_at { get; set; }

        public int? generation { get; set; }

        public string name { get; set; }

        public int? platform_logo { get; set; }
        
        public int? platform_family { get; set; }

        public string slug { get; set; }

        public string summary { get; set; }

        public string updated_at { get; set; }
        
        public string url { get; set; }
        
        public int[] versions { get; set; }
        
        public int[] websites { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
