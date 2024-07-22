using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace KMLReader
{

	#region elements


	public class Update
    {
        public string targetHref;

        public string Change;
        public string Create;
        public string Delete;

        public override string ToString()
        {
            string s = "";
            s = s
                + "targetHref: " + this.targetHref + "\r\n"
                + "Change: " + this.Change + "\r\n"
                + "Create: " + this.Create + "\r\n"
                + "Delete: " + this.Delete + "\r\n"
                ;
            return s;
        }
    }

    public class Object
    {
        public string ID;
        public string targetID;
    }

    public class Feature : Object
    {
        public string name;
        public bool open;
    }
    public class gx_Tour : Feature
    {
        public gx_Playlist playlist;

        public override string ToString()
        {
            string s = "";
            s = s
                + "name: " + this.name + "\r\n"
                + "playlist: " + this.playlist.ToString() + "\r\n"
                ;
            return s;
        }

    }
    public class Placemark : Feature
    {
        public string styleUrl;
        public Point point;

        public override string ToString()
        {
            string s = "";
            s = s
                + "name: " + this.name + "\r\n"
                + "styleUrl: " + this.styleUrl + "\r\n"
                + "point: " + this.point.ToString() + "\r\n"
                ;
            return s;
        }
    }
    public class Container : Feature
    {
        List<Feature> features;

        public Container()
        {
            features = new List<Feature> { };
        }

        public override string ToString()
        {
            string s = "";

            foreach (Feature feature in this.features)
            {
                s += "feature: " + feature.ToString() + "\r\n";
            }

            return s;
        }

    }
    public class KMLDocument : Container
    {
        public Style Style;
        public Placemark Placemark;
        public gx_Tour gxTour;

        public KMLDocument()
        {
        }

        public override string ToString()
        {
            string s = "";

            s = "name: " + this.name + "\r\n";
            s += "\r\n";
            s += "open: " + this.open + "\r\n";
            s += "\r\n";
            s += "Style: " + this.Style + "\r\n";
            s += "\r\n";
            s += "Placemark: " + this.Placemark + "\r\n";
            s += "\r\n";
            s += "gxTour: " + this.gxTour + "\r\n";
            s += "\r\n";
            s += "\r\n";

            return s;
        }

    }

    public class Geometry : Object
    {

    }
    public class Point : Geometry
    {
        public string coordinates;
        public override string ToString()
        {
            string s = "";
            s = s
                + "coordinates: " + this.coordinates + "\r\n"
                ;
            return s;
        }
    }

    public class AbstractStyleSelector : Object
    {

    }
    public class Style : AbstractStyleSelector
    {
        public string ID;
        public IconStyle iconStyle;

        public override string ToString()
        {
            string s = "";
            s = s
                + "ID: " + this.ID + "\r\n"
                + "iconStyle: " + this.iconStyle.ToString() + "\r\n"
                ;
            return s;
        }

    }

    public class AbstractView : Object
    {
        public TimeSpan TimePrimitive;
        //public ViewerOptions gx_viewerOptions;

        public override string ToString()
        {
            string s = "";
            s = s
                + "TimePrimitive: " + this.TimePrimitive.ToString() + "\r\n"
                ;
            return s;
        }
    }
    public class Camera : AbstractView
    {
        public string longitude;
        public string latitude;
        public string altitude;
        public string heading;
        public string tilt;
        public string roll;

        public override string ToString()
        {
            string s = "";
            s = s
                + "longitude: " + this.longitude + "\r\n"
                + "latitude: " + this.latitude + "\r\n"
                + "altitude: " + this.altitude + "\r\n"
                + "heading: " + this.heading + "\r\n"
                + "tilt: " + this.tilt + "\r\n"
                + "roll: " + this.roll + "\r\n"
                ;
            return s;
        }
    }

    public class AbstractSubStyle : Object
    {

    }
    public enum ColorMode
    {
        normal,
        random
    }
    public class ColorStyle : AbstractSubStyle
    {
        public string color;
        public ColorMode colorMode;
        public override string ToString()
        {
            string s = "";
            s = s
                + "color: " + this.color.ToString() + "rn"
                + "colorMode: " + this.colorMode.ToString() + "rn"
                ;
            return s;
        }
    }
    public class IconStyle : ColorStyle
    {
        public double scale;
        public override string ToString()
        {
            string s = "";
            s = s
                + "scale: " + this.scale + "rn"
                ;
            return s;
        }
    }

    public class gx_TourPrimitive
    {
        public gx_Playlist playlist;
        public override string ToString()
        {
            string s = "";
            s = s
                + "\r\n"
                + "Playlist: " + this.playlist.ToString() + "\r\n"
                ;
            return s;
        }
    }
    public class gx_AnimatedUpdate : gx_TourPrimitive
    {
        public string duration;
        public Update update;
        public double delayedStart;

        public override string ToString()
        {
            string s = "";
            s = s
                + "\r\n"
                + "Duration: " + this.duration + "\r\n"
                + "Update: " + this.update + "\r\n"
                + "delayedStart: " + this.delayedStart + "\r\n"
                ;
            return s;
        }
    }
    public class gx_FlyTo : gx_TourPrimitive
    {
        public string duration;
        public string flyToMode; // smooth or bounce
        public AbstractView abstractView;

        public Camera camera;

        public gx_FlyTo()
        {
            duration = "";
            flyToMode = "";
            abstractView = new AbstractView();
        }

        public override string ToString()
        {
            string s = "";
            s = s
                + "\r\n"
                + "Duration: " + this.duration + "\r\n"
                + "FlyToMode: " + this.flyToMode + "\r\n"
                + "AbstractView: " + this.abstractView.ToString() + "\r\n"
                + "Camera: " + this.camera.ToString() + "\r\n"
                ;
            return s;
        }
    }
    public class gx_Wait : gx_TourPrimitive
    {
        public double duration;

        public override string ToString()
        {
            string s = "";
            s = s
                + "duration: " + this.duration + "\r\n"
                ;
            return s;
        }
    }

    public class gx_Playlist : Object
    {
        public gx_AnimatedUpdate animatedUpdate;
        public gx_FlyTo flyTo;
        public gx_Wait wait;

        public List<gx_Playlist> list;

        public override string ToString()
        {
            string s = "";
            s = s
                + "animatedUpdate: " + this.animatedUpdate.ToString() + "\r\n"
                + "flyTo: " + this.flyTo.ToString() + "\r\n"
                + "wait: " + this.wait.ToString() + "\r\n"
                ;
            //+ "list: " + this.list.ToString() + "\r\n"
            return s;
        }
    }

    #endregion


    public class KMLReader
    {
        public static KMLDocument ReadKMLFile(string KMLFile)
        {
            KMLDocument value = new KMLDocument();

            XmlDocument doc = new XmlDocument();
            XmlNode xnDocument;

            try
            {

                #region get document

                if (!File.Exists(KMLFile))
                {
                    return null;
                }

                doc = new XmlDocument();
                doc.LoadXml(File.ReadAllText(KMLFile));

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ns", "http://www.opengis.net/kml/2.2");
                nsmgr.AddNamespace("gx", "http://www.google.com/kml/ext/2.2");

                XmlElement x1 = doc.DocumentElement;
                xnDocument = x1.FirstChild;

                #endregion

                #region Transfer XML data to KML objects

                XmlNode xnName = xnDocument.SelectSingleNode("ns:name", nsmgr);
                if (xnName != null)
                    value.name = xnName.InnerText;
                XmlNode xnOpen = xnDocument.SelectSingleNode("ns:open", nsmgr);
                if (xnOpen != null)
                    value.open = (xnOpen.InnerText == "1") ? true : false;

                Style style = new Style();
                XmlNode xnStyle = xnDocument.SelectSingleNode("ns:Style", nsmgr);
                if (xnStyle != null)
                {
                    style.ID = xnStyle.Attributes["id"].Value;
                    IconStyle iconStyle = new IconStyle();
                    XmlNode xnIconStyle = xnStyle.SelectSingleNode("ns:IconStyle", nsmgr);
                    if (xnIconStyle != null)
                    {
                        iconStyle.scale = Convert.ToDouble(xnIconStyle.InnerText);
                    }
                    style.iconStyle = iconStyle;
                }
                value.Style = style;

                Placemark placemark = new Placemark();
                XmlNode xnPlacemark = xnDocument.SelectSingleNode("ns:Placemark", nsmgr);
                if (xnPlacemark != null)
                {
                    placemark.name = xnPlacemark.SelectSingleNode("ns:name", nsmgr).InnerText;

                    placemark.styleUrl = xnPlacemark.SelectSingleNode("ns:styleUrl", nsmgr).InnerText;

                    Point point = new Point();
                    point.coordinates = xnPlacemark.SelectSingleNode("ns:Point", nsmgr).SelectSingleNode("ns:coordinates", nsmgr).InnerText;
                    placemark.point = point;
                }
                value.Placemark = placemark;

                #region tour

                XmlNode xnGxTour = xnDocument.SelectSingleNode("gx:Tour", nsmgr);
                gx_Tour tour = new gx_Tour();
                if (xnGxTour != null)
                {
                    tour.name = xnGxTour.SelectSingleNode("ns:name", nsmgr).InnerText;

                    gx_Playlist playlist = new gx_Playlist();
                    XmlNode xnPlaylist = xnGxTour.SelectSingleNode("gx:Playlist", nsmgr);
                    if (xnPlaylist != null)
                    {
                        gx_AnimatedUpdate animatedUpdate = new gx_AnimatedUpdate();
                        XmlNode xnAnimatedUpdate = xnPlaylist.SelectSingleNode("gx:AnimatedUpdate", nsmgr);
                        if (xnAnimatedUpdate != null)
                        {
                            animatedUpdate.duration = xnAnimatedUpdate.SelectSingleNode("gx:duration", nsmgr).InnerText;
                            Update update = new Update();
                            XmlNode xnUpdate = xnAnimatedUpdate.SelectSingleNode("ns:Update", nsmgr);
                            if (xnUpdate != null)
                            {
                                update.targetHref = xnUpdate.SelectSingleNode("ns:targetHref", nsmgr).InnerText;
                                XmlNode xnChange = xnUpdate.SelectSingleNode("ns:Change", nsmgr);
                                if (xnChange != null)
                                {
                                    string change = xnChange.InnerXml;
                                    update.Change = change;
                                }
                            }
                            animatedUpdate.update = update;
                        }
                        playlist.animatedUpdate = animatedUpdate;

                        gx_FlyTo flyto = new gx_FlyTo();
                        XmlNode xnFlyTo = xnPlaylist.SelectSingleNode("gx:FlyTo", nsmgr);
                        if (xnFlyTo != null)
                        {
                            flyto.duration = xnFlyTo.SelectSingleNode("gx:duration", nsmgr).InnerText;

                            Camera camera = new Camera();
                            XmlNode xnCamera = xnFlyTo.SelectSingleNode("ns:Camera", nsmgr);
                            if (xnCamera != null)
                            {
                                camera.longitude = xnCamera.SelectSingleNode("ns:longitude", nsmgr).InnerText;
                                camera.latitude = xnCamera.SelectSingleNode("ns:latitude", nsmgr).InnerText;
                                camera.altitude = xnCamera.SelectSingleNode("ns:altitude", nsmgr).InnerText;
                                camera.heading = xnCamera.SelectSingleNode("ns:heading", nsmgr).InnerText;
                                camera.tilt = xnCamera.SelectSingleNode("ns:tilt", nsmgr).InnerText;
                                camera.roll = xnCamera.SelectSingleNode("ns:roll", nsmgr).InnerText;
                            }
                            flyto.camera = camera;
                        }
                        playlist.flyTo = flyto;

                        gx_Wait wait = new gx_Wait();
                        XmlNode xnWait = xnPlaylist.SelectSingleNode("gx:Wait", nsmgr);
                        if (xnWait != null)
                        {
                            wait.duration = Convert.ToDouble(xnWait.SelectSingleNode("gx:duration", nsmgr).InnerText);
                        }
                        playlist.wait = wait;
                    }
                    tour.playlist = playlist;

                }

                value.gxTour = tour;

                #endregion

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return value;
        }
    }



}
