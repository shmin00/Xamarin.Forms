using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Shapes;

[assembly: Dependency(typeof(ResourcesProvider))]
[assembly: Dependency(typeof(Deserializer))]
[assembly: Dependency(typeof(NativeBindingService))]
[assembly: Dependency(typeof(NativeValueConverterService))]

[assembly: ExportRenderer(typeof(Layout), typeof(LayoutRenderer))]
[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewRenderer))]
[assembly: ExportRenderer(typeof(CarouselPage), typeof(CarouselPageRenderer))]
[assembly: ExportRenderer(typeof(Page), typeof(PageRenderer))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRenderer))]
#pragma warning restore CS0618 // Type or member is obsolete
[assembly: ExportRenderer(typeof(FlyoutPage), typeof(FlyoutPageRenderer))]
[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
[assembly: ExportRenderer(typeof(Shell), typeof(ShellRenderer))]

[assembly: ExportRenderer(typeof(Label), typeof(LabelRenderer))]
[assembly: ExportRenderer(typeof(Button), typeof(ButtonRenderer))]
[assembly: ExportRenderer(typeof(Image), typeof(ImageRenderer))]
[assembly: ExportRenderer(typeof(Slider), typeof(SliderRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]
[assembly: ExportRenderer(typeof(Frame), typeof(FrameRenderer))]
[assembly: ExportRenderer(typeof(Stepper), typeof(StepperRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]
[assembly: ExportRenderer(typeof(ProgressBar), typeof(ProgressBarRenderer))]
[assembly: ExportRenderer(typeof(Switch), typeof(SwitchRenderer))]
[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]
[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRenderer))]
[assembly: ExportRenderer(typeof(BoxView), typeof(BoxViewRenderer))]
[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(ActivityIndicatorRenderer))]
[assembly: ExportRenderer(typeof(IndicatorView), typeof(IndicatorViewRenderer))]
[assembly: ExportRenderer(typeof(SearchBar), typeof(SearchBarRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(EntryRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]
[assembly: ExportRenderer(typeof(TableView), typeof(TableViewRenderer))]
[assembly: ExportRenderer(typeof(NativeViewWrapper), typeof(NativeViewWrapperRenderer))]
[assembly: ExportRenderer(typeof(WebView), typeof(WebViewRenderer))]
[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
[assembly: ExportRenderer(typeof(StructuredItemsView), typeof(StructuredItemsViewRenderer))]
[assembly: ExportRenderer(typeof(CarouselView), typeof(CarouselViewRenderer))]
[assembly: ExportRenderer(typeof(SwipeView), typeof(SwipeViewRenderer))]
[assembly: ExportRenderer(typeof(RefreshView), typeof(RefreshViewRenderer))]
[assembly: ExportRenderer(typeof(IndicatorView), typeof(IndicatorViewRenderer))]
[assembly: ExportRenderer(typeof(RadioButton), typeof(RadioButtonRenderer))]

[assembly: ExportImageSourceHandler(typeof(FileImageSource), typeof(FileImageSourceHandler))]
[assembly: ExportImageSourceHandler(typeof(StreamImageSource), typeof(StreamImageSourceHandler))]
[assembly: ExportImageSourceHandler(typeof(UriImageSource), typeof(UriImageSourceHandler))]

[assembly: ExportCell(typeof(TextCell), typeof(TextCellRenderer))]
[assembly: ExportCell(typeof(ImageCell), typeof(ImageCellRenderer))]
[assembly: ExportCell(typeof(SwitchCell), typeof(SwitchCellRenderer))]
[assembly: ExportCell(typeof(EntryCell), typeof(EntryCellRenderer))]
[assembly: ExportCell(typeof(ViewCell), typeof(ViewCellRenderer))]

[assembly: ExportRenderer(typeof(EmbeddedFont), typeof(EmbeddedFontLoader))]

[assembly: ExportHandler(typeof(TapGestureRecognizer), typeof(TapGestureHandler))]
[assembly: ExportHandler(typeof(PinchGestureRecognizer), typeof(PinchGestureHandler))]
[assembly: ExportHandler(typeof(PanGestureRecognizer), typeof(PanGestureHandler))]
[assembly: ExportHandler(typeof(SwipeGestureRecognizer), typeof(SwipeGestureHandler))]
[assembly: ExportHandler(typeof(DragGestureRecognizer), typeof(DragGestureHandler))]
[assembly: ExportHandler(typeof(DropGestureRecognizer), typeof(DropGestureHandler))]


[assembly: ExportRenderer(typeof(Shell), typeof(Xamarin.Forms.Platform.Tizen.Watch.ShellRenderer), TargetIdiom.Watch)]

[assembly: ExportRenderer(typeof(Ellipse), typeof(EllipseRenderer))]
[assembly: ExportRenderer(typeof(Line), typeof(LineRenderer))]
[assembly: ExportRenderer(typeof(Path), typeof(PathRenderer))]
[assembly: ExportRenderer(typeof(Polygon), typeof(PolygonRenderer))]
[assembly: ExportRenderer(typeof(Polyline), typeof(PolylineRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Rectangle), typeof(RectangleRenderer))]
