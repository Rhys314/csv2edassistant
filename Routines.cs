using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csv2edassistant
{
    public static class Routines
    {
        /// <summary>
        /// Process a string as a CSV line and split the line into columns.
        /// </summary>
        /// <param name="sLine">Text line to be split</param>
        /// <param name="splitChr">character to split on</param>
        /// <returns>List<string> with the column data</returns>
        public static List<string> SplitCSV(string sLine, char splitChr = ',')
        {
            var al = new List<string>();
            var quote = '\"';
            var tmp = "";
            var quoted = false;
            var escaped = false;
            char chr;
            for (int i = 0; i < sLine.Length; i++)
            {
                chr = sLine[i];
                if (chr == quote)
                {
                    if (!quoted)
                        quoted = true;
                    else
                    {
                        if (escaped)
                        {
                            tmp += chr;
                            escaped = false;
                        }
                        else
                        {
                            // Test for "", in that case 1 quote should be added, the next one
                            // because the first is the escape character.
                            if (((i + 1) < sLine.Length) && (sLine[i + 1] == quote))
                                escaped = true;
                            else
                                quoted = !quoted;
                        }
                    }
                }
                else if (!quoted && chr == splitChr)
                {
                    al.Add(tmp);
                    tmp = "";
                }
                else
                    tmp += chr;
            }
            al.Add(tmp);
            return al;
        }
        
        public static void AddOrUpdateStation(csvData data, int idx, GameData outputData)
        {
            int foundSystem = -1,
                foundStation = -1;
            // See if we already have the StarSystem
            foundSystem = Routines.IndexOfSystem(outputData, data.stations[idx].SystemName);
            if (foundSystem == -1)
            {
                // Not found, add StarSystem.
                outputData.StarSystems.Add(new StarSystem());
                foundSystem = outputData.StarSystems.Count - 1;
                outputData.StarSystems[foundSystem].Name = data.stations[idx].SystemName;
                outputData.StarSystems[foundSystem].Stations.Add(new Station(data.stations[idx]));
            }
            else
            {
                // Found StarSystem, check if we already have the Station
                foundStation = Routines.IndexOfStation(outputData.StarSystems[foundSystem], data.stations[idx].Name);
                if (foundStation == -1)
                {
                    // Not found, add Station
                    outputData.StarSystems[foundSystem].Stations.Add(new Station(data.stations[idx]));
                    foundStation = outputData.StarSystems[foundSystem].Stations.Count - 1;
                }
                else
                {
                    // Found, update Commodities
                    Routines.UpdateCommodities(outputData.StarSystems[foundSystem].Stations[foundStation],
                                               data.stations[idx].Commodities);
                }
            }
        }

        public static int IndexOfSystem(GameData outputData, string systemName)
        {
            for (int i = 0; i < outputData.StarSystems.Count; i++)
                if (outputData.StarSystems[i].Name.Contains(systemName))
                    return i;
            return -1;
        }

        public static int IndexOfStation(StarSystem outputSystem, string stationName)
        {
            for (int i = 0; i < outputSystem.Stations.Count; i++)
                if (outputSystem.Stations[i].Name.Contains(stationName))
                    return i;
            return -1;
        }

        public static int IndexOfCommodity(Station outputStation, string commodityName)
        {
            for (int i = 0; i < outputStation.Commodities.Count; i++)
                if (outputStation.Commodities[i].Name.Contains(commodityName))
                    return i;
            return -1;
        }

        public static void UpdateCommodities(Station outputStation, List<Commodity> outputCommodity)
        {
            int found;
            for (int i = 0; i < outputCommodity.Count; i++)
            {
                found = IndexOfCommodity(outputStation, outputCommodity[i].Name);
                if (found == -1)
                    // Commodity not found, Add
                    outputStation.Commodities.Add(outputCommodity[i]);
                else
                    // Commodity found, Update
                    // Only update if newer
                    if (outputStation.Commodities[found].LastUpdated < outputCommodity[i].LastUpdated)
                        outputStation.Commodities[found] = outputCommodity[i];
            }

        }

    }
}
