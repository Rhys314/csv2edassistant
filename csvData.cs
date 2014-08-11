using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace csv2edassistant
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to read data from a CSV file
    /// </summary>
    public class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream)
            : base(stream)
        {
        }

        public CsvFileReader(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();
            if (String.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }
    }

    /// <summary>
    /// Class to read csv file and write xml file
    /// </summary>
    class csvData
    {
        public List<Station> stations;

        public csvData(string filename)
        {
            stations = new List<Station>();

            using (CsvFileReader reader = new CsvFileReader(filename))
            {
                int ii, jj = 0, kk;
                bool isStationNew = true, isCommoditynew = true;
                CsvRow row = new CsvRow();

                // discard titles
                reader.ReadRow(row);

                // get the data from the csv file
                while (reader.ReadRow(row))
                {
                    // check if station has already been added...
                    ii = 0;
                    if (stations.Count > 0)
                    {
                        foreach (Station s in stations)
                        {
                            isStationNew = s.Name.CompareTo(row[8]) != 0;
                            if (!isStationNew) break;
                            ii++;
                        }
                    }

                    // ...if the station is new...
                    if (isStationNew)
                    {
                        // ...add a new station and commodity...
                        stations.Add(new Station());
                        stations[ii].Commodities.Add(new Commodity());

                        // ...name the station...
                        stations[ii].Name = row[8];

                        // ...and reset the commodity iterator...
                        jj = 0;

                        // ...then add the commodity data in that row...
                        stations[ii].Commodities[jj].Name = row[7];
                        stations[ii].Commodities[jj].BuyPrice = Convert.ToDecimal(row[0]);
                        stations[ii].Commodities[jj].SellPrice = Convert.ToDecimal(row[1]);
                        stations[ii].Commodities[jj].Supply = Convert.ToDecimal(row[5]);
                        stations[ii].Commodities[jj].LastUpdated = Convert.ToDateTime(row[9]);

                        // ...finally set the check to false and iterate station
                        isStationNew = false;
                        //ii++;
                    }
                    // ...if the station is old...
                    else
                    {
                        // ...then set the station...
                        //kk = ii - 1;
                        kk = ii;

                        // ...and check if the commodity is new...
                        jj = 0;
                        foreach (Commodity c in stations[kk].Commodities)
                        {
                            isCommoditynew = c.Name.CompareTo(row[7]) != 0;
                            if (!isCommoditynew) break;
                            jj++;
                        }

                        // ...if it is new then added it to the station...
                        if (isCommoditynew)
                        {
                            // ...then add a new commodity...
                            stations[kk].Commodities.Add(new Commodity());

                            // ...fill it with data...
                            stations[kk].Commodities[jj].Name = row[7];
                            stations[kk].Commodities[jj].BuyPrice = Convert.ToDecimal(row[0]);
                            stations[kk].Commodities[jj].SellPrice = Convert.ToDecimal(row[1]);
                            stations[kk].Commodities[jj].Supply = Convert.ToDecimal(row[5]);
                            stations[kk].Commodities[jj].LastUpdated = Convert.ToDateTime(row[9]);

                            // ...and increment the commodity iterator
                            jj++;
                        }
                        // ...if it isn't new check it's timestamp...
                        else if (stations[kk].Commodities[jj].LastUpdated < Convert.ToDateTime(row[9]))
                        {
                            // ...and fill it with data
                            stations[kk].Commodities[jj].Name = row[7];
                            stations[kk].Commodities[jj].BuyPrice = Convert.ToDecimal(row[0]);
                            stations[kk].Commodities[jj].SellPrice = Convert.ToDecimal(row[1]);
                            stations[kk].Commodities[jj].Supply = Convert.ToDecimal(row[5]);
                            stations[kk].Commodities[jj].LastUpdated = Convert.ToDateTime(row[9]);
                        }
                    }
                }
            }
        }
    }
}
