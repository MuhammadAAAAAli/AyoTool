using System;
using System.IO;
using System.Linq;
using ayo.Alghoritms;
using ayo.ProcessPDF;
using ayo.Static;

namespace ayo
{
    internal class Program
    {
        private static bool learnModeOk;
        private static bool generateModeOk;

        private static void Main(string[] args)
        {
            try
            {
                if (!args.Any())
                    ShowHelpMenu();
                else
                    CheckIfWeCanStartAnyMode(args, ref clean);

                if (learnModeOk)
                    StartLearnMode(inputFolderPath, outputFolderPath, clean);
                else if (generateModeOk)
                    StartGenerateMode(jsonPath, howManyPassToGenerate, userOptions);
            }
            catch (Exception e)
            {
                Output.ToConsole("Something whent wrong, check \"log.txt\" in bin folder ... ");
                Output.Log(e.Message);
                Output.Log(e.StackTrace);
                Environment.Exit(1);
            }
        }

        #region Help menu 

        private static void ShowHelpMenu()
        {
            Output.ToConsole("Welcome to Ayo");
            Output.ToConsole("A fun tool that helps you be creative and explore the cryptography field");
            Output.ToConsole("Any support for the project is much appreciated");
            Output.ToConsole("\n");
            Output.ToConsole("Usage : ayo [mode] [files] [options] ");
            Output.ToConsole("\n");
            Output.ToConsole("[mode]");
            Output.ToConsole("\t\t --learn \t learn order of words ( will output json ) ( only english at the moment )");
            Output.ToConsole("\t\t --generate \t generate passwords");
            Output.ToConsole("\t\t --crack \t  << currently not supported >>");
            Output.ToConsole("\n");
            Output.ToConsole("[files for learn mode]");
            Output.ToConsole("\t\t --path \t folder with pdfs path");
            Output.ToConsole("\t\t --write \t output folder for json");
            Output.ToConsole("\n");
            Output.ToConsole("[files for generate mode]");
            Output.ToConsole("\t\t --json \t json path generated at previus step");
            Output.ToConsole("\t\t --write \t file path for password output ");
            Output.ToConsole("\n");
            Output.ToConsole("[options for learn mode]");
            Output.ToConsole("\t\t --clean \t erase words with least connections. ex: clean 30 will erase 30%");
            Output.ToConsole("\t\t --path \t output folder for json");
            Output.ToConsole("\n");
            Output.ToConsole("[options for generate mode]");
            Output.ToConsole("\t optional ( grouped together ) :");
            Output.ToConsole("\t\t -w \t word");
            Output.ToConsole("\t\t -c \t following word will have capital first letter");
            Output.ToConsole("\t\t -C \t following word will have capital letters");
            Output.ToConsole("\t\t -s \t special character");
            Output.ToConsole("\t\t -n \t random number beetwen 0 - 1000 and 1900 - 2020");
            Output.ToConsole("\t mandatory ( default 100 ) :");
            Output.ToConsole("\t\t --nr \t number of passwords to be generated");
            Output.ToConsole("\n");
            Output.ToConsole("Example !");
            Output.ToConsole("ayo --learn --path %path_to_folder% --write %path_to_folder% --clean 30");
            Output.ToConsole("ayo --generate --json %path_to_json% --write %path_to_txt_file% -wnsw --nr 10");
            Output.ToConsole("\n");
            Output.ToConsole("Note !");
            Output.ToConsole("The order of the parameters must be mentained ");
            Output.ToConsole("\n");
            Environment.Exit(0);
        } // --help

        #endregion

        #region Learn mode variables

        private static string inputFolderPath;
        private static string outputFolderPath;
        private static int clean = 50; // 50% default

        #endregion

        #region Generate mode variables

        private static int howManyPassToGenerate = 100; // 100 passwords default
        private static string userOptions;
        private static string jsonPath;
        private static string generatedPasswordsFile;

        #endregion

        #region Initial logic - choose mode and initialize

