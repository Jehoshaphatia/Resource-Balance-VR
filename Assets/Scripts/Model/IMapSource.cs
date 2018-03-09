namespace ResourceBalancing.Model
{
    public interface IMapSource
    {
        TileType this [int row, int col] { get; }
        int Rows { get; }
        int Columns { get; }
    }
}
