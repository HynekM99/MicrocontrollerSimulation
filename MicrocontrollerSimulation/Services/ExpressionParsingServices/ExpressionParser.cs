using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices
{
    public class ExpressionParser : IExpressionParser
    {
        public const char AND_SYMBOL = '*';
        public const char OR_SYMBOL = '+';
        public const char XOR_SYMBOL = '⊕';
        public const char NOT_SYMBOL = '¬';

        public static readonly char[] OPERATOR_SYMBOLS = {AND_SYMBOL, OR_SYMBOL, XOR_SYMBOL, NOT_SYMBOL};
        public static readonly char[] ALLOWED_SPECIAL_SYMBOLS = { ' ', '_', '(', ')' };

        public LogicalExpression Parse(string? expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            expression = expression.Trim();
            if (!AllSymbolsAllowed(expression))
            {
                throw new ArgumentException("Expression contains a forbidden symbol.");
            }

            if (expression.Count(c => c == '(') != expression.Count(c => c == ')'))
            {
                throw new ArgumentException("Bracket error.");
            }

            return RealParse(expression, new HashSet<Input>());
        }

        public LogicalExpression RealParse(string expression, HashSet<Input> inputs)
        {
            expression = expression.Trim();

            List<Tuple<string, LogicalExpression>> currentLevelParsedExpressions = new();

            Queue<int> leftBracketIndexes = new();
            Stack<int> rightBracketIndexes = new();

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (c == '(')
                {
                    leftBracketIndexes.Enqueue(i);
                }
                else if (c == ')')
                {
                    if (!leftBracketIndexes.Any())
                    {
                        throw new ArgumentException("Bracket error.");
                    }

                    rightBracketIndexes.Push(i);

                    if (leftBracketIndexes.Count == rightBracketIndexes.Count)
                    {
                        int leftBracket = leftBracketIndexes.Dequeue();
                        int rightBracket = rightBracketIndexes.Pop();

                        leftBracketIndexes.Clear();
                        rightBracketIndexes.Clear();

                        string bracketedExpression = expression.Substring(leftBracket + 1, rightBracket - leftBracket - 1);

                        if (string.IsNullOrWhiteSpace(bracketedExpression))
                        {
                            throw new ArgumentException("Expression contains empty brackets.");
                        }

                        var parsedSubexpression = RealParse(bracketedExpression, inputs);
                        currentLevelParsedExpressions.Add(new(expression.Substring(leftBracket, rightBracket - leftBracket + 1), parsedSubexpression));
                    }
                }
            }

            int j = 0;
            foreach (var expr in currentLevelParsedExpressions)
            {
                string s = expr.Item1;
                LogicalExpression parsedExpression = expr.Item2;

                int sIndex = expression.IndexOf(s);
                expression = expression.Remove(sIndex) + "{" + j++ + "}" + expression.Substring(sIndex + s.Length);
            }

            if (expression.Contains(AND_SYMBOL))
            {
                if (expression.Contains(OR_SYMBOL) || expression.Contains(XOR_SYMBOL))
                {
                    throw new ArgumentException("Different operators on the same level are not allowed.");
                }

                List<LogicalExpression> expressions = new();

                string[] split = expression.Split(AND_SYMBOL);

                foreach (string s in split)
                {
                    string trimmed = s.Trim();

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        throw new ArgumentException("And operator is missing an expression.");
                    }

                    bool isNegated = trimmed[0] == NOT_SYMBOL;
                    if (isNegated) trimmed = trimmed.Substring(1);

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        throw new ArgumentException("Missing an expression.");
                    }

                    bool isAlreadyParsed = trimmed[0] == '{';

                    LogicalExpression toAdd;

                    if (isAlreadyParsed)
                    {
                        int k = int.Parse(trimmed.Substring(1, trimmed.Length - 2));

                        var existingExpression = currentLevelParsedExpressions[k].Item2;
                        toAdd = isNegated ?
                            new Not(existingExpression) :
                            existingExpression;
                    }
                    else
                    {
                        toAdd = ParseAtomicExpression(trimmed, inputs);
                    }

                    expressions.Add(toAdd);
                }

                return new And(expressions);
            }
            else if (expression.Contains(OR_SYMBOL))
            {
                if (expression.Contains(AND_SYMBOL) || expression.Contains(XOR_SYMBOL))
                {
                    throw new ArgumentException("Different operators on the same level are not allowed.");
                }

                List<LogicalExpression> expressions = new();

                string[] split = expression.Split(OR_SYMBOL);

                foreach (string s in split)
                {
                    string trimmed = s.Trim();

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        throw new ArgumentException("Or operator is missing an expression.");
                    }

                    bool isNegated = trimmed[0] == NOT_SYMBOL;
                    if (isNegated) trimmed = trimmed.Substring(1);

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        throw new ArgumentException("Missing an expression.");
                    }

                    bool isAlreadyParsed = trimmed[0] == '{';

                    LogicalExpression toAdd;

                    if (isAlreadyParsed)
                    {
                        int k = int.Parse(trimmed.Substring(1, trimmed.Length - 2));

                        var existingExpression = currentLevelParsedExpressions[k].Item2;
                        toAdd = isNegated ?
                            new Not(existingExpression) :
                            existingExpression;
                    }
                    else
                    {
                        toAdd = ParseAtomicExpression(trimmed, inputs);
                    }

                    expressions.Add(toAdd);
                }

                return new Or(expressions);
            }
            else if (expression.Contains(XOR_SYMBOL))
            {
                if (expression.Contains(AND_SYMBOL) || expression.Contains(OR_SYMBOL))
                {
                    throw new ArgumentException("Different operators on the same level are not allowed.");
                }

                List<LogicalExpression> expressions = new();

                string[] split = expression.Split(XOR_SYMBOL);

                foreach (string s in split)
                {
                    string trimmed = s.Trim();

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        throw new ArgumentException("Xor operator is missing an expression.");
                    }

                    bool isNegated = trimmed[0] == NOT_SYMBOL;
                    if (isNegated) trimmed = trimmed.Substring(1);

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        throw new ArgumentException("Missing an expression.");
                    }

                    bool isAlreadyParsed = trimmed[0] == '{';

                    LogicalExpression toAdd;

                    if (isAlreadyParsed)
                    {
                        int k = int.Parse(trimmed.Substring(1, trimmed.Length - 2));

                        var existingExpression = currentLevelParsedExpressions[k].Item2;
                        toAdd = isNegated ?
                            new Not(existingExpression) :
                            existingExpression;
                    }
                    else
                    {
                        toAdd = ParseAtomicExpression(trimmed, inputs);
                    }

                    expressions.Add(toAdd);
                }

                return new Xor(expressions);
            }
            else
            {
                string trimmed = expression.Trim();

                bool isNegated = trimmed[0] == NOT_SYMBOL;
                if (isNegated) trimmed = trimmed.Substring(1);

                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    throw new ArgumentException("Missing an expression.");
                }

                bool isAlreadyParsed = trimmed[0] == '{';

                if (isAlreadyParsed)
                {
                    int k = int.Parse(trimmed.Substring(1, trimmed.Length - 2));

                    var existingExpression = currentLevelParsedExpressions[k].Item2;
                    return isNegated ?
                        new Not(existingExpression) :
                        existingExpression;
                }
                else
                {
                    return ParseAtomicExpression(trimmed, inputs);
                }
            }
        }

        private LogicalExpression ParseAtomicExpression(string expression, HashSet<Input> inputs)
        {
            string trimmed = expression.Trim();

            bool isNegated = trimmed[0] == NOT_SYMBOL;
            if (isNegated) trimmed = trimmed.Substring(1);

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                throw new ArgumentException("Missing an expression.");
            }

            bool isAlreadyParsed = trimmed[0] == '{';

            LogicalExpression toAdd;

            if (char.IsDigit(trimmed[0]))
            {
                throw new ArgumentException("Input name cannot begin with a digit.");
            }
            if (!trimmed.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                throw new ArgumentException("Input contains forbidden characters.");
            }

            string inputName = trimmed;

            if (inputs.Any(i => i.Name == inputName))
            {
                if (isNegated)
                {
                    toAdd = new Not(inputs.Where(i => i.Name == inputName).First());
                }
                else
                {
                    toAdd = inputs.Where(i => i.Name == inputName).First();
                }
            }
            else
            {
                Input input;
                input = new Input(inputName);
                inputs.Add(input);
                if (isNegated)
                {
                    toAdd = new Not(input);
                }
                else
                {
                    toAdd = input;
                }
            }

            return toAdd;
        }

        private bool AllSymbolsAllowed(string expression)
        {
            return expression.All(c =>
                char.IsLetterOrDigit(c) ||
                ALLOWED_SPECIAL_SYMBOLS.Contains(c) ||
                OPERATOR_SYMBOLS.Contains(c));
        }
    }
}
