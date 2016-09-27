using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    class Waypoint
    {

        private Vector3 _position;
        private GameObject _marker;

        public Waypoint(Vector3 position, GameObject marker)
        {
            _position = position;
            _marker = marker;
        }
        
        public Vector3 Position
        {
            get { return _position; }
        }

        public GameObject Marker
        {
            get { return _marker; }
        }

    }
}
