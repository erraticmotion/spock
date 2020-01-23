// <copyright file="Spock.cs" company="Erratic Motion Ltd">
// Copyright (c) Erratic Motion Ltd. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

#define IN_DEV
namespace ErraticMotion
{
    using System;
    using System.IO;
    using System.Reflection;
    using Test;
    using Test.Tools;

    /// <summary>
    /// Spock.Net, based on the design thinking behind the Java/Groovy tool Spock but
    /// has now morphed into a general replacement for the Cucumber/SpecFlow tool.
    /// </summary>
    public class Spock
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var options = CommandLineManager.GetCommandLineArguments<CommandLineArgs>(args);
            if (!options.IsValid || !options.SearchValid())
            {
                Console.WriteLine(options.Usage);
#if IN_DEV
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
#endif
                Environment.Exit(1);
            }

            if (!string.IsNullOrEmpty(options.I18N))
            {
                var lang = Internationalization.Vocabularies.Find(options.I18N);
                if (lang == null)
                {
                    Console.WriteLine("Language not supported");
                }
                else
                {
                    Console.WriteLine(lang);
                }
#if IN_DEV
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
#endif
                Environment.Exit(0);
            }

            if (options.IsIntrospectionSpecified())
            {
                Console.WriteLine("Introspection target {0}", options.IntrospectionTarget);
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFile(options.IntrospectionTarget);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Introspection target threw an error.");
                    Console.WriteLine(ex.Message);
                }

                if (assembly != null)
                {
                    var engine = new IntrospectionEngine(assembly);
                    var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
                    var path = $"{fi.DirectoryName}\\Welcome.aml";
                    try
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        var welcome = engine.GetWelcomeTopic(new Guid("97f76524-0712-4f9d-85cc-f0f66bad60da"), 1);
                        using (var outputFile = new StreamWriter(path))
                        {
                            foreach (var line in welcome)
                            {
                                outputFile.WriteLine(line);
                            }
                        }

                        Console.WriteLine("Welcome.aml written to '{0}'", path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Deleting the file {0} resulted in an error.", path);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else if (options.IsFileSpecified())
            {
                try
                {
                    var gherkin = Test.Tools.Gherkin.Lexer.For(options.Source().FullName).Parse();
                    var spock = Test.Tools.Spock.Lexer.For(options).Parse(gherkin);
                    using (var outputFile = new StreamWriter(spock.FixtureInvariants.FilePath))
                    {
                        foreach (var line in spock.Spock())
                        {
                            outputFile.WriteLine(line);
                        }
                    }

                    Console.WriteLine(" '{0}' file generated", spock.FixtureInvariants.FilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(ex);
#if IN_DEV
                    Console.WriteLine("Press any key to exit");
                    Console.ReadLine();
#endif
                    Environment.Exit(1);
                }
            }
            else
            {
                var generated = 0;
                var noScenarios = 0;
                var invalidGherkin = 0;
                var gherkinUnknown = 0;
                var notCaught = 0;

                // search a complete directory for .feature files.
                foreach (var fi in options.DirectoryInfo().GetFiles("*.feature"))
                {
                    try
                    {
                        var gherkin = Test.Tools.Gherkin.Lexer.For(fi.FullName).Parse();
                        var spock = Test.Tools.Spock.Lexer.For(options).Parse(gherkin);
                        using (var outputFile = new StreamWriter(spock.FixtureInvariants.FilePath))
                        {
                            foreach (var line in spock.Spock())
                            {
                                outputFile.WriteLine(line);
                            }
                        }

                        generated++;
                    }
                    catch (Test.Tools.Gherkin.GherkinException ex)
                    {
                        switch (ex.Reason)
                        {
                            case Test.Tools.Gherkin.GherkinExceptionType.NoScenariosInFeature:
                                noScenarios++;
                                break;
                            case Test.Tools.Gherkin.GherkinExceptionType.InvalidGherkin:
                                invalidGherkin++;
                                break;
                            default:
                                Console.WriteLine(" Gherkin Unknown: '{0}' file", fi.FullName);
                                Console.WriteLine(ex.Message);
                                gherkinUnknown++;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" Error generating from '{0}' file", fi.FullName);
                        Console.WriteLine(ex.Message);
                        notCaught++;
                    }
                }

                Console.WriteLine("{0} file(s) generated.", generated);
                Console.WriteLine("{0} file(s) with no scenarios.", noScenarios);
                Console.WriteLine("{0} file(s) invalid Gherkin.", invalidGherkin);
                Console.WriteLine("{0} file(s) unknown Gherkin.", gherkinUnknown);
                Console.WriteLine("{0} file(s) not caught.", notCaught);
            }
#if IN_DEV
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
#endif
            Environment.Exit(0);
        }
    }
}
