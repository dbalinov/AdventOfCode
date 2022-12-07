namespace AdventOfCode.Year2022.Day7;

internal sealed class TreeNode
{
    public IDictionary<string, TreeNode> Directories { get; set; } = new Dictionary<string, TreeNode>();

    public int FilesSize { get; set; }

    public TreeNode? Parent { get; set; }

    private int _size;
    public int Size
    {
        get
        {
            if (_size == default)
            {
                _size = GetSize(this);
            }

            return _size;
        }
    }

    public void AddDirectory(string name, TreeNode treeNode)
        => Directories.Add(name, treeNode);

    private static int GetSize(TreeNode node)
        => node.FilesSize +
           node.Directories.Values.Sum(GetSize);
}