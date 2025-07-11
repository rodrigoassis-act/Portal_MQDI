namespace ONS.PortalMQDI.Models.ViewModel
{
    public class SelectItemViewModel<T>
    {
        public string Label { get; set; }
        public T Value { get; set; }
        public string StyleClass { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public bool? Disabled { get; set; }
        public string Id { get; set; }
    }
}
