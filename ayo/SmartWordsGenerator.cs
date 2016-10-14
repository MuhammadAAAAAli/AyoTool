using ayo.Interfaces;
using ayo.Static;

namespace ayo
{
    internal class SmartWordsGenerator
    {
        private readonly int _howManyPasses;
        private readonly ISmartAlghoritm _smartAlghoritm;
        private readonly IOutput _output;

        public SmartWordsGenerator(ISmartAlghoritm smartAlghoritm, IOutput output, int howManyPasses)
        {
            _output = output;
            _smartAlghoritm = smartAlghoritm;
            _howManyPasses = howManyPasses;
        }

        public void Go()
        {
            Output.ToConsole("Generating possible passes ... ");
            for (var i = 0; i < _howManyPasses; i++)
            {
                var passGenerated = _smartAlghoritm.GetNextPassword();
                if (passGenerated.Length >= 8)                                    // only for WPA
                {
                    //      Output.ToConsole(passGenerated);
                    _output.LogGeneratedPasswords(passGenerated);
                    if (i == 1000000)
                        Output.ToConsole("Generated 1 million passwords in " + Output.LogTime());
                    else if (i%1000000 == 0 && i != 0)
                        Output.ToConsole("Generated 1000000 passes !");
                }
                else
                {
                    i--;
                }
            }
        }
    }
}