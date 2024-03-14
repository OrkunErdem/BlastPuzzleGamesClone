namespace BlastPuzzle.Scripts.Cubes
{
    public class VaseCube : CubeItem
    {
        public override string Id => "7";

        public override string PoolInstanceId
        {
            get => "VaseItem";
            set => throw new System.NotImplementedException();
        }
    }
}