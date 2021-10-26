using System;
using System.Text;

namespace ChessAI
{
    public readonly struct BitBoardExample
    {
        public ulong Value { get; }

        public BitBoardExample(ulong value = 0)
        {
            Value = value;
        }


        public override string ToString()
        {
            var builder = new StringBuilder("\tABCDEFGH\n");

            var lineBytes = BitConverter.GetBytes(Value);
            for (short i = 0; i < 8; i++)
            {
                var line = Convert.ToString(lineBytes[i], 2).PadLeft(8, '0');
                builder.AppendLine((i + 1) + "\t" + line);
            }

            return builder.ToString();
        }

        public string ToString1()
        {
            var builder = new StringBuilder("\tABCDEFGH\n");

            var lineBytes = BitConverter.GetBytes(Value);
            for (short i = 0; i < 8; i++)
            {
                var line = Convert.ToString(lineBytes[i], 2).PadLeft(8, '0');
                builder.Append(i + 1).Append("\t").AppendLine(line);
            }

            return builder.ToString();
        }
        
        

        public string ToString2()
        {
            var builder = new StringBuilder("\tABCDEFGH\n");
            var value = Value;
            const short valueByteNum = 8;

            unsafe
            {
                byte* bytes = (byte*)&value;
                string line;
                for (ushort i = 0; i < valueByteNum; i++)
                {
                    line = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
                    builder.Append(i + 1).Append("\t").AppendLine(line);
                }
            }

            return builder.ToString();
        }
    }
}