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

        public static string superTrim(string cellValue, bool All = false)
        {
            do
                cellValue = cellValue.Substring(cellValue.IndexOf(':') + 1, cellValue.Length - cellValue.IndexOf(':') - 1);
            while (All && cellValue.IndexOf(':') != -1);
            cellValue = cellValue.Replace("\"", "").Replace("}", "");
            return cellValue;
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
            if (row.LineText.IndexOf("{\"message\":{\"buyPrice\":") != -1)
            // Firehose dump file
            {
                List<string> splitRow = Routines.SplitCSV(row.LineText, ',');
                /* Sample Result:
                    0 {"message":{"buyPrice":0.0
                    1 categoryName:"metals"
                    2 demand:2519
                    3 demandLevel:2
                    4 itemName:"gold"
                    5 sellPrice:9898.0
                    6 stationName:"Tilian (Maunder's Hope)"
                    7 stationStock:0
                    8 stationStockLevel:0
                    9 timestamp:"2014-09-12T20:47:58.999000+00:00"}
                   10 sender:"YJZ4oHnx89PNiFHb0UBKDgAdP3bo/p34XS0wbQUJ1VQ="
                   11 signature:"TIM5qsL4Yh7pfV7b8u4xm5ryB0b3NYTZj8y+oLxrAM49cOrDPICPi18oIuN+tkWDE4T50gqPkGiXvQCa35yiCw=="
                   12 type:"marketquote"
                   13 version:"0.1"}
                */
                for (int i = 0; i < splitRow.Count - 1; i++)
                    splitRow[i] = superTrim(splitRow[i], i == 0);
                // Old CSV MarketDump format:
                /*
                 0 buyPrice
                 1 sellPrice
                 2 demand
                 3 demandLevel
                 4 stationStock
                 5 stationStockLevel
                 6 categoryName
                 7 itemName
                 8 stationName
                 9 timestamp
                */
                while (row.Count < 10)
                    row.Add(" ");
                row[0] = splitRow[0];
                row[1] = splitRow[5];
                row[2] = splitRow[2];
                row[3] = splitRow[3];
                row[4] = splitRow[7];
                row[5] = splitRow[8];
                row[6] = splitRow[1];
                row[7] = splitRow[4];
                row[8] = splitRow[6];
                row[9] = splitRow[9];
            }
            else
            // Classic CSV
            {
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
            }
            // Return true if any columns read
            return (row.Count > 0);
        }
    }

    /// <summary>
    /// Class to read csv file and write xml file
    /// </summary>
    public class csvData
    {
        public List<Station> stations;

        public csvData(string filename)
        {
            stations = new List<Station>();

            using (CsvFileReader reader = new CsvFileReader(filename))
            {
                int ii, jj = 0, kk;
                bool isStationNew = true,
                     isCommoditynew = true;
                CsvRow row = new CsvRow();

                // discard titles
                //reader.ReadRow(row);

                // get the data from the csv file
                while (reader.ReadRow(row) && row[0] != "buyPrice") // <= Discard Title row here
                {
                    // check if station has already been added...
                    ii = 0;
                    if (stations.Count > 0)
                    {
                        foreach (Station s in stations)
                        {
                            isStationNew = s.CompleteName.CompareTo(row[8]) != 0;
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
                        stations[ii].CompleteName = row[8];
                        // Possible station/system name combination
                        // check and process.
                        if (stations[ii].CompleteName.IndexOf("(") != -1 &&
                            stations[ii].CompleteName.IndexOf(")") != -1 &&
                            stations[ii].CompleteName.IndexOf("(") < stations[ii].CompleteName.IndexOf(")"))
                        {
                            // Extract System name
                            var systemName = stations[ii].CompleteName.Substring(0, stations[ii].CompleteName.IndexOf('(')).Trim();
                            // Extract Station name
                            var stationName = stations[ii].CompleteName.Substring(
                                    stations[ii].CompleteName.IndexOf('(') + 1,
                                    stations[ii].CompleteName.IndexOf(')') - (stations[ii].CompleteName.IndexOf('(') + 1)
                                ).Trim();
                            stations[ii].SystemName = systemName;
                            stations[ii].Name = stationName;
                        }

                        // ...and reset the commodity iterator...
                        jj = 0;

                        // ...then add the commodity data in that row...
                        stations[ii].Commodities[jj].Name = row[7];
                        stations[ii].Commodities[jj].BuyPrice = Convert.ToDecimal(row[0]);
                        stations[ii].Commodities[jj].SellPrice = Convert.ToDecimal(row[1]);
                        stations[ii].Commodities[jj].Supply = Convert.ToDecimal(row[4]);
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
                            stations[kk].Commodities[jj].Supply = Convert.ToDecimal(row[4]);
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
                            stations[kk].Commodities[jj].Supply = Convert.ToDecimal(row[4]);
                            stations[kk].Commodities[jj].LastUpdated = Convert.ToDateTime(row[9]);
                        }
                    }
                }
            }
        }
    }

}
