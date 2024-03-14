namespace BlastPuzzle.Scripts.Cubes
{
    public class RedCube : CubeItem
    {
        public override string Id => "0";

        public override string PoolInstanceId
        {
            get => "RedItem";
            set => throw new System.NotImplementedException();
        }
    }
}