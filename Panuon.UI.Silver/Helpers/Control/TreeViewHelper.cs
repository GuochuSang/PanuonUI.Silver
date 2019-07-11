﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Linq;

namespace Panuon.UI.Silver
{
    public class TreeViewHelper
    {
        static TreeViewHelper()
        {
            EventManager.RegisterClassHandler(typeof(TreeView), TreeView.SelectedItemChangedEvent, new RoutedEventHandler(TreeView_SelectedItemChanged));
            EventManager.RegisterClassHandler(typeof(TreeView), TreeViewItem.SelectedEvent, new RoutedEventHandler(TreeViewItem_Selected));
        }

        #region TreeViewStyle
        public static TreeViewStyle GetTreeViewStyle(DependencyObject obj)
        {
            return (TreeViewStyle)obj.GetValue(TreeViewStyleProperty);
        }

        public static void SetTreeViewStyle(DependencyObject obj, TreeViewStyle value)
        {
            obj.SetValue(TreeViewStyleProperty, value);
        }

        public static readonly DependencyProperty TreeViewStyleProperty =
            DependencyProperty.RegisterAttached("TreeViewStyle", typeof(TreeViewStyle), typeof(TreeViewHelper));
        #endregion

        #region SelectMode
        public static SelectMode GetSelectMode(DependencyObject obj)
        {
            return (SelectMode)obj.GetValue(SelectModeProperty);
        }

        public static void SetSelectMode(DependencyObject obj, SelectMode value)
        {
            obj.SetValue(SelectModeProperty, value);
        }

        public static readonly DependencyProperty SelectModeProperty =
            DependencyProperty.RegisterAttached("SelectMode", typeof(SelectMode), typeof(TreeViewHelper), new PropertyMetadata(SelectMode.Any));
        #endregion

        #region ExpandMode
        public static ExpandMode GetExpandMode(DependencyObject obj)
        {
            return (ExpandMode)obj.GetValue(ExpandModeProperty);
        }

        public static void SetExpandMode(DependencyObject obj, ExpandMode value)
        {
            obj.SetValue(ExpandModeProperty, value);
        }

        public static readonly DependencyProperty ExpandModeProperty =
            DependencyProperty.RegisterAttached("ExpandMode", typeof(ExpandMode), typeof(TreeViewHelper), new PropertyMetadata(ExpandMode.DoubleClick, OnExpandModeChanged));

        private static void OnExpandModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as TreeView;

            treeView.RemoveHandler(TreeViewItem.PreviewMouseDownEvent, new RoutedEventHandler(OnExpandModeItemSelected));

            if ((ExpandMode)e.NewValue == ExpandMode.SingleClick)
            {
                treeView.AddHandler(TreeViewItem.PreviewMouseDownEvent, new RoutedEventHandler(OnExpandModeItemSelected));
            }
        }

        private static void OnExpandModeItemSelected(object sender, RoutedEventArgs e)
        {
            var treeView = sender as TreeView;
            if (e.Source is TreeViewItem)
            {

                var treeViewItem = e.Source as TreeViewItem;
                if (treeViewItem.HasItems)
                {
                    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
                }
            }
        }


        #endregion

        #region ExpandBehaviour
        public static ExpandBehaviour GetExpandBehaviour(DependencyObject obj)
        {
            return (ExpandBehaviour)obj.GetValue(ExpandBehaviourProperty);
        }

        public static void SetExpandBehaviour(DependencyObject obj, ExpandBehaviour value)
        {
            obj.SetValue(ExpandBehaviourProperty, value);
        }

        public static readonly DependencyProperty ExpandBehaviourProperty =
            DependencyProperty.RegisterAttached("ExpandBehaviour", typeof(ExpandBehaviour), typeof(TreeViewHelper), new PropertyMetadata(OnExpandBehaviourChanged));

        private static void OnExpandBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as TreeView;

