using OnlineVotingSystem.Servises;
using OnlineVotingSystem.Uis;
using Spectre.Console;

namespace OnlineVotingSystem;

public class Program
{
    private static async Task Main(string[] args)
    {
        MainUi mainUi = new MainUi();
        await mainUi.Run();

        AnsiConsole.WriteLine("-----");
        AnsiConsole.MarkupLine("| c | [bold]Asrorbek Abrorov[/]. [italic]Thankyou for using.[/]");
        AnsiConsole.WriteLine("-----");
    }
}