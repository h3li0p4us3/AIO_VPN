using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIO_VPN
{
    internal class Program
    {

        public static void AboutProject()
        {
            AnsiConsole.WriteLine();
            Render("Languages used", new BreakdownChart()
                .FullSize()
                .Width(60)
                .ShowPercentage()

                .AddItem("C#", 66.5, Color.Green)
                .AddItem("Shell", 12.2, Color.Aqua)
                .AddItem("C++", 21.3, Color.Blue)
                );

            Console.ReadKey();
        }

        static void Main(string[] args)
        {

            // Check if we can accept key strokes
            if (!AnsiConsole.Profile.Capabilities.Interactive)
            {
                AnsiConsole.MarkupLine("[red]Environment does not support interaction.[/]");
                return;
            }

            // Confirmation
            if (!AskConfirmation())
            {
                return;
            }

            // Ask the user for some different things
            WriteDivider("Strings");
            var name = AskName();

            WriteDivider("Lists");
            string VPNServices = AskVPN();

            

            // Summary
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Results[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.Write(new Table().AddColumns("[grey]Question[/]", "[grey]Answer[/]")
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .AddRow("[grey]user Name[/]", name)
                .AddRow("[grey]VPN Services[/]",VPNServices));
            AnsiConsole.MarkupLine("[blue]press anykey on keyboard to start[/]");
            Console.ReadKey();

            string[] servicesArray = VPNServices.Split(new[] { ", " }, StringSplitOptions.None);

            foreach (var service in servicesArray)
            {
                switch (service)
                {
                    case "OpenVPN":
                        // Code to handle OpenVPN option
                        Console.WriteLine("Installing openVPN");
                        Thread.Sleep(1000);
                        break;
                    case "V2Ray":
                        // Code to handle V2Ray option
                        Console.WriteLine("Installing V2ray");
                        Thread.Sleep(1000);
                        break;
                    case "WireGuard":
                        // Code to handle WireGuard option
                        Console.WriteLine("Installing WireGuard");
                        Thread.Sleep(1000);
                        break;
                    case "SoftEther VPN":
                        // Code to handle SoftEther VPN option
                        Console.WriteLine("Installing SoftEther VPN");
                        Thread.Sleep(1000);
                        break;
                    default:
                        // Code to handle unknown options
                        Console.WriteLine("Invalid choice: " + service);
                        Thread.Sleep(1000);
                        break;
                }
            }



        }

        private static void WriteDivider(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[yellow]{text}[/]").RuleStyle("grey").LeftJustified());
        }

        public static bool AskConfirmation()
        {
            if (!AnsiConsole.Confirm("would u leave a star in Github?"))
            {
                AnsiConsole.MarkupLine("Ok... :(");
                Thread.Sleep(2000);
                Console.Clear();
                return true;
            }
            AnsiConsole.MarkupLine("Thank you :))");
            Thread.Sleep(2000);
            Console.Clear();
            
            return true;
        }

        public static string AskName()
        {
            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
            return name;
        }

        public static string AskVPN()
        {
            var favorites = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .Title("which [green]VPN Services[/] you want to install?")
                    .MoreChoicesText("[grey](Move up and down to reveal more Services)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle a vpn service, [green]<enter>[/] to accept)[/]")
                    .AddChoices(new[]
                    {
                        "OpenVPN", "V2Ray", "WireGuard", "SoftEther VPN"
                    }));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var array in favorites)
            {
                stringBuilder.Append(string.Join(", ", array));
                stringBuilder.Append(", ");
            }
            stringBuilder.Length -= 2; // Remove the trailing comma and space

            string protocolsString = stringBuilder.ToString();
            Console.Clear();
            return protocolsString;
        }

        

        private static void Render(string title, IRenderable chart)
        {
            AnsiConsole.Write(
                new Panel(chart)
                    .Padding(1, 1)
                    .Header(title));
        }

    }

}