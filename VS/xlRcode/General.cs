using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ExcelDna.Integration;
using ExcelDna.Logging;
using ExcelDna.Registration;
using Microsoft.CSharp;
using RDotNet;

using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

using static xlRcode.Global;
using System.Drawing;

namespace xlRcode
{

    public static class Global
    {
        public static REngine _engine { get; set; }
        public static CustomCharacterDevice consoleDotNet { get; set; }
        public static Form myfConsole { get; set; }
    }


    public class AddIn : IExcelAddIn
    {


        public delegate object ParamsAction(params object[] arguments);

        public void AutoOpen()
        {

            // Instantiate form
            myfConsole = new xlRcode.fConsole(); // Instantiate a Form object.
            myfConsole.Hide();

            // Initialize RDotNet
            myRDotNet.InitializeRDotNet();

            // Run all scripts in the Functions Folder
            RunFunctionScripts();

            // Register functions from scripts in the Functions Folder
            CreateFunctions();

            // Register functions from the AddIn
            RegisterFunctions();

        }

        public void AutoClose()
        {
        }

        public void RegisterFunctions()
        {
            // There are various options for wrapping and transforming your functions
            // See the Source\Samples\Registration.Sample project for a comprehensive example
            // Here we just change the attribute before registering the functions
            ExcelRegistration.GetExcelFunctions()
                             .Select(UpdateHelpTopic)
                             .ProcessParamsRegistrations()
                             .RegisterFunctions();

        }

        public ExcelFunctionRegistration UpdateHelpTopic(ExcelFunctionRegistration funcReg)
        {
            funcReg.FunctionAttribute.HelpTopic = "http://www.bing.com";
            return funcReg;
        }

        public static void RunFunctionScripts()
        {

            // Find script files
            string path = xlRcode.Properties.Settings.Default.FunctionsFolder;
            var allowedExtensions = new[] { ".r" };
            string[] fileEntries = Directory.GetFiles(path).Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToArray();

            // Run scripts

            foreach (string fileName in fileEntries) 
            {
                string script = File.ReadAllText(fileName);
                _engine.Evaluate(script);
            }

        }

        public static void CreateFunctions()
        {

            string[] listFunctions = _engine.Evaluate("lsf.str()").AsCharacter().ToArray();
            
            for (int i = 0; i < listFunctions.Length; i++)
            {
                string nameFunction = listFunctions[i];
                var listArgs = _engine.Evaluate("formalArgs(" + listFunctions[i] + ")").AsCharacter();
                string[] fStruct = !(listArgs is null) ? listArgs.ToArray() : Array.Empty<string>();
                int nArgs = fStruct.Length;

                System.Type typeReturn = typeof(object);
                
                System.Type[] typeArgument = new Type[nArgs];
                typeArgument = Enumerable.Repeat(typeof(object), nArgs).ToArray();

                ExcelFunctionAttribute att = new ExcelFunctionAttribute();
                att.Name = nameFunction;
                att.Description = "Description to be defined";
                att.HelpTopic = "http://www.google.com";
                att.ExplicitRegistration = true;
                att.SuppressOverwriteError = true;

                List<object> argAttribs = new List<object>();
                for (int iArg = 0; iArg < nArgs; iArg++)
                {
                    ExcelArgumentAttribute atta = new ExcelArgumentAttribute();
                    atta.Name = fStruct[iArg];
                    atta.Description = "Description of the argument";
                    argAttribs.Add(atta);
                }

                RegisterFunctionOnTheFly(nameFunction, typeReturn, typeArgument, att, argAttribs);
                //Reg(nameFunction, typeReturn, typeArgument, att, argAttribs);

            }

        }

        public static void RegisterFunctionOnTheFly(string nameFunction, System.Type typeReturn, System.Type[] typeArgument, ExcelFunctionAttribute att, List<object> argAttribs)
        {
            DynamicMethod newFunction = new DynamicMethod(nameFunction,
                                  typeReturn,
                                  typeArgument,
                                  typeof(AddIn));
            ILGenerator il = newFunction.GetILGenerator();
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);
            // Method will be completed when CreateDelegate is called during registration.

            // Registration takes a list of Delegates - the registration
            // creates a new assembly and a type every time it is called.
            List<Delegate> delegates = new List<Delegate>();

            //delegates.Add(newFunction.CreateDelegate(typeof(Func<object, object>)));

            object genericFunc(object[] paramsList)
            {
                return xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", paramsList);
            }

            ParamsAction mult = genericFunc;

