﻿using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Parser
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    internal class GroupsDataParser
    {
        private const string Url = "https://urfu.ru/api/schedule/groups/";
        
        [JsonPropertyName("external_id")]
        public int ExternalId { get; set; }
        
        [JsonPropertyName("actual")]
        public bool Actual { get; set; }
        
        [JsonPropertyName("course")]
        public int Course { get; set; }
        
        [JsonPropertyName("updated")]
        public bool Updated { get; set; }
        
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("institute_id")]
        public int InstituteId { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        public string GetJson(string instituteId = default) =>
            new WebClient().DownloadString(Url + instituteId);
        
        public IEnumerable<GroupsDataParser> ParseJson(string json) =>
            JsonSerializer.Deserialize<IEnumerable<GroupsDataParser>>(json);
    }
}