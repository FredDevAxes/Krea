using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GoogleMapControl;
using GMap.NET.MapProviders;
using System.Diagnostics;
using System.Globalization;

namespace GoogleMapComponent
{
    public partial class GoogleMapControl : UserControl
    {
        readonly GMapOverlay top = new GMapOverlay();
        readonly GMapOverlay remote = new GMapOverlay();
        GMapMarkerRect CurentRectMarker = null;
        string mobileGpsLog = string.Empty;
        bool isMouseDown = false;

        public GMapRoute currentRoute;
        public GMarkerGoogle currentMarkerFrom;
        public GMarkerGoogle currentMarkerTo;
        public GMarkerGoogle currentMarkerRemote;
        public GMarkerGoogle currentMarkerRoute;
        public GoogleMapControl()
        {
          
            InitializeComponent();
           
        }

        public void initGoogleMap() {
            if (!DesignMode)
            {
                try
                {
                    System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.bing.com");
                }
                catch
                {
                    gMapControl1.Manager.Mode = AccessMode.CacheOnly;
                    MessageBox.Show("No internet connection available, remote GPS map is going to CacheOnly mode.", "Warning...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                gMapControl1.MapProvider = GMapProviders.GoogleMap;
                string culture = CultureInfo.CurrentCulture.EnglishName;
                string country = culture.Substring(culture.IndexOf('(') + 1, culture.LastIndexOf(')') - culture.IndexOf('(') - 1);

                gMapControl1.SetCurrentPositionByKeywords(country);
                gMapControl1.MinZoom = 3;
                gMapControl1.MaxZoom = 17;
                gMapControl1.Zoom = 4;

                // set current marker1
                currentMarkerFrom = new GMarkerGoogle(gMapControl1.Position, GMarkerGoogleType.arrow);
                currentMarkerFrom.IsHitTestVisible = false;
                currentMarkerFrom.IsVisible = false;
                top.Markers.Add(currentMarkerFrom);
                
                // set current marker2
                currentMarkerTo = new GMarkerGoogle(gMapControl1.Position, GMarkerGoogleType.arrow);
                currentMarkerTo.IsHitTestVisible = false;
                currentMarkerTo.IsVisible = false;
                top.Markers.Add(currentMarkerTo);

                // set current marker2
                currentMarkerRemote = new GMarkerGoogle(gMapControl1.Position, GMarkerGoogleType.blue);
                currentMarkerRemote.IsHitTestVisible = false;
                currentMarkerRemote.IsVisible = false;
                remote.Markers.Add(currentMarkerRemote);

                //set route 
                currentRoute = new GMapRoute("route");
                currentRoute.IsVisible = false;
                top.Routes.Add(currentRoute);

                // set current RoutePos
                this.currentMarkerRoute = new GMarkerGoogle(gMapControl1.Position, GMarkerGoogleType.green);
                currentMarkerRoute.IsHitTestVisible = false;
                currentMarkerRoute.IsVisible = false;
                top.Markers.Add(currentMarkerRoute);


                gMapControl1.MouseMove += new MouseEventHandler(gMapControl1_MouseMove);
                gMapControl1.MouseDown += new MouseEventHandler(gMapControl1_MouseDown);
                gMapControl1.MouseUp += new MouseEventHandler(gMapControl1_MouseUp);
            
                gMapControl1.Overlays.Add(top);
                gMapControl1.Overlays.Add(remote);

              }
        }
 
        void gMapControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;

                this.currentMarkerTo.Position = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                this.currentMarkerTo.IsVisible = true;
                this.currentMarkerRoute.IsVisible = false;
                this.currentRoute.Points.Add(this.currentMarkerTo.Position);

                currentMarkerTo.ToolTipText = "To location " + currentMarkerTo.Position.ToString();
                currentMarkerTo.ToolTip = new GMapToolTip(currentMarkerTo);
                currentMarkerTo.ToolTip.Font = new System.Drawing.Font(SystemFonts.CaptionFont.FontFamily, 8, FontStyle.Bold);

                currentMarkerTo.ToolTipMode = MarkerTooltipMode.Always; 
                gMapControl1.Refresh();
            }
        }


        void gMapControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                this.currentMarkerFrom.IsVisible = false;
                this.currentMarkerTo.IsVisible = false;
                this.currentMarkerRoute.IsVisible = false;
                this.currentRoute.IsVisible = false;
                this.currentRoute.Points.Clear();

                currentMarkerFrom.Position = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                this.currentRoute.Points.Add(currentMarkerFrom.Position);

                currentMarkerFrom.ToolTipText = "Route from location:\n " + currentMarkerFrom.Position.ToString() ;
                currentMarkerFrom.ToolTip = new GMapToolTip(currentMarkerFrom);
                currentMarkerFrom.ToolTip.Font = new System.Drawing.Font(SystemFonts.CaptionFont.FontFamily, 8,FontStyle.Bold);

                currentMarkerFrom.ToolTipMode = MarkerTooltipMode.Always; 
                this.currentMarkerFrom.IsVisible = true;

            }
        }

        public PointLatLng getMarkerPos() {
            if (currentMarkerFrom != null)
            {
                return currentMarkerFrom.Position;
            }
            else {
                return new PointLatLng(0, 0);
            }
        }
        // move current marker with left holding
        void gMapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                if (CurentRectMarker == null)
                {
                   
                    this.currentMarkerRoute.Position = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                    this.currentRoute.Points.Add(this.currentMarkerRoute.Position);
                    this.currentMarkerRoute.IsVisible = true;
                    this.currentRoute.IsVisible = true;
                    
                }

                gMapControl1.UpdateRouteLocalPosition(this.currentRoute); // force instant invalidation
                this.gMapControl1.Refresh();
            }
        }

        public void setRemoteGGPSMarkerLocation(PointLatLng loc)
        {
            
            this.currentMarkerRoute.IsVisible = false;
            this.currentRoute.IsVisible = false;
            this.currentMarkerFrom.IsVisible = false;
            this.currentMarkerTo.IsVisible = false;

            currentMarkerRemote.ToolTipText = "Krea Remote location:\n" + currentMarkerRemote.Position.ToString() ;
            currentMarkerRemote.ToolTip = new GMapToolTip(currentMarkerRemote);
            currentMarkerRemote.IsVisible = true;
            currentMarkerRemote.Position = loc;
            currentMarkerRemote.ToolTip.Font = new System.Drawing.Font(SystemFonts.CaptionFont.FontFamily, 8, FontStyle.Bold);

            currentMarkerRemote.ToolTipMode = MarkerTooltipMode.Always; 
            
            this.gMapControl1.UpdateMarkerLocalPosition(currentMarkerRemote);
            this.gMapControl1.Refresh();
            //this.gMapControl1.ZoomAndCenterMarkers(currentMarkerRemote.Overlay.Id);
        }

        public double getDistance(PointLatLng p1, PointLatLng p2)
        {
            GMapRoute route = new GMapRoute("getDistance");
            route.Points.Add(p1);
            route.Points.Add(p2);
            double distance = route.Distance;
            route.Clear();
            route = null;

            return distance;
        }


        public void updateMarkers()
        {
            if (this.currentRoute != null)
            {
                this.gMapControl1.UpdateMarkerLocalPosition(this.currentMarkerRoute);

            }
        }


    }
}
