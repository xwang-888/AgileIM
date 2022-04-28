using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgileIM.Client.Common
{
    public class AppColors: ResourceDictionary
    {
        public static AppColors Instance = new();

        private PrimaryColors _primaryColors;
        private PrimaryThemes _primaryThemes;
        private ResourceDictionary _primaryColorsResourceDictionaryTemp;
        private ResourceDictionary _primaryThemesResourceDictionaryTemp;

        public PrimaryColors PrimaryColors
        {
            get => _primaryColors;
            set
            {
                if (_primaryColors != value)
                {

                    _primaryColors = value;
                    if (_primaryColorsResourceDictionaryTemp != null)
                        Application.Current.Resources.MergedDictionaries.Remove(_primaryColorsResourceDictionaryTemp);
                    else if (Application.Current.Resources.MergedDictionaries.FirstOrDefault(x =>
                                 x.Source?.AbsoluteUri?.Contains(@"Themes/Colors/Colors/Colors") == true) is
                             { } res) Application.Current.Resources.MergedDictionaries.Remove(res);
                    _primaryColorsResourceDictionaryTemp = new ResourceDictionary
                    {
                        Source = new Uri(
                            $@"pack://application:,,,/AgileIM.Client;component/Themes/Colors/Colors.{value}.xaml")
                    };
                    Application.Current.Resources.MergedDictionaries.Add(_primaryColorsResourceDictionaryTemp);
                }
            }
        }
        public PrimaryThemes PrimaryThemes
        {
            get => _primaryThemes;
            set
            {
                if (_primaryThemes != value)
                {

                    _primaryThemes = value;
                    if (_primaryThemesResourceDictionaryTemp != null)
                        Application.Current.Resources.MergedDictionaries.Remove(_primaryThemesResourceDictionaryTemp);
                    else if (Application.Current.Resources.MergedDictionaries.FirstOrDefault(x =>
                                 x.Source?.AbsoluteUri?.Contains(@"Themes/Colors/Colors/State/Theme") == true) is
                             { } res) Application.Current.Resources.MergedDictionaries.Remove(res);
                    _primaryThemesResourceDictionaryTemp = new ResourceDictionary
                    {
                        Source = new Uri(
                            $@"pack://application:,,,/AgileIM.Client;component/Themes/Colors/State/Theme.{value}.xaml")
                    };
                    Application.Current.Resources.MergedDictionaries.Add(_primaryThemesResourceDictionaryTemp);
                }
            }
        }

    }

    public enum PrimaryColors
    {
        Cyan,
        DaybreakBlue,
        DustRed,
        Lime,
        SunriseYellow,
        SunsetOrange,
        Volcano,
        CalendulaGold
    }
    public enum PrimaryThemes
    {
        Light,
        Dark
    }
}
