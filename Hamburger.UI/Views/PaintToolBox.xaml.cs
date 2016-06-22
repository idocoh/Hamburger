using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using Hamburger.UI.Models.GraphicHelpers;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Hamburger.UI.Views
{
    public sealed partial class PaintToolBox : UserControl
    {
        private static GraphicSelection _selection;
        private static Color DEFAULT_POLYGON_BORFDER_COLOR = Colors.Black;
        public const double DEFAULT_WIDTH = 3;
        private const byte DEFAULT_AREA_ALPHA = 150;
        private SolidColorBrush SelectedColor { get; set; } = new SolidColorBrush(Colors.Yellow);
        private static GraphicsOverlay _polygonsOverlay;
        private static GraphicsOverlay _polylinesOverlay;
        public PaintToolBox()
        {
            this.InitializeComponent();
        }

        private void ColorPickButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPicker.Visibility == Visibility.Visible)
            {
                ColorPicker.Visibility = Visibility.Collapsed;
            }
            else
            {
                ColorPicker.Visibility = Visibility.Visible;
                ColorPicker.Focus(FocusState.Programmatic);
            }
        }

        private void ColorPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            ColorPicker.Visibility = Visibility.Collapsed;
        }

        private void ColorPicker_ColorChanged(object sender, SolidColorBrush e)
        {
            SelectedColor = e;
            ColorPickButton.Foreground = e;
            ColorPicker.Visibility = Visibility.Collapsed;
        }

        private async void LineButton_Click(object sender, RoutedEventArgs e)
        {
            var geometry = await SceneEditHelper.CreatePolylineAsync(View);
            var graphic = new Graphic(geometry);
            graphic.Symbol = new SimpleLineSymbol() { Color = SelectedColor.Color, Width = DEFAULT_WIDTH };
            _polylinesOverlay.Graphics.Add(graphic);
        }

        private async void AreaButton_Click(object sender, RoutedEventArgs e)
        {
            var geometry = await SceneEditHelper.CreatePolygonAsync(View);
            var graphic = new Graphic(geometry);
            graphic.Symbol = new SimpleFillSymbol() { Color = getColorWithAlpha(SelectedColor.Color, DEFAULT_AREA_ALPHA), Outline = new SimpleLineSymbol() { Color = DEFAULT_POLYGON_BORFDER_COLOR } };
            _polygonsOverlay.Graphics.Add(graphic);
        }

        private Color getColorWithAlpha(Color color, byte alpha)
        {
            return Color.FromArgb(alpha, color.R, color.G, color.B);
        }

        public SceneView View
        {
            get { return (SceneView)GetValue(ViewProperty); }
            set
            {
                SetValue(ViewProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(SceneView), typeof(PaintToolBox), new PropertyMetadata(null, new PropertyChangedCallback((DependencyObject d, DependencyPropertyChangedEventArgs e) => initiolizeMapView((SceneView)e.NewValue))));



        private static void initiolizeMapView(SceneView map)
        {
            _polygonsOverlay = new GraphicsOverlay();
            _polygonsOverlay.ID = "PolygonGraphicsOverlay";
            map.GraphicsOverlays.Add(_polygonsOverlay);
            _polylinesOverlay = new GraphicsOverlay();
            _polylinesOverlay.ID = "PolylineGraphicsOverlay";
            map.GraphicsOverlays.Add(_polylinesOverlay);
            _polygonsOverlay.Renderer = new SimpleRenderer() { Symbol = new SimpleFillSymbol() { Color = Colors.Aqua } };
            map.SceneViewTapped += Map_Tapped;
            testView = map;
        }
        private static SceneView testView;

        private static async void Map_Tapped(object sender, MapViewInputEventArgs e)
        {
            // If draw or edit is active, return
            if (SceneEditHelper.IsActive) return;
            // Try to select a graphic from the map location
            await SelectGraphicAsync(e.Position);
        }

        private static async Task SelectGraphicAsync(Point point)
        {
            // Clear previous selection
            if (_selection != null)
            {
                _selection.Unselect();
                _selection.SetVisible();
            }
            _selection = null;

            // Find first graphic from the overlays
            foreach (var overlay in testView.GraphicsOverlays)
            {
                var foundGraphic = await overlay.HitTestAsync(
                        testView,
                        point);

                if (foundGraphic != null)
                {
                    _selection = new GraphicSelection(foundGraphic, overlay);
                    _selection.Select();
                    break;
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selection == null) return; // Selection missing

            // Cancel previous edit
            if (SceneEditHelper.IsActive)
                SceneEditHelper.Cancel();

            Esri.ArcGISRuntime.Geometry.Geometry editedGeometry = null;

            try
            {
                // Edit selected geometry and set it back to the selected graphic
                switch (_selection.GeometryType)
                {
                    case GeometryType.Point:
                        editedGeometry = await SceneEditHelper.CreatePointAsync(
                            View);
                        break;
                    case GeometryType.Polyline:
                        _selection.SetHidden(); // Hide selected graphic from the UI
                        editedGeometry = await SceneEditHelper.EditPolylineAsync(
                            View,
                            _selection.SelectedGraphic.Geometry as Polyline);
                        break;
                    case GeometryType.Polygon:
                        _selection.SetHidden(); // Hide selected graphic from the UI
                        editedGeometry = await SceneEditHelper.EditPolygonAsync(
                            View,
                            _selection.SelectedGraphic.Geometry as Polygon);
                        break;
                    default:
                        break;
                }

                _selection.SelectedGraphic.Geometry = editedGeometry; // Set edited geometry to selected graphic
            }
            catch (TaskCanceledException tce)
            {
                // This occurs if draw operation is canceled or new operation is started before previous was finished.
                Debug.WriteLine("Previous edit operation was canceled.");
            }
            finally
            {
                _selection.Unselect();
                _selection.SetVisible(); // Show selected graphic from the UI
            }
        }
    }
}
