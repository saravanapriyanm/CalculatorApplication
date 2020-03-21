using System;
using System.Text.RegularExpressions;

namespace CalculatorApp
{
    class CalculatorApplication
    {
        static float[] valueStack = new float[100];
        static char[] operatorStack = new char[100];

        static int topValueIndex = -1;
        static int topOperatorIndex = 0;

        static bool ValueStackPush(float inputvalue)
        {
            valueStack[++topValueIndex] = inputvalue;
            return true;
        }
        static float ValueStackPop()
        {
            float poppedValue = valueStack[topValueIndex--];
            return poppedValue;
        }
        static bool OperatorStackPush(char inputOperator)
        {
            operatorStack[++topOperatorIndex] = inputOperator;
            return true;
        }
        static char OperatorStackPop()
        {
            char poppedOperator = operatorStack[topOperatorIndex--];
            return poppedOperator;
        }
        static char OpertorStackPeek()
        {
            return operatorStack[topOperatorIndex];
        }
        public string EvaluateExpression()
        {
            char[] tokens = null;

            Console.WriteLine("Enter the Expression to Evaluate");

            string inputExpression = Console.ReadLine();

            inputExpression = inputExpression.Trim();

            if (CheckValidInputOrNot(inputExpression))
            {
                tokens = inputExpression.ToCharArray();

                for (int i = 0; i < tokens.Length; i++)
                {
                    switch (tokens[i])
                    {
                        case ' ':
                            continue;

                        case '(':
                            OperatorStackPush(tokens[i]);
                            break;

                        case '+':
                        case '-':
                        case '*':
                        case '/':

                            while (topOperatorIndex > 0 && CheckOperatorPrecedence(tokens[i], OpertorStackPeek()))
                            {
                                ValueStackPush(PerformOperation(OperatorStackPop(), ValueStackPop(), ValueStackPop()));
                            }
                            OperatorStackPush(tokens[i]);
                            break;

                        case ')':

                            while (OpertorStackPeek() != '(')
                            {
                                ValueStackPush(PerformOperation(OperatorStackPop(), ValueStackPop(), ValueStackPop()));
                            }
                            OperatorStackPop();
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '.':

                            string numberstring = "";

                            while ((i < tokens.Length && tokens[i] >= '0' && tokens[i] <= '9') || (i < tokens.Length && tokens[i] == '.'))
                            {
                                numberstring += tokens[i++];
                            }
                            i--;
                            ValueStackPush(float.Parse(numberstring));
                            break;
                    }


                }
                while (topOperatorIndex > 0)
                {
                    ValueStackPush(PerformOperation(OperatorStackPop(), ValueStackPop(), ValueStackPop()));
                }
                string output = Convert.ToString(ValueStackPop());
                return output;
            }
            else
            {
                return "Expression Not Match";
            }
        }
        static bool CheckOperatorPrecedence(char firstOperator, char secondOperator)
        {
            if (secondOperator == '(' || secondOperator == ')')
            {
                return false;
            }
            if ((firstOperator == '*' || firstOperator == '/') && (secondOperator == '+' || secondOperator == '-'))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        static float PerformOperation(char operatorSymbol, float secondOperand, float firstOperand)
        {
            switch (operatorSymbol)
            {
                case '+':
                    return firstOperand + secondOperand;
                case '-':
                    return firstOperand - secondOperand;
                case '*':
                    return firstOperand * secondOperand;
                case '/':
                    return firstOperand / secondOperand;
            }
            return 0;
        }
        public static bool CheckValidInputOrNot(string inputExpression)
        {

            Regex rgx = new Regex(@"^[\d\(\+\-. ]+[\d\+\-\*\/\(\). ]+[\d\) ]+$");

            if (rgx.IsMatch(inputExpression))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static void Main(string[] args)
        {
            while (true)
            {
                CalculatorApplication calc = new CalculatorApplication();

                Console.WriteLine(calc.EvaluateExpression());

                Console.WriteLine();
            }
        }
    }
}


