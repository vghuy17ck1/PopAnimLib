using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model
{
    public class SubAnim
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "dummy")]
        public int Dummy { get; set; }
        [JsonProperty(PropertyName = "fps")]
        public short FPS { get; set; }
        [JsonProperty(PropertyName = "frames")]
        public short Frames { get; set; }
        [JsonProperty(PropertyName = "starting_frame")]
        public short StartingFrame { get; set; }
        [JsonProperty(PropertyName = "ending_frame")]
        public short EndingFrame { get; set; }
        [JsonProperty(PropertyName = "transforms")]
        public List<SubAnimTransform> SubAnimTransforms { get; set; }
    }
}
