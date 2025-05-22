using Scaffold.Calculations;
using Scaffold.Core.Interfaces;
using SCaFFOLD_Console;

Console.WriteLine("   SSS    CCC   aaa    FFFF  FFFF   OOOO   LL    DDDDD                                 ");
Console.WriteLine("  SS     CC       aa   FF    FF    OO  OO  LL    DD  DD                                ");
Console.WriteLine("   SSS   CC     aaaa   FFFF  FFFF  OO  OO  LL    DD  DD                                ");
Console.WriteLine("     SS  CC    aa aa   FF    FF    OO  OO  LL    DD  DD                                ");
Console.WriteLine("   SSS    CCC   aaaaa  FF    FF     OOOO   LLLLL DDDDD                                 ");
Console.WriteLine("                                                                          (c) 1976-2025");

ICalculation calc = new TestCalculation();


while (true)
{
    Console.WriteLine("");
    Console.Write("> ");
    string? inputCommand = Console.ReadLine();
    Dictionary<string, Action<ICalculation, string>> commands = Commands.getCommands();
    try
    {
        bool hasCommandParams = false;
        int idx = inputCommand.IndexOf(" ");
        if (idx <= 0) { idx = inputCommand.Length; }
        string comm = inputCommand.Substring(0, idx);
        string commandParams = "";
        if (inputCommand.Length > idx + 1)
        {
            commandParams = inputCommand.Substring(idx + 1);
        }
        commands[comm](calc, commandParams);
    }
    catch
    {
        Console.WriteLine("Command not recognised");
    }
}



