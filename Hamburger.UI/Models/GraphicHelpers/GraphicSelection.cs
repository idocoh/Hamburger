using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamburger.UI.Models.GraphicHelpers
{
    class GraphicSelection
    {
        public GraphicSelection(Graphic graphic, GraphicsOverlay overlay)
        {
            SelectedGraphic = graphic;
            SelectedOverlay = overlay;
        }

        public Graphic SelectedGraphic { get; set; }

        public GraphicsOverlay SelectedOverlay { get; set; }

        public GeometryType GeometryType
        {
            get { return SelectedGraphic.Geometry.GeometryType; }
        }

        public void Select()
        {
            SelectedGraphic.IsSelected = true;
        }

        public void Unselect()
        {
            SelectedGraphic.IsSelected = false;
        }

        public void SetVisible()
        {
            SelectedGraphic.IsVisible = true;
        }

        public void SetHidden()
        {
            SelectedGraphic.IsVisible = false;
        }
    }
}
