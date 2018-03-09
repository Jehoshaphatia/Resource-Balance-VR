namespace ResourceBalancing.Model
{
    public struct Index2D
    {
        public int row;
        public int col;

        public Index2D(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}