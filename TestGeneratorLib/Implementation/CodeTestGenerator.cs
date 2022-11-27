using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Formatting;

namespace TestGeneratorLib.Implementation
{
    public abstract class CodeTestGenerator
    {
        private readonly AttributeListSyntax _attr;
        private readonly StatementSyntax _body;

        protected CodeTestGenerator(string attr)
        {
            _attr = CreateUnitTestAttribute(attr);
            _body = GetUnitTestBody();
        }

        private IEnumerable<MethodDeclarationSyntax> GetPublicMethods(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().
                Where(m => m.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)));
        }

        private string GenerateClass(ClassDeclarationSyntax classDeclaration, NamespaceDeclarationSyntax newNamespace, in SyntaxList<UsingDirectiveSyntax> usings)
        {
            var compilationUnit = CompilationUnit();
            var newClass = ClassDeclaration(classDeclaration.Identifier.Text + "Tests")
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
            var publicMethods = GetPublicMethods(classDeclaration);
            newClass = newClass.AddMembers(GenerateTestMethods(publicMethods).ToArray());
            compilationUnit = compilationUnit.AddUsings(usings.ToArray());
            compilationUnit = compilationUnit.AddMembers(newNamespace.AddMembers(newClass));
            return compilationUnit.NormalizeWhitespace().ToString();
        }

        private IEnumerable<MethodDeclarationSyntax> GenerateTestMethods(IEnumerable<MethodDeclarationSyntax> oldMethods)
        {
            var newMethods = new List<MethodDeclarationSyntax>();
            foreach (var method in oldMethods)
            {
                newMethods.Add(GenerateTestMethod(method.Identifier.Text));
            }
            return newMethods;
        }

        protected IEnumerable<string> GenerateClasses(CompilationUnitSyntax root)
        {
            var newClasses = new List<string>();
            var oldNamespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            var newNamespace = CreateNewNamespace(oldNamespace);
            var usings = root.Usings.Add(GetDefaultUsing());
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var classDeclaration in classes)
            {
                newClasses.Add(GenerateClass(classDeclaration, newNamespace, in usings));
            }
            return newClasses;
        }


        private NamespaceDeclarationSyntax CreateNewNamespace(NamespaceDeclarationSyntax? oldNamespace)
        {
            return NamespaceDeclaration(
                oldNamespace == null ? IdentifierName("Tests") : IdentifierName($"{oldNamespace.Name}.Tests"));
        }

        /// <summary>
        /// Generate Test Attribute for corresponding Unit Test Library
        /// </summary>
        /// <param name="attributeName">Attribute Name</param>
        /// <returns></returns>
        private AttributeListSyntax CreateUnitTestAttribute(string attributeName)
        {
            return AttributeList(
                             SingletonSeparatedList(
                             Attribute(IdentifierName(attributeName)
                                 )));
        }

        /// <summary>
        /// Test Method with body and unit test attribute
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private MethodDeclarationSyntax GenerateTestMethod(string methodName)
        {
            return MethodDeclaration(attributeLists: List<AttributeListSyntax>().Add(_attr),
                modifiers: TokenList(Token(SyntaxKind.PublicKeyword)),
                returnType: PredefinedType(Token(SyntaxKind.VoidKeyword)),
                explicitInterfaceSpecifier: null,
                identifier: Identifier(methodName + "Test"),
                typeParameterList: null,
                parameterList: ParameterList(),
                constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                body: Block(_body),
                semicolonToken: new SyntaxToken()).
                WithAdditionalAnnotations(Formatter.Annotation);
        }

        /// <summary>
        /// Default using for Unit Test generator
        /// </summary>
        /// <returns></returns>
        protected abstract UsingDirectiveSyntax GetDefaultUsing();

        /// <summary>
        /// Gets body of Unit Test
        /// </summary>
        /// <returns></returns>
        protected abstract StatementSyntax GetUnitTestBody();
    }
}
