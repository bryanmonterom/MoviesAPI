using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class TheatersNearFilterDTO
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
        private int distanceInKms = 10;
        private int maxDistanceKms = 50;
        public int DistanceInKms
        {
            get { return distanceInKms; }
            set { distanceInKms = (value > maxDistanceKms) ? distanceInKms : value; }
        }
    }
}
