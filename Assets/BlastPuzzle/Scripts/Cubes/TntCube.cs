namespace BlastPuzzle.Scripts.Cubes
{
    public class TntCube : CubeItem
    {
        public override string Id => "4";

        public override string PoolInstanceId
        {
            get => "TntItem";
            set => throw new System.NotImplementedException();
        }
    }
}