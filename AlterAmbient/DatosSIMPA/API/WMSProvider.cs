using System.Collections.Generic;

namespace MissionPlanner.Maps
{
    using GMap.NET;
    using GMap.NET.MapProviders;
    using GMap.NET.Projections;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// WMS Custom
    /// </summary>
    public class WMSProvider : GMapProvider
    {
        public static readonly WMSProvider Instance;

        public static string szWmsLayer = "HY.PhysicalWaters.Waterbodies";

        public static string CustomWMSURL =
            "https://wms.mapama.gob.es/sig/Agua/RedHidrograficaMDT/wms.aspx";

        public GPoint pos1;
        public GPoint pos2;


        WMSProvider()
        {
            MaxZoom = 10;            
        }

        static WMSProvider()
        {
            Instance = new WMSProvider();

            Type mytype = typeof(GMapProviders);
            FieldInfo field = mytype.GetField("DbHash", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<int, GMapProvider> list = (Dictionary<int, GMapProvider>)field.GetValue(Instance);

            list.Add(Instance.DbId, Instance);
        }

        #region GMapProvider Members

        readonly Guid id = new Guid("4574218D-B552-4CAF-89AE-F20951BBDB2B");

        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "WMS Custom";

        public override string Name
        {
            get { return name; }
        }

        GMapProvider[] overlays;

        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {                    
                string url = MakeTileImageUrl(pos, pos2, zoom, LanguageStr);

                return GetTileImageUsingHttp(url);

            }catch(Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        string MakeTileImageUrl(GPoint pos, GPoint pos2, int zoom, string language)
        {

            //px1.Offset(0, Projection.TileSize.Height);
            PointLatLng p1 = Projection.FromPixelToLatLng(pos, zoom);

            //px2.Offset(Projection.TileSize.Width, 0);
            PointLatLng p2 = Projection.FromPixelToLatLng(pos2, zoom);

            string ret;            

            string extra = "?";

            if (CustomWMSURL.Contains("?"))
                extra = "&";

            //if there is a layer, use it  
            if (szWmsLayer != "")
            {
                ret = string.Format(CultureInfo.InvariantCulture, CustomWMSURL + extra +
                    "REQUEST=GetMap&SERVICE=WMS&VERSION=1.1.1&FORMAT=image%2Fpng&STYLES=&TRANSPARENT=TRUE&" +
                    "layers=" + szWmsLayer +
                    "&BGCOLOR=0xFFFFFF&SRS=EPSG%3A4326&CRS=EPSG%3A4326&MAP_RESOLUTION=112.5&WIDTH=944&HEIGHT=414&BBOX={0},{1},{2},{3}", p1.Lng, p2.Lat, p2.Lng, p1.Lat);
                //"&styles=&bbox={0},{1},{2},{3}&width={4}&height={5}&srs=EPSG:25830&format=image/png", p1.Lng, p1.Lat,
                //p2.Lng, p2.Lat, Projection.TileSize.Width, Projection.TileSize.Height);

            }
            else
            {
                ret = string.Format(CultureInfo.InvariantCulture,
                    CustomWMSURL + extra +
                    "VERSION=1.1.1&REQUEST=GetMap&SERVICE=WMS&styles=&bbox={0},{1},{2},{3}&width={4}&height={5}&srs=EPSG:4326&format=image/png",
                    p1.Lng, p1.Lat, p2.Lng, p2.Lat, Projection.TileSize.Width, Projection.TileSize.Height);
            }

            return ret;
        }

 
    }
}


