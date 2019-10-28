using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AX.WPF.Controls
{
    [TemplatePart(Name = "PART_TreeView", Type = typeof(TreeView))]
    public class FilterTreeSelector : ItemsControl
    {
        static FilterTreeSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterTreeSelector), new FrameworkPropertyMetadata(typeof(FilterTreeSelector)));
        }

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(FilterTreeSelector), new PropertyMetadata("", FilterTextPropertyChangedCallback));

        public Func<object, IEnumerable<string>> TreeConverter
        {
            get { return (Func<object, IEnumerable<string>>)GetValue(TreeConverterProperty); }
            set { SetValue(TreeConverterProperty, value); }
        }

        public static readonly DependencyProperty TreeConverterProperty =
            DependencyProperty.Register("TreeConverter", typeof(Func<object, IEnumerable<string>>), typeof(FilterTreeSelector), new PropertyMetadata(null));

        public Func<object, string, bool> Filter
        {
            get { return (Func<object, string, bool>)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(Func<object, string, bool>), typeof(FilterTreeSelector), new PropertyMetadata(null));

        public List<TreeViewItem> TreeViewItems
        {
            get { return (List<TreeViewItem>)GetValue(TreeViewItemsProperty); }
            set { SetValue(TreeViewItemsProperty, value); }
        }

        public static readonly DependencyProperty TreeViewItemsProperty =
            DependencyProperty.Register("TreeViewItems", typeof(List<TreeViewItem>), typeof(FilterTreeSelector), new PropertyMetadata(null));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            var treeConverter = TreeConverter;            
            if (treeConverter != null)
            {
                List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
                foreach (var item in Items)
                {
                    var treePath = treeConverter(item);
                    TreeViewItem lastLevelItem = null;
                    foreach (var level in treePath)
                    {
                        if (lastLevelItem != null)
                        {
                            var newLevelItem = lastLevelItem.Items.OfType<TreeViewItem>().FirstOrDefault(i => (i.Header as string) == level);
                            if (newLevelItem != null)
                            {
                                lastLevelItem = newLevelItem;
                            }
                            else
                            {
                                lastLevelItem.Items.Add(new TreeViewItem() { Header = level });
                            }
                        }
                        else
                        {
                            var newLevelItem = treeViewItems.FirstOrDefault(i => (i.Header as string) == level);
                            if (newLevelItem != null)
                            {
                                lastLevelItem = newLevelItem;
                            }
                            else
                            {
                                treeViewItems.Add(lastLevelItem = new TreeViewItem() { Header = level });
                            }
                        }
                    }
                    var actualItem = new TreeViewItem() { Header = item };
                    lastLevelItem.Items.Add(actualItem);
                    
                }
                TreeViewItems = treeViewItems;
            }
            else
            {
                TreeViewItems = Items.Cast<object>().Select(x => new TreeViewItem() { Header = x }).ToList();
            }
            
        }

        private static void FilterTextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FilterTreeSelector)d).OnFilterTextChanged();
        }

        private void OnFilterTextChanged()
        {
            if (Filter != null && FilterText != null && FilterText.Length > 0)
            {               
                CheckSetVisibility(TreeViewItems);
            }
            else
            {
                SetAllVisible(TreeViewItems);
            }

        }

        private void CheckSetVisibility(IEnumerable<TreeViewItem> treeViewItems)
        {
            foreach (var item in treeViewItems)
            {
                if (item.Items != null && item.Items.Count > 0)
                {
                    var children = item.Items.OfType<TreeViewItem>();
                    CheckSetVisibility(children);
                    if (children.Any(c=>c.Visibility == Visibility.Visible))
                    {
                        item.Visibility = Visibility.Visible;
                        item.IsExpanded = true;
                    }
                    else
                    {
                        item.Visibility = Visibility.Collapsed;
                        item.IsExpanded = false;
                    }
                }
                else
                {
                    item.Visibility = Filter(item.Header, FilterText) ? Visibility.Visible : Visibility.Collapsed;
                }

            }
        }

        private void SetAllVisible(IEnumerable<TreeViewItem> treeViewItems)
        {
            foreach (var item in treeViewItems)
            {
                item.Visibility = Visibility.Visible;
                SetAllVisible(item.Items.OfType<TreeViewItem>());
                item.IsExpanded = false;
            }
        }
    }
}
