using System;
using System.Collections.Generic;

namespace csv2edassistant
{
    public class Route
    {
        private List<Manifest> manifests;

        public List<Manifest> Manifests
        {
            get { return manifests; }
        }
        public StarSystem CenterSystem
        {
            get { return manifests.Count > 0 ? manifests[0].StartSystem : null; }
        }
        public string CenterSystemName
        {
            get { return manifests[0].Trades[0].StartSystem.Name; }
        }
        public Station CenterStation
        {
            get { return manifests.Count > 0 ? manifests[0].StartStation : null; }
        }
        public string CenterStationName
        {
            get { return manifests[0].Trades[0].StartStation.Name; }
        }
        public decimal Trips
        {
            get { return manifests.Count; }
        }
        public decimal Profit
        {
            get
            {
                decimal profit = 0;

                foreach (Manifest manifest in manifests)
                    profit += manifest.Profit;

                return profit;
            }
        }
        public decimal AverageProfitPerTrip
        {
            get { return Trips > 0 ? decimal.Floor(Profit / Trips) : 0; }
        }

        public DateTime OldestDate
        {
            get
            {
                DateTime oldest = DateTime.MaxValue;

                foreach (Manifest manifest in manifests)
                    if (manifest.OldestDate < oldest)
                        oldest = manifest.OldestDate;

                return oldest;
            }
        }

        public Route()
        {
            manifests = new List<Manifest>();
        }
        public Route(Route copy)
        {
            manifests = new List<Manifest>();

            foreach (Manifest copyManifest in copy.Manifests)
                manifests.Add(new Manifest(copyManifest));
        }

        public bool Equals(Route compareTo)
        {
            if (Trips != compareTo.Trips || Profit != compareTo.Profit || AverageProfitPerTrip != compareTo.AverageProfitPerTrip)
                return false;

            return true;
        }
    }
}
