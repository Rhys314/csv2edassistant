using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace csv2edassistant
{
    public partial class mainWindow : Form
    {
        public mainWindow()
        {
            InitializeComponent();
        }

        private void csvFilename_TextChanged(object sender, EventArgs e)
        {
            data = new csvData(csvFilename.Text);
        }

        private void browseCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "CSV File";
            //fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Comma Seperated Variable (*.csv)|*.csv";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                csvFilename.Text = fdlg.FileName;
                data = new csvData(csvFilename.Text);
            }
        }

        private void edaFilename_TextChanged(object sender, EventArgs e)
        {
            if (!File.Exists(edaFilename.Text))
            {
                FileStream newfile = File.Open(edaFilename.Text, FileMode.OpenOrCreate);
                newfile.Close();
                fileWasCreated = true;
            }
        }

        private void browseEDA_Click(object sender, EventArgs e)
        {
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = "EDAssistant File";
            //fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "ED Trade Assistant (*.edassistant)|*.edassistant";
            fdlg.FilterIndex = 2;
            fdlg.CheckFileExists = false;
            fdlg.CreatePrompt = false;
            fdlg.OverwritePrompt = false;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                edaFilename.Text = fdlg.FileName;
                if (!File.Exists(edaFilename.Text) || (new FileInfo( edaFilename.Text ).Length == 0 ))
                {
                    // Create new file.
                    FileStream newfile = File.Open(edaFilename.Text, FileMode.OpenOrCreate);
                    newfile.Close();
                    outputData = new GameData();
                    fileWasCreated = true;
                }
                else
                {
                    // Deserialize XML data
                    fileWasCreated = false;
                    XmlSerializer deserializer = new XmlSerializer(typeof(GameData));
                    TextReader reader = new StreamReader(edaFilename.Text);
                    object obj = deserializer.Deserialize(reader);
                    outputData = (GameData)obj;
                    reader.Close();
                }
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            stationSystemMap map = new stationSystemMap();
            FileStream fileStream = File.Open(edaFilename.Text, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            // get star system data to match stations
            for (int ii = 0; ii < data.stations.Count; ii++)
            {
                string systemName = map.checkIfStationPresent(data.stations[ii].CompleteName);
                if (systemName.Length == 0)
                    systemName = data.stations[ii].SystemName;
                string stationName = data.stations[ii].Name;
                if (systemName.Length == 0) 
                {
                    // Last Option, because name not found: Ask.
                    starSystemNames_Input names = new starSystemNames_Input(data.stations[ii].CompleteName);
                    if (names.ShowDialog(this) == DialogResult.OK)
                        systemName = names.starSystemName.Text;
                    data.stations[ii].SystemName = systemName;
                    map.addSystemStation(outputData.StarSystems[ii].Name, data.stations[ii].CompleteName);
                    names.Dispose();
                }
                // Ready to add/update the data.
                Routines.AddOrUpdateStation(data, ii, outputData);

                /*
                // if the system the station is in has alread been defind use that...
                if (systemName.Length > 0)
                {
                    outputData.StarSystems.Add(new StarSystem());
                    outputData.StarSystems[ii].Name = systemName;
                    outputData.StarSystems[ii].Stations.Add(new Station(data.stations[ii]));
                }
                // ...otherwise ask
                else
                {
                    // Extract Station/System Name if possible
                    if (data.stations[ii].Name.IndexOf("(") != -1 &&
                        data.stations[ii].Name.IndexOf(")") != -1 &&
                        data.stations[ii].Name.IndexOf("(") < data.stations[ii].Name.IndexOf(")"))
                    {
                        outputData.StarSystems.Add(new StarSystem());
                        // Extract System name
                        outputData.StarSystems[ii].Name = data.stations[ii].Name.Substring(0, data.stations[ii].Name.IndexOf('(')).Trim();
                        // Extract Station name
                        data.stations[ii].Name = data.stations[ii].Name.Substring(
                                data.stations[ii].Name.IndexOf('('),
                                data.stations[ii].Name.IndexOf(')') - data.stations[ii].Name.IndexOf('(')
                            ).Trim();
                        outputData.StarSystems[ii].Stations.Add(new Station(data.stations[ii]));
                        map.addSystemStation(outputData.StarSystems[ii].Name, data.stations[ii].Name);
                    }
                    else
                    {
                        // Last Option, because name not found: Ask.
                        starSystemNames_Input names = new starSystemNames_Input(data.stations[ii].Name);
                        if (names.ShowDialog(this) == DialogResult.OK)
                        {
                            outputData.StarSystems.Add(new StarSystem());
                            outputData.StarSystems[ii].Name = names.starSystemName.Text;
                            outputData.StarSystems[ii].Stations.Add(new Station(data.stations[ii]));
                        }
                        // ...then add the system to the list
                        map.addSystemStation(outputData.StarSystems[ii].Name, data.stations[ii].Name);
                        names.Dispose();
                    }
                }
                */
            }

            // write game data to xml file
            serializer.Serialize(fileStream, outputData);

            // write station data to dat file
            map.writeMapFile();

            // close file
            fileStream.Close();

            // close dialogue
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            if (File.Exists(edaFilename.Text) && fileWasCreated) File.Delete(edaFilename.Text);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
