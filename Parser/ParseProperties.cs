using System.Text.Json.Serialization;

namespace Parser
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    internal class ParseProperties
    {
        public ParseProperties(int externalId, bool actual, int course,
            bool updated, int id, string title, int instituteId)
        {
            ExternalId = externalId;
            Actual = actual;
            Course = course;
            Updated = updated;
            Id = id;
            InstituteId = instituteId;
            Title = title;
        }

        [JsonPropertyName("external_id")]
        public int ExternalId { get; }
        
        [JsonPropertyName("actual")]
        public bool Actual { get; }
        
        [JsonPropertyName("course")]
        public int Course { get; }
        
        [JsonPropertyName("updated")]
        public bool Updated { get; }
        
        [JsonPropertyName("id")]
        public int Id { get; }
        
        [JsonPropertyName("institute_id")]
        public int InstituteId { get; }
        
        [JsonPropertyName("title")]
        public string Title { get; }
    }
}