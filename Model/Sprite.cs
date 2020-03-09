﻿using Newtonsoft.Json;

namespace Model
{
    public class Sprite
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "properties")]
        public SpriteProperties Properties { get; set; }
    }

    public class SpriteProperties
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "width")]
        public short Width { get; set; }
        [JsonProperty(PropertyName = "height")]
        public short Height { get; set; }
        [JsonProperty(PropertyName = "x1")]
        public int X1 { get; set; }
        [JsonProperty(PropertyName = "y1")]
        public int Y1 { get; set; }
        [JsonProperty(PropertyName = "x2")]
        public int X2 { get; set; }
        [JsonProperty(PropertyName = "y2")]
        public int Y2 { get; set; }
        [JsonProperty(PropertyName = "left")]
        public short Left { get; set; }
        [JsonProperty(PropertyName = "top")]
        public short Top { get; set; }
    }
}
