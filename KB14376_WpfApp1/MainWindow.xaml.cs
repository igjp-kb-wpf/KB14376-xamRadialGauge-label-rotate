using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KB14376_WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void xamRadialGauge1_LayoutUpdated(object sender, EventArgs e)
        {
            IEnumerable<TextBlock> list = FindVisualChildren<TextBlock>(xamRadialGauge1);

            foreach (TextBlock tb in list)
            {
                // TextBlock のテキストを整数に変換する（テキストが数値であることが前提）
                int val = Int32.Parse(tb.Text);

                // 回転角度
                double angle = 0;

                if (val == 0 || val == 50 || val == 100)
                {
                    // val が 0, 50, 100 の場合は回転させない
                    angle = 0;
                }
                else if (val >= 10 && val <= 40)
                {
                    // val が 10, 20, 30, 40 の場合は正の方向に18度ずつ大きくなるように回転
                    int multiplier = val / 10; // 1, 2, 3, 4 になる
                    angle = multiplier * 18;
                }
                else if (val >= 60 && val <= 90)
                {
                    // val が 60, 70, 80, 90 の場合は負の方向に18度ずつ小さくなるように回転
                    int multiplier = (100 - val) / 10; // 4, 3, 2, 1 になる
                    angle = -(multiplier * 18);
                }

                // TextBlock の中心点を設定 (回転の基準を中央にする)
                tb.RenderTransformOrigin = new Point(.5, .5);

                // 計算した角度で TextBlock を回転させる
                RotateTransform rotateTransform1 = new RotateTransform(angle);
                tb.RenderTransform = rotateTransform1;
            }
        }

        // このメソッドは、与えられた親要素（dependencyObject）の中にある
        // 指定された型（ここでは TextBlock）の子要素をすべて見つけるための再帰メソッドです。
        // 例えば、Canvas の中にある TextBlock や他の UI 要素を検索するために使います。
        IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            // もし親要素が null の場合は、処理をやめます。
            if (dependencyObject == null)
                yield break;

            // 現在の要素が指定された型 T （ここでは TextBlock）である場合、それを返します。
            if (dependencyObject is T)
                yield return (T)dependencyObject;

            // 親要素に含まれる子要素を再帰的に調べます。
            // 子要素が複数存在する場合でも、すべての子要素を調べていきます。
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }
    }
}
