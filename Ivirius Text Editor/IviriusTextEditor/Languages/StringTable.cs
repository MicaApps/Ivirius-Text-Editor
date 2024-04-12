﻿using CommunityToolkit.WinUI.Helpers;
using Humanizer;
using IviriusTextEditor.Core.Helpers;
using Microsoft.AppCenter;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Microsoft.Toolkit.Uwp.UI.Triggers;
using Windows.Devices.Custom;
using Windows.Perception.Spatial;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Automation;

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
        public static string PasteRTF;
        public static string PasteSimple;
        public static string Clipboard;
        public static string Font;
        public static string Document;
        public static string Paragraph;
        public static string ImageInsert;
        public static string LinkInsert;
        public static string DateInsert;
        public static string Default;
        public static string Bold;
        public static string Italic;
        public static string UnderlineDefault;
        public static string UnderlineNone;
        public static string UnderlineSingle;
        public static string UnderlineDash;
        public static string UnderlineDotted;
        public static string UnderlineDouble;
        public static string UnderlineThick;
        public static string UnderlineWave;
        public static string Strikethrough;
        public static string Subscript;
        public static string Superscript;
        public static string FontColor;
        public static string Advanced;
        public static string Highlight;
        public static string HighlightColor;
        public static string Casing;
        public static string Lowercase;
        public static string Uppercase;
        public static string Sentencecase;
        public static string Titlecase;
        public static string EraseFormatting;
        public static string NormalTextStyle;
        public static string TitleTextStyle;
        public static string ContentTextStyle;
        public static string HeaderTextStyle;
        public static string Title2TextStyle;
        public static string MediumTextStyle;
        public static string FinishedTextStyle;
        public static string SubtitleTextStyle;
        public static string UnfinishedTextStyle;
        public static string BookTitleTextStyle;
        public static string StrongTextStyle;
        public static string QuoteTextStyle;
        public static string ShowAll;
        public static string List;
        public static string Bullet;
        public static string Numbers;
        public static string UppercaseList;
        public static string LowercaseList;
        public static string Roman;
        public static string ParagraphOptions;
        public static string ParagraphSpacing;
        public static string s1pt;
        public static string s115pt;
        public static string s150pt;
        public static string s2pt;
        public static string Left;
        public static string Right;
        public static string Center;
        public static string Justify;
        public static string SelectAll;
        public static string SelectAllTooltip;
        public static string DefaultTimeFormat;
        public static string Link;
        public static string Detete;
        public static string NewFile;
        public static string OpenFile;
        public static string OpenMultiple;
        public static string SaveMenu;
        public static string SaveAsMenu;
        public static string Changelog;
        public static string ViewModes;
        public static string TabletMode;
        public static string FullScreen;
        public static string NewTab;
        public static string Language;
        public static string LanguageDesc;
        public static string Theme;
        public static string ThemeDesc;
        public static string AccentOnTitleBar;
        public static string AccentOnTitleBarDesc;
        public static string win32background;
        public static string win32backDesc;
        public static string QuickAccess;
        public static string QuickAccessDesc;
        public static string CutSettings;
        public static string CopySettings;
        public static string PasteSettings;
        public static string DeleteSettings;
        public static string Accessibility;
        public static string AccessibilityDesc;
        public static string ShowStatusBar;
        public static string HomePageStartup;
        public static string ShowRuler;
        public static string PrivacyandTools;
        public static string PrivacyandToolsDesc;
        public static string DevMode;
        public static string ShowNews;
        public static string ResetSettings;
        public static string SaveRestart;
        public static string InsiderTitle;
        public static string InsiderContent;

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
                AppTitle = "Ivirius Text Editor";
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
                // New strings. They may still have to be localized.
                // (remove this once the strings below have been localized into all languages)
                PasteRTF = "Paste rich text";
                PasteSimple = "Paste simple";
                Clipboard = "Clipboard";
                Font = "Font";
                Default = "Default";
                Bold = "Bold (Ctrl+B)";
                Italic = "Italic (Ctrl+I)";
                UnderlineDefault = "Underline (Ctrl+U)";
                UnderlineNone = "None";
                UnderlineSingle = "Single";
                UnderlineDash = "Dash";
                UnderlineDotted = "Dotted";
                UnderlineDouble = "Double";
                UnderlineThick = "Thick";
                UnderlineWave = "Wave";
                Strikethrough = "Strikethrough (Ctrl+T)";
                Subscript = "Subscript";
                Superscript = "Superscript";
                FontColor = "Font color";
                Advanced = "Advanced";
                Highlight = "Text highlight";
                HighlightColor = "Highlight color";
                Casing = "Casing/Caps";
                Lowercase = "lower case";
                Uppercase = "UPPER CASE";
                Sentencecase = "Sentence case";
                Titlecase = "Title Case";
                EraseFormatting = "Erase formatting";
                NormalTextStyle = "Normal\ncontent";
                TitleTextStyle = "Title";
                ContentTextStyle = "Content";
                HeaderTextStyle = "Header";
                Title2TextStyle = "Title 2";
                MediumTextStyle = "Medium";
                FinishedTextStyle = "Finished";
                SubtitleTextStyle = "Subtitle";
                UnfinishedTextStyle = "Unfinished";
                BookTitleTextStyle = "Book\nTitle";
                StrongTextStyle = "Strong";
                QuoteTextStyle = "Quote";
                ShowAll = "Show all";
                List = "List";
                Bullet = "Bullets";
                Numbers = "Numbers";
                UppercaseList = "Uppercase letters";
                LowercaseList = "Lowercase letters";
                Roman = "Roman numerals";
                ParagraphOptions = "Paragraph options";
                ParagraphSpacing = "Paragraph spacing\nDefault: 1.00";
                s1pt = "1 point";
                s115pt = "1.15 points";
                s150pt = "1.5 points";
                s2pt = "2 points";
                Left = "Left (Ctrl+L)";
                Right = "Right (Ctrl+R)";
                Center = "Center (Ctrl+E)";
                Justify = "Justify (Ctrl+J)";
                SelectAll = "Select All";
                SelectAllTooltip = "Select All (Ctrl+A)";
                ImageInsert = "Insert\nImage";
                LinkInsert = "Insert\nLink";
                DateInsert = "Date &\nTime";
                DefaultTimeFormat = "This PC's default time format";
                Delete = "Delete";
                NewFile = "New file";
                OpenFile = "Open file";
                OpenMultiple = "Open multiple files";
                SaveMenu = "Save";
                SaveAsMenu = "Save As";
                Changelog = "Changelog";
                ViewModes = "View modes";
                TabletMode = "Tablet writing mode";
                FullScreen = "Toggle full screen (F11)";
                NewTab = "New tab";
                InsiderTitle = "Dev Channel";
                InsiderContent = "The Dev Channel is the Insider CHannel that receives builds with the latest features and may contain features that may not get released to the public. Beware, as these builds may be buggy or crash!";
                // Settings
                // Contains all the strings used in the Settings page
                Language = "Language";
                LanguageDesc = "Preferred language of the app";
                Theme = "Theme";
                ThemeDesc = "Set the desired theme for the app";
                AccentOnTitleBar = "Show accent color on the title bar";
                AccentOnTitleBarDesc = "Toggle the accent color on the title bar";
                win32background = "Solid color as the document's background";
                win32backDesc = "Get a legacy Win32 feel with a solid background on the workspace";
                QuickAccess = "Quick access pins";
                QuickAccessDesc = "Choose your favorite items to be shown in the quick access toolbar";
                CutSettings = "Cut";
                CopySettings = "Copy";
                PasteSettings = "Paste";
                DeleteSettings = "Delete";
                Accessibility = "Accessibility";
                AccessibilityDesc = "All means of accessibility";
                ShowStatusBar = "Show status bar";
                HomePageStartup = "Enable home page on startup";
                ShowRuler = "Show ruler";
                PrivacyandTools = "Privacy and Tools";
                PrivacyandToolsDesc = "Settings related to general privacy and experiments";
                DevMode = "Developer Mode";
                ShowNews = "Show news";
                ResetSettings = "Reset app settings";
                SaveRestart = "Save and Restart";
            }
            if (SettingsHelper.GetSettingString("Language") == "ro-ro")
            {
                AppTitle = "Ivirius Text Editor";
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
                // New strings. They may still have to be localized.
                // (remove this once the strings below have been localized into all languages)
                PasteRTF = "Paste rich text";
                PasteSimple = "Paste simple";
                Clipboard = "Clipboard";
                Font = "Font";
                Default = "Default";
                Bold = "Bold (Ctrl+B)";
                Italic = "Italic (Ctrl+I)";
                UnderlineDefault = "Underline (Ctrl+U)";
                UnderlineNone = "None";
                UnderlineSingle = "Single";
                UnderlineDash = "Dash";
                UnderlineDotted = "Dotted";
                UnderlineDouble = "Double";
                UnderlineThick = "Thick";
                UnderlineWave = "Wave";
                Strikethrough = "Strikethrough (Ctrl+T)";
                Subscript = "Subscript";
                Superscript = "Superscript";
                FontColor = "Font color";
                Advanced = "Advanced";
                Highlight = "Text highlight";
                HighlightColor = "Highlight color";
                Casing = "Casing/Caps";
                Lowercase = "lower case";
                Uppercase = "UPPER CASE";
                Sentencecase = "Sentence case";
                Titlecase = "Title Case";
                EraseFormatting = "Erase formatting";
                NormalTextStyle = "Normal\ncontent";
                TitleTextStyle = "Title";
                ContentTextStyle = "Content";
                HeaderTextStyle = "Header";
                Title2TextStyle = "Title 2";
                MediumTextStyle = "Medium";
                FinishedTextStyle = "Finished";
                SubtitleTextStyle = "Subtitle";
                UnfinishedTextStyle = "Unfinished";
                BookTitleTextStyle = "Book\nTitle";
                StrongTextStyle = "Strong";
                QuoteTextStyle = "Quote";
                ShowAll = "Show all";
                List = "List";
                Bullet = "Bullets";
                Numbers = "Numbers";
                UppercaseList = "Uppercase letters";
                LowercaseList = "Lowercase letters";
                Roman = "Roman numerals";
                ParagraphOptions = "Paragraph options";
                ParagraphSpacing = "Paragraph spacing\nDefault: 1.00";
                s1pt = "1 point";
                s115pt = "1.15 points";
                s150pt = "1.5 points";
                s2pt = "2 points";
                Left = "Left (Ctrl+L)";
                Right = "Right (Ctrl+R)";
                Center = "Center (Ctrl+E)";
                Justify = "Justify (Ctrl+J)";
                SelectAll = "Select All";
                SelectAllTooltip = "Select All (Ctrl+A)";
                ImageInsert = "Insert\nImage";
                LinkInsert = "Insert\nLink";
                DateInsert = "Date &\nTime";
                DefaultTimeFormat = "This PC's default time format";
                Delete = "Delete";
                NewFile = "New file";
                OpenFile = "Open file";
                OpenMultiple = "Open multiple files";
                SaveMenu = "Save";
                SaveAsMenu = "Save As";
                Changelog = "Changelog";
                ViewModes = "View modes";
                TabletMode = "Tablet writing mode";
                FullScreen = "Toggle full screen (F11)";
                NewTab = "New tab";
            }
        }
    }
}
