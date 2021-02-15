using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.IGDB
{
    public class Game
    {
        public int id { get; set; }

        public int[] age_ratings { get; set; }

        public double? aggregated_rating { get; set; }

        public int? aggregated_rating_count { get; set; }

        public int[] alternative_names { get; set; }

        public int[] artworks { get; set; }

        public int[] bundles { get; set; }

        public GameCategory category { get; set; }

        public int? collection { get; set; }

        public int? cover { get; set; }

        public string created_at { get; set; }

        public int[] dlcs { get; set; }

        public int[] expansions { get; set; }

        public int[] external_games { get; set; }

        public string first_release_date { get; set; }

        public int? follows { get; set; }

        public int? franchise { get; set; }

        public int[] franchises { get; set; }

        public int[] game_engines { get; set; }

        public int[] game_modes { get; set; }

        public int[] genres { get; set; }

        public int? hypes { get; set; }

        public int[] involved_companies { get; set; }

        public int[] keywords { get; set; }

        public int[] multiplayer_modes { get; set; }

        public string name { get; set; }

        public int? parent_game { get; set; }

        public int[] platforms { get; set; }

        public int[] player_perspectives { get; set; }

        public double rating { get; set; }

        public int? rating_count { get; set; }

        public int[] release_dates { get; set; }

        public int[] screenshots { get; set; }

        public int[] similar_games { get; set; }

        public string slug { get; set; }

        public int[] standalone_expansions { get; set; }

        public GameStatus status { get; set; }

        public string storyline { get; set; }

        public string summary { get; set; }

        public int[] tags { get; set; }

        public int[] themes { get; set; }

        public double? total_rating { get; set; }

        public int? total_rating_count { get; set; }

        public string updated_at { get; set; }

        public string url { get; set; }

        public int? version_parent { get; set; }

        public string version_title { get; set; }

        public int[] videos { get; set; }

        public int[] websites { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
