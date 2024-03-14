namespace BlastPuzzle.Scripts.Cubes
{
    public class GreenCube : CubeItem
    {
        public override string Id => "3";

        public override string PoolInstanceId
        {
            get => "GreenItem";
            set => throw new System.NotImplementedException();
        }



    }
}