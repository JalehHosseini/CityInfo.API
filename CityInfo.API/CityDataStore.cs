using CityInfo.API.Models;

namespace CityInfo.API;

public class CityDataStore
{
    public List<CityDto> Cities { get; set; }
    //public static CityDataStore Instance { get; } = new CityDataStore();

    public CityDataStore()
    {
        Cities = new List<CityDto>()
        {
            new CityDto(){Id=1,Name="Tehran",Description="This Is My City",
                PointOfInterest=new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto(){Id=1,Name="Jaye Didani 1",Description="This Is Jaye Didani 1"},
                    new PointOfInterestDto(){Id=2,Name="Jaye Didani 2",Description="This Is Jaye Didani 2"},
                }},
            new CityDto(){Id=2,Name="Shiraz",Description="This Is My City",PointOfInterest=new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto(){Id=3,Name="Jaye Didani 3",Description="This Is Jaye Didani 3"},
                    new PointOfInterestDto(){Id=4,Name="Jaye Didani 4",Description="This Is Jaye Didani 4"},
                }},
            new CityDto(){Id=3,Name="Ahvaz",Description="This Is My City",PointOfInterest=new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto(){Id=5,Name="Jaye Didani 5",Description="This Is Jaye Didani 5"},
                    new PointOfInterestDto(){Id=6,Name="Jaye Didani 6",Description="This Is Jaye Didani 6"},
                }}
        };
    }
}