            // We can also create some attributes to customize the function registration:
            List<object> funcAttribs = new List<object>();
            funcAttribs.Add(att);

            List<List<object>> argAttribsList = new List<List<object>>();
            argAttribsList.Add(argAttribs);

            List<ExcelParameterRegistration> argRegList = new List<ExcelParameterRegistration>();
            for (int iArg = 0; iArg < argAttribs.Count; iArg++)
            {
                ExcelParameterRegistration atta = new ExcelParameterRegistration((ExcelArgumentAttribute)argAttribs[iArg]);
                argRegList.Add(atta);
            }

            delegates.Add(mult);
            
            Func<object[], object> funcDel = mult.Invoke;
            List<Func<object[], object>> listFunc = new List<Func<object[], object>>();
            listFunc.Add(funcDel);


            dynamic myFunc = new  { };
            switch (argAttribs.Count) 
            {
                case 0:
                    System.Linq.Expressions.Expression<Func<object>> myLambdaFunc0 = () => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "");
                    myFunc = myLambdaFunc0;
                    break;
                case 1:
                    System.Linq.Expressions.Expression<Func<object, object>> myLambdaFunc1 = p1 => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1);
                    myFunc = myLambdaFunc1;
                    break;
                case 2:
                    System.Linq.Expressions.Expression<Func<object, object, object>> myLambdaFunc2 = (p1, p2) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2);
                    myFunc = myLambdaFunc2;
                    break;
                case 3:
                    System.Linq.Expressions.Expression<Func<object, object, object, object>> myLambdaFunc3 = (p1, p2, p3) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2, p3);
                    myFunc = myLambdaFunc3;
                    break;
                case 4:
                    System.Linq.Expressions.Expression<Func<object, object, object, object, object>> myLambdaFunc4 = (p1, p2, p3, p4) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2, p3, p4);
                    myFunc = myLambdaFunc4;
                    break;
                case 5:
                    System.Linq.Expressions.Expression<Func<object, object, object, object, object, object>> myLambdaFunc5 = (p1, p2, p3, p4, p5) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2, p3, p4, p5);
                    myFunc = myLambdaFunc5;
                    break;
                case 6:
                    System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object>> myLambdaFunc6 = (p1, p2, p3, p4, p5, p6) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2, p3, p4, p5, p6);
                    myFunc = myLambdaFunc6;
                    break;
                case 7:
                    System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object>> myLambdaFunc7 = (p1, p2, p3, p4, p5, p6, p7) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2, p3, p4, p5, p6, p7);
                    myFunc = myLambdaFunc7;
                    break;
                case 8:
                    System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object>> myLambdaFunc8 = (p1, p2, p3, p4, p5, p6, p7, p8) => xlRcode.MyFunctions.XLRFUNCTION(nameFunction, "", p1, p2, p3, p4, p5, p6, p7, p8);
                    myFunc = myLambdaFunc8;
                    break;
            }
            
            //System.Linq.Expressions.Expression<Func<object[], object>> myLambdaFunc = (x) => xlRcode.MyFunctions.xlR_Function(nameFunction, "", x);

            ExcelFunctionRegistration funcReg = new ExcelFunctionRegistration(myFunc, att, argRegList);
            List<ExcelFunctionRegistration> funcRegList = new List<ExcelFunctionRegistration>();
            funcRegList.Add(funcReg);

            funcRegList.ProcessParamsRegistrations().RegisterFunctions();

            //ExcelAsyncUtil.QueueAsMacro(() => { ExcelIntegration.RegisterDelegates(delegates, funcAttribs, argAttribsList); });

        }


        // Alternative registration of functions (UNDER MAINTENANCE)
        public static void Reg(string nameFunction, System.Type typeReturn, System.Type[] typeArgument, ExcelFunctionAttribute att, List<object> argAttribs) 
        {
            
            string paramsStr = "";
            string argsStr = "";
            if (argAttribs.Count > 0)
            {
                paramsStr = "object " + String.Join(", object ", argAttribs.Select(x => ((ExcelArgumentAttribute)x).Name).ToList());
                argsStr = ", " + String.Join(", ", argAttribs.Select(x => ((ExcelArgumentAttribute)x).Name).ToList());
            }

            const string quote = "\"";
            string code =
                @"
                namespace xlRcode
                {
                    public class NewFunctions
                    { 
                    
                        public static object " + nameFunction + "(" + paramsStr + ")" + @"
                        {
                            return xlRcode.MyFunctions.xlR_Function(" + quote + nameFunction + quote + ", " + quote + quote + argsStr + @");
                        }
                    }
                }";

            RegisterCodeRuntime_Provider(code);
            //RegisterCodeRuntime_RoslynScripting(code);

        }

        public static void RegisterCodeRuntime_Provider(string code)
        {

            var refs = AppDomain.CurrentDomain.GetAssemblies();
            var refFiles = refs.Where(a => !a.IsDynamic).Select(a => a.Location).ToArray();
            var cp = new System.CodeDom.Compiler.CompilerParameters(refFiles);
            cp.GenerateInMemory = true;
            cp.GenerateExecutable = false;
            cp.ReferencedAssemblies.Add("xlRcode.dll");

            //CompilerParameters cp = new CompilerParameters();
            //cp.GenerateExecutable = false;
            //cp.GenerateInMemory = false;
            //cp.TreatWarningsAsErrors = false;
            //cp.ReferencedAssemblies.Add("System.dll"); //, "System.Windows.Forms.dll", "ExcelDna.Integration.dll" );
            //cp.ReferencedAssemblies.Add("xlRcode.dll");


            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, code);
            if (!cr.Errors.HasErrors)
            {
                Assembly asm = cr.CompiledAssembly;
                Type[] types = asm.GetTypes();
                List<MethodInfo> methods = new List<MethodInfo>();

                // Get list of MethodInfo's from assembly for each method with ExcelFunction attribute
                foreach (Type type in types)
                {
                    foreach (MethodInfo info in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    {
                        methods.Add(info);
                    }
                }

                ExcelAsyncUtil.QueueAsMacro(() => { ExcelDna.Integration.ExcelIntegration.RegisterMethods(methods); });

            }
            else
            {
                MessageBox.Show("Errors during compile!");
            }

        }

        public class ParmModel
        {
            public string Name { get; set; }
        }

        public static void RegisterCodeRuntime_RoslynScripting(string code)
        {

            var model = new ParmModel { Name = "Rick" };

            var opt = ScriptOptions.Default;
            opt.AddReferences(typeof(string).Assembly.FullName, typeof(ParmModel).Assembly.FullName);

            opt.AddImports("System");
            opt.AddImports("xlRcode");

            var state = CSharpScript.RunAsync(code, opt, model, model.GetType()).Result;

        }

    }

    public class MyCharacterDevice : RDotNet.Devices.ConsoleDevice
    {
        public StringBuilder sb = new StringBuilder();

        public new void WriteConsole(string output, int length, RDotNet.Internals.ConsoleOutputType outputType)
        {
            sb.Append(output);
        }

        //rest of the implementation here
    }

    public class CustomCharacterDevice : RDotNet.Devices.ICharacterDevice
    {
        public StringBuilder msg = new StringBuilder();

        public void WriteConsole(string output, int length, RDotNet.Internals.ConsoleOutputType outputType)
        {
            msg.Append(output);

            // My Console
            RichTextBox tbConsoleExcel = (RichTextBox)xlRcode.Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleExcel"].Controls["tbConsoleExcel"];
            WinFormsExtensions.AppendLine(tbConsoleExcel, System.Environment.NewLine + output, Color.Black, SetUp.rConsoleLineLimit);
        }

        // functions below are copy-pasted from ConsoleDevice implementation
        public string ReadConsole(string prompt, int capacity, bool history)
        {
            Console.Write(prompt);
            //return Console.ReadLine();

            // My Console
            RichTextBox tbConsoleExcel = (RichTextBox)xlRcode.Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleExcel"].Controls["tbConsoleExcel"];
            WinFormsExtensions.AppendLine(tbConsoleExcel, System.Environment.NewLine + prompt, Color.Black, SetUp.rConsoleLineLimit);

            return WinFormsExtensions.ReadLine(tbConsoleExcel);
        }

        public void ShowMessage(string message)
        {
            Console.Write(message);
        }

        public void Busy(RDotNet.Internals.BusyType which)
        {
            if (which == RDotNet.Internals.BusyType.None)
            { }
            else if (which == RDotNet.Internals.BusyType.ExtendedComputation)
            { }
        }

        public void Callback()
        { 
        
        }

        public RDotNet.Internals.YesNoCancel Ask(string question)
        {
            Console.Write("{0} [y/n/c]: ", question);
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                switch (Char.ToLower(input[0]))
                {
                    case 'y':
                        return RDotNet.Internals.YesNoCancel.Yes;

                    case 'n':
                        return RDotNet.Internals.YesNoCancel.No;

                    case 'c':
                        return RDotNet.Internals.YesNoCancel.Cancel;
                }
            }
            return default(RDotNet.Internals.YesNoCancel);
        }

        public void Suicide(string message)
        {
            Console.Error.WriteLine(message);
            CleanUp(RDotNet.Internals.StartupSaveAction.Suicide, 2, false);
        }

        public void ResetConsole()
        {
            Console.Clear();
        }

        public void FlushConsole()
        {
            Console.Write(string.Empty);
        }

        public void ClearErrorConsole()
        {
            Console.Clear();
        }

        public void CleanUp(RDotNet.Internals.StartupSaveAction saveAction, int status, bool runLast)
        {
            Environment.Exit(status);
        }

        public bool ShowFiles(string[] files, string[] headers, string title, bool delete, string pager)
        {
            int count = files.Length;
            for (int index = 0; index < count; index++)
            {
                try
                {
                    Console.WriteLine(headers);
                    Console.WriteLine(File.ReadAllText(files[index]));
                    if (delete)
                    {
                        File.Delete(files[index]);
                    }
                }
                catch (IOException)
                {
                    return false;
                }
            }
            return true;
        }

        public string ChooseFile(bool create)
        {
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }
            if (create && !File.Exists(path))
            {
                File.Create(path).Close();
            }
            if (File.Exists(path))
            {
                return path;
            }
            return null;
        }

        public void EditFile(string file)
        { }

        public SymbolicExpression LoadHistory(Language call, SymbolicExpression operation, Pairlist args, REnvironment environment)
        {
            return environment.Engine.NilValue;
        }

        public SymbolicExpression SaveHistory(Language call, SymbolicExpression operation, Pairlist args, REnvironment environment)
        {
            return environment.Engine.NilValue;
        }

        public SymbolicExpression AddHistory(Language call, SymbolicExpression operation, Pairlist args, REnvironment environment)
        {
            return environment.Engine.NilValue;
        }
    }


    public static class myRDotNet
    {

        internal static void InitializeRDotNet()
        {

            SetUp.rHome = Properties.Settings.Default.RHome;
            SetUp.rPath = Properties.Settings.Default.RPath;
            SetUp.CRANMirror = Properties.Settings.Default.CRANMirror;
            SetUp.rConsoleLineLimit = Properties.Settings.Default.ConsoleLineLimit;
            SetUp.initialInstructions = File.ReadAllText(Properties.Settings.Default.InitializationCodeFile);

            try
            {
                // IT WAS NOT WORKING ON MORE RECENT VERSIONS OF R (> 4.0.2 and < 4.2.0)
                string rHome = SetUp.rHome;
                string rPath = SetUp.rPath;
                REngine.SetEnvironmentVariables(rPath, rHome);

                RDotNet.StartupParameter sp = new StartupParameter();
                sp.Interactive = true;
                sp.Quiet = false;

                consoleDotNet = new CustomCharacterDevice();

                _engine = REngine.GetInstance("", true, sp, consoleDotNet);
                _engine.Initialize(sp, consoleDotNet, true);
                _engine.AutoPrint = true; // Display all results

                //RESet path
                // https://github.com/rdotnet/rdotnet/issues/151
                //Workaround from GitHub - explicitly include R libs in PATH so R environment can find them. Not sure why R can't find them when
                //we set this via Environment.SetEnvironmentVariable
                _engine.Evaluate("Sys.setenv(PATH = paste(\"" + rPath.Replace(Path.DirectorySeparatorChar.ToString(), "/")  + "\", Sys.getenv(\"PATH\"), sep=\";\"))");


                //Set default CRAN mirror
                string CRANMirror = SetUp.CRANMirror;
                _engine.Evaluate("options(repos=structure(c(CRAN='" + CRANMirror + "')))");

                //Initial instructions
                _engine.Evaluate(SetUp.initialInstructions);

            }
            catch (Exception ex)
            {
                LogDisplay.WriteLine("Error initializing RDotNet: " + ex.Message);
            }
        }
    }

    public class SetUp
    {

        public static string rHome = Properties.Settings.Default.RHome;
        public static string rPath = Properties.Settings.Default.RPath;

        public static string CRANMirror = Properties.Settings.Default.CRANMirror;

        public static int rConsoleLineLimit = Properties.Settings.Default.ConsoleLineLimit;

        //INITIAL INSTRUCTIONS
        public static string initialInstructions = File.ReadAllText(Properties.Settings.Default.InitializationCodeFile);

    }

}
