using Buildings.Application;
using UnityEngine;

namespace Buildings.Domain.Walls
{
    /**
     * Extracts the necessary data from the wall configurator and keeps them in this place so as not to waste memory
     */
    public class WallData : MonoBehaviour
    {
        public WallConfigurator wallConfigurator;

        public byte BasesCount { get; set; }
        public byte FrontsCount { get; set; }
        public byte RightsCount { get; set; }
        public byte BacksCount { get; set; }
        public byte LeftsCount { get; set; }

        public void Start()
        {
            BasesCount = (byte) wallConfigurator.bases.Length;
            FrontsCount = (byte) wallConfigurator.fronts.Length;
            RightsCount = (byte) wallConfigurator.rights.Length;
            BacksCount = (byte) wallConfigurator.backs.Length;
            LeftsCount = (byte) wallConfigurator.rights.Length;
        }
    }
}