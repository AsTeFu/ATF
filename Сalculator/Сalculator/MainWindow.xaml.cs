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

            eText = "";
            Field.Text = "";
            Answer.Text = "";

            operators.AddRange(new string[] { "+", "-", "/", "*", "."});
        }

        NCalc.Expression expression;
        string eText;

        private string LastSymbol {
            get {
                if (eText.Length > 0)
                    return eText[eText.Length - 1].ToString();
                else return "";
            }
        }

        List<string> operators = new List<string>();
        
        
        public void ClickNumericalButton(object sender, EventArgs e) {
            isChengeOp = false;
            int tmp;

            if (LastSymbol != "" && !operators.Contains(LastSymbol) && !int.TryParse(LastSymbol, out tmp)) {
                eText += "*";
                Field.Text += "*";
            }

            string num = ((TextBlock)((Grid)sender).Children[1]).Text;

            if (eText == "0") {
                Field.Text = num;
                eText = num;
            }
            else {
                Field.Text += num;
                eText += num;
            }

            Calculate();
        }
        public void ClickOperatorButton(object sender, EventArgs e) {
            isChengeOp = false;
            string oper = ((TextBlock)((Grid)sender).Children[1]).Text;

            string lastSymbol;
            if (eText.Length != 0) {
                if (eText.Length > 1) {
                    lastSymbol = eText[eText.Length - 1].ToString();
                    if (lastSymbol != "+" &&
                        lastSymbol != "-" &&
                        lastSymbol != "*" &&
                        lastSymbol != "/")
                        {
                        Field.Text += oper;
                        eText += oper;
                    } else {
                        Field.Text = Field.Text.Substring(0, Field.Text.Length - 1) + oper;
                        eText = eText.Substring(0, eText.Length - 1) + oper;
                    }  
                } else {
                    Field.Text += oper;
                    eText += oper;
                }
            }
        }

        public void ClickParenthesRight(object sender, EventArgs e) {
            isChengeOp = false;
            int tmp;
            if (int.TryParse(LastSymbol, out tmp)) {
                Field.Text += "*(";
                eText += "*(";
            }
            else {
                Field.Text += "(";
                eText += "(";
            }
        }
        public void ClickParenthesLeft(object sender, EventArgs e) {

            isChengeOp = false; Field.Text += ")";
            eText += ")";

            Calculate();
        }
       
        public void ClickEqualButton(object sender, EventArgs e) {
            isChengeOp = false;
            int amount = 0;
            for (int i = 0; i < eText.Length; i++) {
                if (eText[i] == '(')
                    amount++;
                else if (eText[i] == ')')
                    amount--;
            }

            for (int i = 0; i < amount; i++) {
                eText += ")";
                Field.Text += ")";
            }

            Calculate();
            try {
                expression = new NCalc.Expression(eText);

                expression.Parameters["p"] = 3.14;
                expression.Parameters["e"] = 2.71828;

                Field.Text = expression.Evaluate().ToString().Replace(",", ".");
                eText = expression.Evaluate().ToString().Replace(",", ".");
            } catch {
                
            }
        }
        public void ClickCButton(object sender, EventArgs e) {
            Field.Text = Answer.Text = eText = "";
            isChengeOp = false;
        }
        public void ClickDeleteButton(object sender, EventArgs e) {
            isChengeOp = false;
            if (Field.Text != "") {
                Field.Text = Field.Text.Substring(0, Field.Text.Length - 1);

                if (LastSymbol == "]") {
                    eText = eText.Substring(0, eText.Length - 3);
                } else eText = eText.Substring(0, eText.Length - 1);
            }

            Calculate();
        }

        public void ClickPointButton(object sender, EventArgs e) {
            int tmp;
            if (eText != "" && !eText.Contains(".") && int.TryParse(LastSymbol, out tmp)) {
                Field.Text += ".";
                eText += ".";
            }
        }

        private bool isChengeOp;
        public void ClickChengeOperator(object sender, EventArgs e) {
            if (!isChengeOp) {
                Field.Text = "-(" + Field.Text + ")";
                eText = "-(" + eText + ")";
            } else {
                Field.Text = Field.Text.Substring(2, Field.Text.Length - 3);
                eText = eText.Substring(2, eText.Length - 3);
            }

            isChengeOp = !isChengeOp;
            Calculate();
        }

        private void Calculate() {
            try {
                string tmp = eText;

                if (LastSymbol == ".")
                    tmp += "0";

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

                expression.Parameters["p"] = 3.14;
                expression.Parameters["e"] = 2.71828;

                Answer.Text = expression.Evaluate().ToString().Replace(",", ".");
            }
            catch {
                Answer.Text = "";
            }
        }

        //========================================

        private void ClickE(object sender, EventArgs e) {
            int tmp;
            if (int.TryParse(LastSymbol, out tmp)) {
                Field.Text += "*e";
                eText += "*[e]";
            }
            else {
                Field.Text += "e";
                eText += "[e]";
            }

            Calculate();
        }
        private void ClickPi(object sender, EventArgs e) {
            int tmp;
            if (int.TryParse(LastSymbol, out tmp)) {
                Field.Text += "*π";
                eText += "*[p]";
            }
            else {
                Field.Text += "π";
                eText += "[p]";
            }

            Calculate();
        }

        private void CLickFunc(object sender, EventArgs e) {
            string func = ((TextBlock)((Grid)sender).Children[1]).Text;
            if (OneOperand()) {
                if (LastSymbol != "") {
                    Field.Text = func + "(" + Field.Text + ")";
                    eText = func + "(" + eText + ")";
                } else {
                    Field.Text = func + "(" + Field.Text;
                    eText = func + "(" + eText;
                }
            } else {
                if (LastSymbol == "") {
                    Field.Text = func + "(";
                    eText = func + "(";
                } else {
                    Field.Text = "*" + func + "(";
                    eText = "*" + func + "(";
                }
            }

            Calculate();
        }

        private bool OneOperand() {
            for (int i = 0; i < 4; i++) {
                if (eText.Contains(operators[i]))
                    return false;
            }
            return true;
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
