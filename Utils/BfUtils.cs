using System.Text;

namespace Utils;

public class BFProcessor
{
    private (Dictionary<int, int> lookUpByOpening,Dictionary<int, int> lookupByEnclosing) _lookups;
    private string _sourceCode;
    
     public BFProcessor(string source)
     {
         _sourceCode = source;
         
     }
    private readonly char[] BrainfuckKeywords = { '>', '<', '+', '-', '.', ',', '[', ']' };

    private void Clean()
    {
        var sb = new StringBuilder(_sourceCode);
        int i = 0;
        while (i < sb.Length)
        {
            int j = i;
            while (i<sb.Length && !BrainfuckKeywords.Contains(sb[i]))
            {
                i++;
            }

            if (!BrainfuckKeywords.Contains(sb[j]))
            {
                sb.Remove(j, i - j);
                i = j;
            }

            i++;
        }

        _sourceCode = sb.ToString();
    }
    public string PreProcess()
    {
        Clean();
        _lookups =  BuildLoopLookup();
        return _sourceCode;
    }

    private (Dictionary<int, int> LookUpByOpening,Dictionary<int, int> LookupByEnclosing) BuildLoopLookup()
    {
        Dictionary<int, int> LookUpByOpening = new();
        Dictionary<int, int> LookupByEnclosing = new();
        Stack<int> stack = new();
        for (int i = 0; i < _sourceCode.Length; i++)
        {
            if(_sourceCode[i]=='[') stack.Push(i);
            else if (_sourceCode[i] == ']')
            {
                var openingIndex=stack.Pop();
                LookUpByOpening.Add(openingIndex,i);
                LookupByEnclosing.Add(i,openingIndex);
            }
        }

        return (LookUpByOpening,LookupByEnclosing);
    }
    public  byte IncrementMod256(byte val) => (byte)((val + 1) % 256);
    public  byte DecrementMod256(byte val) => (byte)((val - 1) % 256);
    public  void SafeIncrement(ref int pointer,ref byte[] tape)
    {
        if (++pointer >= tape.Length)
        {
            Array.Resize(ref tape,tape.Length*2);
        }
    }
    public  void SafeDecrement(ref int pointer)
    {
        pointer = pointer > 0 ? pointer-1 : 0;
    }
    
    public  void EvaluateLoopStart(byte current,ref int commandIndex)
    {
        if (current != 0) return;
        //look for enclosing ] aka loop end
        commandIndex = _lookups.lookUpByOpening[commandIndex];
    }
    public  void EvaluateLoopEnd(byte current,ref int commandIndex)
    {
        if (current == 0) return;
        //look for enclosing ] aka loop end
        commandIndex = _lookups.lookupByEnclosing[commandIndex];
    }
}