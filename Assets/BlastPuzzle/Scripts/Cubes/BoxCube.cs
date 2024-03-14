namespace BlastPuzzle.Scripts.Cubes
{
    public class BoxCube : CubeItem
    {
        public override string Id => "5";

        public override string PoolInstanceId
        {
            get => "BoxItem";
            set => throw new System.NotImplementedException();
        }


   
    }
}