
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model
{
    public class PopAnim
    {
        [JsonProperty(PropertyName = "anim")]
        public string Anim { get; set; }
        [JsonProperty(PropertyName = "sprites")]
        public List<Sprite> Sprites { get; set; }
        [JsonProperty(PropertyName = "sub_anims")]
        public List<SubAnim> SubAnims { get; set; }
    }
}
