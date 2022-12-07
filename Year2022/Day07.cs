using NUnit.Framework;

namespace Year2022;

public class Day07 : BaseDayTest
{
    private readonly FileOrFolder root = new("/");
    private FileOrFolder currentNode;

    private readonly Dictionary<string, decimal> _sizeFolder = new Dictionary<string, decimal>();

    [Test]
    public override void Part1()
    {
        decimal result;

        foreach (var line in lines)
        {
            ParseCommand(line);
        }

        CountSizeMini(root);
        result = _sizeFolder.Where(x => x.Value <= 100000).Sum(x => x.Value);

        Console.WriteLine(result);
        Assert.Pass();
    }

    private void ParseCommand(string line)
    {
        if (line.StartsWith("$ cd"))
        {
            Cd(line[5..]);
        }else if (!line.StartsWith("$ ls"))
        {
            AddFile(line);
        }
    }

    [Test]
    public override void Part2()
    {
        decimal result;

        foreach (var line in lines)
        {
            ParseCommand(line);
        }

        CountSizeMini(root);

        var spaceCurrent = 70000000m - _sizeFolder.First().Value;
        var spaceNeed = 30000000m - spaceCurrent;
        
        result = _sizeFolder.Where(x => x.Value >= spaceNeed).MinBy(x => x.Value).Value;

        Console.WriteLine(result);
        Assert.Pass();
    }

    private void Cd(string slash)
    {
        if (slash == "/")
        {
            currentNode = root;
        }
        else if (slash == "..")
        {
            currentNode = currentNode.Parrent;
        }
        else
        {
            currentNode = currentNode.Childs.First(x => x.Filename == slash);
        }
    }

    private void AddFile(string line)
    {
        if (line.StartsWith("dir"))
        {
            currentNode.AddChild(new FileOrFolder(line[4..]));
        }
        else
        {
            var part = line.Split(' ');
            currentNode.AddChild(new FileOrFolder(part[1], decimal.Parse(part[0])));
        }
    }

    private void CountSizeMini(FileOrFolder folder)
    {
        if (folder.Type == TypeFileOrFolder.File) return;

        TryAddWithSuffix(_sizeFolder, folder.Filename, folder.AllSize());

        foreach (var childFolder in folder.Childs.Where(x => x.Type == TypeFileOrFolder.Folder))
        {
            CountSizeMini(childFolder);
        }
    }

    public void TryAddWithSuffix<T>(Dictionary<string, T> dic, string key, T value)
    {
        while (dic.ContainsKey(key))
        {
            key += "_";
        }
        
        dic.Add(key, value);
    }
    
    public class FileOrFolder
    {
        public readonly TypeFileOrFolder Type;
        public readonly string Filename;
        public decimal Size;

        public readonly IList<FileOrFolder> Childs = new List<FileOrFolder>();
        public FileOrFolder Parrent;

        public FileOrFolder(string filename, decimal size)
        {
            Type = TypeFileOrFolder.File;
            Filename = filename;
            Size = size;
        }
        
        public FileOrFolder(string filename)
        {
            Type = TypeFileOrFolder.Folder;
            Filename = filename;
        }

        public void AddChild(FileOrFolder child)
        {
            child.Parrent = this;
            Childs.Add(child);
        }

        public decimal AllSize()
        {
            if (Type == TypeFileOrFolder.Folder && Size == 0)
            {
                Size = Childs.Sum(x => x.AllSize());
            }

            return Size;
        }
    }
    
    public enum TypeFileOrFolder
    {
        File,
        Folder
    }
}