            treeView.RemoveHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(OnExpandBehaviourItemSelected));

            if ((ExpandBehaviour)e.NewValue == ExpandBehaviour.OnlyOne)
            {
                treeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(OnExpandBehaviourItemSelected));
            }
        }

        private static void OnExpandBehaviourItemSelected(object sender, RoutedEventArgs e)
        {
            var treeView = sender as TreeView;
            if (e.OriginalSource is TreeViewItem)
            {
                var treeViewItem = e.OriginalSource as TreeViewItem;

                if (treeViewItem.HasItems)
                {
                    var lastTreeViewItem = GetLastExpandedItem(treeView);
                    if (lastTreeViewItem != null)
                    {
                        if (lastTreeViewItem == treeViewItem)
                            return;
                        lastTreeViewItem.IsExpanded = false;
                    }

                    SetLastExpandedItem(treeView, treeViewItem);
                }
            }
        }

        #endregion

        #region HoverBrush
        public static Brush GetExpandedBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ExpandedBrushProperty);
        }

        public static void SetExpandedBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(ExpandedBrushProperty, value);
        }

        public static readonly DependencyProperty ExpandedBrushProperty =
            DependencyProperty.RegisterAttached("ExpandedBrush", typeof(Brush), typeof(TreeViewHelper));
        #endregion

        #region SelectedBrush
        public static Brush GetSelectedBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SelectedBrushProperty);
        }

        public static void SetSelectedBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectedBrushProperty, value);
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.RegisterAttached("SelectedBrush", typeof(Brush), typeof(TreeViewHelper));
        #endregion

        #region SelectedForeground
        public static Brush GetSelectedForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SelectedForegroundProperty);
        }

        public static void SetSelectedForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectedForegroundProperty, value);
        }

        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.RegisterAttached("SelectedForeground", typeof(Brush), typeof(TreeViewHelper));
        #endregion

        #region ItemHeight
        public static double GetItemHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(ItemHeightProperty);
        }

        public static void SetItemHeight(DependencyObject obj, double value)
        {
            obj.SetValue(ItemHeightProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(TreeViewHelper));
        #endregion

        #region ItemIcon
        public static object GetItemIcon(DependencyObject obj)
        {
            return (object)obj.GetValue(ItemIconProperty);
        }

        public static void SetItemIcon(DependencyObject obj, object value)
        {
            obj.SetValue(ItemIconProperty, value);
        }

        public static readonly DependencyProperty ItemIconProperty =
            DependencyProperty.RegisterAttached("ItemIcon", typeof(object), typeof(TreeViewHelper));
        #endregion

        #region (Internal) LastSelectedItem
        internal static TreeViewItem GetLastSelectedItem(DependencyObject obj)
        {
            return (TreeViewItem)obj.GetValue(LastSelecteedItemProperty);
        }

        internal static void SetLastSelecteedItem(DependencyObject obj, TreeViewItem value)
        {
            obj.SetValue(LastSelecteedItemProperty, value);
        }

        internal static readonly DependencyProperty LastSelecteedItemProperty =
            DependencyProperty.RegisterAttached("LastSelecteedItem", typeof(TreeViewItem), typeof(TreeViewHelper));
        #endregion

        #region (Internal) LastExpandedItem
        internal static TreeViewItem GetLastExpandedItem(DependencyObject obj)
        {
            return (TreeViewItem)obj.GetValue(LastExpandedItemProperty);
        }

        internal static void SetLastExpandedItem(DependencyObject obj, TreeViewItem value)
        {
            obj.SetValue(LastExpandedItemProperty, value);
        }

        internal static readonly DependencyProperty LastExpandedItemProperty =
            DependencyProperty.RegisterAttached("LastExpandedItem", typeof(TreeViewItem), typeof(TreeViewHelper));
        #endregion

        #region (Internal) TreeViewHook
        private static void TreeView_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var treeView = sender as TreeView;
                if (GetSelectMode(treeView) != SelectMode.ChildOnly)
                    return;

                var sourceData = treeView.SelectedItem;
                if (sourceData is TreeViewItem)
                {
                    if (((TreeViewItem)sourceData).HasItems)
                        e.Handled = true;
                }
                else
                {
                    if (!(treeView.ItemTemplate is HierarchicalDataTemplate))
                        return;
                    var itemsPath = ((Binding)((HierarchicalDataTemplate)treeView.ItemTemplate)?.ItemsSource)?.Path?.Path;
                    if (string.IsNullOrEmpty(itemsPath))
                        return;

                    var propertyInfo = sourceData.GetType().GetProperty(itemsPath);
                    if (propertyInfo == null)
                        return;

                    var children = propertyInfo.GetValue(sourceData, null) as ICollection;
                    if (children == null)
                        return;

                    if (children != null && children.Count != 0)
                        e.Handled = true;
                }
            }
            catch { }
        }

        private static void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                var treeView = sender as TreeView;
                if (GetSelectMode(treeView) != SelectMode.ChildOnly)
                    return;

                if (e.OriginalSource is TreeViewItem)
                {
                    var treeViewItem = e.OriginalSource as TreeViewItem;

                    var oldItem = GetLastSelectedItem(treeView);
                    if (treeViewItem.HasItems)
                    {
                        treeViewItem.IsSelected = false;

                        if (oldItem != null && !oldItem.IsSelected)
                        {
                            SetLastSelecteedItem(treeView, null);
                            oldItem.IsSelected = true;
                        }
                    }
                    else
                    {
                        if (oldItem != null)
                            oldItem.IsSelected = false;
                        SetLastSelecteedItem(treeView, treeViewItem);
                    }
                }
            }
            catch { }
        }

        #endregion

    }
}
