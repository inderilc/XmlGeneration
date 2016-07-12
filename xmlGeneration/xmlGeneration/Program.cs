using FishbowlSDK;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Xml;


namespace xmlGeneration
{
    class Program
    {
        public static string initDB()
        {
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Database = @"C:\Program Files\Fishbowl\database\data\EXAMPLE.FDB";
            csb.UserID = "gone";
            csb.Password = "fishing";
            csb.Port = 3050;
            csb.ServerType = FbServerType.Default;
            return csb.ToString();
        }


        static void Main(string[] args)
        {
            string scriptLine = @"SELECT location.id as LocationID,location.typeid as TypeID,location.PARENTID as ParentID,location.name as Name,location.ACTIVEFLAG as Active,
Location.RECEIVABLE as Receivable,location.LOCATIONGROUPID as LocationGroupID,locationgroup.name as LocationGroupName,location.SORTORDER as SortOrder,tag.id as TagID,tag.num as TagNumber
from LOCATION join LOCATIONGROUP on LOCATIONGROUP.id = location.LOCATIONGROUPID join tag on tag.locationid = location.id and tag.TYPEID = 10 order by location.id";
            string xmlFilePath = @"C:\Users\Owner\Desktop\Locations.xml";

            Console.WriteLine("------XML Generator----");
            Console.WriteLine("Press Enter to Generate XML Document");

            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                FbConnection db = new FbConnection(initDB());
                db.Open();
                List<Location> myLocs = new List<Location>();
                myLocs = db.Query<Location>(scriptLine).ToList();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                {
                    writer.WriteStartElement("Locations");
                    foreach (Location L in myLocs)
                    {
                        L.writeXML(writer);
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            else
            {
                Console.WriteLine("Exiting Program");
            }
        }

 
      
    }
    
    class Location
    {
        public string LocationID { get; set; }
        public string TypeID { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public string Receivable { get; set; }
        public string LocationGroupID { get; set; }
        public string LocationGroupName { get; set; }
        public string SortOrder { get; set; }
        public string TagID { get; set; }
        public string TagNumber { get; set; }

        public void writeXML(XmlWriter writer)
        {
            writer.WriteStartElement("Location");
            writer.WriteElementString("LocationID", this.LocationID);
            writer.WriteElementString("TypeID", this.TypeID);
            writer.WriteElementString("ParentID", this.ParentID);
            writer.WriteElementString("Name", this.Name);
            writer.WriteElementString("Active", this.Active);
            writer.WriteElementString("Receivable", this.Receivable);
            writer.WriteElementString("LocationGroupID", this.LocationGroupID);
            writer.WriteElementString("SortOrder", this.SortOrder);
            writer.WriteElementString("TagID", this.TagID);
            writer.WriteElementString("TagNumber", this.TagNumber);
            writer.WriteEndElement();
        }
    }
}
