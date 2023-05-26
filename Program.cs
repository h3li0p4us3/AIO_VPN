﻿using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console.Extensions;
using System.IO;
using System.Diagnostics;

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

            if (!AnsiConsole.Profile.Capabilities.Interactive)
            {
                AnsiConsole.MarkupLine("[red]Environment does not support interaction.[/]");
                return;
            }

            if (!AskConfirmation())
            {
                return;
            }

            while (true)
            {
                Console.Clear();
                // Check if we can accept key strokes
                

                Directory.CreateDirectory(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AIO_VPN"));
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
                    .AddRow("[grey]VPN Services[/]", VPNServices));
                AnsiConsole.MarkupLine("[blue]press anykey on keyboard to start[/]");
                Console.ReadKey();

                string[] servicesArray = VPNServices.Split(new[] { ", " }, StringSplitOptions.None);

                foreach (var service in servicesArray)
                {
                    switch (service)
                    {
                        case "OpenVPN":
                            // Code to handle OpenVPN Installer
                            Console.WriteLine("Installing openVPN");
                            Thread.Sleep(1000);
                            Console.Clear();
                            OpenVPNInstall();

                            break;
                        case "V2Ray":
                            // Code to handle V2Ray Installer
                            Console.WriteLine("Installing V2ray");
                            Thread.Sleep(1000);
                            break;
                        case "WireGuard":
                            // Code to handle WireGuard Installer
                            Console.WriteLine("Installing WireGuard");
                            Thread.Sleep(1000);
                            break;
                        case "SoftEther VPN":
                            // Code to handle SoftEther VPN Installer
                            Console.WriteLine("Installing SoftEther VPN");
                            Thread.Sleep(1000);
                            break;
                        default:
                            // Code to handle unknown Installer
                            Console.WriteLine("Invalid choice: " + service);
                            Thread.Sleep(1000);
                            break;
                    }
                }

            }

        }

        private static void OpenVPNInstall()
        {
            // URL : https://swupdate.openvpn.org/community/releases/OpenVPN-2.6.4-I001-amd64.msi

            const string fileUrl = "https://cdn.discordapp.com/attachments/1090264607328829511/1111670639460352120/OpenVPN-2.6.4-I001-amd64.msi";
            string destinationPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AIO_VPN",
                "OpenVPN-2.6.4-I001-amd64.msi");

            var webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;

            if(!File.Exists(destinationPath))
            {
                Console.WriteLine("Downloading file...");
                var downloadCompleted = new ManualResetEvent(false);
                webClient.DownloadFileAsync(new Uri(fileUrl), destinationPath);
                webClient.DownloadFileCompleted += (s, e) => downloadCompleted.Set();
                downloadCompleted.WaitOne();
                AnsiConsole.MarkupLine("[grey] Download Finished![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[grey] No Need for download. file already exist![/]");
                Thread.Sleep(2000);
                Console.Clear();
            }

            #region Start OpenVPN Installer msi
            
            AnsiConsole.MarkupLine("[grey]in Installer file, press on Customize and select[/] [green]OpenSSL Utilities [/][grey]then install the openVPN[/]\n");

            try
            {
                // Start the process
                Process.Start(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AIO_VPN",
                "OpenVPN-2.6.4-I001-amd64.msi"));
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error ->[/] {ex.Message}");
                Console.ReadKey();
                return;
            }
            #endregion



            AnsiConsole.MarkupLine("[green]OpenVPN[/] [grey]Installer is open. Finish installing then press any key to continue[/]");
            Console.ReadKey();

        }
        private static void WebClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage <100)
            {
                AnsiConsole.Markup($"\r Downloading [green]OpenVPN-2.6.4-I001-amd64.msi[/][grey] -> [/][blue]{e.ProgressPercentage}%[/]");
            }
            else if (e.ProgressPercentage == 100)
            {
                
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
                return true;
            }
            AnsiConsole.MarkupLine("Thank you :))");
            Thread.Sleep(2000);
            
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