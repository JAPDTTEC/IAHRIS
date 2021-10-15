using System;

namespace DataBaseUtils
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class DataBaseUtils
    {
        public DataBaseUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        /// <summary>
        /// MBD compact method (c) 2004 Alexander Youmashev
        /// !!IMPORTANT!!
        /// !make sure there's no open connections
        ///    to your db before calling this method!
        /// !!IMPORTANT!!
        /// </summary>
        /// <param name="connectionString">connection string to your db</param>
        /// <param name="mdwfilename">FULL name
        ///     of an MDB file you want to compress.</param>
        public static void CompactAccessDB(string connectionString, string mdwfilename)
        {
            JRO.JetEngine jro = new JRO.JetEngine();

            string OldDb = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + mdwfilename;
            string NewDb = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + mdwfilename + ".tmp";

            try
            {
                jro.CompactDatabase(OldDb, NewDb);
                Console.WriteLine("Finalizó la compactación de la base de datos");

                System.IO.File.Delete(mdwfilename);
                System.IO.File.Move(mdwfilename + ".tmp", mdwfilename);
            }
            catch (Exception e)
            { 
            }
        }
    }
}