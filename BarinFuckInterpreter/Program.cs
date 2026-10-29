using Utils;

Console.Clear();

string source = File.ReadAllText("./main.b");
BFProcessor processor = new(source);

source = processor.PreProcess();


int initialCapacity = 256;
byte[] tape = new byte[initialCapacity];
int pointer=0;
int commandIndex = 0;
while (commandIndex < source.Length)
{

    switch (source[commandIndex])
    {
        case '>' : processor.SafeIncrement(ref pointer,ref tape); break;
        case '<' : processor.SafeDecrement(ref pointer); break; 
        case '+' : tape[pointer] = processor.IncrementMod256(tape[pointer]); break;
        case '-' : tape[pointer] = processor.DecrementMod256(tape[pointer]); break;
        case '.' : Console.Write((char)tape[pointer]); break;
        case ',' : tape[pointer] = ReadInput(); break;
        case '[' : processor.EvaluateLoopStart(tape[pointer],ref commandIndex); break;
        case ']' : processor.EvaluateLoopEnd(tape[pointer],ref commandIndex); break;

    }
    commandIndex++;
}



byte ReadInput()
{
    var s =Console.ReadLine();
    if (string.IsNullOrEmpty(s) || s.Length == 0) throw new Exception("err");

    return (byte)s[0];
}

return 0;