using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace csv2edassistant
{
    class stationSystemMap
    {
        public stationSystemMap()
        {
            string filename = Path.GetDirectoryName(Application.ExecutablePath) + "\\StationMapping.dat";

            systems = new Dictionary<string, List<string>>();

            if (File.Exists(filename))
            {
                string line;
                using (StreamReader mapStream = new StreamReader(filename))
                {
                    while ((line = mapStream.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { '|' });
                        for (int ii = 1; ii < parts.Length; ii++)
                        {
                            addSystemStation(parts[0], parts[ii]);
                        }
                    }
                }
            }
        }

        public void writeMapFile()
        {
            string filename = Path.GetDirectoryName(Application.ExecutablePath) + "\\StationMapping.dat";
            StringBuilder line = new StringBuilder();
            
            using (StreamWriter output = new StreamWriter(filename))
            {
                foreach (KeyValuePair<string, List<string>> sys in systems)
                {
                    line.Insert(0, sys.Key + "|");
                    foreach (string station in sys.Value)
                    {
                        line.Append(station + "|");
                    }
                    line.Append("\n");
                    output.Write(line.ToString());
                    line.Clear();
                }
            }
        }

        public void addSystemStation(string system, string station)
        {
            // if the system is present add the station to it...
            if (systems.ContainsKey(system))
            {
                systems[system].Add(station);
            }
            // if not add the system and stations
            else
            {
                List<string> buffer = new List<string>();
                buffer.Add(station);
                systems.Add(system, buffer);
            }

        }

        public string checkIfStationPresent(string station)
        {
            string rtv = "";

            foreach (KeyValuePair<string, List<string>> pair in systems)
            {
                if (pair.Value.Contains(station))
                {
                    rtv = pair.Key;
                    break;
                }
            }

            return rtv;
        }

        private Dictionary<string, List<string>> systems;
    }
}
