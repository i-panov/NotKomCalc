using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotKomCalc
{
    public partial class MainForm : Form
    {
        private enum MathOperator: int
        {
            PLUS = 1,
            MINUS = 2,
            MUL = 4,
            DIV = 8,
        };

        private MathOperator? currentOperator;

        private string prevNumber;

        public MainForm()
        {
            InitializeComponent();
        }

        private void num_Click(object sender, EventArgs e) => onEnterDigit((sender as Button).Name.Last());

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
                onEnterDigit(e.KeyChar);
            else if (e.KeyChar == '+')
                onMathOperatorEnter(MathOperator.PLUS);
            else if (e.KeyChar == '-')
                onMathOperatorEnter(MathOperator.MINUS);
            else if (e.KeyChar == '*')
                onMathOperatorEnter(MathOperator.MUL);
            else if (e.KeyChar == '/')
                onMathOperatorEnter(MathOperator.DIV);
            else if (e.KeyChar == '.' || e.KeyChar == ',')
                onDecimalSeparatorEnter();
        }

        private void onEnterDigit(char digit)
        {
            if (currentOperator != null)
                result.Text = "0";

            if (result.Text == "0")
            {
                if (digit == '0')
                    return;

                result.Text = digit.ToString();
            }
            else if (result.Text.Length < 10) // чтобы не было переполнения
                result.Text += digit;
        }

        private void mathOperator_Click(object sender, EventArgs e) => onMathOperatorEnter((MathOperator)int.Parse((sender as Button).Tag as string));

        private void onMathOperatorEnter(MathOperator op)
        {
            solve();
            currentOperator = op;
            prevNumber = result.Text;
        }

        private void changeSign_Click(object sender, EventArgs e)
        {
            if (!result.Text.StartsWith("-"))
            {
                if (result.Text != "0")
                    result.Text = '-' + result.Text;
            }
            else
                result.Text = result.Text.Substring(1);
        }

        private readonly string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        private void operatorPoint_Click(object sender, EventArgs e) => onDecimalSeparatorEnter();

        private void onDecimalSeparatorEnter()
        {
            if (!result.Text.Contains(decimalSeparator))
                result.Text += decimalSeparator;
        }

        private void operatorSolve_Click(object sender, EventArgs e) => solve();

        private void clearText_Click(object sender, EventArgs e)
        {
            result.Text = "0";
        }

        private void clearAll_Click(object sender, EventArgs e)
        {
            prevNumber = null;
            currentOperator = null;
            result.Text = "0";
        }

        private void solve()
        {
            if (prevNumber is null || currentOperator is null)
                return;

            var left = double.Parse(prevNumber);
            var right = double.Parse(result.Text);

            if (currentOperator == MathOperator.PLUS)
                result.Text = (left + right).ToString();
            else if (currentOperator == MathOperator.MINUS)
                result.Text = (left - right).ToString();
            else if (currentOperator == MathOperator.MUL)
                result.Text = (left * right).ToString();
            else if (currentOperator == MathOperator.DIV)
                result.Text = (left / right).ToString();
        }
    }
}
