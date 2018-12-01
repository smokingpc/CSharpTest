using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace CodeTextExecTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string codes = textBox1.Text;
            textBox2.Text = "";
            RunText1("MessageBox.Show(msg);");
            RunText2(codes, "CodeTextExecTest", "CMyTestClass");
        }
        private void RunText2(string codes, string name_space, string class_name)
        {
            List<string> UsingList = new List<string>()
                {
                    "System",
                    "System.Windows.Forms",
                };
            List<string> ImportList = new List<string>()
                {
                    "System.dll",
                    "System.Windows.Forms.dll",
                };

            CompilerParameters pParams =
                new CompilerParameters(ImportList.ToArray());
            pParams.GenerateInMemory = true;
            //Compile
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            List<string> source_codes = new List<string>();
            source_codes.Add(codes);
            CompilerResults pResults = provider.CompileAssemblyFromSource(
                                            pParams, source_codes.ToArray());

            if (pResults.Errors != null && pResults.Errors.Count > 0)
            {
                foreach (CompilerError pError in pResults.Errors)
                    MessageBox.Show(pError.ToString());
            }
            else
            {
                var result = (dynamic)pResults.CompiledAssembly.CreateInstance(
                                            name_space + "." + class_name);

                result.TestFunction("23939889");
            }
        }
        private void RunText1(string codes)
        {
            List<string> UsingList = new List<string>()
                {
                    "System",
                    "System.Windows.Forms",
                };
            List<string> ImportList = new List<string>()
                {
                    "System.dll",
                    "System.Windows.Forms.dll",
                };

            CodeMemberMethod pMethod = new CodeMemberMethod();
            pMethod.Name = "TestFunction";
            pMethod.Attributes = MemberAttributes.Public;
            pMethod.Parameters.Add(new
                CodeParameterDeclarationExpression(typeof(string), "msg"));
            pMethod.ReturnType = new CodeTypeReference(typeof(void));
            pMethod.Statements.Add(new CodeSnippetStatement(codes));
            //new CodeSnippetExpression(" MessageBox.Show(msg);"));

            CodeTypeDeclaration pClass =
                new System.CodeDom.CodeTypeDeclaration("MyTestClass");
            pClass.Attributes = MemberAttributes.Public;
            pClass.Members.Add(pMethod);

            CodeNamespace pNamespace = new CodeNamespace("CodeTextExecTest");
            pNamespace.Types.Add(pClass);
            foreach (string item in UsingList)
                pNamespace.Imports.Add(new CodeNamespaceImport(item));

            //Create compile unit
            CodeCompileUnit pUnit = new CodeCompileUnit();
            pUnit.Namespaces.Add(pNamespace);

            //Make compilation parameters
            CompilerParameters pParams =
                new CompilerParameters(ImportList.ToArray());
            pParams.GenerateInMemory = true;
            //Compile
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            CompilerResults pResults = provider.CompileAssemblyFromDom(pParams, pUnit);

            if (pResults.Errors != null && pResults.Errors.Count > 0)
            {
                foreach (CompilerError pError in pResults.Errors)
                    MessageBox.Show(pError.ToString());
            }
            else
            {
                var result = (dynamic)pResults.CompiledAssembly.CreateInstance("CodeTextExecTest.MyTestClass");

                result.TestFunction("23939889");
            }
        }
    }
}
