using System.Collections.Generic;

namespace csv2edassistant
{
    public class StarSystem
    {
        private string name;
        private List<Station> stations;
        public bool isAnarchy;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public List<Station> Stations
        {
            get { return stations; }
        }
        public bool IsAnarchy
        {
            get { return isAnarchy; }
            set { isAnarchy = value; }
        }

        public StarSystem()
        {
            stations = new List<Station>();
        }
        public StarSystem(StarSystem copy)
        {
            stations = new List<Station>();

            name = copy.Name;

            foreach (Station station in copy.Stations)
                stations.Add(new Station(station));
        }
    }
}
