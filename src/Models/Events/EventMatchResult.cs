namespace Models.Events
{
    public class EventMatchResult
    {
        public bool Success { get; set; }
        public EventMatchItem[] MatchItems { get; set; } 
    }

    public class EventMatchItem
    {
        public EventMatchItem Type { get; set; }
        public string Data { get; set; }
    }

    public enum EventMatchItemType
    {
        Text
    }
}