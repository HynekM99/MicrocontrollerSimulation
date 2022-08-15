using MicrocontrollerSimulation.JDOs;
using MicrocontrollerSimulation.Models.Functions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MicrocontrollerSimulation.JDOs.Functions;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.JDOs.Expressions;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;

namespace MicrocontrollerSimulation.Services.ProjectConversionServices
{
    public class ProjectToJsonService : IConvertProjectService
    {
        private readonly string _projectName;
        private readonly FunctionsCollection _functions;

        public ProjectToJsonService(
            string projectName,
            FunctionsCollection functions)
        {
            _projectName = projectName;
            _functions = functions;
        }

        public string Convert()
        {
            List<FunctionJDO> functions = new();

            foreach (var function in _functions)
            {
                functions.Add(ConvertFunction(function));
            }

            ProjectJDO project = new()
            {
                Name = _projectName,
                Functions = functions,
            };

            return JsonConvert.SerializeObject(project, Formatting.Indented);
        }

        private FunctionJDO ConvertFunction(Function function)
        {
            return new()
            {
                Name = function.Name,
                LogicalExpression = ConvertExpression(function.Expression)
            };
        }

        private LogicalExpressionJDO ConvertExpression(LogicalExpression expression)
        {
            LogicalExpressionJDO expressionJDO;

            if (expression is CustomExpression)
            {
                expressionJDO = new CustomExpressionJDO();
                expressionJDO.LogicalExpressions.Add(ConvertExpression(expression.LogicalExpressions.First()));
            }
            else if (expression is Input input)
            {
                expressionJDO = new InputJDO
                {
                    Name = input.AsString
                };
            }
            else if (expression is Not)
            {
                expressionJDO = new NotJDO();
                expressionJDO.LogicalExpressions.Add(ConvertExpression(expression.LogicalExpressions.First()));
            }
            else if (expression is And)
            {
                expressionJDO = new AndJDO();
                expression.LogicalExpressions.
                    ForEach(e => expressionJDO.LogicalExpressions.Add(ConvertExpression(e)));
            }
            else if (expression is Or)
            {
                expressionJDO = new OrJDO();
                expression.LogicalExpressions.
                    ForEach(e => expressionJDO.LogicalExpressions.Add(ConvertExpression(e)));
            }
            else
            {
                expressionJDO = new XorJDO();
                expression.LogicalExpressions.
                    ForEach(e => expressionJDO.LogicalExpressions.Add(ConvertExpression(e)));
            }

            expressionJDO.Type = expression.GetType().Name;
            return expressionJDO;
        }
    }
}
