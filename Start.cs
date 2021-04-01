using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using static MelonLoader.MelonLogger;

namespace Dawn.EncodingFix
{
    internal sealed class Start : MelonMod
    {
        public override void VRChat_OnUiManagerInit()
        {
            MelonPreferences.CreateCategory("EncodingFix", "Encoding Fix");
            MelonPreferences.CreateEntry("EncodingFix", "Enabled", true, "Enabled (Requires Restart"); // Don't wanna cache it yet.
            MelonPreferences.CreateEntry("EncodingFix", "PageID", 0, "Advanced: SetPageID (int) Default = 0");
            
            LocalPrefsSaved(); // Don't wanna call OnPrefs for eveyone lol.
        }
        public override void OnPreferencesSaved()
        {
            LocalPrefsSaved();
        }
        private static void LocalPrefsSaved()
        {
            m_Enabled = MelonPreferences.GetEntryValue<bool>("EncodingFix", "Enabled");
            if (!m_Enabled) return;
            m_PageID = MelonPreferences.GetEntryValue<int>("EncodingFix", "PageID");
           
            SetCodingPage((uint) m_PageID);
        }
        private static bool m_Enabled;
        private static int m_PageID;
        [DllImport("kernel32.dll", SetLastError = true)] // Credits to Bluescream for the DLLImport links!
        private static extern bool SetConsoleOutputCP(uint wCodePageID);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);

        static void SetCodingPage(uint i = 65001) // https://stackoverflow.com/questions/38533903/set-c-sharp-console-application-to-unicode-output/59307528#59307528
        {
            //Credits to Cillié Malan
            SetConsoleOutputCP(i);
            SetConsoleCP(i);
            Console.OutputEncoding = Encoding.GetEncoding((int) i);
            Console.InputEncoding = Encoding.GetEncoding((int) i);

        }
    }
}