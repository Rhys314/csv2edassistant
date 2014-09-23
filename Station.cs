using System.Collections.Generic;

namespace csv2edassistant
{
    public class Station
    {
        private string name;
        private string system;
        private string nameComplete;
        private List<Commodity> commodities;

        public string CompleteName
        {
            get { return nameComplete; }
            set { nameComplete = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string SystemName
        {
            get { return system; }
            set { system = value; }
        }
        public List<Commodity> Commodities
        {
            get { return commodities; }
        }

        public Station()
        {
            commodities = new List<Commodity>();
            name = "";
            system = "";
        }
        public Station(Station copy)
        {
            commodities = new List<Commodity>();
            name = copy.Name;
            system = copy.SystemName;
            foreach (Commodity commodity in copy.Commodities)
                commodities.Add(new Commodity(commodity));
        }
    }
}
