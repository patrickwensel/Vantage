using System;
using System.Windows;
using System.Windows.Media;

namespace Vantage.WPF.Helpers
{
    public static class VisualHelper
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
                return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static T FindChild<T>(DependencyObject child) where T : DependencyObject
        {            
            //get parent item
            try 
            {
                DependencyObject childObject = VisualTreeHelper.GetChild(child, 0);

                //we've reached the end of the tree
                if (childObject == null)
                    return null;

                //check if the parent matches the type we're looking for
                T typedChild = childObject as T;
                if (typedChild != null)
                    return typedChild;
                else
                    return FindChild<T>(childObject);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception : {ex}");
                return null;
            }            
        }
    }
}