        private static void CheckIfWeCanStartAnyMode(string[] args, ref int clean)
        {
            switch (args[0])
            {
                case "--learn":
                    switch (args[1])
                    {
                        case "--path":
                            switch (Directory.Exists(args[2]))
                            {
                                case true:
                                    inputFolderPath = args[2];
                                    switch (args[3])
                                    {
                                        case "--write":
                                            switch (Directory.Exists(args[4]))
                                            {
                                                case true:
                                                    outputFolderPath = args[4];
                                                    switch (args[5])
                                                    {
                                                        case "--clean":
                                                            clean = Convert.ToInt32(args[6]);
                                                            learnModeOk = true;
                                                            break;
                                                    }
                                                    break;
                                                default:
                                                    Output.ToConsole("Output folder does not exists ! ");
                                                    Environment.Exit(1);
                                                    break;
                                            }
                                            break;
                                        default:
                                            Output.ToConsole("Need output folder path !");
                                            Environment.Exit(1);
                                            break;
                                    }

                                    break;
                                case false:
                                    Output.ToConsole("Input folder does not exists ! ");
                                    Environment.Exit(1);
                                    break;
                            }
                            break;
                        default:
                            Output.ToConsole("Need input folder path !");
                            Environment.Exit(1);
                            break;
                    }
                    break;

                case "--generate":
                    switch (args[1])
                    {
                        case "--json":
                        {
                            switch (Validate.Json(args[2]))
                            {
                                case false:
                                    Output.ToConsole("Not a valid Json file !");
                                    Environment.Exit(1);
                                    break;
                                case true:
                                {
                                    jsonPath = args[2];
                                    switch (args[3])
                                    {
                                        case "--write":
                                            switch (Validate.IsValidFolderPath(args[4]))
                                            {
                                                case false:
                                                    Output.ToConsole("Something wrong with the output file path !");
                                                    Environment.Exit(1);
                                                    break;
                                                case true:
                                                    generatedPasswordsFile = args[4];
                                                    switch (Validate.UserOptionsArValid(args[5]))
                                                    {
                                                        case false:
                                                            Output.ToConsole("User options not valid !");
                                                            Environment.Exit(1);
                                                            break;
                                                        case true:
                                                            userOptions = args[5];
                                                            switch (args[6])
                                                            {
                                                                case "--nr":
                                                                {
                                                                    switch (
                                                                        int.TryParse(args[7], out howManyPassToGenerate)
                                                                        )
                                                                    {
                                                                        case false:
                                                                        {
                                                                            Output.ToConsole(
                                                                                "Number of pass to generate not a valid int !");
                                                                            Environment.Exit(1);
                                                                            break;
                                                                        }
                                                                        case true:
                                                                        {
                                                                            generateModeOk = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                default:
                {
                    ShowHelpMenu();
                    break;
                }
            }
        }

        private static void StartLearnMode(string inputFolderPath, string outputFolderPath, int clean)
        {
            new Serialize(inputFolderPath, outputFolderPath, clean);
        }

        private static void StartGenerateMode(string jsonPath, int howManyPassToGenerate, string userOptions)
        {
            var alghoritm = new SmartAlghoritm(new Generate_With_Json_Mode(jsonPath));
            var output = new Output(generatedPasswordsFile);
            var generate = new SmartWordsGenerator(alghoritm, output, howManyPassToGenerate);


            if (userOptions != null)
                foreach (var option in userOptions.Replace("-", "").ToCharArray().ToList())
                {
                    switch (option)
                    {
                        case 'c':
                        {
                            alghoritm.Queue.Enqueue(alghoritm.NextWordWithCapitalFirstLetter);
                            break;
                        }
                        case 'w':
                        {
                            alghoritm.Queue.Enqueue(alghoritm.GetRawWordAndAddOptions);
                            break;
                        }
                        case 's':
                        {
                            alghoritm.Queue.Enqueue(alghoritm.GetSpecialChar);
                            break;
                        }
                        case 'n':
                        {
                            alghoritm.Queue.Enqueue(alghoritm.GetNumber);
                            break;
                        }
                        case 'C':
                        {
                            alghoritm.Queue.Enqueue(alghoritm.NextWordWithCapitalLetters);
                            break;
                        }
                    }
                }
            generate.Go();
        }

        #endregion
    }
}