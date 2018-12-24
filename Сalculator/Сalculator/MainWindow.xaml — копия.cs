using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using NCalc;


namespace Сalculator {

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();

            Field.Text = "";
            Answer.Text = "";
        }

        NCalc.Expression expression;
        string expressionText;

        private string LastSymbol {
            get {
                if (Field.Text.Length > 0)
                    return Field.Text[Field.Text.Length - 1].ToString();
                else return "";
            }
        }
        
        public void ClickNumericalButton(object sender, EventArgs e) {
            string num = ((TextBlock)((Grid)sender).Children[1]).Text;

            if (Field.Text == "0")
                Field.Text = num;
            else Field.Text += num;

            Calculate();
        }
        public void ClickOperatorButton(object sender, EventArgs e) {
            string oper = ((TextBlock)((Grid)sender).Children[1]).Text;

            string lastSymbol;
            if (Field.Text.Length != 0) {
                if (Field.Text.Length > 1) {
                    lastSymbol = Field.Text[Field.Text.Length - 1].ToString();
                    if (lastSymbol != "+" &&
                        lastSymbol != "-" &&
                        lastSymbol != "*" &&
                        lastSymbol != "/")
                        {
                        Field.Text += oper;
                    } else {
                        Field.Text = Field.Text.Substring(0, Field.Text.Length - 1) + oper;
                    }  
                } else {
                    Field.Text += oper;
                }
            }
        }

        public void ClickParenthesRight(object sender, EventArgs e) {
            int tmp;
            if (int.TryParse(LastSymbol, out tmp))
                Field.Text += "*(";
            else Field.Text += "(";
        }
        public void ClickParenthesLeft(object sender, EventArgs e) {
            Field.Text += ")";

            Calculate();
        }
       
        public void ClickEqualButton(object sender, EventArgs e) {
            int amount = 0;
            for (int i = 0; i < Field.Text.Length; i++) {
                if (Field.Text[i] == '(')
                    amount++;
                else if (Field.Text[i] == ')')
                    amount--;
            }

            for (int i = 0; i < amount; i++) {
                Field.Text += ")";
            }

            Calculate();
            try {
                expression = new NCalc.Expression(Answer.Text);

                expression.Parameters["π"] = 3.14;
                expression.Parameters["e"] = 2.718281828459045;

                Field.Text = expression.Evaluate().ToString();
            } finally {
                
            }
        }
        public void ClickCButton(object sender, EventArgs e) {
            Field.Text = Answer.Text = "";
        }
        public void ClickDeleteButton(object sender, EventArgs e) {
            if (Field.Text != "") {
                Field.Text = Field.Text.Substring(0, Field.Text.Length - 1);
            }

            Calculate();
        }

        private void Calculate() {
            try {
                string tmp = Field.Text;
                int amount = 0;
                for (int i = 0; i < tmp.Length; i++) {
                    if (tmp[i] == '(')
                        amount++;
                    else if (tmp[i] == ')')
                        amount--;
                }

                for (int i = 0; i < amount; i++) {
                    tmp += ")";
                }

                expression = new NCalc.Expression(tmp);

                expression.Parameters["π"] = 3.14;
                //expression.Parameters["e"] = 2.71828;

                Answer.Text = expression.Evaluate().ToString();
            }
            catch {
                Answer.Text = "";
            }
        }

        //========================================

        private void ClickE(object sender, EventArgs e) {
            int tmp;
            if (int.TryParse(LastSymbol, out tmp))
                Field.Text += "*e)";
            else Field.Text += "e";

            Calculate();
        }

        private void ClickPi(object sender, EventArgs e) {
            int tmp;
            if (int.TryParse(LastSymbol, out tmp))
                Field.Text += "*[π]";
            else Field.Text += "[π]";

            Calculate();
        }

        private float coefficentMyltiply = 0.9f;
        public void MouseEnterEvn(object sender, EventArgs e) {
            Rectangle obj = ((Rectangle)((Grid)sender).Children[0]);
            Color startColor = (Color)obj.Fill.GetValue(SolidColorBrush.ColorProperty);

            SolidColorBrush colorBrush = new SolidColorBrush(Color.Multiply(startColor, coefficentMyltiply));

            obj.Fill = colorBrush;
        }
        public void MouseLeaveEvn(object sender, EventArgs e) {
            Rectangle obj = ((Rectangle)((Grid)sender).Children[0]);
            Color startColor = (Color)obj.Fill.GetValue(SolidColorBrush.ColorProperty);
            
            SolidColorBrush colorBrush = new SolidColorBrush(Color.Multiply(startColor, 1 / coefficentMyltiply));

            obj.Fill = colorBrush;
        }
    }

}
