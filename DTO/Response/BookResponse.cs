using System.Text.Json.Serialization;

namespace LoyalLib.DTO.Response;

public class BookResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("cover_color")]
    public string CoverColor { get; set; }
    
    [JsonPropertyName("author")]
    public string Author { get; set; }
    
    [JsonPropertyName("cover")]
    public string Cover { get; set; }
    
    [JsonPropertyName("epoch")]
    public string Epoch { get; set; }
    
    [JsonPropertyName("href")]
    public string Href { get; set; }
    
    [JsonPropertyName("genre")]
    public string Genre { get; set; }
    
    [JsonPropertyName("simple_thumb")]
    public string SimpleThumb { get; set; }
    
    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("cover_thumb")]
    public string CoverThumb { get; set; }
    
    public bool ReadByUser { get; set; }
    
}