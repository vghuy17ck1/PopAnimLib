using Newtonsoft.Json;

namespace Model
{
    public class Transform
    {
        [JsonProperty(PropertyName = "id")]
        public byte ID { get; set; }
        [JsonProperty(PropertyName = "type")]
        public byte Type { get; set; }
        [JsonProperty(PropertyName = "rotate")]
        public short? Rotate { get; set; }
        [JsonProperty(PropertyName = "point")]
        public Point Point { get; set; }
        [JsonProperty(PropertyName = "matrix")]
        public Matrix Matrix { get; set; }
        [JsonProperty(PropertyName = "color")]
        public Color Color { get; set; }
    }

    public class SubAnimTransform
    {
        [JsonProperty(PropertyName = "ref_id")]
        public short RefID { get; set; }
        [JsonProperty(PropertyName = "ref_type")]
        public short RefType { get; set; }
        [JsonProperty(PropertyName = "ref_index")]
        public short RefIndex { get; set; }
        [JsonProperty(PropertyName = "transform")]
        public Transform Transform { get; set; }
    }

    public class Point
    {
        [JsonProperty(PropertyName = "left")]
        public int? Left { get; set; }
        [JsonProperty(PropertyName = "top")]
        public int? Top { get; set; }
    }

    public class Matrix
    {
        [JsonProperty(PropertyName = "x1")]
        public double? X1 { get; set; }
        [JsonProperty(PropertyName = "y1")]
        public double? Y1 { get; set; }
        [JsonProperty(PropertyName = "x2")]
        public double? X2 { get; set; }
        [JsonProperty(PropertyName = "y2")]
        public double? Y2 { get; set; }
    }

    public class Color
    {
        [JsonProperty(PropertyName = "red")]
        public byte Red { get; set; }
        [JsonProperty(PropertyName = "green")]
        public byte Green { get; set; }
        [JsonProperty(PropertyName = "blue")]
        public byte Blue { get; set; }
        [JsonProperty(PropertyName = "alpha")]
        public byte Alpha { get; set; }
    }
}
