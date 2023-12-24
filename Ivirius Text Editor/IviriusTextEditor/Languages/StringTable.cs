using IviriusTextEditor.Core.Helpers;

namespace IviriusTextEditor.Languages
{
    public class StringTable
    {
        public static string AppTitle;
        public static string RibbonFile;
        public static string RibbonEdit;
        public static string RibbonInsert;
        public static string RibbonTools;
        public static string RibbonHelp;
        public static string Undo;
        public static string Redo;
        public static string Cut;
        public static string Copy;
        public static string Paste;
        public static string New;
        public static string Open;
        public static string Save;
        public static string SaveAs;
        public static string Delete;
        public static string Print;
        public static string FindAndReplace;
        public static string Find;
        public static string Replace;
        public static string FindReplaceWarning;
        public static string CaseSensitive;
        public static string MatchWords;
        public static string ReplaceAll;
        public static string Yes;
        public static string No;
        public static string Cancel;
        public static string FindPreview;
        public static string AutosaveOn;
        public static string AutosaveOff;
        public static string Search;
        public static string Settings;
        public static string Console;
        public static string Restart;
        public static string About;
        public static string Website;
        public static string Store;
        public static string Bug;
        public static string News;
        public static string YouTube;
        public static string PrivacyPolicy;
        public static string Discord;
        public static string Twitter;

        public StringTable()
        {
            ReadLanguage();
        }

        public static void ReadLanguage()
        {
            if (SettingsHelper.GetSettingString("Language") == null)
            {
                SettingsHelper.SetSetting("Language", "en-us");
            }
            if (SettingsHelper.GetSettingString("Language") == "en-us")
            {
                AppTitle = "Ivirius Text Editor Plus";
                RibbonFile = "File";
                RibbonEdit = "Edit";
                RibbonInsert = "Insert";
                RibbonTools = "Tools";
                RibbonHelp = "Help";
                Undo = "Undo (Ctrl + Z)";
                Redo = "Redo (Ctrl + Y)";
                Cut = "Cut (Ctrl + X)";
                Copy = "Copy (Ctrl + C)";
                Paste = "Paste (Ctrl + V)";
                New = "New file (Ctrl + N)";
                Open = "Open file (Ctrl + O)";
                Save = "Save file (Ctrl + S)";
                SaveAs = "Save file as (Ctrl + Shift + S)";
                Delete = "Delete file (Ctrl + Shift + D)";
                Print = "Print (Ctrl + P)";
                FindAndReplace = "Find and Replace";
                Find = "Find";
                Replace = "Replace";
                FindReplaceWarning = "Do not abuse the Find and Replace feature by trying to replace too many characters. Hangs or crashes may occur upon intentional overload.";
                CaseSensitive = "Case sensitive";
                MatchWords = "Match whole words";
                ReplaceAll = "Replace all";
                Yes = "Yes";
                No = "No";
                Cancel = "Cancel";
                FindPreview = "Find preview (select text and click Enter to jump to the selection)";
                AutosaveOn = "Autosave: On";
                AutosaveOff = "Autosave: Off";
                Search = "Search...";
                Settings = "Settings";
                Console = "Open console";
                Restart = "Restart app";
                About = "About";
                Website = "Website";
                Store = "Store";
                Bug = "Bug report";
                News = "News";
                YouTube = "YouTube";
                PrivacyPolicy = "Privacy Policy";
                Discord = "Discord";
                Twitter = "Twitter";
            }
            if (SettingsHelper.GetSettingString("Language") == "ro-ro")
            {
                AppTitle = "Ivirius Text Editor Plus";
                RibbonFile = "Fișier";
                RibbonEdit = "Editare";
                RibbonInsert = "Inserare";
                RibbonTools = "Unelte";
                RibbonHelp = "Ajutor";
                Undo = "Anulare (Ctrl + Z)";
                Redo = "Revenire (Ctrl + Y)";
                Cut = "Tăiere (Ctrl + X)";
                Copy = "Copiere (Ctrl + C)";
                Paste = "Lipire (Ctrl + V)";
                New = "Fișier nou (Ctrl + N)";
                Open = "Deschidere fișier (Ctrl + O)";
                Save = "Salvare fișier (Ctrl + S)";
                SaveAs = "Salvare fișier ca (Ctrl + Shift + S)";
                Delete = "Ștergere fișier (Ctrl + Shift + D)";
                Print = "Printare (Ctrl + P)";
                FindAndReplace = "Căutare și înlocuire";
                Find = "Caută";
                Replace = "Înlocuire";
                FindReplaceWarning = "A nu se abuza de unealta pentru găsire și înlocuire prin încercarea de a înlocui prea multe caractere. Pot apărea erori.";
                CaseSensitive = "Potrivire litere mari și mici";
                MatchWords = "Potrivire cuvinte întregi";
                ReplaceAll = "Înloc. tot.";
                Yes = "Da";
                No = "Nu";
                Cancel = "Anulare";
                FindPreview = "Previzualizare găsiri (selectează text și apasă Enter pentru a ajunge la destinația dorită)";
                AutosaveOn = "Salvare automată: Pornită";
                AutosaveOff = "Salvare automată: Oprită";
                Search = "Căutare...";
                Settings = "Setări";
                Console = "Deschidere consolă";
                Restart = "Repornire aplicație";
                About = "Despre";
                Website = "Website";
                Store = "Magazin";
                Bug = "Raport erori";
                News = "Știri";
                YouTube = "YouTube";
                PrivacyPolicy = "Poliță de confidențialitate";
                Discord = "Discord";
                Twitter = "Twitter";
            }
        }
    }
}
