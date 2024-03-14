namespace BlastPuzzle.Scripts.Cubes
{
    public class YellowCube : CubeItem
    {
        public override string Id => "2";

        public override string PoolInstanceId
        {
            get => "YellowItem";
            set => throw new System.NotImplementedException();
        }
    }
}