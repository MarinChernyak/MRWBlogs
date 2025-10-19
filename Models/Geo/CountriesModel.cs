using SMGeo.GeoEntities;

namespace MRWBlogs.Models.Geo
{
    public class CountriesModel
    {
        public int SelectedCountryId { get; set; }
        public List<Country> Countries { get; set; } = new List<Country>();

        public CountriesModel()
        {
            InitCollections();
        }
        protected void InitCollections()
        {
            using (var context = new SMGeoContext())
            {
                Countries = context.Countries.OrderBy(c => c.CountryName).ToList();
            }
        }
    }
}
