using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions;
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
                throw new ForbiddenSymbolException();
            }

            if (expression.Count(c => c == '(') != expression.Count(c => c == ')'))
            {
                throw new BracketException();
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
                        throw new BracketException();
                    }

                    rightBracketIndexes.Push(i);

                    if (leftBracketIndexes.Count == rightBracketIndexes.Count)
                    {
                        int leftBracket = leftBracketIndexes.Dequeue();
                        int rightBracket = rightBracketIndexes.Pop();

                        leftBracketIndexes.Clear();
                        rightBracketIndexes.Clear();

                        string expressionWithBrackets = expression.Substring(leftBracket, rightBracket - leftBracket + 1);
                        string bracketedExpression = expression.Substring(leftBracket + 1, rightBracket - leftBracket - 1);

                        if (string.IsNullOrWhiteSpace(bracketedExpression))
                        {
                            throw new BracketsEmptyException();
                        }

                        var parsedSubexpression = RealParse(bracketedExpression, inputs);
                        currentLevelParsedExpressions.Add(new Tuple<string, LogicalExpression>(expressionWithBrackets, parsedSubexpression));
                    }
                }
            }

            expression = ReplaceAlreadyParsedExpressions(expression, currentLevelParsedExpressions);

            List<LogicalExpression> alreadyParsed = currentLevelParsedExpressions.Select(t => t.Item2).ToList();

            if ((expression.Contains(AND_SYMBOL) && expression.Contains(OR_SYMBOL)) ||
                (expression.Contains(OR_SYMBOL) && expression.Contains(XOR_SYMBOL)) ||
                (expression.Contains(AND_SYMBOL) && expression.Contains(XOR_SYMBOL)))
            {
                if (expression.Contains(OR_SYMBOL) || expression.Contains(XOR_SYMBOL))
                {
                    throw new OperatorsException();
                }
            }

            if (expression.Contains(AND_SYMBOL))
            {
                return new And(ParseElementalSubexpressions(AND_SYMBOL, expression, alreadyParsed, inputs));
            }
            else if (expression.Contains(OR_SYMBOL))
            {
                return new Or(ParseElementalSubexpressions(OR_SYMBOL, expression, alreadyParsed, inputs));
            }
            else if (expression.Contains(XOR_SYMBOL))
            {
                return new Xor(ParseElementalSubexpressions(XOR_SYMBOL, expression, alreadyParsed, inputs));
            }
            return ParseElementalSubexpression(expression, currentLevelParsedExpressions.Select(t => t.Item2).ToList(), inputs);
        }

        private string ReplaceAlreadyParsedExpressions(string expression, List<Tuple<string, LogicalExpression>> alreadyParsed)
        {
            int i = 0;
            foreach (var e in alreadyParsed)
            {
                string s = e.Item1;
                LogicalExpression parsedExpression = e.Item2;

                int index = expression.IndexOf(s);
                expression = expression.Remove(index) + "{" + i++ + "}" + expression.Substring(index + s.Length);
            }
            return expression;
        }

        private List<LogicalExpression> ParseElementalSubexpressions(char delimitingOperator, string expression, List<LogicalExpression> alreadyParsed, HashSet<Input> inputs)
        {
            string[] split = expression.Split(delimitingOperator);
            return ParseElementalSubexpressions(split, alreadyParsed, inputs);
        }

        private List<LogicalExpression> ParseElementalSubexpressions(string[] elements, List<LogicalExpression> alreadyParsed, HashSet<Input> inputs)
        {
            List<LogicalExpression> expressions = new();

            foreach (string s in elements)
            {
                expressions.Add(ParseElementalSubexpression(s, alreadyParsed, inputs));
            }

            return expressions;
        }

        private LogicalExpression ParseElementalSubexpression(string element, List<LogicalExpression> alreadyParsed, HashSet<Input> inputs)
        {
            element = element.Trim();

            if (element.StartsWith(NOT_SYMBOL))
            {
                return new Not(ParseElementalSubexpression(element.Substring(1), alreadyParsed, inputs));
            }

            if (element.StartsWith('{'))
            {
                if (!element.EndsWith('}'))
                {
                    throw new RedundantCharactersException();
                }

                int i = int.Parse(element.Substring(1, element.Length - 2));

                return alreadyParsed[i];
            }
            else
            {
                return ParseInput(element, inputs);
            }
        }

        private LogicalExpression ParseInput(string name, HashSet<Input> inputs)
        {
            name = name.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new MissingInputException();
            }
            if (char.IsDigit(name[0]))
            {
                throw new InputNameException("Input name cannot begin with a digit.");
            }
            if (!name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                throw new InputNameException();
            }

            if (inputs.Any(i => i.Name == name))
            {
                return inputs.Where(i => i.Name == name).First();
            }
            else
            {
                var input = new Input(name);
                inputs.Add(input);
                return input;
            }
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
