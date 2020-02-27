using System;
using UnityEngine;

namespace People.Domain
{
    /**
     * Basic representation of Person
     */
    public class Person
    {
        public Guid Guid { get; set; }

        public Vector3 Position { get; set; }
        public Vector2 Position2D { get; set; }
        public Vector2 Destination2D { get; set; }
        
        public Quaternion Rotation { get; set; }
        
        public PersonMode PersonMode { get; set; }
        public PersonType PersonType { get; set; }
        
        public PersonObjectPoolingComponent PersonObjectPoolingComponent { get; set; }
    }
}