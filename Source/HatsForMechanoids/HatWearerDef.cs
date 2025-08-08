using Verse;

namespace HatsForMechanoids
{
    public class HatWearerDef : Def
    {
        public float xOffset = 0f;
        public float zOffset = 0f;

        public OffsetsForDirection north = new OffsetsForDirection();
        public OffsetsForDirection east = new OffsetsForDirection();
        public OffsetsForDirection south = new OffsetsForDirection();
        public OffsetsForDirection west = new OffsetsForDirection();

        public class OffsetsForDirection
        {
            public float xOffset = 0f;
            public float zOffset = 0f;
        }
    }
}