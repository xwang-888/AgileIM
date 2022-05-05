using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using AgileIM.Client.Models;

namespace AgileIM.Client.Controls
{
    [TemplatePart(Name = PARK_ArrowToggleButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PARK_ListView, Type = typeof(ListView))]
    public class AccountBox : Selector
    {


        private const string PARK_ArrowToggleButton = "ToggleButton";
        private const string PARK_ListView = "ListView";
        private ToggleButton ArrowToggleButton;
        private ListView ListView;

        public static readonly DependencyProperty AccountProperty = DependencyProperty.Register(
            "Account", typeof(string), typeof(AccountBox), new PropertyMetadata(default(string)));

        public string Account
        {
            get => (string)GetValue(AccountProperty);
            set => SetValue(AccountProperty, value);
        }

        public override void OnApplyTemplate()
        {
            ArrowToggleButton = GetTemplateChild(PARK_ArrowToggleButton) as ToggleButton;
            ListView = GetTemplateChild(PARK_ListView) as ListView;
            ArrowToggleButton.Click += ArrowToggleButton_Click;
            ListView.SelectionChanged += ListView_SelectionChanged;
            base.OnApplyTemplate();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count is 1)
            {
                SelectedItem = e.AddedItems[0];
                ListView.SelectedItem = e.AddedItems[0];
            }

            ArrowToggleButton.IsChecked = false;
            isOpen = false;
        }

        private bool isOpen = false;
        private void ArrowToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (isOpen)
            {
                ArrowToggleButton.IsChecked = false;
                isOpen = false;
            }
            else
                isOpen = true;

        }
    }
}
