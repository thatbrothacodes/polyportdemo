using System;

namespace PolyPort.Demo.Models {
    public class Token {
        public Guid id { get; }
        public string label {get; set;}
        public int iat { get; set; }
        public string name { get; set; }
        public string sub { get; set; }
    }
}