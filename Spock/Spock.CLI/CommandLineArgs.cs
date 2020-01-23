// <copyright file="CommandLineArgs.cs" company="Erratic Motion Ltd">
// Copyright (c) Erratic Motion Ltd. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ErraticMotion
{
    using System.Collections.Generic;
    using Test;
    using Test.Tools;
    using Test.Tools.CommandLine.Parser;

    /// <summary>
    /// Spock command line parameters.
    /// </summary>
    internal class CommandLineArgs : ArgumentDefinitionBase, ISpockOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgs"/> class.
        /// </summary>
        public CommandLineArgs()
        {
            this.SetIgnore = true;
        }

        /// <summary>
        /// Gets or sets the test runner.
        /// </summary>
        [Argument(
            ShortName = "t",
            LongName = "testrunner",
            Description = "The type of test runner used to run the generated test fixtures",
            Required = false)]
        public TestRunner TestRunner { get; set; }

        /// <summary>
        /// Gets or sets the Gherkin .feature file to be processed.
        /// </summary>
        [Argument(
            ShortName = "f",
            LongName = "feature",
            Description = "The Gherkin .feature file to be processed",
            Required = false)]
        public string SourceFilePath { get; set; }

        /// <summary>
        /// Gets or sets the qualified namespace.
        /// </summary>
        [Argument(
            ShortName = "n",
            LongName = "namespace",
            Description = "The fully qualified .NET namespace. If not present will infer from the .feature # namespace: <value>",
            Required = false)]
        public string QualifiedNamespace { get; set; }

        /// <summary>
        /// Gets or sets the qualified namespace.
        /// </summary>
        [Argument(
            ShortName = "d",
            LongName = "directory",
            Description = "The fully qualified file system directory to search for Gherkin .feature files.",
            Required = false)]
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the introspection target.
        /// </summary>
        [Argument(
            ShortName = "i",
            LongName = "introspection",
            Description = "The fully qualified path to the test fixture assembly we want to apply introspection to.",
            Required = false)]
        public string IntrospectionTarget { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to add the Ignore attribute when creating test fixtures.
        /// </summary>
        /// <value>
        /// <c>True</c> if the Ignore attribute is to be added; otherwise, <c>false</c>.
        /// </value>
        [Argument(
            ShortName = "g",
            LongName = "ignore",
            Description = "A value indicating whether to add the Ignore attribute when creating test fixtures.",
            Required = false)]
        public bool SetIgnore { get; set; }

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        /// <value>
        /// The category description. The value to add to an Test Category attribute. If not
        /// present, then no Test Category attribute will be added.
        /// </value>
        [Argument(
            ShortName = "c",
            LongName = "category",
            Description = "A value that will be used as a description within the Test Category attribute.",
            Required = false)]
        public string CategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language. Display the Gherkin keywords and their associated language specific
        /// versions to the console window.
        /// </value>
         [Argument(
             ShortName = "l",
             LongName = "lang",
             Description = "Displays the Gherkin keywords and their associated language specific versions to the console window.",
             Required = false)]
        public string I18N { get; set; }

        public override IEnumerable<string> SampleUsage()
        {
            yield return "   spock /?\n\tDisplay the Spock command line arguments and sample usage";
            yield return "   spock -i:\"c:\\projects\\parkeon.test.electra.customer.croydon.dll\"\n\tCreates a Welcome topic (.aml) file from the test fixtures contained in the assembly specified in the -i:<string> path.";
            yield return "   spock -t:MSTest -n:Parkeon.Test -f:\"c:\\projects\\customer.feature\"\n\tCreates a MSTest based Test Fixture";
            yield return "   spock -testrunner:NUnit -namespace:Parkeon.Test -directory:c:\\projects\\customer\\features\\";

            yield return "   Supported Languages";
            foreach (var lang in Internationalization.Vocabularies)
            {
                yield return $"   {lang:l}";
            }
        }
    }
}