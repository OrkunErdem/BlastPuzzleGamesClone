namespace BlastPuzzle.Scripts.Cubes
{
    public class BlueCube : CubeItem
    {
        public override string Id => "1";

        public override string PoolInstanceId
        {
            get => "BlueItem";
            set => throw new System.NotImplementedException();
        }
    }
}